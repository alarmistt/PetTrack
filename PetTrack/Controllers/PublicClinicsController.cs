using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.ClinicModels;
using PetTrack.ModelViews.ClinicScheduleModels;
using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.Controllers
{
    [Route("api/public/clinics")]
    [ApiController]
    public class PublicClinicsController : ControllerBase
    {
        private readonly IClinicService _clinicService;
        private readonly IClinicScheduleService _scheduleService;
        private readonly IServicePackageService _servicePackageService;

        public PublicClinicsController(IClinicService clinicService, IServicePackageService servicePackageService, IClinicScheduleService scheduleService)
        {
            _clinicService = clinicService;
            _servicePackageService = servicePackageService;
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Retrieves details of a specific clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to retrieve.</param>
        /// <returns>The clinic's detailed information.</returns>
        [HttpGet("{clinicId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClinicById(string clinicId)
        {
            var result = await _clinicService.GetClinicByIdAsync(clinicId);
            return Ok(BaseResponseModel<ClinicResponse>.OkDataResponse(result, "Clinic retrieved successfully"));
        }

        /// <summary>
        /// Retrieves all approved clinics.
        /// </summary>
        /// <returns>A list of all approved clinics.</returns>
        [HttpGet("approved")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllApprovedClinics()
        {
            var result = await _clinicService.GetAllApprovedClinicsAsync();
            return Ok(BaseResponseModel<List<ClinicResponse>>.OkDataResponse(result, "Approved clinic list retrieved successfully"));
        }

        /// <summary>
        /// Retrieves all service packages offered by a specific clinic.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic.</param>
        /// <returns>A list of service packages for the clinic.</returns>
        [HttpGet("{clinicId}/service-packages")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServicePackageById(string clinicId)
        {
            var result = await _servicePackageService.GetPackagesByClinicAsync(clinicId);
            return Ok(BaseResponseModel<List<ServicePackageResponse>>.OkDataResponse(result, "Service packages retrieved sucessfully"));
        }

        /// <summary>
        /// Retrieves all working schedules of a specific clinic.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic.</param>
        /// <returns>A list of working schedules for the clinic.</returns>
        [HttpGet("{clinicId}/schedules")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSchedulesById(string clinicId)
        {
            var result = await _scheduleService.GetSchedulesByClinicAsync(clinicId);
            return Ok(BaseResponseModel<List<ClinicScheduleResponse>>.OkDataResponse(result, "Schedules retrieved sucessfully"));
        }
    }
}
