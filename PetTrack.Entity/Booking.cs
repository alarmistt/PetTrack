using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class Booking : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public string SlotId { get; set; }
        public Slot Slot { get; set; }
        public string ServicePackageId { get; set; }
        public ServicePackage ServicePackage { get; set; }

        public DateTimeOffset AppointmentDate { get; set; }
        public string Status { get; set; }  // [Pending, Paid, InProgress, Confirmed, Completed (cronjob nếu đã qua booking-time và status == confirmed -> completed), Cancelled, Refunded]
        public decimal? Price { get; set; }
        public decimal? PlatformFee { get; set; }
        public decimal? ClinicReceiveAmount { get; set; }
        public ICollection<WalletTransaction> Transactions { get; set; }
        public ICollection<BookingNotification> Notifications { get; set; }
    }
}
