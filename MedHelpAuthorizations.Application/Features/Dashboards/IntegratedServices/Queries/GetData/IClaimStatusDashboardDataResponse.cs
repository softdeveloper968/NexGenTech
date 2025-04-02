using System.Collections.Generic;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;

public interface IClaimStatusDashboardResponse
{
    List<ClaimStatusTotal> ClaimStatusTransactionTotals { get; set; }
    List<ClaimStatusTotal> ClaimStatusUploadedTotals { get; set; }
    List<ClaimStatusTotal> DenialReasonTotals { get; set; }
    List<ClaimStatusTotal> ClaimStatusInProcessTotals { get; set; }
    ClaimStatusDashboardData ClaimStatusDashboardData { get; set; }
    //List<ClaimStatusRevenueTotal> ClaimStatusRevenueTotals { get; set; }
}