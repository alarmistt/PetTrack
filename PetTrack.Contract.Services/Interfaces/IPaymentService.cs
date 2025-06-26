using Net.payOS.Types;
using PetTrack.ModelViews.Payment;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreatePaymentResult> CreateLinkAsync(CreatePaymentLinkRequest request);
        Task<CreatePaymentResult> CreateLinkBookingAsync(CreateLinkBookingRequest request);
    }
}
