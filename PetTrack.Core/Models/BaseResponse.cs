using PetTrack.Core.Constants;
using PetTrack.Core.Enums;

namespace PetTrack.Core.Models
{
    public class BaseResponse
    {
        public string? Message { get; set; }
        public StatusCodeHelper StatusCode { get; set; }
        public string Code { get; set; }

        public BaseResponse(StatusCodeHelper statusCode, string code, string? message = null)
        {
            Message = message;
            StatusCode = statusCode;
            Code = code;
        }

        public static BaseResponse OkMessageResponse(string message)
        {
            return new BaseResponse(StatusCodeHelper.OK, ResponseCodeConstants.SUCCESS, message);
        }
    }
}
