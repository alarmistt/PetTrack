namespace PetTrack.ModelViews.Payment
{
    public class CreatePaymentLinkRequest
    {
        public int Price { get; set; }
        public string? Description = "Deposit ";
        public string ReturnUrl = "https://mystic-blind-box.web.app/wallet-success";
        public string CancelUrl = "https://mystic-blind-box.web.app/wallet-fail";
    }

}
