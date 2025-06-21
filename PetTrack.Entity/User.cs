using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } // [User, Admin, Clinic]
        public string? AvatarUrl { get; set; }
        public bool IsPasswordSet { get; set; }

        public ICollection<Clinic> Clinics { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<BookingNotification> Notifications { get; set; }
        public ICollection<TopUpTransaction> TopUps { get; set; }
        public Wallet Wallet { get; set; }
    }
}
