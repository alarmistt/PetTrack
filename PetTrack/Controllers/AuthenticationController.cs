using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.AuthenticationModels;

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
        /// Authenticates user via Google login token.
        /// </summary>
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogleAsync([FromBody] GoogleLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(BaseResponseModel<string>.BadRequestResponse("Invalid input"));

            var result = await _authenticationService.Login(request);
            return Ok(BaseResponseModel<AuthenticationModel>.OkDataResponse(result, "Login successful"));
        }
    }
}
