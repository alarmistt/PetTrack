using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.Booking;

namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private IBookingNotificationService _bookingNotificationService;
        public NotificationController(IBookingNotificationService bookingNotificationService)
        {
            _bookingNotificationService = bookingNotificationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetNotifications(int pageIndex = 1, int pageSize = 10)
        {
            var result = await _bookingNotificationService.ListNotificationsByUserIdAsync(pageIndex, pageSize);
            return Ok(BaseResponseModel<BookingResponseModel>.OkDataResponse(result, "Get data successful"));
        }
        [HttpDelete("{notificationId}")]    
        public async Task<IActionResult> DeleteNotification(string notificationId)
        {
            await _bookingNotificationService.DeleteNotification(notificationId);
            return Ok(BaseResponseModel<string>.OkMessageResponseModel("Delete booking successful"));
        }
    }
}
