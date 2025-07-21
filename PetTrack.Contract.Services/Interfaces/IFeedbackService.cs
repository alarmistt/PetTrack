using PetTrack.ModelViews.FeedbackModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> CreateFeedbackAsync(string userId, FeedbackCreateRequest request);
        Task<List<FeedbackResponse>> GetFeedbacksAsync();

    }
}
