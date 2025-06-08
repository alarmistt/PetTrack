using PetTrack.Contract.Repositories.PaggingItems;
using PetTrack.ModelViews.Notification;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IBookingNotificationService
    {
         Task SendBookingNotificationAsync(string bookingId, string userId, string type, string subject, string content);
         Task<PaginatedList<NotificationResponse>> ListNotificationsByUserIdAsync(int pageIndex = 1, int pageSize = 10);
         Task DeleteNotification(string notificationId);
    }
}
