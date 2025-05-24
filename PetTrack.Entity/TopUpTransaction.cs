using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class TopUpTransaction : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // [PayOS, MoMo..]
        public string Status { get; set; } // [Pending, Success, Failed ]
        public string TransactionCode { get; set; }
    }
}
