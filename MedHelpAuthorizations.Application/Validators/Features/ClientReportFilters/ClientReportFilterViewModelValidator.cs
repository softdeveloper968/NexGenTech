using FluentValidation;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.ClientReportFilters
{
    public class ClientReportFilterViewModelValidator : AbstractValidator<DailyClaimReportFilterLayout>
    {
        public readonly int customDateRangeEnum = 30;
        public readonly bool isFilterClicked = false;
        public ClientReportFilterViewModelValidator(bool isFilterClick = false)
        {
            isFilterClicked = isFilterClick;
            RuleFor(rpt => rpt.PresetFilterTypeSelection)
                    .NotEmpty()
                        .WithMessage("Filter By is Required!");

            RuleFor(rpt => rpt.PresetDateRangeSelection)
                    .NotEmpty()
                        .WithMessage("Preset Date Range is Required!");
            //.Must((model, PresetDateRangeSelection) => !ValidateFilterNameUnique(model, (int)PresetDateRangeSelection))
            //    .WithMessage("Must Select a Valid Date Range!");

            RuleFor(rpt => rpt.CustomEnumDateRange)
                 .Must((model, CustomEnumDateRange) => !ValidateCustomDateRangeNotAllowed(model, (int)CustomEnumDateRange))
                         .WithMessage("Must be a relative range when saving filters.!");
        }
        private bool ValidateFilterNameUnique(DailyClaimReportFilterLayout model, int dateRangeTypeEnum)
        {
            // Check if the filter name is null/empty or already exists
            if (isFilterClicked && customDateRangeEnum == (int)model.CustomEnumDateRange)
            {
                return true;
            }
            return false;
        }
        private bool ValidateCustomDateRangeNotAllowed(DailyClaimReportFilterLayout model, int CustomEnumDateRange)
        {
            // Check if the filter name is null/empty or already exists
            if (isFilterClicked && customDateRangeEnum == (int)model.PresetDateRangeSelection)
            {
                return true;
            }
            return false;
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<DailyClaimReportFilterLayout>.CreateWithOptions((DailyClaimReportFilterLayout)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
