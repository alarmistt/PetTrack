using PetTrack.Core.Enums;
using PetTrack.Core.Models;

namespace PetTrack.Core.Helpers
{
    public class WithdrawQueryObject : BaseQueryObject
    {
        public string? WalletId { get; set; }
        public string? UserId { get; set; }         /// Lọc theo UserId của chủ ví (tùy chọn)
        public WalletTransactionStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
