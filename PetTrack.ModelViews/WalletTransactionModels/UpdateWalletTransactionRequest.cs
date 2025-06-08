using PetTrack.Core.Enums;

namespace PetTrack.ModelViews.WalletTransactionModels
{
    public class UpdateWalletTransactionRequest
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public WalletTransactionType TransactionType { get; set; } // [TopUp, BookingPayment, Refund, ComissionFee, ReceiveAmount]
    }
}
