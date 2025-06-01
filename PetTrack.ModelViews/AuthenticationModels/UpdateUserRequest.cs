namespace PetTrack.ModelViews.UserModels
{
    public class UpdateUserRequest
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
