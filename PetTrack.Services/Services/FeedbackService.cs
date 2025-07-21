using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Constants;
using PetTrack.Core.Enums;
using PetTrack.Core.Exceptions;
using PetTrack.Entity;
using PetTrack.ModelViews.FeedbackModels;
using PetTrack.ModelViews.Mappers;

namespace PetTrack.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<FeedbackResponse> CreateFeedbackAsync(string userId, FeedbackCreateRequest request)
        {
            var booking = await _unitOfWork.GetRepository<Booking>().Entities
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId);

            if (booking.UserId != userId)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND,"You can only feedback your own booking");

            if (booking.Status != BookingStatus.Completed.ToString())
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "You can only feedback your own booking");

            if (booking.Feedback != null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "You can only feedback your own booking");

            var feedback = new Feedback
            {
                BookingId = booking.Id,
                UserId = userId,
                Comment = request.Comment,
            };

            await _unitOfWork.GetRepository<Feedback>().InsertAsync(feedback);
            await _unitOfWork.SaveAsync();

            return feedback.ToFeedbackDto();
        }

        public async Task<List<FeedbackResponse>> GetFeedbacksAsync()
        {
            var feedbacks = await _unitOfWork.GetRepository<Feedback>().Entities
                .Include(f => f.User)
                .Include(f => f.Booking)
                .ToListAsync();

            if (feedbacks == null || !feedbacks.Any())
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "No feedbacks found");

            return feedbacks.ToFeedbackDtoList();
        }
    }
}
