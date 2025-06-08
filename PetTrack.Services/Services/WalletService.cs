using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Helpers;
using PetTrack.Entity;
using PetTrack.ModelViews.Mappers;
using PetTrack.ModelViews.WalletModels;

namespace PetTrack.Services.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContext;

        public WalletService(IUnitOfWork unitOfWork, IUserContextService userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;

        }
        public async Task<WalletResponse> CreateWalletAsync(CreateWalletRequest request)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.UserId)
                ?? throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "User not found");

            if (user.DeletedTime.HasValue)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "User has already deleted");

            var existingWallet = await _unitOfWork.GetRepository<Wallet>().FindAsync(w =>
                w.UserId == request.UserId && !w.DeletedTime.HasValue);

            if (existingWallet != null)
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.FAILED, "Wallet already exists");

            var wallet = new Wallet
            {
                UserId = user.Id,
                Balance = 0
            };

            await _unitOfWork.GetRepository<Wallet>().InsertAsync(wallet);
            await _unitOfWork.SaveAsync();

            return wallet.ToWalletDto();
        }

        public async Task CreateWalletIfNotExistsAsync(string userId)
        {

            var exists = await _unitOfWork.GetRepository<Wallet>().Entities.AnyAsync(w => w.UserId == userId && !w.DeletedTime.HasValue);
            if (exists)
                return;

            var wallet = new Wallet
            {
                UserId = userId,
                Balance = 0
            };

            await _unitOfWork.GetRepository<Wallet>().InsertAsync(wallet);
            await _unitOfWork.SaveAsync();
        }

        public async Task<WalletResponse> GetMyWalletAsync()
        {
            var userId = _userContext.GetUserId();

            var wallet = await _unitOfWork.GetRepository<Wallet>()
                .FindAsync(w => w.UserId == userId && !w.DeletedTime.HasValue);

            if (wallet == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Wallet not found or can be deleted");

            return wallet.ToWalletDto();
        }

        public async Task<WalletResponse> GetWalletByIdAsync(string walletId)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().GetByIdAsync(walletId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Wallet not found");

            if(wallet.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.FAILED, "Wallet has already deleted");
            }

            return wallet.ToWalletDto();
        }


        public async Task<WalletResponse> UpdateBalanceAsync(string walletId, UpdateWalletRequest request)
        {
            var wallet = await _unitOfWork.GetRepository<Wallet>().GetByIdAsync(walletId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Wallet not found");

            if (wallet.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.FAILED, "Wallet has already deleted");
            }

            wallet.Balance = request.Balance;
            wallet.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
            await _unitOfWork.SaveAsync();

            return wallet.ToWalletDto();
        }
    }
}
