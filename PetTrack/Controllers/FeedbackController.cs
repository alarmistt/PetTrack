using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.FeedbackModels;

namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IUserContextService _userContextService;

        public FeedbackController(IFeedbackService feedbackService, IUserContextService userContextService)
        {
            _feedbackService = feedbackService;
            _userContextService = userContextService;
        }

        [HttpPost("feedbacks")]
        [Authorize]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateRequest request)
        {
            var userId = _userContextService.GetUserId();
            var result = await _feedbackService.CreateFeedbackAsync(userId, request);
            return Ok(BaseResponseModel<FeedbackResponse>.OkDataResponse(result, "Feedback created successfully"));
        }

        [HttpGet("feedbacks")]
        [Authorize]
        public async Task<IActionResult> GetFeedbacks()
        {
            var result = await _feedbackService.GetFeedbacksAsync();
            return Ok(BaseResponseModel<List<FeedbackResponse>>.OkDataResponse(result, "Feedbacks retrieved successfully"));
        }

    }
}
