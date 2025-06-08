using PetTrack.Entity;
using PetTrack.ModelViews.ClinicModels;
using PetTrack.ModelViews.ClinicScheduleModels;
using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class ClinicMapper
    {
        public static ClinicResponse ToClinicDto(this Clinic clinic)
        {
            return new ClinicResponse
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber,
                Slogan = clinic.Slogan,
                Description = clinic.Description,
                BannerUrl = clinic.BannerUrl,
                Status = clinic.Status,
                CreatedTime = clinic.CreatedTime,
                OwnerUserId = clinic.OwnerUserId,
                OwnerFullName = clinic.Owner?.FullName,
                Schedules = clinic.Schedules?.Select(s => new ClinicScheduleResponse
                {
                    DayOfWeek = s.DayOfWeek,
                    OpenTime = s.OpenTime,
                    CloseTime = s.CloseTime
                }).ToList() ?? new List<ClinicScheduleResponse>(),
                ServicePackages = clinic.ServicePackages?.Select(p => new ServicePackageResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                }).ToList() ?? new List<ServicePackageResponse>()
            };
        }

        public static List<ClinicResponse> ToClinicDtoList(this IEnumerable<Clinic> clinics)
        {
            return clinics.Select(c => c.ToClinicDto()).ToList();
        }
    }
}
