using FluentValidation;
using PetTrack.Core.Enums;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.ModelViews.Validators
{
    public class CreateWalletTransactionRequestValidator : AbstractValidator<CreateWalletTransactionRequest>
    {
        public CreateWalletTransactionRequestValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty().WithMessage("WalletId is required");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be >= 0");

            RuleFor(x => x.TransactionType)
                .IsInEnum().WithMessage("Invalid transaction type");

            RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description must be at most 255 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

            When(x => BookingRelatedTypes.Contains(x.TransactionType), () =>
            {
                RuleFor(x => x.BookingId)
                    .NotEmpty().WithMessage("BookingId is required for this transaction type");
            });
        }

        private static readonly WalletTransactionType[] BookingRelatedTypes =
        {
            WalletTransactionType.BookingPayment,
            WalletTransactionType.ComissionFee,
            WalletTransactionType.ReceiveAmount,
            WalletTransactionType.Refund
        };
    }
}
