using FluentValidation;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.ClientFeeSchedule
{
    public class ClientFeeScheduleValidator : AbstractValidator<AddEditClientFeeScheduleCommand>
    {
        public ClientFeeScheduleValidator()
        {
            RuleFor(requests => requests.Name)
                       .Must(x => x != null).WithMessage("Name Is Required!");

            RuleFor(requests => requests.StartDate)
                        .LessThanOrEqualTo(requests => requests.EndDate)
                        .When(requests => requests.StartDate != null && requests.EndDate != null)
                        .WithMessage("StartDate must not be greater than EndDate");

            RuleFor(requests => requests.StartDate)
                .NotNull().WithMessage("StartDate Is Required!");

            RuleFor(requests => requests.EndDate)
                .NotNull().WithMessage("EndDate Is Required!");
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditClientFeeScheduleCommand>.CreateWithOptions((AddEditClientFeeScheduleCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
