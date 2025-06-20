namespace PetTrack.ModelViews.SlotModels
{
    public class SlotResponse
    {
        public string Id { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
