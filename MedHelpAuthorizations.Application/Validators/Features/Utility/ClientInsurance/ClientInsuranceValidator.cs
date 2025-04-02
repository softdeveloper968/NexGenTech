using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Commands.AddEdit;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Validators.Features.Utility.ClientInsurance
{
    public class ClientInsuranceValidator: AbstractValidator<AddEditInsuranceCommand>
    {
        public ClientInsuranceValidator()
        {
            RuleFor(requests => requests.PayerIdentifier)
                .NotEmpty().When(requests => requests.RequirePayerIdentifier)
                .WithMessage("Payer Identifier is required");

        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditInsuranceCommand>.CreateWithOptions((AddEditInsuranceCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
