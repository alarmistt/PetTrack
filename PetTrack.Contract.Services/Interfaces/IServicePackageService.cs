using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IServicePackageService
    {
        Task<ServicePackageResponse> CreateServicePackageAsync(string clinicId, CreateServicePackageRequest request);

        Task<ServicePackageResponse> UpdateServicePackageAsync(string packageId, UpdateServicePackageRequest request);

        Task DeleteServicePackageAsync(string packageId);

        Task<List<ServicePackageResponse>> GetPackagesByClinicAsync(string clinicId);
    }
}