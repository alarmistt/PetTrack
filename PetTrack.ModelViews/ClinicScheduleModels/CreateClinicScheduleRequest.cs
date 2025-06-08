namespace PetTrack.ModelViews.ClinicScheduleModels
{
    public class CreateClinicScheduleRequest
    {
        public int DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}