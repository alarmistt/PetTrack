namespace PetTrack.Contract.Services.Interfaces
{
    public interface ITopUpTransactionService
    {
        Task CreateTopUpTransactionAsync(string accoundId, decimal amount,string transactionCode);
        Task CheckStatusTransactionAsync(string orderCode);
    }
}
