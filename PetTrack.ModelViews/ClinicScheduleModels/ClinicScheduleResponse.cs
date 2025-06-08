namespace PetTrack.ModelViews.ClinicScheduleModels
{
    public class ClinicScheduleResponse
    {
        public int DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}