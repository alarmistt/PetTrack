using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class BookingStatusHistory : BaseEntity
    {
        public string BookingId { get; set; }
        public Booking Booking { get; set; }
        public string OldStatus { get; set; }
        public string? NewStatus { get; set; }
        public string? ChangeReason { get; set; }
        public string ChangedBy { get; set; }
    }
}
