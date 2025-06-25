using FluentValidation;
using PetTrack.ModelViews.ChatModels;

namespace PetTrack.ModelViews.Validators
{
    public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
    {
        public SendMessageRequestValidator()
        {
            RuleFor(x => x.ReceiverId)
                .NotEmpty().WithMessage("ReceiverId is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(1000).WithMessage("Content must be at most 1000 characters.");
        }
    }
}
