using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Repositories.PaggingItems;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Helpers;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;
using PetTrack.ModelViews.Notification;
using PetTrack.Services.Infrastructure;

namespace PetTrack.Services.Services
{
    public class BookingNotificationService : IBookingNotificationService
    {
        public IUnitOfWork _unitOfWork;
        private IUserContextService _userContextService;
        private readonly IMapper _mapper;
        public BookingNotificationService(IUnitOfWork unitOfWork, IUserContextService userContextService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _mapper = mapper;
        }
        public async Task DeleteNotification(string notificationId)
        {
           BookingNotification? notification = await _unitOfWork.GetRepository<BookingNotification>().Entities
                .FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notification == null)
            {
                throw new ArgumentException("Notification not found", nameof(notificationId));
            }
            notification.DeletedTime = CoreHelper.SystemTimeNow;
            _unitOfWork.GetRepository<BookingNotification>().Update(notification);
            _unitOfWork.GetRepository<BookingNotification>().Save();
        }

        public async Task<PaginatedList<NotificationResponse>> ListNotificationsByUserIdAsync(int pageIndex = 1, int pageSize = 10)
        {
            string userId = _userContextService.GetUserId() ?? throw new ArgumentException("User not found", nameof(_userContextService));
            var query = _unitOfWork.GetRepository<BookingNotification>().Entities
                    .Where(n => n.UserId == userId && !n.DeletedTime.HasValue)
                    .OrderByDescending(n => n.CreatedTime);
            var paginatedList = await PaginatedList<BookingNotification>.CreateAsync(query, pageIndex, pageSize);
            var mapped = paginatedList.Items
                .Select(b => _mapper.Map<NotificationResponse>(b))
                .ToList();
            return new PaginatedList<NotificationResponse>(
                mapped,
                paginatedList.TotalCount,
                paginatedList.PageNumber,
                paginatedList.PageSize
            );
        }

        public Task SendBookingNotificationAsync(string bookingId, string userId, string type,string subject, string content)
        {
            throw new NotImplementedException();
        }
    }
}
