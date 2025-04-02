using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Commands.AddEdit;
using MedHelpAuthorizations.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.ClientLocation
{
    public class AddEditClientLocationCommandValidator : AbstractValidator<AddEditClientLocationCommand>
    {
        public AddEditClientLocationCommandValidator()
        {
            RuleFor(command => command.Name)
                           .Must(x => !string.IsNullOrEmpty(x)).WithMessage(ValidationTextHelper.LocationName);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditClientLocationCommand>.CreateWithOptions((AddEditClientLocationCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
