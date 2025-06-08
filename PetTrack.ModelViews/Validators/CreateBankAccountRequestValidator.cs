using FluentValidation;
using PetTrack.ModelViews.BankAccountModels;

namespace PetTrack.ModelViews.Validators
{
    public class CreateBankAccountRequestValidator : AbstractValidator<CreateBankAccountRequest>
    {
        public CreateBankAccountRequestValidator()
        {
            RuleFor(x => x.BankName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BankNumber).NotEmpty().MaximumLength(50);
        }
    }
}
