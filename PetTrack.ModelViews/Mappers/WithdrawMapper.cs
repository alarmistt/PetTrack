using PetTrack.Entity;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class WithdrawMapper
    {
        public static WithdrawResponse ToWithdrawDto(this WalletTransaction transaction)
        {
            return new WithdrawResponse
            {
                TransactionId = transaction.Id,
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                BankName = transaction.BankName,
                BankNumber = transaction.BankNumber,
                Status = transaction.Status,
                CreatedTime = transaction.CreatedTime
            };
        }

        public static List<WithdrawResponse> ToWithdrawDtoList(this IEnumerable<WalletTransaction> transactions)
        {
            return transactions.Select(t => t.ToWithdrawDto()).ToList();
        }
    }
}
