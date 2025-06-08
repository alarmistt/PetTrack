using PetTrack.ModelViews.WalletModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IWalletService
    {
        Task<WalletResponse> CreateWalletAsync(CreateWalletRequest request);
        Task CreateWalletIfNotExistsAsync(string userId);
        Task<WalletResponse> UpdateBalanceAsync(string walletId, UpdateWalletRequest request);
        Task<WalletResponse> GetWalletByIdAsync(string walletId);
        Task<WalletResponse> GetMyWalletAsync();
    }
}
