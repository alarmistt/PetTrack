using PetTrack.Core.Enums;
using PetTrack.Core.Models;

namespace PetTrack.Core.Helpers
{
    public class UserQueryObject : BaseQueryObject
    {
        public string? Id { get; set; }                  // Tìm theo ID cụ thể
        public string? FullName { get; set; }            // Tìm theo tên
        public string? Email { get; set; }               // Tìm theo email
        public string? PhoneNumber { get; set; }         // Tìm theo số điện thoại
        public UserRole? Role { get; set; }                // Lọc theo role: "User", "Clinic"
    }
}