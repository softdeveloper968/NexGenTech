using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetRpaAssignedInsurances;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetProcedureCodes;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Models.IntegratedServices.DashboardPresets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class ClientReportFilterViewModel
    {
        [Required]
        public ReportsEnum ReportName { get; set; }
        [Required]
        public ReportCategoryEnum ReportCategory { get; set; }
        public bool HasDailyClaimReport { get; set; } = false;
        public bool HasDefaultFilter { get; set; } = false;
        public bool HasCustomReports { get; set; } = false;//AA-204 - Used For Custom Reports
        public bool AutoRun { get; set; } = false;
        public PresetFilterTypeEnum? FilterBy { get; set; } = null;
        public CustomPresetDateRangesEnum? PresetDateRanges { get; set; } = null;
        public List<GetAllClientInsurancesByClientIdResponse> Insurances { get; set; } = null;
        public List<GetRpaAssignedInsurancesResponse> RPAInsurances { get; set; } = null;
        public List<int> ServiceTypes { get; set; } = null;
        public List<ClaimStatusExceptionReasonCategoryEnum?> DenialCategories { get; set; } = null;
        public List<GetClaimStatusClientProcedureCodeResponse> ProcedureCodes { get; set; } = null;
        public List<GetClientLocationsByClientIdResponse> ClientLocations { get; set; } = null;
        public List<GetAllProvidersResponse> ClientProviders { get; set; } = null;
        /// <summary>
        /// Used for Handling CustomReport payloads like Report Name, Type, Display Columns, Filter options, preview Report query...
        /// AA-204, AA-205
        /// </summary>
        public SaveCustomReportPayload SaveCustomReportPayloads { get; set; } = null;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ClientReportFilterViewModel)obj;

            return ReportName == other.ReportName &&
                   ReportCategory == other.ReportCategory &&
                   HasDefaultFilter == other.HasDefaultFilter &&
                   AutoRun == other.AutoRun &&
                   FilterBy == other.FilterBy &&
                   PresetDateRanges == other.PresetDateRanges &&
                   ReportHelper.AreListsEqual(Insurances, other.Insurances) &&
                   ReportHelper.AreListsEqual(ServiceTypes, other.ServiceTypes) &&
                   ReportHelper.AreListsEqual(DenialCategories, other.DenialCategories) &&
                   ReportHelper.AreListsEqual(ProcedureCodes, other.ProcedureCodes) &&
                   ReportHelper.AreListsEqual(ClientLocations, other.ClientLocations) &&
                   ReportHelper.AreListsEqual(ClientProviders, other.ClientProviders);
        }

        public override int GetHashCode()
        {
            // Implement GetHashCode() method if needed
            return base.GetHashCode();
        }

    }

}
