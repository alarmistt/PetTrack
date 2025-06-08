using PetTrack.Entity;
using PetTrack.ModelViews.WalletModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class WalletMapper
    {
        public static WalletResponse ToWalletDto(this Wallet wallet)
        {
            return new WalletResponse
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                CreatedTime = wallet.CreatedTime,
                LastUpdatedTime = wallet.LastUpdatedTime,
            };
        }

        public static List<WalletResponse> ToWalletDtoList(this List<Wallet> wallets)
        {
            return wallets.Select(x => x.ToWalletDto()).ToList();
        }
    }
}
