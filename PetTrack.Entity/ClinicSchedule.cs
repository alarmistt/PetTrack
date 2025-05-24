using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class ClinicSchedule : BaseEntity
    {
        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public int DayOfWeek { get; set; } // 0 = Sunday, 1 = Monday... 6 = Saturday
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}
