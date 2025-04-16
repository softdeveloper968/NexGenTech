using FluentValidation;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.ClientReportFilters
{
    public class UpdateAndSaveAsClientReportFilterValidator : AbstractValidator<AddEditClientReportFiltersCommand>
    {
        public readonly List<GetClientReportFilterResponse> reportFilters;
        public readonly List<GetClientCustomReportFilterResponse> customReportFilters;
        public UpdateAndSaveAsClientReportFilterValidator(List<GetClientCustomReportFilterResponse> customReportFilters)
        {
            this.customReportFilters = customReportFilters;

            RuleFor(model => model.FilterName)
                .NotEmpty().WithMessage("FilterName is required!")
                .Length(1, 100).WithMessage("FilterName should be between 1 and 100 characters.")
                .Must((model, filterName) => !ValidateFilterNameUnique(model, filterName))
                    .WithMessage("FilterName already exists, please change it.");

            RuleFor(rpt => rpt.FilterConfiguration)
                    .NotEmpty()
                        .WithMessage("Filter Configurations are Required.");

            RuleFor(rpt => rpt.ReportId)
                    .NotEmpty()
                        .WithMessage("Report type is Required.");

        }
        public UpdateAndSaveAsClientReportFilterValidator(List<GetClientReportFilterResponse> reportFilters)
        {
            this.reportFilters = reportFilters;

            //RuleFor(command => command.FilterName)
            //    .NotEmpty().WithMessage("FilterName is required!")
            //    .Length(1, 100).WithMessage("FilterName should be between 1 and 100 characters.")
            //    .When(command => command.SaveAsNewFilter)
            //        .Must(ValidateFilterNameUnique).WithMessage("FilterName already exists, please change it.");
            RuleFor(model => model.FilterName)
                .NotEmpty().WithMessage("FilterName is required!")
                .Length(1, 100).WithMessage("FilterName should be between 1 and 100 characters.")
                .Must((model, filterName) => !ValidateFilterNameUnique(model, filterName))
                    .WithMessage("FilterName already exists, please change it.");

            RuleFor(rpt => rpt.FilterConfiguration)
                    .NotEmpty()
                        .WithMessage("Filter Configurations are Required.");

            RuleFor(rpt => rpt.ReportId)
                    .NotEmpty()
                        .WithMessage("Report type is Required.");

        }
        private bool ValidateFilterNameUnique(AddEditClientReportFiltersCommand model, string filterName)
        {
            if (model.SaveAsNewFilter)
            {
                // Check if the filter name is null/empty or already exists
                if (string.IsNullOrEmpty(filterName?.Trim()) ||
                    reportFilters.Any(filter => filter.FilterName.Equals(filterName)))
                {
                    return true;
                }
                return false;

            }
            else
            {
                return false;
            }
            return true;
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
