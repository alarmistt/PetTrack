using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.BankAccountModels;

namespace PetTrack.Controllers
{
    /// <summary>
    /// Controller for managing bank accounts linked to user profiles.
    /// Supports operations such as listing, creating, updating, and deleting bank accounts.
    /// </summary>
    [Route("api/bank-accounts")]
    [ApiController]
    [Authorize(Roles = "Clinic")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly IUserContextService _userContextService;

        public BankAccountController(IBankAccountService bankAccountService, IUserContextService userContextService)
        {
            _bankAccountService = bankAccountService;
            _userContextService = userContextService;
        }

        /// <summary>
        /// Retrieves all bank accounts associated with the currently authenticated user.
        /// </summary>
        /// <returns>A list of bank accounts belonging to the current user.</returns>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserAccounts()
        {
            var userId = _userContextService.GetUserId();
            var result = await _bankAccountService.GetByUserIdAsync(userId);
            return Ok(BaseResponseModel<List<BankAccountResponse>>.OkDataResponse(result, "Bank accounts retrieved successfully"));
        }

        /// <summary>
        /// Retrieves all bank accounts for a specific user by their user ID.
        /// Restricted to admin or internal tool usage.
        /// </summary>
        /// <param name="userId">The ID of the user whose bank accounts are being retrieved.</param>
        /// <returns>A list of bank accounts associated with the specified user.</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var result = await _bankAccountService.GetByUserIdAsync(userId);
            return Ok(BaseResponseModel<List<BankAccountResponse>>.OkDataResponse(result, "Bank accounts retrieved successfully"));
        }

        /// <summary>
        /// Creates a new bank account for the currently authenticated user.
        /// </summary>
        /// <param name="request">The bank account details including bank name and account number.</param>
        /// <returns>The newly created bank account.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBankAccountRequest request)
        {
            var userId = _userContextService.GetUserId();
            var result = await _bankAccountService.CreateAsync(userId, request);
            return Ok(BaseResponseModel<BankAccountResponse>.OkDataResponse(result, "Bank account created successfully"));
        }

        /// <summary>
        /// Deletes a bank account by its ID.
        /// Only the account owner or an administrator has permission to delete.
        /// </summary>
        /// <param name="id">The ID of the bank account to delete.</param>
        /// <returns>The ID of the deleted bank account.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _bankAccountService.DeleteAsync(id);
            return Ok(BaseResponseModel<string>.OkDataResponse(id, "Bank account deleted successfully"));
        }

        /// <summary>
        /// Updates the details of an existing bank account.
        /// Only the account owner can perform this action.
        /// </summary>
        /// <param name="id">The ID of the bank account to update.</param>
        /// <param name="request">The updated bank account information.</param>
        /// <returns>The updated bank account details.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateBankAccountRequest request)
        {
            var userId = _userContextService.GetUserId();
            var result = await _bankAccountService.UpdateAsync(userId, id, request);
            return Ok(BaseResponseModel<BankAccountResponse>.OkDataResponse(result, "Bank account updated successfully"));
        }
    }
}
