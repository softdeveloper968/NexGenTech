using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Models.IntegratedServices.DashboardPresets;
using System.Collections.Generic;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.Base
{
    public class EncounterNoteFilter
    {
        public CustomPresetDateRangesEnum? DateOfServiceRangeEnum { get; set; }
        public CustomPresetDateRangesEnum? DateOfResultRangeEnum { get; set; }
        public IEnumerable<GetAllProvidersResponse> ProviderOptions { get; set; } = new List<GetAllProvidersResponse>();
        public List<GetClientLocationsByClientIdResponse> LocationOptions { get; set; } = new();
        public List<SpecialtyEnum> SelectedSpecialtyOptions { get; set; } = new();

        public EncounterDateRange DateOfServiceDateRange { get; set; }
        public EncounterDateRange DateOfResultDateRange { get; set; }
        public EncounterDashboardFilterTypeEnum EncounterDashboardFilterTypeEnum { get; set; } = EncounterDashboardFilterTypeEnum.ServiceDate;
    }
    public class EncounterDateRange
    {
        public EncounterDateRange() { }
        public EncounterDateRange(DateTime start, DateTime end) { Start = start; End = end; }
        public EncounterDateRange(DateTime? start, DateTime? end)
        {
            Start = start.HasValue ? start.Value : DateTime.MinValue;
            End = end.HasValue ? end.Value : DateTime.MinValue;
        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
