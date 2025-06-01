using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Enums;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.ModelViews.AuthenticationModels;
using PetTrack.ModelViews.UserModels;

namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticate a user with Google and return JWT token.
        /// </summary>
        /// <param name="request">Google login credentials.</param>
        /// <returns>JWT token and user information.</returns>
        [HttpPost("login-google")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithGoogleAsync([FromBody] GoogleLoginRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _authenticationService.Login(request);
            return Ok(BaseResponseModel<AuthenticationModel>.OkDataResponse(result, "Login successfully"));
        }

        /// <summary>
        /// Get the current authenticated user's profile.
        /// </summary>
        /// <returns>User profile information.</returns>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var result = await _authenticationService.GetUserInfo();
            return Ok(BaseResponseModel<UserResponseModel>.OkDataResponse(result, "User profile retrieved successfully."));
        }

        /// <summary>
        /// Retrieves a paginated list of users with optional filters.
        /// </summary>
        /// <param name="query">Query filters and pagination parameters.</param>
        /// <returns>Paginated user list.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryObject query)
        {
            if (query.Role.HasValue && !Enum.IsDefined(typeof(UserRole), query.Role.Value))
            {
                return BadRequest(BaseResponseModel<string>.BadRequestResponse("Invalid role value."));
            }

            var result = await _authenticationService.GetPagedUsers(query);
            return Ok(BaseResponseModel<BasePaginatedList<UserResponseModel>>.OkDataResponse(result, "User list retrieved successfully."));
        }

        /// <summary>
        /// Get user details by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>User details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _authenticationService.GetUserById(id);
            return Ok(BaseResponseModel<UserResponseModel>.OkDataResponse(result, "User retrieved successfully."));
        }

        /// <summary>
        /// Update a user's profile information (Admin or user themself).
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="request">The updated user information.</param>
        /// <returns>The updated user details.</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            var result = await _authenticationService.UpdateUserAsync(id, request);
            return Ok(BaseResponseModel<UserResponseModel>.OkDataResponse(result, "User updated successfully."));
        }

        /// <summary>
        /// Soft-delete a user by ID (Admin only).
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>Confirmation of deletion.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _authenticationService.DeleteUserAsync(id);
            return Ok(BaseResponseModel<string>.OkDataResponse(id, "User deleted successfully."));
        }

        /// <summary>
        /// Updates the role of a user (Admin only).
        /// </summary>
        /// <param name="id">The ID of the user whose role will be updated.</param>
        /// <param name="request">
        /// The new role to assign to the user.  
        /// 
        /// **Allowed values:**
        /// - `0` = `User`  
        /// - `2` = `Clinic`  
        /// 
        /// **Note:** Assigning the `Admin` role (`1`) via this API is not allowed.
        /// </param>
        /// <returns>The updated user information.</returns>
        [HttpPut("{id}/update-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRoleAsync(string id, [FromBody] UpdateUserRoleRequest request)
        {
            var result = await _authenticationService.UpdateUserRoleAsync(id, request.NewRole);
            return Ok(BaseResponseModel<UserResponseModel>.OkDataResponse(result, "User role updated successfully."));
        }

    }
}