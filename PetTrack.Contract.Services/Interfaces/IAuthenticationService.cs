using PetTrack.Core.Enums;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.ModelViews.AuthenticationModels;
using PetTrack.ModelViews.UserModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> Login(GoogleLoginRequest request);
        Task<UserResponseModel> GetUserInfo();
        Task<BasePaginatedList<UserResponseModel>> GetPagedUsers(UserQueryObject query);
        Task<UserResponseModel> GetUserById(string id);
        Task SetPasswordAsync(string userId, SetPasswordRequest request);
        // Register & Login (Password)
        Task<UserResponseModel> RegisterAsync(UserRegistrationRequest request);
        Task<AuthenticationModel> LoginWithEmailPasswordAsync(LoginRequest request);
        // User hoặc Admin update profile
        Task<UserResponseModel> UpdateUserAsync(string id, UpdateUserRequest request);
        Task DeleteUserAsync(string id);
        // Admin-only: update role
        Task<UserResponseModel> UpdateUserRoleAsync(string userId, UserRole newRole);
        Task UpdateRoleClinicAsync(string userId);
    }
}
