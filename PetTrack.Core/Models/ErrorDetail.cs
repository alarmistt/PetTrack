using System.Text.Json.Serialization;

namespace PetTrack.Core.Models
{
    public class ErrorDetail
    {
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("errorMessage")]
        public object? ErrorMessage { get; set; }
    }
}
