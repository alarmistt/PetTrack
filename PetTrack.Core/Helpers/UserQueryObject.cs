using PetTrack.Core.Enums;

namespace PetTrack.Core.Helpers
{
    public class UserQueryObject
    {
        public string? Id { get; set; }                  // Tìm theo ID cụ thể
        public string? FullName { get; set; }            // Tìm theo tên
        public string? Email { get; set; }               // Tìm theo email
        public string? PhoneNumber { get; set; }         // Tìm theo số điện thoại
        public UserRole? Role { get; set; }                // Lọc theo role: "User", "Clinic"

        // Pagination
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        // Sorting
        public string? SortBy { get; set; }              // "FullName", "Email", ...

        public bool IsDescending { get; set; } = false;  // Sắp xếp tăng/giảm dần
    }
}