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
        /// <summary>
        /// check slot 1 ngày bất kì của 1 phòng khám -- Status Inactive là đã có người đặt
        /// </summary>
        /// <param name="clinicId">Nhập Id phòng khám</param>
        /// <param name="apointmentDate">Nhập ngày muốn check (format yyyy-MM-dd)</param>
        /// <returns></returns>
        [HttpGet("check-slot")]
        public async Task<IActionResult> CheckExistSlotClinic(string clinicId, [FromQuery] string apointmentDate)
        {
            if (!DateTime.TryParseExact(apointmentDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var date))
            {
                return BadRequest("apointmentDate have to format yyyy-MM-dd (VD: 2025-06-24)");
            }
            var result = await _slotService.CheckExistSlotAsync(clinicId,date);
            return Ok(BaseResponseModel<List<CheckSlotReponse>>.OkDataResponse(result, "Slots retrieved successfully"));
        }
    }
}
