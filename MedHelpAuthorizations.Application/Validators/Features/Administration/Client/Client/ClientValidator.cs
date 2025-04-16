using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Shared.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.Client.Client
{
    public class ClientValidator : AbstractValidator<AddEditClientCommand>
    {
        public ClientValidator()
        {
            RuleFor(command => command.Name)
                 .Must(x => !string.IsNullOrEmpty(x)).WithMessage(ValidationTextHelper.ClientName);

            RuleFor(command => command.ClientCode)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage(ValidationTextHelper.ClientCode);

            RuleFor(command => command.AutoLogMinutes)
           .GreaterThan(0)
           .When(x => x.AutoLogMinutes.HasValue)
           .WithMessage("AutoLogMinutes must be greater than 0 if provided.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditClientCommand>.CreateWithOptions((AddEditClientCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
