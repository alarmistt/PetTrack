using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
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
        private readonly IUserContextService _contextService;

        public AuthenticationController(IAuthenticationService authenticationService, IUserContextService contextService)
        {
            _authenticationService = authenticationService;
            _contextService = contextService;
        }

        /// <summary>
        /// Register a new user using email and password.
        /// </summary>
        /// <param name="request">Registration details.</param>
        /// <returns>User information after register</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.RegisterAsync(request);
            return Ok(BaseResponseModel<UserResponseModel>.OkDataResponse(result, "User registered successfully"));
        }

        /// <summary>
        /// Login with email and password.
        /// </summary>
        /// <param name="request">Login credentials.</param>
        /// <returns>JWT token and user information.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithEmailPassword([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationService.LoginWithEmailPasswordAsync(request);
            return Ok(BaseResponseModel<AuthenticationModel>.OkDataResponse(result, "Login successfully"));
        }

        /// <summary>
        /// <summary>
        /// Sets a password for the authenticated user. This is required if the user logged in using Google or has not set a password yet.
        /// </summary>
        /// <param name="request">The request containing the new password details.</param>
        /// <returns>A response indicating the success or failure of the password setting operation.</returns>
        [HttpPost("set-password")]
        [Authorize]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request)
        {
            var userId = _contextService.GetUserId();
            await _authenticationService.SetPasswordAsync(userId, request);
            return Ok(BaseResponse.OkMessageResponse("Password set successfully"));
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
        /// Update the authenticated user's own profile.
        /// </summary>
        /// <param name="request">New profile data.</param>
        /// <returns>Updated user info.</returns>
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfileAsync([FromBody] UpdateUserRequest request)
        {
            var userId = _contextService.GetUserId();
            var result = await _authenticationService.UpdateUserAsync(userId, request);
            return Ok(BaseResponseModel<UserResponseModel>.OkDataResponse(result, "Profile updated successfully."));
        }

    }
}