using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.SlotModels;

namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        /// <summary>
        /// Get slots of a clinic
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetByClinic(string clinicId)
        {
            var result = await _slotService.GetSlotsByClinicIdAsync(clinicId);
            return Ok(BaseResponseModel<List<SlotResponse>>.OkDataResponse(result, "Slots retrieved successfully"));
        }
    }
}
