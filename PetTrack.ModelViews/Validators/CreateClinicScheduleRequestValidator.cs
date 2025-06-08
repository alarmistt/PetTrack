using FluentValidation;
using PetTrack.ModelViews.ClinicScheduleModels;

namespace PetTrack.ModelViews.Validators
{
    public class CreateClinicScheduleRequestValidator : AbstractValidator<CreateClinicScheduleRequest>
    {
        public CreateClinicScheduleRequestValidator()
        {
            RuleFor(x => x.DayOfWeek)
                .InclusiveBetween(0, 6).WithMessage("DayOfWeek must be between 0 (Sunday) and 6 (Saturday)");

            RuleFor(x => x.OpenTime)
                .LessThan(x => x.CloseTime).WithMessage("Open time must be earlier than close time");

            RuleFor(x => x)
                .Must(x => x.CloseTime > x.OpenTime)
                .WithMessage("Close time must be after open time.");

            RuleFor(x => x)
                .Must(x => x.OpenTime != x.CloseTime)
                .WithMessage("Open time and close time must not be the same");

            RuleFor(x => (x.CloseTime - x.OpenTime).TotalMinutes)
                .GreaterThanOrEqualTo(60)
                .WithMessage("Working duration must be at least 1 hour")
                .Must(minutes => minutes % 60 == 0)
                .WithMessage("Working duration must be a multiple of 1 hour (e.g. 2h, 3h, etc)");
        }
    }
}
