using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Helpers;
using PetTrack.Entity;
using PetTrack.ModelViews.ClinicScheduleModels;
using PetTrack.ModelViews.Mappers;

namespace PetTrack.Services.Services
{
    public class ClinicScheduleService : IClinicScheduleService
    {
        private readonly IDomainHelperService _helperService;
        private readonly IUserContextService _userContextService;
        private readonly IUnitOfWork _unitOfWork;

        public ClinicScheduleService(IUnitOfWork unitOfWork, IUserContextService userContextService, IDomainHelperService helperService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _helperService = helperService;

        }
        public async Task<ClinicScheduleResponse> CreateScheduleAsync(string clinicId, CreateClinicScheduleRequest request)
        {
            var currentUserId = _userContextService.GetUserId();
            var clinic = await _helperService.EnsureUserOwnsClinicAsync(clinicId, currentUserId);

            // check duplcate dayOfWeek of Schedule
            bool isDuplicated = await _unitOfWork.GetRepository<ClinicSchedule>().Entities.AnyAsync(s => s.ClinicId == clinic.Id && s.DayOfWeek == request.DayOfWeek && !s.DeletedTime.HasValue);

            if (isDuplicated)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.DUPLICATE, "Schedule has duplicated the dayOfWeek");
            }

            var schedule = new ClinicSchedule
            {
                ClinicId = clinic.Id,
                DayOfWeek = request.DayOfWeek,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime
            };

            await _unitOfWork.GetRepository<ClinicSchedule>().InsertAsync(schedule);
            await _unitOfWork.SaveAsync();

            return schedule.ToScheduleDto();
        }

        public async Task DeleteScheduleAsync(string scheduleId)
        {
            var currentUserId = _userContextService.GetUserId();
            var schedule = await _helperService.EnsureUserOwnsScheduleAsync(scheduleId, currentUserId);

            schedule.DeletedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<ClinicSchedule>().UpdateAsync(schedule);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<ClinicScheduleResponse>> GetSchedulesByClinicAsync(string clinicId)
        {
            var clinic = await _helperService.GetActiveClinicByIdAsync(clinicId);

            var schedules = await _unitOfWork.GetRepository<ClinicSchedule>()
                .FindListAsync(s => s.ClinicId == clinic.Id && !s.DeletedTime.HasValue);

            return schedules.ToScheduleDtoList();
        }

        public async Task<ClinicScheduleResponse> UpdateScheduleAsync(string scheduleId, UpdateClinicScheduleRequest request)
        {
            var currentUserId = _userContextService.GetUserId();
            var schedule = await _helperService.EnsureUserOwnsScheduleAsync(scheduleId, currentUserId);

            // check duplcate dayOfWeek of Schedule
            bool isDuplicated = await _unitOfWork.GetRepository<ClinicSchedule>().Entities.AnyAsync(s => s.ClinicId == schedule.ClinicId && s.DayOfWeek == request.DayOfWeek && s.Id != schedule.Id && !s.DeletedTime.HasValue);

            if (isDuplicated)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.DUPLICATE, "Schedule has duplicated the dayOfWeek");
            }

            schedule.DayOfWeek = request.DayOfWeek;
            schedule.OpenTime = request.OpenTime;
            schedule.CloseTime = request.CloseTime;
            schedule.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<ClinicSchedule>().UpdateAsync(schedule);
            await _unitOfWork.SaveAsync();

            return schedule.ToScheduleDto();
        }
    }
}
