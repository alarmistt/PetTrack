using PetTrack.Core.Enums;

namespace PetTrack.ModelViews.WalletTransactionModels
{
    public class CreateWalletTransactionRequest
    {
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
        public WalletTransactionType TransactionType { get; set; } // [TopUp, BookingPayment, Refund, ComissionFee, ReceiveAmount]
        public string? Description { get; set; }
        public string? BookingId { get; set; }
    }
}
