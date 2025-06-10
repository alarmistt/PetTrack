using Microsoft.EntityFrameworkCore;
using Net.payOS;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Enums;
using PetTrack.Entity;

namespace PetTrack.Services.Services
{
    public class TopUpTransactionService : ITopUpTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayOS _payOS;
        private readonly IWalletService _walletService;
        private IUserContextService _userContextService;

        public TopUpTransactionService(IUnitOfWork unitOfWork, PayOS payOS, IWalletService walletService, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _payOS = payOS;
            _walletService = walletService;
            _userContextService = userContextService;
        }

        public async Task CreateTopUpTransactionAsync(string accountId, decimal amount, string transactionCode)
        {
            var transaction = new TopUpTransaction
            {
                UserId = accountId,
                Amount = amount,
                PaymentMethod = "PayOS",
                Status = TopUpTransactionStatus.Pending.ToString(),
                TransactionCode = transactionCode
            };
            _unitOfWork.GetRepository<TopUpTransaction>().Insert(transaction);
            await _unitOfWork.GetRepository<TopUpTransaction>().SaveAsync();
        }

        public async Task CheckStatusTransactionAsync(string transactionCode)
        {
            var transaction = await _unitOfWork.GetRepository<TopUpTransaction>()
                .Entities.FirstOrDefaultAsync(t => t.TransactionCode == transactionCode);
            if (transaction == null)
                throw new ArgumentException("Transaction not found");
            var checking = await _payOS.getPaymentLinkInformation(long.Parse(transactionCode));

            if (checking.status == "PAID")
            {
                var userId = _userContextService.GetUserId() ?? throw new ArgumentException("User not found", nameof(_userContextService));
                var wallet = await _unitOfWork.GetRepository<Wallet>()
                    .Entities.FirstOrDefaultAsync(w => w.UserId == userId && !w.DeletedTime.HasValue);
                await _walletService.AddBalanceAsync(wallet!.Id, transaction.Amount);
                transaction.Status = TopUpTransactionStatus.Success.ToString();
                transaction.LastUpdatedTime = DateTimeOffset.UtcNow;

                _unitOfWork.GetRepository<TopUpTransaction>().Update(transaction);
                await _unitOfWork.GetRepository<TopUpTransaction>().SaveAsync();
            }
            else
            {
                throw new ArgumentException("Transaction is not paid yet");
            }
            
        }
    }
}
