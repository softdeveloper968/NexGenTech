using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IClaimStatusDashboardQueryBase
    {
        public int ClaimStatusBatchId { get; set; }
        public int ClientLocationId { get; set; }
        public int ClientProviderId { get; set; }
        public string ProviderName { get; set; }
        public string LocationName { get; set; }
        public int? PatientId { get; set; }
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public DateTime? ClaimBilledFrom { get; set; }
        public DateTime? ClaimBilledTo { get; set; }
        public string CommaDelimitedLineItemStatusIds { get; set; }
        public DateGroupingTypeEnum? DateGroupingType { get; set; }
        public string ClientInsuranceIds { get; set; }// AA-120
        public string ExceptionReasonCategoryIds { get; set; }// AA-120
        public string AuthTypeIds { get; set; }// AA-120
        public string ProcedureCodes { get; set; }// AA-120
        public string ClientLocationIds { get; set; }// AA-120
        public string ClientProviderIds { get; set; }// AA-120
        public DateTime? ReceivedFrom { get; set; }
        public DateTime? ReceivedTo { get; set; }
        public DateTime? TransactionDateFrom { get; set; }
        public DateTime? TransactionDateTo { get; set; }
        public string FlattenedLineItemStatus { get; set; }
        public string DashboardType { get; set; }

        public bool HasGroupByKeySelector { get; set; }
        public ClaimStatusTypeEnum? ClaimStatusType { get; set; }
        public string ClaimStatusTypeValue { get; set; }
        public bool? HasProcedureDashboard { get; set; }
    }
}
