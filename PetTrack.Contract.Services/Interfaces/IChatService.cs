using PetTrack.Core.Models;
using PetTrack.ModelViews.ChatModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IChatService
    {
        Task<MessageResponse> SendMessageAsync(string senderId, SendMessageRequest request);
        Task<BasePaginatedList<MessageResponse>> GetChatHistoryAsync(string userId, ChatHistoryQueryObject query);
        Task<List<MessageResponse>> GetNewMessagesAsync(string userId, string clinicId, DateTimeOffset since);
        Task MarkMessagesAsReadAsync(string userId, string clinicId);
    }
}
