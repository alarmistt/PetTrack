using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Helpers;
using PetTrack.Entity;
using PetTrack.ModelViews.Mappers;
using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.Services.Services
{
    public class ServicePackageService : IServicePackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IDomainHelperService _helperService;

        public ServicePackageService(
            IUnitOfWork unitOfWork,
            IUserContextService userContextService,
            IDomainHelperService helperService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _helperService = helperService;
        }

        public async Task<ServicePackageResponse> CreateServicePackageAsync(string clinicId, CreateServicePackageRequest request)
        {
            var currentUserId = _userContextService.GetUserId();
            var clinic = await _helperService.EnsureUserOwnsClinicAsync(clinicId, currentUserId);

            var package = new ServicePackage
            {
                ClinicId = clinic.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            await _unitOfWork.GetRepository<ServicePackage>().InsertAsync(package);
            await _unitOfWork.SaveAsync();

            return package.ToPackageDto();
        }

        public async Task<ServicePackageResponse> UpdateServicePackageAsync(string packageId, UpdateServicePackageRequest request)
        {
            var currentUserId = _userContextService.GetUserId();
            var package = await _helperService.EnsureUserOwnsPackageAsync(packageId, currentUserId);

            package.Name = request.Name;
            package.Description = request.Description;
            package.Price = request.Price;
            package.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<ServicePackage>().UpdateAsync(package);
            await _unitOfWork.SaveAsync();

            return package.ToPackageDto();
        }

        public async Task DeleteServicePackageAsync(string packageId)
        {
            var currentUserId = _userContextService.GetUserId();
            var package = await _helperService.EnsureUserOwnsPackageAsync(packageId, currentUserId);

            package.DeletedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<ServicePackage>().UpdateAsync(package);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<ServicePackageResponse>> GetPackagesByClinicAsync(string clinicId)
        {
           var clinic = await _helperService.GetActiveClinicByIdAsync(clinicId);

            var packages = await _unitOfWork.GetRepository<ServicePackage>()
                .FindListAsync(p => p.ClinicId == clinic.Id && !p.DeletedTime.HasValue);

            return packages.ToPackageDtoList();
        }
    }
}
