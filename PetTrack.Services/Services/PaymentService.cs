using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Entity;
using PetTrack.ModelViews.Payment;

namespace PetTrack.Services.Services
{
    public class PaymentService : IPaymentService
    {
        public readonly PayOS _payOS;
        public readonly IUnitOfWork _unitOfWork;
        private IUserContextService _userContextService;
        private ITopUpTransactionService _topUpTransactionService;
        public PaymentService(PayOS payOS, IUnitOfWork unitOfWork, IUserContextService userContextService, ITopUpTransactionService topUpTransactionService)
        {
            _unitOfWork = unitOfWork;
            _payOS = payOS;
            _userContextService = userContextService;
            _topUpTransactionService = topUpTransactionService;
        }
        public async Task<CreatePaymentResult> CreateLinkAsync(CreatePaymentLinkRequest request)
        {
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            var accountId = _userContextService.GetUserId() ?? throw new ArgumentException("User not found", nameof(_userContextService));
            var account = await _unitOfWork.GetRepository<User>().Entities.FirstOrDefaultAsync(u => u.Id == accountId);
            await _topUpTransactionService.CreateTopUpTransactionAsync(accountId, request.Price, orderCode.ToString());
            ItemData item = new ItemData(accountId, 1, request.Price);
            var descriptions = request.Description = $"Deposit {request.Price}";
            List<ItemData> items = new List<ItemData> { item };
            var expiredAt = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
            PaymentData paymentDataPayment = new PaymentData(orderCode, request.Price, descriptions, items, request.CancelUrl, request.ReturnUrl, null, null, null, null, null, expiredAt);
            try
            {
                var createdLink = await _payOS.createPaymentLink(paymentDataPayment);
                return createdLink;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
    }
}
