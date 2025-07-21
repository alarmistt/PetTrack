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
            if (request.Comment == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.INVALID_INPUT, "Comment must be between 10 and 500 characters");

            var user = await _unitOfWork.GetRepository<User>().Entities
                .FirstOrDefaultAsync(u => u.Id == userId) ?? 
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            var feedback = new Feedback
            {
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
                .ToListAsync();

            if (feedbacks == null || !feedbacks.Any())
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "No feedbacks found");

            return feedbacks.ToFeedbackDtoList();
        }
    }
}
