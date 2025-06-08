using PetTrack.ModelViews.ClinicScheduleModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IClinicScheduleService
    {
        Task<ClinicScheduleResponse> CreateScheduleAsync(string clinicId, CreateClinicScheduleRequest request);
        Task<ClinicScheduleResponse> UpdateScheduleAsync(string scheduleId, UpdateClinicScheduleRequest request);
        Task DeleteScheduleAsync(string scheduleId);
        Task<List<ClinicScheduleResponse>> GetSchedulesByClinicAsync(string clinicId);
    }
}
