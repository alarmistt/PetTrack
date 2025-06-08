using PetTrack.Core.Enums;

namespace PetTrack.Core.Helpers
{
    public class WalletTransactionQueryObject
    {
        public string? WalletId { get; set; }
        public WalletTransactionType? TransactionType { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public string? UserId { get; set; }

        // Paging & Sorting
        public string? SortBy { get; set; } = "CreatedTime";
        public bool IsDescending { get; set; } = true;

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
