using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;
using PetTrack.ModelViews.Payment;


namespace PetTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymenService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymenService = paymentService;
        }
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentLinkRequest request)

        {
            var paymentIntent = await _paymenService.CreateLinkAsync(request);
            return Ok(BaseResponseModel<CreatePaymentResult>.OkDataResponse(paymentIntent, "Get data successful"));
        }

    }

}
