using FluentValidation;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.FeeScheduleEntry
{
    public class AddEditFeeScheduleEntryValidator : AbstractValidator<AddEditFeeScheduleEntryViewModel>
    {
        public AddEditFeeScheduleEntryValidator()
        {
            RuleFor(requests => requests.Fee)
                       .Must(x => x != null).WithMessage("Fee Is Required!");


            RuleFor(requests => requests.ClientCptCodeId)
                      .Must(x => x != null).WithMessage("Procedure Is Required!");

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditFeeScheduleEntryViewModel>.CreateWithOptions((AddEditFeeScheduleEntryViewModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
