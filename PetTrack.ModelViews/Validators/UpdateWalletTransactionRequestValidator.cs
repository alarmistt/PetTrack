using FluentValidation;
using PetTrack.ModelViews.WalletTransactionModels;

namespace PetTrack.ModelViews.Validators
{
    public class UpdateWalletTransactionRequestValidator : AbstractValidator<UpdateWalletTransactionRequest>
    {
        public UpdateWalletTransactionRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be >= 0");

            RuleFor(x => x.TransactionType)
                .IsInEnum().WithMessage("Invalid transaction type");

            RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description must be at most 255 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
