using FluentValidation;
using PetTrack.ModelViews.UserModels;

namespace PetTrack.ModelViews.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(255);

            RuleFor(x => x.Address)
                .MaximumLength(500);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20);

            RuleFor(x => x.AvatarUrl)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));
        }
    }
}
