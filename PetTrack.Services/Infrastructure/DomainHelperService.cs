using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Exceptions;
using PetTrack.Entity;

namespace PetTrack.Services.Infrastructure
{
    public class DomainHelperService : IDomainHelperService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DomainHelperService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region CLINIC

        public async Task<Clinic> GetActiveClinicByIdAsync(string clinicId)
        {
            var clinic = await _unitOfWork.GetRepository<Clinic>().GetByIdAsync(clinicId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Clinic not found");

            if (clinic.DeletedTime.HasValue)
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.FAILED, "Clinic is deleted");

            return clinic;
        }

        public async Task<Clinic> EnsureUserOwnsClinicAsync(string clinicId, string userId)
        {
            var clinic = await GetActiveClinicByIdAsync(clinicId);

            if (clinic.OwnerUserId != userId)
                throw new ErrorException(StatusCodes.Status403Forbidden, ResponseCodeConstants.UNAUTHORIZED, "You are not allowed to access this clinic");

            return clinic;
        }

        public async Task<Clinic> GetOwnedClinicWithDetailsAsync(string clinicId, string userId)
        {
            var clinic = await _unitOfWork.GetRepository<Clinic>().Entities
                .Include(c => c.Schedules)
                .Include(c => c.ServicePackages)
                .FirstOrDefaultAsync(c =>
                    c.Id == clinicId &&
                    c.OwnerUserId == userId &&
                    !c.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Clinic not found or unauthorized");

            return clinic;
        }

        #endregion

        #region SCHEDULE

        public async Task<ClinicSchedule> EnsureUserOwnsScheduleAsync(string scheduleId, string userId)
        {
            var schedule = await _unitOfWork.GetRepository<ClinicSchedule>().Entities
                .Include(s => s.Clinic)
                .FirstOrDefaultAsync(s =>
                    s.Id == scheduleId &&
                    !s.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Schedule not found");

            if (schedule.Clinic == null)
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.FAILED, "Clinic reference is missing");

            if (schedule.Clinic.OwnerUserId != userId)
                throw new ErrorException(StatusCodes.Status403Forbidden, ResponseCodeConstants.UNAUTHORIZED, "You are not allowed to access this schedule");

            return schedule;
        }

        #endregion

        #region PACKAGE

        public async Task<ServicePackage> EnsureUserOwnsPackageAsync(string packageId, string userId)
        {
            var package = await _unitOfWork.GetRepository<ServicePackage>().Entities
                .Include(p => p.Clinic)
                .FirstOrDefaultAsync(p =>
                    p.Id == packageId &&
                    !p.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Package not found");

            if (package.Clinic == null)
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.FAILED, "Clinic reference is missing");

            if (package.Clinic.OwnerUserId != userId)
                throw new ErrorException(StatusCodes.Status403Forbidden, ResponseCodeConstants.UNAUTHORIZED, "You are not allowed to access this package");

            return package;
        }

        #endregion
    }
}
