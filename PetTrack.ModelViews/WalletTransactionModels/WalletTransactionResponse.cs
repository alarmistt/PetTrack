using PetTrack.Core.Enums;

namespace PetTrack.ModelViews.WalletTransactionModels
{
    public class WalletTransactionResponse
    {
        public string Id { get; set; }
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? BookingId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
