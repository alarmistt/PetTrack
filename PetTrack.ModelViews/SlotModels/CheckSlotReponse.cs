using PetTrack.Core.Enums;

namespace PetTrack.ModelViews.SlotModels
{
    public class CheckSlotReponse
    {
        public string Id { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status  {get; set; }
}
    
}
