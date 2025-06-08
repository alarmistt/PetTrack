using FluentValidation;
using PetTrack.ModelViews.WalletModels;

namespace PetTrack.ModelViews.Validators
{
    public class UpdateWalletRequestValidator : AbstractValidator<UpdateWalletRequest>
    {
        public UpdateWalletRequestValidator()
        {
            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Balance must be >= 0");
        }
    }
}
