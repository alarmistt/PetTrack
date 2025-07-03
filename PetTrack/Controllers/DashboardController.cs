using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;
using PetTrack.ModelViews.Dashboard;
using System.Threading.Tasks;

namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;   
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        ///doanh thu hàng tháng của phòng khám trong 1 năm
        /// </summary>
        /// <param name="clinicId">chọn phòng khám</param>
        /// <param name="year">chọn năm</param>
        /// <returns></returns>
        [HttpGet("revenue-clinic")]
        public async Task<IActionResult> GetRevenueClinicAsync(string clinicId, int year )
        {
            var result = await _dashboardService.GetMonthlyRevenueByClinicAsync(clinicId, year);
            return Ok(BaseResponseModel<RevenueClinicResponse>.OkDataResponse(result, "Get data successful"));
        }
        /// <summary>
        /// Số lượng booking hàng tháng của phòng khám trong 1 năm
        /// </summary>
        [HttpGet("booking-count-clinic")]
        public async Task<IActionResult> GetMonthlyBookingCount(string clinicId, int year)
        {
            var result = await _dashboardService.GetMonthlyBookingCountByClinicAsync(clinicId, year);
            return Ok(BaseResponseModel<BookingCountByMonthResponse>.OkDataResponse(result, "Get data successful"));
        }

        /// <summary>
        ///doanh thu hàng tháng của phòng nền tảng pettrack trong 1 năm
        /// </summary>
        /// <param name="year">chọn năm</param>
        /// <returns></returns>
        [HttpGet("revenue-platform")]
        public async Task<IActionResult> GetRevenuePlatformAsync(int year)
        {
            var result = await _dashboardService.GetMonthlyRevenuePlatformAsync(year);
            return Ok(BaseResponseModel<RevenueClinicResponse>.OkDataResponse(result, "Get data successful"));
        }

    }
}
