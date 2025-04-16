using System;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IClaimStatusDashboardInitialQuery : IClaimStatusDashboardQueryBase
    {
        public DateTime? TransactionDateFrom { get; set; }
        public DateTime? TransactionDateTo { get; set; }
        public DateTime? ReceivedFrom { get; set; }
        public DateTime? ReceivedTo { get; set; }
        public int? PatientId { get; set; }
    }
}
