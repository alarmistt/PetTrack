using PetTrack.Core.Enums;
using PetTrack.Core.Models;

namespace PetTrack.Core.Helpers
{
    public class WalletTransactionQueryObject : BaseQueryObject
    {
        public string? WalletId { get; set; }
        public WalletTransactionType? Type { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public string? UserId { get; set; }
    }
}
