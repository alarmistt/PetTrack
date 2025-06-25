using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Enums;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.ChatModels;
using PetTrack.ModelViews.Mappers;
using PetTrack.Services.SignalR;

namespace PetTrack.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task<MessageResponse> SendMessageAsync(string senderId, SendMessageRequest request)
        {
            if (senderId == request.ReceiverId)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Cannot send message to self.");

            var receiver = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.ReceiverId);
            if (receiver == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Receiver does not exist.");
            }


            if (string.IsNullOrWhiteSpace(request.Content))
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Message content cannot be empty.");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                Status = MessageStatus.Sent.ToString(),
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };

            await _unitOfWork.GetRepository<Message>().InsertAsync(message);
            await _unitOfWork.SaveAsync();

            var response = message.ToMessageDto();

            // Send real-time message via SignalR
            await _hubContext.Clients.Group(request.ReceiverId).SendAsync("ReceiveMessage", response);

            return response;
        }

        public async Task<BasePaginatedList<MessageResponse>> GetChatHistoryAsync(string userId, ChatHistoryQueryObject query)
        {
            var messagesQuery = _unitOfWork.GetRepository<Message>().Entities
                .Where(m => (m.SenderId == userId && m.ReceiverId == query.ClinicId) ||
                            (m.SenderId == query.ClinicId && m.ReceiverId == userId))
                .OrderByDescending(m => m.CreatedTime);

            var totalCount = await messagesQuery.CountAsync();
            var items = await messagesQuery
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
            var dto = items.ToMessageDtoList();

            return new BasePaginatedList<MessageResponse>(dto, totalCount, query.PageIndex, query.PageSize);
        }

        public async Task<List<MessageResponse>> GetNewMessagesAsync(string userId, string clinicId, DateTimeOffset since)
        {
            var messages = await _unitOfWork.GetRepository<Message>().Entities
                .Where(m => m.ReceiverId == userId &&
                            m.SenderId == clinicId &&
                            m.CreatedTime > since)
                .OrderBy(m => m.CreatedTime)
                .ToListAsync();

            return messages.ToMessageDtoList();
        }

        public async Task MarkMessagesAsReadAsync(string userId, string clinicId)
        {
            var messages = await _unitOfWork.GetRepository<Message>().Entities
                .Where(m => m.ReceiverId == userId &&
                            m.SenderId == clinicId &&
                            m.Status != MessageStatus.Read.ToString())
                .ToListAsync();

            foreach (var msg in messages)
            {
                msg.Status = MessageStatus.Read.ToString();
                msg.LastUpdatedTime = CoreHelper.SystemTimeNow;
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
