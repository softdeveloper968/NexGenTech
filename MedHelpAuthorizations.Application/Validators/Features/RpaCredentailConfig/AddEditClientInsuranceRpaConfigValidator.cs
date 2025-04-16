using FluentValidation;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.RpaCredentailConfig
{
    internal class AddEditClientInsuranceRpaConfigValidator : AbstractValidator<GetAllClientInsuranceRpaConfigurationsResponse>
    {
        public AddEditClientInsuranceRpaConfigValidator()
        {
            RuleFor(requests => requests.ClientInsuranceId)
                       .Must(x => x > 0).WithMessage("ClientInsuranceId Is Required!");

            RuleFor(requests => requests.ClientRpaCredentialConfigId)
                       .Must(x => x > 0).WithMessage("ClientRpaCredentialConfigId Is Required!");

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<GetAllClientInsuranceRpaConfigurationsResponse>.CreateWithOptions((GetAllClientInsuranceRpaConfigurationsResponse)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
