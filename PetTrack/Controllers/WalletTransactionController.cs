using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.Controllers
{
    [Route("api/wallet-transactions")]
    [ApiController]
    [Authorize]
    public class WalletTransactionController : ControllerBase
    {
        private readonly IWalletTransactionService _transactionService;

        public WalletTransactionController(IWalletTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Retrieves all wallet transactions associated with the specified user ID.
        /// Accessible by authenticated users including Admin, Clinic, and User.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the wallet.</param>
        /// <returns>List of wallet transactions.</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var result = await _transactionService.GetByUserIdAsync(userId);
            return Ok(BaseResponseModel<List<WalletTransactionResponse>>.OkDataResponse(result, "Transactions retrieved successfully"));
        }

        /// <summary>
        /// Retrieves the details of a specific wallet transaction by transaction ID.
        /// Accessible by authenticated users including Admin, Clinic, and User.
        /// </summary>
        /// <param name="id">The ID of the wallet transaction.</param>
        /// <returns>The wallet transaction details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _transactionService.GetByIdAsync(id);
            return Ok(BaseResponseModel<WalletTransactionResponse>.OkDataResponse(result, "Transaction retrieved successfully"));
        }
    }
}
