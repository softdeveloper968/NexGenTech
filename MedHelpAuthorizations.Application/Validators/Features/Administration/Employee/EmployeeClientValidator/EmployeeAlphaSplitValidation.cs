using FluentValidation;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Commands.AddEdit;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.Employee.EmployeeClientValidator
{
    public class EmployeeAlphaSplitValidation : AbstractValidator<EmployeeClientAlphaSplit>
    {
        public EmployeeAlphaSplitValidation()
        {
            RuleFor(requests => requests.CustomBeginAlpha)
                           .Must(x => x != null).WithMessage("StartRange Is Required!");

            RuleFor(requests => requests.CustomEndAlpha)
                           .Must(x => x != null).WithMessage("StartRange Is Required!");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<EmployeeClientAlphaSplit>.CreateWithOptions((EmployeeClientAlphaSplit)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}