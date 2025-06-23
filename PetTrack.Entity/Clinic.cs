using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class Clinic : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Slogan { get; set; }
        public string Description { get; set; }
        public string? BannerUrl { get; set; }
        public string Status { get; set; }  // [Approved, Pending, Rejected]

        public string OwnerUserId { get; set; }
        public User Owner { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<ServicePackage> ServicePackages { get; set; }
        public ICollection<ClinicSchedule> Schedules { get; set; }

    }
}
