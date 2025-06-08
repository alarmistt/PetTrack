using PetTrack.Entity;
using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class ServicePackageMapper
    {
        public static ServicePackageResponse ToPackageDto(this ServicePackage servicePackage)
        {
            return new ServicePackageResponse
            {
                Id = servicePackage.Id,
                Name = servicePackage.Name,
                Description = servicePackage.Description,
                Price = servicePackage.Price,
            };
        }

        public static List<ServicePackageResponse> ToPackageDtoList(this IEnumerable<ServicePackage> servicePackages)
        {
            return servicePackages.Select(p => p.ToPackageDto()).ToList();
        }
    }
}
