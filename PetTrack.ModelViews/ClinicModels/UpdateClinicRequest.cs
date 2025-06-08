using PetTrack.ModelViews.ClinicScheduleModels;
using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.ModelViews.ClinicModels
{
    public class UpdateClinicRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Slogan { get; set; }
        public string Description { get; set; }
        public string? BannerUrl { get; set; }
        //public List<UpdateClinicScheduleRequest> Schedules { get; set; }
        //public List<UpdateServicePackageRequest> ServicePackages { get; set; }
    }
}
