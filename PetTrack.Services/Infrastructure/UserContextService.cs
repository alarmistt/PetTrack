using Microsoft.AspNetCore.Http;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Exceptions;

namespace PetTrack.Services.Infrastructure
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
                throw new UnauthorizedException("User is not authenticated.");

            var id = user.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            return id ?? throw new UnauthorizedException("User ID not found in token.");
        }

        public string? GetUserRole()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        }
    }
}
