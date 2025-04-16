using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using System;

namespace MedHelpAuthorizations.Application.Features.Reports.DailyClaimReports
{
    public class DailyClaimStatusReportResponse : ClaimStatusDailyReportResponse
    {
        /// <summary>
        /// Total Claim Received or Total Uploaded claims.
        /// </summary>
        public int ClaimReceived { get; set; } = 0;
        public string ClaimDate { get; set; } = string.Empty;
        public decimal ChargeAmount { get; set; } = 0.0m;
    }
}
