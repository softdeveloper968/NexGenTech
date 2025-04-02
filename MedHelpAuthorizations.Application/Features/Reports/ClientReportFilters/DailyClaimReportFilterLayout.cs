using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders;
using MedHelpAuthorizations.Shared.Models.IntegratedServices.DashboardPresets;
using System.Collections.Generic;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class DailyClaimReportFilterLayout
    {
        public CustomPresetDateRangesEnum PresetDateRangeSelection { get; set; } = CustomPresetDateRangesEnum.LastWeek;
        public PresetFilterTypeEnum PresetFilterTypeSelection { get; set; } = PresetFilterTypeEnum.BilledOnDate;
        public List<int> ClientRPAInsuranceOptions { get; set; } = new();
        public List<int> ClientExceptionReasonOptions { get; set; } = new();
        public List<int> ClientServiceTypeOptions { get; set; } = new();
        public List<GetClientLocationsByClientIdResponse> ClientLocationOptions { get; set; } = new();
        public List<GetAllProvidersResponse> ClientProviderOptions { get; set; } = new();
        public List<int> ClientProcedureCodeOptions { get; set; } = new();
        public bool PresetSelectionChanges { get; set; } = false;
        public CustomPresetDateRangesEnum CustomEnumDateRange { get; set; } = CustomPresetDateRangesEnum.DailyClaimReportCustomDateRange;//((PresetDateRangesEnum)30);
        public List<GetClientLocationsByClientIdResponse> Locations { get; set; } = new();
        public List<GetAllProvidersResponse> Providers { get; set; } = new();
        //public DateRange? CustomDateRange { get; set; } = null;
        //public DateRange DateRangeReceivedDates { get; set; } = new();
        //public DateRange DateRangeServiceDates { get; set; } = new();
        //public DateRange DateRangeTransactedDates { get; set; } = new();
        //public DateRange DateRangeBilledOnDates { get; set; } = new();

        public DateTime? CustomDateRangeStartDate { get; set; }
        public DateTime? CustomDateRangeEndDate { get; set; }
        public DateTime? DateRangeReceivedDatesStartDate { get; set; }
        public DateTime? DateRangeReceivedDatesEndDate { get; set; }
        public DateTime? DateRangeServiceDatesStartDate { get; set; }
        public DateTime? DateRangeServiceDatesEndDate { get; set; }
        public DateTime? DateRangeTransactedDatesStartDate { get; set; }
        public DateTime? DateRangeTransactedDatesEndDate { get; set; }
        public DateTime? DateRangeBilledOnDatesStartDate { get; set; }
        public DateTime? DateRangeBilledOnDatesEndDate { get; set; }



    }
}
