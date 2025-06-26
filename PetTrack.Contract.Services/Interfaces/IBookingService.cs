using PetTrack.Contract.Repositories.PaggingItems;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseModel> CreateBookingsAsync(BookingRequestModel model);
        Task DeleteBookingAsync(string id);
        Task<BookingResponseModel> GetBookingByIdAsync(string id);
        Task<PaginatedList<BookingResponseModel>> GetListBooking(int pageIndex, int pageSize, string? clinicId = null, string? userId = null, string? status = null);
    }
}
