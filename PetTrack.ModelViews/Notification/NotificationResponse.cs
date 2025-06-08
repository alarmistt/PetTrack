namespace PetTrack.ModelViews.Notification
{
    public class NotificationResponse
    {
        public string BookingId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; } 
        public DateTime? SentAt { get; set; }
        public string? ErrorMessage { get; set; }
        public string Content { get; set; }
    }
}
