using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Enums;
using PetTrack.Core.Exceptions;
using PetTrack.Entity;
using PetTrack.ModelViews.Mappers;
using PetTrack.ModelViews.SlotModels;

namespace PetTrack.Services.Services
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SlotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task CheckExistSlotAsync(string clinicId, DateTime AppointmentDate)
        {
            throw new NotImplementedException();
        }
        public async Task<List<SlotResponse>> GetSlotsByClinicIdAsync(string clinicId)
        {
            var slots = await _unitOfWork.GetRepository<Slot>().Entities
                .Where(x => x.ClinicId == clinicId && !x.DeletedTime.HasValue).ToListAsync();

            return slots.ToSlotDtoList();
        }

        public async Task GenerateSlotsFromClinicScheduleAsync(string clinicId)
        {
            var clinic = await _unitOfWork.GetRepository<Clinic>().GetByIdAsync(clinicId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Clinic not found");

            if (!Enum.TryParse<ClinicStatus>(clinic.Status, out var statusEnum) || statusEnum != ClinicStatus.Approved)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.FAILED, "Clinic must be approved before generating slots");
            }

            var existingSlots = await _unitOfWork.GetRepository<Slot>().Entities
                .AnyAsync(x => x.ClinicId == clinicId && !x.DeletedTime.HasValue);

            if (existingSlots)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.FAILED, "Slots already exist for this clinic.");
            }

            var schedules = await _unitOfWork.GetRepository<ClinicSchedule>().Entities
                .Where(x => x.ClinicId == clinicId && !x.DeletedTime.HasValue)
                .ToListAsync();

            var slotsToAdd = new List<Slot>();

            foreach (var schedule in schedules)
            {
                var duration = (int)(schedule.CloseTime - schedule.OpenTime).TotalMinutes;
                var slotCount = duration / 60;

                for (int i = 0; i < slotCount; i++)
                {
                    var startTime = schedule.OpenTime.Add(TimeSpan.FromMinutes(i * 60));
                    var endTime = startTime.Add(TimeSpan.FromMinutes(60));

                    slotsToAdd.Add(new Slot
                    {
                        ClinicId = clinicId,
                        DayOfWeek = schedule.DayOfWeek,
                        StartTime = startTime,
                        EndTime = endTime
                    });
                }
            }

            await _unitOfWork.GetRepository<Slot>().InsertRangeAsync(slotsToAdd);
            await _unitOfWork.SaveAsync();

        }

        public async Task SyncSlotsAfterScheduleUpdatedAsync(ClinicSchedule updatedSchedule)
        {
            // Xóa toàn bộ slot cũ theo ClinicSchedule
            var oldSlots = await _unitOfWork.GetRepository<Slot>().Entities
                .Where(x => x.ClinicId == updatedSchedule.ClinicId
                    && x.DayOfWeek == updatedSchedule.DayOfWeek)
                .ToListAsync();

            await _unitOfWork.GetRepository<Slot>().DeleteRangeAsync(oldSlots);
            await _unitOfWork.SaveAsync();

            // Tạo lại Slot mới từ schedule mới
            var duration = (int)(updatedSchedule.CloseTime - updatedSchedule.OpenTime).TotalMinutes;
            var slotCount = duration / 60;

            var newSlots = new List<Slot>();
            for (int i = 0; i < slotCount; i++)
            {
                var startTime = updatedSchedule.OpenTime.Add(TimeSpan.FromMinutes(i * 60));
                var endTime = startTime.Add(TimeSpan.FromMinutes(60));
                newSlots.Add(new Slot
                {
                    ClinicId = updatedSchedule.ClinicId,
                    DayOfWeek = updatedSchedule.DayOfWeek,
                    StartTime = startTime,
                    EndTime = endTime
                });
            }

            await _unitOfWork.GetRepository<Slot>().InsertRangeAsync(newSlots);
            await _unitOfWork.SaveAsync();
        }


        public async Task RemoveSlotsAfterScheduleDeletedAsync(ClinicSchedule deletedSchedule)
        {
            var slotsToDelete = await _unitOfWork.GetRepository<Slot>().Entities
                .Where(x => x.ClinicId == deletedSchedule.ClinicId &&
                            x.DayOfWeek == deletedSchedule.DayOfWeek &&
                            !x.DeletedTime.HasValue)
                .ToListAsync();

            await _unitOfWork.GetRepository<Slot>().DeleteRangeAsync(slotsToDelete);
            await _unitOfWork.SaveAsync();
        }

       
    }
}
