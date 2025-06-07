using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class BookingNotification : BaseEntity
    {
        public string BookingId { get; set; }
        public Booking Booking { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; } // [BookingConfirmed, BookingCancelled, Reminder..]
        public string Content { get; set; }
    }
}
