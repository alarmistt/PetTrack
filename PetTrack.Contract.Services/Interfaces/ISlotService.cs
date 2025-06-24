using PetTrack.Entity;
using PetTrack.ModelViews.SlotModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface ISlotService
    {
        Task<List<SlotResponse>> GetSlotsByClinicIdAsync(string clinicId);
        Task GenerateSlotsFromClinicScheduleAsync(string clinicId);
        Task SyncSlotsAfterScheduleUpdatedAsync(ClinicSchedule updatedSchedule);
        Task RemoveSlotsAfterScheduleDeletedAsync(ClinicSchedule deletedSchedule);
        Task<List<CheckSlotReponse>> CheckExistSlotAsync(string clinicId, DateTime AppointmentDate);
    }

}
