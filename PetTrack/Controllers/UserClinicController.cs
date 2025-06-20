using Google.Api.Gax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.ClinicModels;
using PetTrack.ModelViews.ClinicScheduleModels;
using PetTrack.ModelViews.ServicePackageModels;
using PetTrack.ModelViews.WalletTransactionModels;
using PetTrack.Services.Infrastructure;

namespace PetTrack.Controllers
{
    [Route("api/user/clinics")]
    [ApiController]
    public class UserClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;
        private readonly IClinicScheduleService _scheduleService;
        private readonly IServicePackageService _servicePackageService;
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly IUserContextService _userContextService;

        public UserClinicController(IClinicService clinicService, IServicePackageService servicePackageService, IClinicScheduleService scheduleService, IWalletTransactionService walletTransactionService, IUserContextService userContextService)
        {
            _clinicService = clinicService;
            _servicePackageService = servicePackageService;
            _scheduleService = scheduleService;
            _walletTransactionService = walletTransactionService;
            _userContextService = userContextService;
        }

        #region CLINIC
        /// <summary>
        /// Creates a new clinic registration request.
        /// </summary>
        /// <param name="request">The details of the clinic to register.</param>
        /// <returns>Returns the newly created clinic's information.</returns>
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateClinic([FromBody] CreateClinicRequest request)
        {
            var result = await _clinicService.CreateClinicAsync(request);
            return Ok(BaseResponseModel<ClinicResponse>.OkDataResponse(result, "Created clinic successfully"));
        }

        /// <summary>
        /// Updates a clinic registration before it is approved.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to update.</param>
        /// <param name="request">The updated clinic information.</param>
        /// <returns>Returns the updated clinic details.</returns>
        [HttpPut("{clinicId}")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> UpdateClinic(string clinicId, [FromBody] UpdateClinicRequest request)
        {
            var result = await _clinicService.UpdateClinicAsync(clinicId, request);
            return Ok(BaseResponseModel<ClinicResponse>.OkDataResponse(result, "Updated clinic successfully"));
        }
        #endregion

        #region SCHEDULES
        /// <summary>
        /// Creates a new schedule for the specified clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to add the schedule to.</param>
        /// <param name="request">The schedule details to be created.</param>
        /// <returns>Returns the newly created schedule.</returns>
        [HttpPost("{clinicId}/schedules")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> CreateSchedule(string clinicId, [FromBody] CreateClinicScheduleRequest request)
        {
            var result = await _scheduleService.CreateScheduleAsync(clinicId, request);
            return Ok(BaseResponseModel<ClinicScheduleResponse>.OkDataResponse(result, "Created schedule sucessfully"));
        }

        /// <summary>
        /// Updates an existing clinic schedule.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule to update.</param>
        /// <param name="request">The updated schedule information.</param>
        /// <returns>Returns the updated schedule details.</returns>
        [HttpPut("schedules/{scheduleId}")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> UpdateSchedule(string scheduleId, [FromBody] UpdateClinicScheduleRequest request)
        {
            var result = await _scheduleService.UpdateScheduleAsync(scheduleId, request);
            return Ok(BaseResponseModel<ClinicScheduleResponse>.OkDataResponse(result, "Updated schedule sucessfully"));
        }

        /// <summary>
        /// Deletes a schedule by its ID.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule to delete.</param>
        /// <returns>Confirmation of successful deletion.</returns>
        [HttpDelete("schedules/{scheduleId}")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> DeleteSchedule(string scheduleId)
        {
            await _scheduleService.DeleteScheduleAsync(scheduleId);
            return Ok(BaseResponse.OkMessageResponse("Deleted schedule sucessfully"));
        }
        #endregion

        #region PACKAGES
        /// <summary>
        /// Creates a new service package for a specific clinic.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic.</param>
        /// <param name="request">The details of the service package to create.</param>
        /// <returns>Returns the newly created service package information.</returns>
        [HttpPost("{clinicId}/service-package")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> CreateServicePackage(string clinicId, [FromBody] CreateServicePackageRequest request)
        {
            var result = await _servicePackageService.CreateServicePackageAsync(clinicId, request);
            return Ok(BaseResponseModel<ServicePackageResponse>.OkDataResponse(result, "Created service package successfully"));
        }

        /// <summary>
        /// Updates an existing service package.
        /// </summary>
        /// <param name="packageId">The unique identifier of the service package to update.</param>
        /// <param name="request">The updated service package information.</param>
        /// <returns>Returns the updated service package details.</returns>
        [HttpPut("service-packages/{packageId}")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> UpdateServicePackage(string packageId, [FromBody] UpdateServicePackageRequest request)
        {
            var result = await _servicePackageService.UpdateServicePackageAsync(packageId, request);
            return Ok(BaseResponseModel<ServicePackageResponse>.OkDataResponse(result, "Updated service package successfully"));
        }

        /// <summary>
        /// Deletes an existing service package.
        /// </summary>
        /// <param name="packageId">The unique identifier of the service package to delete.</param>
        /// <returns>Confirmation of deletion.</returns>
        [HttpDelete("service-packages/{packageId}")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> DeleteServicePackage(string packageId)
        {
            await _servicePackageService.DeleteServicePackageAsync(packageId);
            return Ok(BaseResponse.OkMessageResponse("Deleted service package sucessfully"));
        }
        #endregion

        #region WALLET / WITHDRAW
        /// <summary>
        /// Submits a withdrawal request from the clinic's wallet.
        /// </summary>
        /// <param name="request">Withdrawal information.</param>
        /// <returns>Created withdrawal transaction (pending status).</returns>
        [HttpPost("withdraw")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> RequestWithdraw([FromBody] WithdrawRequest request)
        {
            var userId = _userContextService.GetUserId();
            var result = await _walletTransactionService.RequestWithdrawAsync(userId, request);
            return Ok(BaseResponseModel<WithdrawResponse>.OkDataResponse(result, "Withdrawal request submitted successfully"));
        }
        #endregion
    }
}
