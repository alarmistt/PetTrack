using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.ModelViews.ClinicModels;

namespace PetTrack.Controllers
{
    [Route("api/admin/clinics")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public AdminClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        /// <summary>
        /// Retrieves a paginated list of clinics with optional filtering and sorting.
        /// </summary>
        /// <param name="query">
        /// The query parameters used for filtering, sorting, and pagination.
        ///
        /// <br/><br/>
        /// <b>Allowed values for <c>Status</c>:</b>
        /// - <c>0</c> = <b>Pending</b>  
        /// - <c>1</c> = <b>Approved</b>  
        /// - <c>2</c> = <b>Rejected</b>  
        /// </param>
        /// <returns>A paginated list of clinic records matching the criteria.</returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedClinics([FromQuery] ClinicQueryObject query)
        {
            var result = await _clinicService.GetPagedClinicsAsync(query);
            return Ok(BaseResponseModel<BasePaginatedList<ClinicResponse>>.OkDataResponse(result, "Clinic list retrieved successfully"));
        }


        /// <summary>
        /// Approves a pending clinic registration request.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to approve.</param>
        /// <returns>
        /// Confirmation message upon successful approval.
        /// </returns>
        [HttpPut("{clinicId}/approve")]
        public async Task<IActionResult> ApproveClinic(string clinicId)
        {
            await _clinicService.ApproveClinicAsync(clinicId);
            return Ok(BaseResponse.OkMessageResponse("Clinic approved successfully."));
        }

        /// <summary>
        /// Rejects a pending clinic registration request.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to reject.</param>
        /// <returns>
        /// Confirmation message upon successful rejection.
        /// </returns>
        [HttpPut("{clinicId}/reject")]
        public async Task<IActionResult> RejectClinic(string clinicId)
        {
            await _clinicService.RejectClinicAsync(clinicId);
            return Ok(BaseResponse.OkMessageResponse("Clinic rejected successfully."));
        }

        /// <summary>
        /// Soft-deletes a clinic. Only accessible to Clinic role.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to delete.</param>
        /// <returns>
        /// Confirmation message upon successful deletion.
        /// </returns>
        [HttpDelete("{clinicId}")]
        public async Task<IActionResult> DeleteClinic(string clinicId)
        {
            await _clinicService.DeleteClinicAsync(clinicId);
            return Ok(BaseResponse.OkMessageResponse("Clinic deleted successfully."));
        }

        /// <summary>
        /// Restores a previously soft-deleted clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to restore.</param>
        /// <returns>
        /// Confirmation message upon successful restoration.
        /// </returns>
        [HttpPost("{clinicId}/restore")]
        public async Task<IActionResult> RestoreClinic(string clinicId)
        {
            await _clinicService.RestoreClinicAsync(clinicId);
            return Ok(BaseResponse.OkMessageResponse("Clinic restored successfully."));
        }
    }
}
