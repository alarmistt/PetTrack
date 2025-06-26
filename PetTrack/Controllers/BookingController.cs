using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Repositories.PaggingItems;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;

namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        public IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBookings(int pageIndex = 1, int pageSize = 10, string? clinicId = null, string? userId = null, string? status = null)
        {
            PaginatedList<BookingResponseModel> bookings = await _bookingService.GetListBooking(pageIndex, pageSize, clinicId, userId, status);

            return Ok(BaseResponseModel<BookingResponseModel>.OkDataResponse(bookings, "Get data successful"));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return Ok(BaseResponseModel<BookingResponseModel>.OkDataResponse(booking, "Get data successful"));
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequestModel model)
        {

            BookingResponseModel booking =  await _bookingService.CreateBookingsAsync(model);
            return Ok(BaseResponseModel<BookingResponseModel>.OkDataResponse(booking, "Get data successful"));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return Ok(BaseResponseModel<string>.OkMessageResponseModel("Delete booking successful"));
        }
    }

}