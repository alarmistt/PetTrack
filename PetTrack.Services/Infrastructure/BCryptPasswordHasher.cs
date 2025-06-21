using PetTrack.Core.Helpers;

namespace PetTrack.Services.Infrastructure
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
