using PetTrack.Core.Helpers;

namespace PetTrack.ModelViews.Booking
{
    public class BookingRequestModel
    {

        public string SlotId { get; set; }

        public string ServicePackageId { get; set; }

        public DateTimeOffset AppointmentDate { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(SlotId))
            {
                throw new ArgumentException("Clinic cannot be null or empty.", nameof(SlotId));
            }
            if (string.IsNullOrEmpty(ServicePackageId))
            {
                throw new ArgumentException("Package cannot be null or empty.", nameof(ServicePackageId));
            }
            if(AppointmentDate < CoreHelper.SystemTimeNow)
            {
                throw new ArgumentException("Appointment date cannot be in the past.", nameof(AppointmentDate));
            }
        }
    }
}
