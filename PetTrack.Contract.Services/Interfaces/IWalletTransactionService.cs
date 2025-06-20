using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IWalletTransactionService
    {
        Task<WalletTransactionResponse> CreateTopUpTransactionAsync(string walletId, decimal amount, string? description = null);
        Task<List<WalletTransactionResponse>> CreateBookingTransactionsAsync(string userWalletId, string ownerWalletId, string adminWalletId, decimal bookingAmount, string bookingId);
        Task<WalletTransactionResponse> CreateRefundTransactionAsync(string walletId, decimal amount, string bookingId, string? description = null);
        Task<BasePaginatedList<WalletTransactionResponse>> GetPagedTransactionsAsync(WalletTransactionQueryObject query);
        Task<List<WalletTransactionResponse>> GetByUserIdAsync(string userId);
        Task<WalletTransactionResponse> GetByIdAsync(string transactionId);
        Task<WithdrawResponse> RequestWithdrawAsync(string userId, WithdrawRequest request);
        Task<BasePaginatedList<WithdrawResponse>> GetPagedWithdrawsAsync(WithdrawQueryObject query);
        Task ApprovedWithdrawAsync(string transactionId);
        Task RejectWithdrawAsync(string transactionId);

    }
}
