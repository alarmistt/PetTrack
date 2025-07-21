using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Repositories.PaggingItems;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Enums;
using PetTrack.Core.Helpers;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;

namespace PetTrack.Services.Services
{
    public class BookingService : IBookingService
    {
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IUserContextService _userContextService;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork; 
            _userContextService = userContextService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Mapper cannot be null");
        }
        public async Task<BookingResponseModel> CreateBookingsAsync(BookingRequestModel model)
        {
            model.Validate();
            Slot? slot = await _unitOfWork.GetRepository<Slot>().Entities
                .FirstOrDefaultAsync(m => m.Id == model.SlotId);
            if(slot == null)
            {
                throw new Exception("Slot is null");
            }
            Booking booking = _mapper.Map<Booking>(model);    
            booking.UserId = _userContextService.GetUserId() ?? throw new ArgumentException("User not found", nameof(_userContextService));
            ServicePackage? package = await _unitOfWork.GetRepository<ServicePackage>().Entities.FirstOrDefaultAsync(pa => pa.Id == model.ServicePackageId);
            booking.PlatformFee = package.Price * 0.15m;
            booking.ClinicReceiveAmount = package.Price * 0.85m;
            booking.ClinicId = slot.ClinicId;
            booking.SlotId = slot.Id;
            booking.Price = package.Price;
            booking.Status = BookingStatus.Pending.ToString();
            await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
            await _unitOfWork.GetRepository<Booking>().SaveAsync();
            return _mapper.Map<BookingResponseModel>(booking);
        }

        public async Task DeleteBookingAsync(string id)
        {
            Booking? booking = await _unitOfWork.GetRepository<Booking>().Entities.FirstOrDefaultAsync(bo => bo.Id == id);
            if(booking == null)
            {
                throw new ArgumentException("Booking not found");
            }
            booking.DeletedTime = CoreHelper.SystemTimeNow;
            await _unitOfWork.GetRepository<Booking>().UpdateAsync(booking);
            await _unitOfWork.GetRepository<Booking>().SaveAsync();
        }

        public async Task<BookingResponseModel> GetBookingByIdAsync(string id)
        {
            return _mapper.Map<BookingResponseModel>(
                await _unitOfWork.GetRepository<Booking>().Entities.FirstOrDefaultAsync(bo => bo.Id == id));
            
        }

        public async Task<PaginatedList<BookingResponseModel>> GetListBooking(int pageIndex, int pageSize, string? clinicId = null, string? userId = null, string? status = null)
        {
            var query = _unitOfWork.GetRepository<Booking>().Entities
                .Where(bo => !bo.DeletedTime.HasValue)
                .Include(b => b.User)
                .Include(b => b.Clinic)
                .Include(b => b.ServicePackage)
                .AsQueryable();

            if (!string.IsNullOrEmpty(clinicId))
                query = query.Where(b => b.ClinicId == clinicId);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(b => b.UserId == userId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(b => b.Status == status);

            query = query.OrderByDescending(b => b.CreatedTime);

            var pagedBookings = await PaginatedList<Booking>.CreateAsync(query, pageIndex, pageSize);

            var mapped = pagedBookings.Items
                .Select(b => _mapper.Map<BookingResponseModel>(b))
                .ToList();

            return new PaginatedList<BookingResponseModel>(
                mapped,
                pagedBookings.TotalCount,
                pagedBookings.PageNumber,
                pagedBookings.PageSize
            );
        }

    }
}
