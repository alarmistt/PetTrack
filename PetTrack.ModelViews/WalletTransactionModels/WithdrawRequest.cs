namespace PetTrack.ModelViews.WalletTransactionModels
{
    public class WithdrawRequest
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
    }
}
