using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Helpers;
using PetTrack.Entity;
using PetTrack.ModelViews.BankAccountModels;
using PetTrack.ModelViews.Mappers;

namespace PetTrack.Services.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BankAccountResponse> CreateAsync(string userId, CreateBankAccountRequest request)
        {
            bool check = await _unitOfWork.GetRepository<User>().Entities.AnyAsync(x => x.Id == userId && !x.DeletedTime.HasValue);

            if(!check)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");
            }

            var bankAccount = new BankAccount
            {
                UserId = userId,
                BankName = request.BankName,
                BankNumber = request.BankNumber,
            };

            await _unitOfWork.GetRepository<BankAccount>().InsertAsync(bankAccount);
            await _unitOfWork.SaveAsync();

            return bankAccount.ToBankAccountDto();
        }

        public async Task DeleteAsync(string bankAccountId)
        {
            var bankAccount = await _unitOfWork.GetRepository<BankAccount>().GetByIdAsync(bankAccountId)
                         ?? throw new ErrorException(StatusCodes.Status404NotFound, "Bank account not found");

            bankAccount.DeletedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<BankAccount>().UpdateAsync(bankAccount);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<BankAccountResponse>> GetByUserIdAsync(string userId)
        {
            var bankAccounts = await _unitOfWork.GetRepository<BankAccount>().Entities
            .Where(x => x.UserId == userId && !x.DeletedTime.HasValue)
            .ToListAsync();

            return bankAccounts.ToBankAccountDtoList();
        }

        public async Task<BankAccountResponse> UpdateAsync(string userId, string bankAccountId, UpdateBankAccountRequest request)
        {
            var bankAccount = await _unitOfWork.GetRepository<BankAccount>().GetByIdAsync(bankAccountId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, "Bank account not found");

            if(bankAccount.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "Bank account has already deleted");
            }

            if (bankAccount.UserId != userId)
            {
                throw new ErrorException(StatusCodes.Status403Forbidden, "You are not authorized to update this bank account.");
            }

            bankAccount.BankName = request.BankName;
            bankAccount.BankNumber = request.BankNumber;

            await _unitOfWork.GetRepository<BankAccount>().UpdateAsync(bankAccount);
            await _unitOfWork.SaveAsync();

            return bankAccount.ToBankAccountDto();
        }
    }
}
