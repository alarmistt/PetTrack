using PetTrack.Entity;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IDomainHelperService
    {
        Task<Clinic> GetActiveClinicByIdAsync(string clinicId);
        Task<Clinic> EnsureUserOwnsClinicAsync(string clinicId, string userId);
        Task<ServicePackage> EnsureUserOwnsPackageAsync(string packageId, string userId);
        Task<ClinicSchedule> EnsureUserOwnsScheduleAsync(string scheduleId, string userId);
        Task<Clinic> GetOwnedClinicWithDetailsAsync(string clinicId, string userId);

    }
}
