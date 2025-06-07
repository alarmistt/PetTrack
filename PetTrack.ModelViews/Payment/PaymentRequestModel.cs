namespace PetTrack.ModelViews.Payment
{
    public class PaymentRequestModel
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }

}
