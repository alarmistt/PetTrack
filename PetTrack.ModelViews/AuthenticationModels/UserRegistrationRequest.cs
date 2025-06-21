namespace PetTrack.ModelViews.AuthenticationModels
{
    public class UserRegistrationRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
