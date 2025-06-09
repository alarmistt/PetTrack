using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Entity;
using PetTrack.ModelViews.Payment;

namespace PetTrack.Services.Services
{
    public class PaymentService : IPaymentService
    {
        public readonly PayOS _payOS;
        public readonly IUnitOfWork _unitOfWork;
        public PaymentService(PayOS payOS, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _payOS = payOS;
        }
        public async Task<CreatePaymentResult> CreateLinkAsync(CreatePaymentLinkRequest request)
        {
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));

            var account = await _unitOfWork.GetRepository<User>().Entities.FirstOrDefaultAsync(u => u.Id == request.AccountId);

            ItemData item = new ItemData(request.AccountId, 1, request.Price);
            var descriptions = request.Description = $"Deposit {request.Price}";
            List<ItemData> items = new List<ItemData> { item };
            var expiredAt = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
            PaymentData paymentDataPayment = new PaymentData(orderCode, request.Price, descriptions, items, request.CancelUrl, request.ReturnUrl, null, null, null, null, null, expiredAt);
            try
            {
                var createdLink = await _payOS.createPaymentLink(paymentDataPayment);
                return createdLink;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
    }
}
