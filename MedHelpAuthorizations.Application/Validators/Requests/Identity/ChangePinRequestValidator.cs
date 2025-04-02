using MedHelpAuthorizations.Application.Requests.Identity;

namespace MedHelpAuthorizations.Application.Validators.Requests.Identity
{
    public class ChangePinRequestValidator : AbstractValidator<ChangePinRequest>
    {
        public ChangePinRequestValidator(IStringLocalizer<ChangePinRequestValidator> localizer)
        {
            RuleFor(request => request.Pin)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Current Pin is required!"]);

            RuleFor(request => request.NewPin)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Pin is required!"])
                .MaximumLength(5).WithMessage(localizer["Pin must be a 5 character Alpha-numeric value!"])
                .MinimumLength(5).WithMessage(localizer["Pin must be a 5 character Alpha-numeric value!"]);

            RuleFor(request => request.ConfirmNewPin)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Pin Confirmation is required!"])
                .Equal(request => request.NewPin).WithMessage(x => localizer["Pin don't match"]);
        }
    }
}
