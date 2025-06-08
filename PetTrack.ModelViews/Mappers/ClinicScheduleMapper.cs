using PetTrack.Entity;
using PetTrack.ModelViews.ClinicScheduleModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class ClinicScheduleMapper
    {
        public static ClinicScheduleResponse ToScheduleDto(this ClinicSchedule schedule)
        {
            return new ClinicScheduleResponse
            {
                DayOfWeek = schedule.DayOfWeek,
                OpenTime = schedule.OpenTime,
                CloseTime = schedule.CloseTime,
            };
        }

        public static List<ClinicScheduleResponse> ToScheduleDtoList(this IEnumerable<ClinicSchedule> schedules)
        {
            return schedules.Select(s => s.ToScheduleDto()).ToList();
        }
    }
}
