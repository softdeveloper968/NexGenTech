using FluentValidation;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.ClientReportFilters
{
    public class AddEditClientReportFiltersCommandValidator : AbstractValidator<AddEditClientReportFiltersCommand>
    {
        public AddEditClientReportFiltersCommandValidator()
        {
            RuleFor(rpt => rpt.FilterName)
                    .NotEmpty()
                        .WithMessage("FilterName is Required!");
            RuleFor(rpt => rpt.FilterConfiguration)
                    .NotEmpty()
                        .WithMessage("Filter Configurations are Required.");
            RuleFor(rpt => rpt.ReportId)
                    .NotEmpty()
                        .WithMessage("Report type is Required.");
                    //.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Note Content must be with-in 250 characters");
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditClientReportFiltersCommand>.CreateWithOptions((AddEditClientReportFiltersCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
