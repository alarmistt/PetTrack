using PetTrack.Entity;
using PetTrack.ModelViews.AuthenticationModels;

namespace PetTrack.ModelViews.TopUpModels
{
    public class TopUpResponse
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string UserId { get; set; }
        public UserResponse User { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // [PayOS, MoMo..]
        public string Status { get; set; } // [Pending, Success, Failed ]
        public string TransactionCode { get; set; }
    }
}
