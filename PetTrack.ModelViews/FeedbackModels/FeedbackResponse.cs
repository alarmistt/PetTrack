namespace PetTrack.ModelViews.FeedbackModels
{
    public class FeedbackResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }

}
