using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Enums;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.ClinicModels;
using PetTrack.ModelViews.Mappers;

namespace PetTrack.Services.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IDomainHelperService _helperService;
        private readonly IUserContextService _userContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISlotService _slotService;

        public ClinicService(IUnitOfWork unitOfWork, IUserContextService userContext, IDomainHelperService helperService, IAuthenticationService authenticationService, ISlotService slotService)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _helperService = helperService;
            _authenticationService = authenticationService;
            _slotService = slotService;
        }

        public async Task<ClinicResponse> CreateClinicAsync(CreateClinicRequest request)
        {
            string userId = _userContext.GetUserId();

            var clinic = new Clinic
            {
                Name = request.Name,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Slogan = request.Slogan,
                Description = request.Description,
                BannerUrl = request.BannerUrl,
                Status = ClinicStatus.Pending.ToString(),
                OwnerUserId = userId,
                Schedules = request.Schedules.Select(s => new ClinicSchedule
                {
                    DayOfWeek = s.DayOfWeek,
                    OpenTime = s.OpenTime,
                    CloseTime = s.CloseTime
                }).ToList(),
                ServicePackages = request.ServicePackages.Select(sp => new ServicePackage
                {
                    Name = sp.Name,
                    Description = sp.Description,
                    Price = sp.Price
                }).ToList()
            };

            await _unitOfWork.GetRepository<Clinic>().InsertAsync(clinic);
            await _unitOfWork.SaveAsync();

            return clinic.ToClinicDto();
        }

        public async Task<List<ClinicResponse>> GetAllApprovedClinicsAsync()
        {
            var clinics = await _unitOfWork.GetRepository<Clinic>().Entities
               .Where(c => c.Status == ClinicStatus.Approved.ToString() && !c.DeletedTime.HasValue)
               .Include(c => c.Schedules)
               .Include(c => c.ServicePackages)
               .ToListAsync();

            return clinics.ToClinicDtoList();
        }

        public async Task<ClinicResponse> GetClinicByIdAsync(string id)
        {
            var clinic = await _unitOfWork.GetRepository<Clinic>().Entities
                .Include(c => c.Schedules)
                .Include(c => c.ServicePackages)
                .FirstOrDefaultAsync(c => c.Id == id && !c.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Clinic not found");

            return clinic.ToClinicDto();
        }

        public async Task ApproveClinicAsync(string clinicId)
        {
            var clinic = await _helperService.GetActiveClinicByIdAsync(clinicId);

            clinic.Status = ClinicStatus.Approved.ToString();
            clinic.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<Clinic>().UpdateAsync(clinic);
            await _unitOfWork.SaveAsync();

            await _authenticationService.UpdateRoleClinicAsync(clinic.OwnerUserId);

            await _slotService.GenerateSlotsFromClinicScheduleAsync(clinicId);
        }


        public async Task RejectClinicAsync(string clinicId)
        {
            var clinic = await _helperService.GetActiveClinicByIdAsync(clinicId);

            clinic.Status = ClinicStatus.Rejected.ToString();
            clinic.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<Clinic>().UpdateAsync(clinic);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ClinicResponse> UpdateClinicAsync(string clinicId, UpdateClinicRequest request)
        {
            var ownerId = _userContext.GetUserId();
            var clinic =  await _helperService.GetOwnedClinicWithDetailsAsync(clinicId, ownerId);

            clinic.Name = request.Name;
            clinic.Address = request.Address;
            clinic.PhoneNumber = request.PhoneNumber;
            clinic.Slogan = request.Slogan;
            clinic.Description = request.Description;
            clinic.BannerUrl = request.BannerUrl;

            //clinic.Schedules.Clear();
            //clinic.Schedules = request.Schedules.Select(s => new ClinicSchedule
            //{
            //    ClinicId = clinic.Id,
            //    DayOfWeek = s.DayOfWeek,
            //    OpenTime = s.OpenTime,
            //    CloseTime = s.CloseTime
            //}).ToList();

            //clinic.ServicePackages.Clear();
            //clinic.ServicePackages = request.ServicePackages.Select(sp => new ServicePackage
            //{
            //    Id = sp.Id,
            //    ClinicId = clinic.Id,
            //    Name = sp.Name,
            //    Description = sp.Description,
            //    Price = sp.Price
            //}).ToList();

            clinic.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<Clinic>().UpdateAsync(clinic);
            await _unitOfWork.SaveAsync();

            return clinic.ToClinicDto();
        }

        public async Task DeleteClinicAsync(string clinicId)
        {
            var clinic = await _helperService.GetActiveClinicByIdAsync(clinicId);

            clinic.DeletedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<Clinic>().UpdateAsync(clinic);
            await _unitOfWork.SaveAsync();
        }


        public async Task RestoreClinicAsync(string clinicId)
        {
            var clinic = await _helperService.GetActiveClinicByIdAsync(clinicId);

            clinic.DeletedTime = null;
            clinic.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<Clinic>().UpdateAsync(clinic);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<ClinicResponse>> GetPagedClinicsAsync(ClinicQueryObject query)
        {
            var clinicsQuery = _unitOfWork.GetRepository<Clinic>().Entities
                .Include(c => c.Owner)
                .Include(c => c.Schedules)
                .Include(c => c.ServicePackages)
                .Where(c => !c.DeletedTime.HasValue);

            if (!string.IsNullOrWhiteSpace(query.Name))
                clinicsQuery = clinicsQuery.Where(c => c.Name.Contains(query.Name));

            if (!string.IsNullOrWhiteSpace(query.Address))
                clinicsQuery = clinicsQuery.Where(c => c.Address.Contains(query.Address));

            if (!string.IsNullOrWhiteSpace(query.OwnerUserId))
                clinicsQuery = clinicsQuery.Where(c => c.OwnerUserId == query.OwnerUserId);

            if (query.Status.HasValue)
                clinicsQuery = clinicsQuery.Where(c => c.Status == query.Status.Value.ToString());

            if (query.IncludedDeleted)
            {
                clinicsQuery = clinicsQuery.Where(c => c.DeletedTime.HasValue);
            }
            // Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                clinicsQuery = query.SortBy?.ToLower() switch
                {
                    "name" => query.IsDescending ? clinicsQuery.OrderByDescending(c => c.Name) : clinicsQuery.OrderBy(c => c.Name),
                    "status" => query.IsDescending ? clinicsQuery.OrderByDescending(c => c.Status) : clinicsQuery.OrderBy(c => c.Status),
                    _ => query.IsDescending ? clinicsQuery.OrderByDescending(c => c.CreatedTime) : clinicsQuery.OrderBy(c => c.CreatedTime)
                };
            }else
            {
                clinicsQuery = query.IsDescending ? clinicsQuery.OrderByDescending(c => c.CreatedTime) : clinicsQuery.OrderBy(c => c.CreatedTime);
            }
            
            var total = await clinicsQuery.CountAsync();

            var items = await clinicsQuery
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var dto = items.ToClinicDtoList();

            return new BasePaginatedList<ClinicResponse>(dto, total, query.PageIndex, query.PageSize);
        }
    }
}
