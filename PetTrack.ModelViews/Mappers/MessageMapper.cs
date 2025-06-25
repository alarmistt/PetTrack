using PetTrack.Entity;
using PetTrack.ModelViews.ChatModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class MessageMapper
    {
        public static MessageResponse ToMessageDto(this Message message)
        {

            return new MessageResponse
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                Status = message.Status,
                CreatedTime = message.CreatedTime
            };
        }

        public static List<MessageResponse> ToMessageDtoList(this IEnumerable<Message> messages)
        {
            return messages.Select(m => m.ToMessageDto()).ToList();
        }
    }
}