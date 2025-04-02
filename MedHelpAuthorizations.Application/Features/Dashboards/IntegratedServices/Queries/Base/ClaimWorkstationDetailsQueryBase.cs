using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public class ClaimWorkstationDetailsQueryBase : ClaimStatusDashboardQueryBase, IClaimWorkstationDetailQuery
    {
        public DateTime? ClaimStatusTransactionChange { get; set; }
        public DateTime? LastClaimStatusCharged { get; set; }
        public PresetFilterTypeEnum? PresetFilterTypeSelectionType { get; set; }
        public ClaimLineItemStatusEnum? PreviousStatus { get; set; }
        public ClaimLineItemStatusEnum? CurrentStatus { get; set; }
        public int ClaimStatusBatchId { get; set; }
        public ClaimWorkstationSearchOptions? ClaimWorkstationSearchOptions { get; set; }
        public DateTime? ClaimStatusTransactionChangeStartDate { get; set; }
        public string ClaimStatusCategory { get; set; } = null;
        //public List<int?> ClientLocationIds { get; set; } = null;
        //public List<int?> ClientInsuranceIds { get; set; } = null;
        //public List<int?> ClientProviderIds { get; set; } = null;

        //Take out When Implemented MultiSelect Location and Provider Globally
        // This is only here to avoid inheritence issues. 
        public int ClientLocationId { get; set; }
        public int ClientProviderId { get; set; }
        public string ProviderName { get; set; }
        public string LocationName { get; set; }
    }
}