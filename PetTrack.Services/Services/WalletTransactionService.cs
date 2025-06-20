using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Enums;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.Mappers;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.Services.Services
{
    public class WalletTransactionService : IWalletTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletTransactionResponse> CreateTopUpTransactionAsync(string walletId, decimal amount, string? description = null)
        {
            return await CreateTransactionAsync(walletId, amount, WalletTransactionType.TopUp, null, description, null);
        }

        public async Task<List<WalletTransactionResponse>> CreateBookingTransactionsAsync(
            string userWalletId, string ownerWalletId, string adminWalletId,
            decimal bookingAmount, string bookingId)
        {
            decimal ownerAmount = bookingAmount * 0.95m;
            decimal adminFee = bookingAmount - ownerAmount;

            var userTx = await CreateTransactionAsync(userWalletId, -bookingAmount, WalletTransactionType.BookingPayment, null, $"Payment for booking {bookingId}", bookingId);
            var ownerTx = await CreateTransactionAsync(ownerWalletId, ownerAmount, WalletTransactionType.ReceiveAmount, null, $"Receive from booking {bookingId}", bookingId);
            var adminTx = await CreateTransactionAsync(adminWalletId, adminFee, WalletTransactionType.ComissionFee, null, $"Commission for booking {bookingId}", bookingId);

            return new List<WalletTransactionResponse> { userTx, ownerTx, adminTx };
        }

        public async Task<WalletTransactionResponse> CreateRefundTransactionAsync(string walletId, decimal amount, string bookingId, string? description = null)
        {
            var exists = await _unitOfWork.GetRepository<WalletTransaction>()
                .Entities.AnyAsync(t => t.WalletId == walletId && t.BookingId == bookingId && t.Type == WalletTransactionType.Refund.ToString());

            if (exists)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.DUPLICATE, "Already refunded this booking.");

            return await CreateTransactionAsync(walletId, amount, WalletTransactionType.Refund, null, description ?? $"Refund for booking {bookingId}", bookingId);
        }

        public async Task<BasePaginatedList<WalletTransactionResponse>> GetPagedTransactionsAsync(WalletTransactionQueryObject query)
        {
            var walletRepo = _unitOfWork.GetRepository<Wallet>().Entities
                .Where(w => !w.DeletedTime.HasValue);

            var transactionsQuery = _unitOfWork.GetRepository<WalletTransaction>().Entities
                .Where(t => !t.DeletedTime.HasValue);

            if (!string.IsNullOrWhiteSpace(query.WalletId))
                transactionsQuery = transactionsQuery.Where(t => t.WalletId == query.WalletId);

            if (query.Type.HasValue)
                transactionsQuery = transactionsQuery.Where(t => t.Type == query.Type.Value.ToString());

            if (query.FromDate.HasValue)
                transactionsQuery = transactionsQuery.Where(t => t.CreatedTime >= query.FromDate.Value);

            if (query.ToDate.HasValue)
                transactionsQuery = transactionsQuery.Where(t => t.CreatedTime <= query.ToDate.Value);

            // Filter theo UserId liên kết qua Wallet
            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                var walletIds = await walletRepo
                    .Where(w => w.UserId == query.UserId)
                    .Select(w => w.Id)
                    .ToListAsync();

                transactionsQuery = transactionsQuery.Where(t => walletIds.Contains(t.WalletId));
            }

            // Sorting
            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                transactionsQuery = query.SortBy?.ToLower() switch
                {
                    "amount" => query.IsDescending
                        ? transactionsQuery.OrderByDescending(t => t.Amount)
                        : transactionsQuery.OrderBy(t => t.Amount),

                    _ => query.IsDescending
                        ? transactionsQuery.OrderByDescending(t => t.CreatedTime)
                        : transactionsQuery.OrderBy(t => t.CreatedTime)
                };
            }
            else
            {
                transactionsQuery = transactionsQuery.OrderByDescending(t => t.CreatedTime);
            }
            

            var totalCount = await transactionsQuery.CountAsync();

            var items = await transactionsQuery
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var dto = items.ToTransactionDtoList();

            return new BasePaginatedList<WalletTransactionResponse>(dto, totalCount, query.PageIndex, query.PageSize);
        }

        public async Task<WalletTransactionResponse> GetByIdASync(string walletTransactionId)
        {
            var transaction = await _unitOfWork.GetRepository<WalletTransaction>().GetByIdAsync(walletTransactionId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Transaction not found");

            if (transaction.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.FAILED, "Transaction already deleted");
            }

            return transaction.ToTransactionDto();
        }

        public async Task<List<WalletTransactionResponse>> GetByUserIdAsync(string userId)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().Entities
                .Where(w => w.UserId == userId && !w.DeletedTime.HasValue)
                .FirstOrDefaultAsync()
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Wallet not found for user");

            var transactions = await _unitOfWork.GetRepository<WalletTransaction>().Entities
                .Where(t => t.WalletId == wallet.Id && !t.DeletedTime.HasValue)
                .OrderByDescending(t => t.CreatedTime)
                .ToListAsync();

            return transactions.ToTransactionDtoList();
        }

        public async Task<WalletTransactionResponse> GetByIdAsync(string transactionId)
        {
            var transaction = await _unitOfWork.GetRepository<WalletTransaction>().Entities
                .Where(t => t.Id == transactionId && !t.DeletedTime.HasValue)
                .FirstOrDefaultAsync()
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Transaction not found");

            return transaction.ToTransactionDto();
        }

        // Shared private method
        private async Task<WalletTransactionResponse> CreateTransactionAsync(string walletId, decimal amount, WalletTransactionType type, WalletTransactionStatus? status, string? description, string? bookingId)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().GetByIdAsync(walletId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Wallet not found");

            if (wallet.DeletedTime.HasValue)
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.FAILED, "Wallet is deleted");

            wallet.Balance += amount;

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = type.ToString(),
                Status = status.ToString(),
                Description = description,
                BookingId = bookingId,
            };

            await _unitOfWork.GetRepository<WalletTransaction>().InsertAsync(transaction);
            await _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
            await _unitOfWork.SaveAsync();

            return transaction.ToTransactionDto();
        }

        #region Withdrawal
        public async Task<WithdrawResponse> RequestWithdrawAsync(string userId, WithdrawRequest request)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().Entities
                .FirstOrDefaultAsync(w => w.UserId == userId && !w.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Wallet not found");

            if (wallet.Balance < request.Amount)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FAILED, "Insufficient balance for withdrawal");
            }

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = request.Amount,
                Type = WalletTransactionType.Withdraw.ToString(),
                Status = WalletTransactionStatus.Pending.ToString(),
                Description = request.Description,
                BankName = request.BankName,
                BankNumber = request.BankNumber,
            };

            await _unitOfWork.GetRepository<WalletTransaction>().InsertAsync(transaction);
            await _unitOfWork.SaveAsync();

            return transaction.ToWithdrawDto();
        }

        public async Task<BasePaginatedList<WithdrawResponse>> GetPagedWithdrawsAsync(WithdrawQueryObject query)
        {
            var walletRepo = _unitOfWork.GetRepository<Wallet>().Entities
        .       Where(w => !w.DeletedTime.HasValue);

            var transactionsQuery = _unitOfWork.GetRepository<WalletTransaction>().Entities
                .Where(t => !t.DeletedTime.HasValue && t.Type == WalletTransactionType.Withdraw.ToString());

            if (!string.IsNullOrWhiteSpace(query.WalletId))
                transactionsQuery = transactionsQuery.Where(t => t.WalletId == query.WalletId);

            if (query.Status.HasValue)
                transactionsQuery = transactionsQuery.Where(t => t.Status == query.Status.Value.ToString());

            if (query.FromDate.HasValue)
                transactionsQuery = transactionsQuery.Where(t => t.CreatedTime >= query.FromDate.Value);

            if (query.ToDate.HasValue)
                transactionsQuery = transactionsQuery.Where(t => t.CreatedTime <= query.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                var walletIds = await walletRepo
                    .Where(w => w.UserId == query.UserId)
                    .Select(w => w.Id)
                    .ToListAsync();

                transactionsQuery = transactionsQuery.Where(t => walletIds.Contains(t.WalletId));
            }

            // Sorting
            transactionsQuery = query.SortBy?.ToLower() switch
            {
                "amount" => query.IsDescending ? transactionsQuery.OrderByDescending(t => t.Amount) : transactionsQuery.OrderBy(t => t.Amount),
                _ => query.IsDescending ? transactionsQuery.OrderByDescending(t => t.CreatedTime) : transactionsQuery.OrderBy(t => t.CreatedTime)
            };

            var totalCount = await transactionsQuery.CountAsync();

            var items = await transactionsQuery
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var dto = items.ToWithdrawDtoList();

            return new BasePaginatedList<WithdrawResponse>(dto, totalCount, query.PageIndex, query.PageSize);
        }

        public async Task ApprovedWithdrawAsync(string transactionId)
        {
            var transaction = await _unitOfWork.GetRepository<WalletTransaction>()
                .GetByIdAsync(transactionId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Withdraw transaction not found");

            if (transaction.Type != WalletTransactionType.Withdraw.ToString())
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FAILED, "Transaction is not a withdraw");

            if (transaction.Status != WalletTransactionStatus.Pending.ToString())
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FAILED, "Transaction is already processed");

            var wallet = await _unitOfWork.GetRepository<Wallet>().GetByIdAsync(transaction.WalletId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Wallet not found");

            // Confirm balance before deducting
            if (wallet.Balance < transaction.Amount)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FAILED, "Insufficient balance at approval time");

            transaction.Status = WalletTransactionStatus.Completed.ToString();
            transaction.LastUpdatedTime = CoreHelper.SystemTimeNow;

            // Deduct the balance
            wallet.Balance -= transaction.Amount;

            await _unitOfWork.GetRepository<WalletTransaction>().UpdateAsync(transaction);
            await _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
            await _unitOfWork.SaveAsync();
        }

        public async Task RejectWithdrawAsync(string transactionId)
        {
            var transaction = await _unitOfWork.GetRepository<WalletTransaction>().GetByIdAsync(transactionId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Withdraw transaction not found");

            if (transaction.Type != WalletTransactionType.Withdraw.ToString())
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FAILED, "Transaction is not a withdraw");

            if (transaction.Status != WalletTransactionStatus.Pending.ToString())
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FAILED, "Transaction is already processed");

            transaction.Status = WalletTransactionStatus.Rejected.ToString();
            transaction.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<WalletTransaction>().UpdateAsync(transaction);
            await _unitOfWork.SaveAsync();
        }

        #endregion
    }
}
