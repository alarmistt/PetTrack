using PetTrack.Contract.Repositories.PaggingItems;
using PetTrack.ModelViews.Booking;
using PetTrack.ModelViews.TopUpModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface ITopUpTransactionService
    {
        Task CreateTopUpTransactionAsync(string accoundId, decimal amount,string transactionCode);
        Task CheckStatusTransactionAsync(string orderCode);
        Task<PaginatedList<TopUpResponse>> GetTopUpTransaction(int pageIndex, int pageSize, string? userId = null, string? status = null);
    }
}
