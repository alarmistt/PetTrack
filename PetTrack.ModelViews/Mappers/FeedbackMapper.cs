using PetTrack.Entity;
using PetTrack.ModelViews.FeedbackModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class FeedbackMapper
    {
        public static FeedbackResponse ToFeedbackDto(this Feedback feedback)
        {
            return new FeedbackResponse
            {
                Id = feedback.Id,
                UserId = feedback.UserId,
                FullName = feedback.User?.FullName,
                Comment = feedback.Comment,
                CreatedTime = feedback.CreatedTime
            };
        }
        public static List<FeedbackResponse> ToFeedbackDtoList(this IEnumerable<Feedback> feedbacks)
        {
            return feedbacks.Select(f => f.ToFeedbackDto()).ToList();
        }
    }
}
