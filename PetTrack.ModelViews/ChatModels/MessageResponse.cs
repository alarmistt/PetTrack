namespace PetTrack.ModelViews.ChatModels
{
    public class MessageResponse
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}