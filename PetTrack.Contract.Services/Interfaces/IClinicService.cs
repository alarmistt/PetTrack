using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.ModelViews.ClinicModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IClinicService
    {
        Task<BasePaginatedList<ClinicResponse>> GetPagedClinicsAsync(ClinicQueryObject query);
        Task<ClinicResponse> CreateClinicAsync(CreateClinicRequest request);
        Task<ClinicResponse> UpdateClinicAsync(string id, UpdateClinicRequest request);
        Task DeleteClinicAsync(string clinicId);
        Task RestoreClinicAsync(string clinicId);
        Task ApproveClinicAsync(string clinicId);
        Task RejectClinicAsync(string clinicId);
        Task<ClinicResponse> GetClinicByIdAsync(string id);
        Task<List<ClinicResponse>> GetAllApprovedClinicsAsync();
    }
}
