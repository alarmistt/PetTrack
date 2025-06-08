using AutoMapper;
using PetTrack.Entity;
using PetTrack.ModelViews.Notification;

namespace PetTrack.Services.MapperProfile
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationResponse, BookingNotification>().ReverseMap();
        }
    }
}
