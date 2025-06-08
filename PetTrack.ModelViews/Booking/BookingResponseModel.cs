using PetTrack.Entity;

namespace PetTrack.ModelViews.Booking
{
    public class BookingResponseModel
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string ClinicId { get; set; }
        public string ClinicName { get; set; }

        public string ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }

        public DateTimeOffset AppointmentDate { get; set; }
        public string Status { get; set; } 

        public decimal? Price { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
