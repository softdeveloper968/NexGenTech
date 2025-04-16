using FluentValidation;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Commands.AddEdit;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration
{
    public class AddEditClientCptCodeCommandValidator : AbstractValidator<AddEditClientCptCodeCommand>
    {
        public AddEditClientCptCodeCommandValidator(IStringLocalizer<AddEditClientCptCodeCommandValidator> localizer)
        {
            RuleFor(request => request.LookupName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["LookupName is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            RuleFor(request => request.ShortDescription)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["ShortDescription is required!"]);
            RuleFor(request => request.Code)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Code is required!"])
                .Must(x => !string.IsNullOrWhiteSpace(x) && x.Length == 5).WithMessage(x => localizer["Code must be 5 characters"]);
            RuleFor(request => request.ScheduledFee)
                .Must(x => x != null).WithMessage(x => localizer["Scheduled Fee is required!"])
                .Must(x => (x >= 0)).WithMessage(x => localizer["Scheduled Fee must be >= $0.00!"]);
        }
    }
}
