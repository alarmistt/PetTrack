using Microsoft.AspNetCore.Mvc;
using PetTrack.ModelViews.Payment;
using PetTrack.Services.Services;


namespace PetTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PayOSService _payOSService;

        public PaymentController(PayOSService payOSService)
        {
            _payOSService = payOSService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestModel model)
        {
            try
            {
                var url = await _payOSService.CreatePaymentLinkAsync(model.Amount, model.Description, model.ReturnUrl);
                return Ok(new { checkoutUrl = url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

}
