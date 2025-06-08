using FluentValidation;
using PetTrack.ModelViews.ClinicModels;

namespace PetTrack.ModelViews.Validators
{
    public class UpdateClinicRequestValidator : AbstractValidator<UpdateClinicRequest>
    {
        public UpdateClinicRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Clinic name is required")
                .MaximumLength(255);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(20);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000);

            RuleFor(x => x.Slogan)
                .MaximumLength(255);

            RuleFor(x => x.BannerUrl)
                .MaximumLength(255);
        }
    }
}
