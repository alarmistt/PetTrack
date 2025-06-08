using PetTrack.Entity;
using PetTrack.ModelViews.BankAccountModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class BankAccountMapper
    {
        public static BankAccountResponse ToBankAccountDto(this BankAccount bankAccount)
        {
            return new BankAccountResponse
            {
                Id = bankAccount.Id,
                BankName = bankAccount.BankName,
                BankNumber = bankAccount.BankNumber
            };
        }

        public static List<BankAccountResponse> ToBankAccountDtoList(this List<BankAccount> bankAccounts)
        {
            return bankAccounts.Select(x => ToBankAccountDto(x)).ToList();
        }
    }
}
