using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class BookingNotification : BaseEntity
    {
        public string BookingId { get; set; }
        public Booking Booking { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Type { get; set; } // [BookingConfirmed, BookingCancelled, Reminder..]
        public string Channel { get; set; } // [Email, Push Notification]
        public string Status { get; set; } // [Pending, Status, Failed]
        public DateTime? SentAt { get; set; }
        public int? RetryCount { get; set; } 
        public string? ErrorMessage { get; set; }
        public string Content { get; set; }
    }
}
