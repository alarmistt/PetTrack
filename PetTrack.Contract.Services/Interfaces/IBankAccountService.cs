using PetTrack.ModelViews.BankAccountModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task<List<BankAccountResponse>> GetByUserIdAsync(string userId);
        Task<BankAccountResponse> CreateAsync(string userId, CreateBankAccountRequest request);
        Task<BankAccountResponse> UpdateAsync(string userId, string bankAccountId, UpdateBankAccountRequest request);
        Task DeleteAsync(string bankAccountId);
    }
}
