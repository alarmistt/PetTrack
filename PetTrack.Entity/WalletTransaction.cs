using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class WalletTransaction : BaseEntity
    {
        public string WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public string Type { get; set; } // [TopUp, BookingPayment, Refund, ComissionFee, ReceiveAmount]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
