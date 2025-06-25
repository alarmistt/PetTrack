using FluentValidation;
using PetTrack.Core.Helpers;
using PetTrack.ModelViews.AuthenticationModels;

namespace PetTrack.ModelViews.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Password)
                .Must(PasswordHelper.IsStrongPassword)
                .WithMessage("Password isn't valid.");
        }
    }
}
