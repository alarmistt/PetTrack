namespace PetTrack.ModelViews.WalletTransactionModels
{
    public class WithdrawResponse
    {
        public string TransactionId { get; set; }
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
