using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.Controllers
{
    [Route("api/admin/wallet-transactions")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminWalletTransactionController : ControllerBase
    {
        private readonly IWalletTransactionService _walletTransactionService;

        public AdminWalletTransactionController(IWalletTransactionService walletTransactionService)
        {
            _walletTransactionService = walletTransactionService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedTransactions([FromQuery] WalletTransactionQueryObject query)
        {
            var result = await _walletTransactionService.GetPagedTransactionsAsync(query);
            return Ok(BaseResponseModel<BasePaginatedList<WalletTransactionResponse>>.OkDataResponse(result, "Paged transactions retrieved successfully"));
        }
    }
}
