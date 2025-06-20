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

        /// <summary>
        /// Retrieves a paginated list of withdrawal transactions with optional filters.
        /// </summary>
        /// <param name="query">
        /// The query parameters used for filtering, sorting, and pagination of withdrawal transactions.
        /// <br/><br/>
        /// <b>WalletId</b>: Filter by a specific wallet ID.  
        /// <b>UserId</b>: Filter by the owner of the wallet.  
        /// <b>Status</b>: Filter by transaction status.  
        /// <br/>
        /// Allowed values:
        /// <ul>
        /// <li><c>0</c> = <b>Pending</b></li>
        /// <li><c>1</c> = <b>Completed</b></li>
        /// <li><c>2</c> = <b>Rejected</b></li>
        /// </ul>
        /// <b>FromDate</b>: Start of transaction date range (inclusive).  
        /// <b>ToDate</b>: End of transaction date range (inclusive).  
        /// <br/>
        /// <b>SortBy</b>: (inherited from BaseQueryObject) Field to sort by, e.g., <c>createdTime</c>, <c>amount</c>.  
        /// <b>IsDescending</b>: (inherited from BaseQueryObject) Whether sorting should be descending.  
        /// <b>PageIndex</b>: (inherited from BaseQueryObject) Page number (starting from 1).  
        /// <b>PageSize</b>: (inherited from BaseQueryObject) Number of items per page.  
        /// </param>
        /// <returns>
        /// A paginated list of wallet withdrawal transactions that match the query filters.
        /// </returns>
        [HttpGet("withdraws")]
        public async Task<IActionResult> GetPagedWithdraws([FromQuery] WithdrawQueryObject query)
        {
            var result = await _walletTransactionService.GetPagedWithdrawsAsync(query);
            return Ok(BaseResponseModel<BasePaginatedList<WalletTransactionResponse>>.OkDataResponse(result, "Paged withdraw requests retrieved successfully"));
        }

        /// <summary>
        /// Approves a pending withdraw transaction and deducts the balance from the associated wallet.
        /// </summary>
        /// <param name="transactionId">The ID of the withdraw transaction to approve.</param>
        /// <returns>Confirmation message upon successful approval.</returns>
        [HttpPut("withdraws/{transactionId}/approve")]
        public async Task<IActionResult> ApproveWithdraw(string transactionId)
        {
            await _walletTransactionService.ApprovedWithdrawAsync(transactionId);
            return Ok(BaseResponse.OkMessageResponse( "Withdraw approved successfully"));
        }

        /// <summary>
        /// Rejects a pending withdraw transaction without deducting any balance.
        /// </summary>
        /// <param name="transactionId">The ID of the withdraw transaction to reject.</param>
        /// <returns>Confirmation message upon successful rejection.</returns>
        [HttpPut("withdraws/{transactionId}/reject")]
        public async Task<IActionResult> RejectWithdraw(string transactionId)
        {
            await _walletTransactionService.RejectWithdrawAsync(transactionId);
            return Ok(BaseResponse.OkMessageResponse("Withdraw rejected successfully"));
        }

        /// <summary>
        /// Retrieves a paginated list of wallet transactions with optional filters.
        /// </summary>
        /// <param name="query">
        /// Query parameters for filtering and pagination:
        /// <br/><br/>
        /// <b>WalletId</b>: Filter by a specific wallet ID.  
        /// <b>UserId</b>: Filter transactions for wallets owned by the specified user.  
        /// <b>Type</b>: Filter by transaction type.  
        /// <br/>
        /// Allowed values:
        /// <ul>
        /// <li><c>0</c> = <b>TopUp</b></li>
        /// <li><c>1</c> = <b>BookingPayment</b></li>
        /// <li><c>2</c> = <b>Refund</b></li>
        /// <li><c>3</c> = <b>ComissionFee</b></li>
        /// <li><c>4</c> = <b>ReceiveAmount</b></li>
        /// <li><c>5</c> = <b>Withdraw</b></li>
        /// </ul>
        /// <b>FromDate</b>: Start of date range filter (inclusive).  
        /// <b>ToDate</b>: End of date range filter (inclusive).  
        /// <b>SortBy</b>: Field to sort by (e.g., <c>createdTime</c>, <c>amount</c>).  
        /// <b>IsDescending</b>: Whether sorting is in descending order.  
        /// <b>PageIndex</b>: Page number (1-based).  
        /// <b>PageSize</b>: Number of items per page.  
        /// </param>
        /// <returns>Paginated result containing wallet transactions that match the filters.</returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetAllTransactions([FromQuery] WalletTransactionQueryObject query)
        {
            var result = await _walletTransactionService.GetPagedTransactionsAsync(query);
            return Ok(BaseResponseModel<BasePaginatedList<WalletTransactionResponse>>.OkDataResponse(result, "Paged transactions retrieved successfully"));
        }

        /// <summary>
        /// Retrieves details of a wallet transaction by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet transaction.</param>
        /// <returns>Detailed information about the wallet transaction.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _walletTransactionService.GetByIdAsync(id);
            return Ok(BaseResponseModel<WalletTransactionResponse>.OkDataResponse(result, "Transaction retrieved successfully"));
        }
    }
}
