using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class Slot : BaseEntity
    {
        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public int DayOfWeek { get; set; } // 0 = Sunday ... 6 Sarturday
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
