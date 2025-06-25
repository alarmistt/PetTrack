using Microsoft.AspNetCore.SignalR;

namespace PetTrack.Services.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Lấy userId từ claim "id"
            return connection.User?.FindFirst("id")?.Value;
        }
    }
}
