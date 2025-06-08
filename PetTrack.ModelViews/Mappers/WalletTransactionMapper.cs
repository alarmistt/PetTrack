using PetTrack.Entity;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class WalletTransactionMapper
    {
        public static WalletTransactionResponse ToTransactionDto(this WalletTransaction transaction)
        {
            return new WalletTransactionResponse
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                BookingId = transaction.BookingId,
                Type = transaction.Type,
                CreatedTime = transaction.CreatedTime
            };
        }

        public static List<WalletTransactionResponse> ToTransactionDtoList(this List<WalletTransaction> transactionList)
        {
            return transactionList.Select(x => x.ToTransactionDto()).ToList();
        }
    }
}
