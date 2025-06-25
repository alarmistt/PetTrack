using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Models;
using PetTrack.ModelViews.ChatModels;


namespace PetTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IUserContextService _userContext;

        public ChatController(IChatService chatService, IUserContextService userContext)
        {
            _chatService = chatService;
            _userContext = userContext;
        }

        /// <summary>
        /// Sends a new chat message from the authenticated user to the specified clinic.
        /// </summary>
        /// <param name="request">The message request containing clinicId and message content.</param>
        /// <returns>The message that was sent.</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var senderId = _userContext.GetUserId();
            var result = await _chatService.SendMessageAsync(senderId, request);
            return Ok(BaseResponseModel<MessageResponse>.OkDataResponse(result, "Message sent"));
        }

        /// <summary>
        /// Retrieves full chat history between the authenticated user and the specified clinic with pagination.
        /// </summary>
        /// <param name="query">Query object containing clinicId, pageIndex, pageSize.</param>
        /// <returns>Paginated list of past messages.</returns>
        [HttpGet("history")]
        public async Task<IActionResult> GetChatHistory([FromQuery] ChatHistoryQueryObject query)
        {
            var userId = _userContext.GetUserId();
            var result = await _chatService.GetChatHistoryAsync(userId, query);
            return Ok(BaseResponseModel<BasePaginatedList<MessageResponse>>.OkDataResponse(result, "Chat history loaded"));
        }

        /// <summary>
        /// Retrieves new messages from a clinic since a specified timestamp.
        /// </summary>
        /// <param name="clinicId">The clinic ID to get new messages from.</param>
        /// <param name="since">The timestamp to get messages after.</param>
        /// <returns>A list of new messages.</returns>
        [HttpGet("new")]
        public async Task<IActionResult> GetNewMessages([FromQuery] string clinicId, [FromQuery] DateTimeOffset since)
        {
            var userId = _userContext.GetUserId();
            var result = await _chatService.GetNewMessagesAsync(userId, clinicId, since);
            return Ok(BaseResponseModel<List<MessageResponse>>.OkDataResponse(result, "New messages loaded"));
        }

        /// <summary>
        /// Marks all unread messages from a specific clinic as read.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic whose messages will be marked as read.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPatch("mark-as-read")]
        public async Task<IActionResult> MarkMessagesAsRead([FromQuery] string clinicId)
        {
            var userId = _userContext.GetUserId();
            await _chatService.MarkMessagesAsReadAsync(userId, clinicId);
            return Ok(BaseResponse.OkMessageResponse("Messages marked as read"));
        }
    }
}
