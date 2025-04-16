using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimStatusTrendsResponse : IClaimStatusTrendsResponse
    {
        public List<ClaimStatusTrendTotal> ClaimStatusTrendsChargeTotals { get; set; } = new List<ClaimStatusTrendTotal>();
        public List<ClaimStatusTrendTotal> ClaimStatusTrendsTotals { get; set; } = new List<ClaimStatusTrendTotal>();
    }
}