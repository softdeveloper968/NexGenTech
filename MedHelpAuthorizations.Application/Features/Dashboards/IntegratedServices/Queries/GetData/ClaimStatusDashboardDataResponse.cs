using System.Collections.Generic;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimStatusDashboardResponse : IClaimStatusDashboardResponse
    {
        public List<ClaimStatusTotal> ClaimStatusTransactionTotals { get; set; } = new List<ClaimStatusTotal>();
        public List<ClaimStatusTotal> ClaimStatusUploadedTotals { get; set; } = new List<ClaimStatusTotal>();
        public List<ClaimStatusTotal> DenialReasonTotals { get; set; } = new List<ClaimStatusTotal>();
        public List<ClaimStatusTotal> ClaimStatusInProcessTotals { get; set; } = new List<ClaimStatusTotal>();
        //public List<ClaimStatusRevenueTotal> ClaimStatusRevenueTotals { get; set; } = new List<ClaimStatusRevenueTotal>();

        public ClaimStatusDashboardData ClaimStatusDashboardData { get; set; } = new ClaimStatusDashboardData();
    }
}