using FluentValidation;
using PetTrack.ModelViews.ServicePackageModels;

namespace PetTrack.ModelViews.Validators
{
    public class CreateServicePackageRequestValidator : AbstractValidator<CreateServicePackageRequest>
    {
        public CreateServicePackageRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Package name is required")
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
}
