using FluentValidation;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.Employee.AddEditEmployeeValidator
{
    public class AddEditEmployeeValidator : AbstractValidator<AddEditEmployeeCommand>
    {
        public AddEditEmployeeValidator()
        {
            //RuleFor(requests => requests.FirstName)
            //           .Must(x => x != null).WithMessage("FirstName Is Required!");

            //RuleFor(requests => requests.LastName)
            //           .Must(x => x != null).WithMessage("LastName Is Required!");

            RuleFor(requests => requests.EmployeeNumber)
                      .Must(x => x != null).WithMessage("Employee Number Is Required!");

            RuleFor(requests => requests.DefaultEmployeeRoleId)
                      .Must(x => x != null).WithMessage("EmployeeRole Is Required!");

            //RuleFor(requests => requests.ClaimCountRequired)
            //   .LessThanOrEqualTo(200)
            //   .WithMessage("ClaimCount cannot exceed more than 200.");

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditEmployeeCommand>.CreateWithOptions((AddEditEmployeeCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
