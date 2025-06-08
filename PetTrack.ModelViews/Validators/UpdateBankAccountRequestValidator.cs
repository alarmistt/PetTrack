using FluentValidation;
using PetTrack.ModelViews.BankAccountModels;

namespace PetTrack.ModelViews.Validators
{
    public class UpdateBankAccountRequestValidator : AbstractValidator<UpdateBankAccountRequest>
    {
        public UpdateBankAccountRequestValidator()
        {
            RuleFor(x => x.BankName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BankNumber).NotEmpty().MaximumLength(50);
        }
    }
}
