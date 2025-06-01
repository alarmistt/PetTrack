namespace PetTrack.ModelViews.AuthenticationModels
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}