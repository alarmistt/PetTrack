using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.WalletModels;

namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        /// Retrieves a wallet by its unique wallet ID.
        /// Only accessible by Admin.
        /// </summary>
        /// <param name="walletId">The unique identifier of the wallet.</param>
        /// <returns>A wallet response if found; otherwise, a not found response.</returns>
        [HttpGet("{walletId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWalletById(string walletId)
        {
            var result = await _walletService.GetWalletByIdAsync(walletId);
            return Ok(BaseResponseModel<WalletResponse>.OkDataResponse(result, "Wallet retrieved successfully"));
        }

        /// <summary>
        /// Retrieves the wallet of the currently authenticated user (User, Clinic, or Admin).
        /// </summary>
        /// <returns>The wallet associated with the logged-in user.</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyWallet()
        {
            var result = await _walletService.GetMyWalletAsync();
            return Ok(BaseResponseModel<WalletResponse>.OkDataResponse(result, "Your wallet retrieved successfully"));
        }

        /// <summary>
        /// Creates a wallet for a specific user.
        /// Only accessible by Admins.
        /// </summary>
        /// <param name="request">The wallet creation request containing the target user's ID.</param>
        /// <returns>The newly created wallet.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request)
        {
            var result = await _walletService.CreateWalletAsync(request);
            return Ok(BaseResponseModel<WalletResponse>.OkDataResponse(result, "Wallet created successfully"));
        }
    }
}
