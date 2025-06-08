using FluentValidation;
using PetTrack.ModelViews.ClinicModels;

namespace PetTrack.ModelViews.Validators
{
    public class CreateClinicRequestValidator : AbstractValidator<CreateClinicRequest>
    {
        public CreateClinicRequestValidator()
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

            RuleFor(x => x.Schedules)
                .NotEmpty().WithMessage("At least one schedule is required")
                .Must(s => s.Select(x => x.DayOfWeek).Distinct().Count() == s.Count)
                .WithMessage("Each day of the week must only appear once");

            RuleForEach(x => x.Schedules).SetValidator(new CreateClinicScheduleRequestValidator());

            RuleFor(x => x.ServicePackages)
                .NotEmpty().WithMessage("At least one service package is required");

            RuleForEach(x => x.ServicePackages).SetValidator(new CreateServicePackageRequestValidator());
        }
    }
}
