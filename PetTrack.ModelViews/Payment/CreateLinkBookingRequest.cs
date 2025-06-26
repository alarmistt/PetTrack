namespace PetTrack.ModelViews.Payment
{
    public class CreateLinkBookingRequest
    {
        public int Price { get; set; }
        public string BookingId { get; set; }
        public string? Description = "Booking";
        public string ReturnUrl = "https://mystic-blind-box.web.app/wallet-success";
        public string CancelUrl = "https://mystic-blind-box.web.app/wallet-fail";
    }

}
