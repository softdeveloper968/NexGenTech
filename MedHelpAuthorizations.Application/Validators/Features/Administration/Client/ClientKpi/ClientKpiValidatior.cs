using FluentValidation;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Employee.EmployeeClientConfigValidator
{
    public class ClientKpiValidator : AbstractValidator<ClientKpiDto>
    {
        public ClientKpiValidator()
        {
            RuleFor(requests => requests.RemainingDailyClaimCount)
               .GreaterThanOrEqualTo(0)
               .WithMessage("Employee assigned daily claim count cannont exceed the clients remaining daily count.");
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ClientKpiDto>.CreateWithOptions((ClientKpiDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
