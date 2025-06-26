using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;
using PetTrack.ModelViews.Payment;
using PetTrack.ModelViews.TopUpModels;


namespace PetTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymenService;
        private readonly ITopUpTransactionService _topUpTransactionService;
        public PaymentController(IPaymentService paymentService, ITopUpTransactionService topUpTransactionService)
        {
            _paymenService = paymentService;
            _topUpTransactionService = topUpTransactionService;
        }
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentLinkRequest request)

        {
            var paymentIntent = await _paymenService.CreateLinkAsync(request);
            return Ok(BaseResponseModel<CreatePaymentResult>.OkDataResponse(paymentIntent, "Get data successful"));
        }
        [HttpPost("create-booking-payment")]
        public async Task<IActionResult> CreateBookingPayment([FromBody] CreateLinkBookingRequest request)

        {
            var paymentIntent = await _paymenService.CreateLinkBookingAsync(request);
            return Ok(BaseResponseModel<CreatePaymentResult>.OkDataResponse(paymentIntent, "Get data successful"));
        }
        [HttpPost("check-status-transaction")]
        public async Task<IActionResult> CheckStatusTransaction(string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
            {
                return BadRequest(BaseResponseModel<string>.BadRequestResponseModel("Transaction code is required"));
            }
            try
            {
                await _topUpTransactionService.CheckStatusTransactionAsync(orderCode);
                return Ok(BaseResponseModel<string>.OkMessageResponseModel("Transaction status checked successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponseModel<string>.InternalErrorResponseModel($"An error occurred: {ex.Message}"));
            }
        }
        [HttpGet("get-history-transaction")]
       public async Task<IActionResult> GetTopUpTransactionAsync(int pageIndex = 1, int pageSize =10, string? userId = null, string? status = null)
        {
            return Ok(BaseResponseModel<TopUpResponse>
                .OkDataResponse( await _topUpTransactionService.GetTopUpTransaction(pageIndex,pageSize,userId,status), "Get data successful"));
        }

    }

}
