using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public interface IClaimStatusTrendsResponse
    {
        List<ClaimStatusTrendTotal> ClaimStatusTrendsChargeTotals { get; set; }
        List<ClaimStatusTrendTotal> ClaimStatusTrendsTotals { get; set; }
    }
}