using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class WalletTransaction : BaseEntity
    {        
        public string Type { get; set; } // [TopUp, BookingPayment, Refund, ComissionFee, ReceiveAmount, Withdraw]
        public string? Status { get; set; } // [Pending, Approved, Rejected]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        
        public string? BookingId { get; set; }
        public string WalletId { get; set; }

        public string? BankName { get; set; }
        public string? BankNumber { get; set; }

        public Booking? Booking { get; set; }
        public Wallet Wallet { get; set; }
    }
}
