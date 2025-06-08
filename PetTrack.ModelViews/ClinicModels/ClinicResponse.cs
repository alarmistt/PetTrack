using PetTrack.ModelViews.ClinicScheduleModels;
using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.ModelViews.ClinicModels
{
    public class ClinicResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Slogan { get; set; }
        public string Description { get; set; }
        public string? BannerUrl { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

        public string OwnerUserId { get; set; }
        public string? OwnerFullName { get; set; }

        public List<ClinicScheduleResponse> Schedules { get; set; }
        public List<ServicePackageResponse> ServicePackages { get; set; }
    }
}
