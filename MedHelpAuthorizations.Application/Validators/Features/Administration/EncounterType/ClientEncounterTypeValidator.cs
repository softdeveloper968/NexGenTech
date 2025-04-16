using MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Commands.AddEdit;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.EncounterType
{
    public class ClientEncounterTypeValidator : AbstractValidator<AddEditClientEncounterTypeCommand>
    {
        public ClientEncounterTypeValidator(IStringLocalizer<AddEditClientEncounterTypeCommand> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
        }
    }
}
