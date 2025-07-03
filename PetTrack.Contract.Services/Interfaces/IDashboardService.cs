using PetTrack.Entity;
using PetTrack.ModelViews.Dashboard;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<RevenueClinicResponse>> GetMonthlyRevenueByClinicAsync(string clinicId, int year);
        Task<List<BookingCountByMonthResponse>> GetMonthlyBookingCountByClinicAsync(string clinicId, int year);
        Task<List<RevenueClinicResponse>> GetMonthlyRevenuePlatformAsync(int year);

    }
}
