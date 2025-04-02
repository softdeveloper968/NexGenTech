using System;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IClaimStatusDashboardStandardQuery : IClaimStatusDashboardQueryBase
    {
        public int ClaimStatusBatchId { get; set; }
        public int ClientLocationId { get; set; } 
        public int ClientProviderId { get; set; }
        public string ProviderName { get; set; }
        public string LocationName { get; set; }
        public DateTime? TransactionDateFrom { get; set; }
        public DateTime? TransactionDateTo { get; set; }
        public DateTime? ReceivedFrom { get; set; }
        public DateTime? ReceivedTo { get; set; }
        public int? PatientId { get; set; }

    }
}
