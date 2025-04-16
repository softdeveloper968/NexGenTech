using FluentValidation;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.Employee.EmployeeClientValidator
{
    public class EmployeeClientValidator : AbstractValidator<AddEditEmployeeClientCommand>
    {
        public EmployeeClientValidator()
		{
			RuleFor(command => command.AssignedClientEmployeeRoles)
				.Must(roles => roles != null && roles.Any()).WithMessage("At least 1 assigned employee role is required.");

			RuleFor(command => command.Employee.RemainingAverageDailyClaim)
				.GreaterThanOrEqualTo(0).WithMessage("Employee assigned daily claim count cannot exceed remaining daily count.");

			RuleFor(command => command.AssignedAverageDailyClaimCount)
				.GreaterThanOrEqualTo(0).WithMessage("Employee assigned daily claim count must be greater than or equal to 0.");
		}
		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditEmployeeClientCommand>.CreateWithOptions((AddEditEmployeeClientCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
