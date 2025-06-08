using FluentValidation;
using PetTrack.ModelViews.WalletModels;

namespace PetTrack.ModelViews.Validators
{
    public class CreateWalletRequestValidator : AbstractValidator<CreateWalletRequest>
    {
        public CreateWalletRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
