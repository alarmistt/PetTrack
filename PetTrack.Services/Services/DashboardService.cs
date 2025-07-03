using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Enums;
using PetTrack.Entity;
using PetTrack.ModelViews.Dashboard;

namespace PetTrack.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        public DashboardService(
            IUnitOfWork unitOfWork,
            IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }
        public async Task<List<RevenueClinicResponse>> GetMonthlyRevenueByClinicAsync(string clinicId, int year)
        {
            if (string.IsNullOrEmpty(clinicId))
            {
                throw new ArgumentException("Clinic ID cannot be null or empty.", nameof(clinicId));
            }
            var statuses = new[] { BookingStatus.Paid.ToString(),
                BookingStatus.Completed.ToString(),
               BookingStatus.Confirmed.ToString() };

            var revenues =  _unitOfWork.GetRepository<Booking>().Entities
                .Where(b => b.ClinicId == clinicId
                    && b.AppointmentDate.Year == year
                    && statuses.Contains(b.Status)
                    && b.Price.HasValue)
                .GroupBy(b => b.AppointmentDate.Month)
                .Select(g => new RevenueClinicResponse
                {
                    Month = g.Key,
                    Revenue = g.Sum(b => b.ClinicReceiveAmount ?? 0)
                })
                .ToList();

            // Ensure all months are present (even if revenue is 0)
            var result = Enumerable.Range(1, 12)
                .Select(m => new RevenueClinicResponse
                {
                    Month = m,
                    Revenue = revenues.FirstOrDefault(r => r.Month == m)?.Revenue ?? 0
                })
                .ToList();

            return result;
        }
        public async Task<List<RevenueClinicResponse>> GetMonthlyRevenuePlatformAsync(int year)
        {
            var statuses = new[] { BookingStatus.Paid.ToString(),
                BookingStatus.Completed.ToString(),
               BookingStatus.Confirmed.ToString() };

            var revenues = _unitOfWork.GetRepository<Booking>().Entities
                .Where(b => b.AppointmentDate.Year == year
                    && statuses.Contains(b.Status)
                    && b.Price.HasValue)
                .GroupBy(b => b.AppointmentDate.Month)
                .Select(g => new RevenueClinicResponse
                {
                    Month = g.Key,
                    Revenue = g.Sum(b => b.PlatformFee ?? 0)
                })
                .ToList();

            // Ensure all months are present (even if revenue is 0)
            var result = Enumerable.Range(1, 12)
                .Select(m => new RevenueClinicResponse
                {
                    Month = m,
                    Revenue = revenues.FirstOrDefault(r => r.Month == m)?.Revenue ?? 0
                })
                .ToList();

            return result;
        }
        public async Task<List<BookingCountByMonthResponse>> GetMonthlyBookingCountByClinicAsync(string clinicId, int year)
        {
            var statuses = new[] { BookingStatus.Paid.ToString(),
                BookingStatus.Completed.ToString(),
               BookingStatus.Confirmed.ToString() };

            var counts = await _unitOfWork.GetRepository<Booking>().Entities
                .Where(b => b.ClinicId == clinicId
                            && b.AppointmentDate.Year == year
                            && statuses.Contains(b.Status))
                .GroupBy(b => b.AppointmentDate.Month)
                .Select(g => new BookingCountByMonthResponse
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Đảm bảo trả về đủ 12 tháng
            var result = Enumerable.Range(1, 12)
                .Select(m => new BookingCountByMonthResponse
                {
                    Month = m,
                    Count = counts.FirstOrDefault(r => r.Month == m)?.Count ?? 0
                })
                .ToList();

            return result;
        }
    }
}
