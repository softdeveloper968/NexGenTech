using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IClaimWorkstationDetailQuery : IClaimStatusDashboardStandardQuery
    {
        public DateTime? ClaimStatusTransactionChange { get; set; }
        public DateTime? LastClaimStatusCharged { get; set; }
        public PresetFilterTypeEnum? PresetFilterTypeSelectionType { get; set; }
        public ClaimLineItemStatusEnum? PreviousStatus { get; set; }
        public ClaimLineItemStatusEnum? CurrentStatus { get; set; }
        public ClaimWorkstationSearchOptions? ClaimWorkstationSearchOptions { get; set; }
        public DateTime? ClaimStatusTransactionChangeStartDate { get; set; }
        public string ClaimStatusCategory { get; set; }
        //public List<int?> ClientLocationIds { get; set; }
        //public List<int?> ClientInsuranceIds { get; set; }
        //public List<int?> ClientProviderIds { get; set; }

    }
}
