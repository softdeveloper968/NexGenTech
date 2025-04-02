using System;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimStatusDailyReportResponse
    {
        public DateTime ClaimBilledDate { get; set; }
        public int Reviewed { get; set; }
        public decimal ReviewedPercentage { get; set; }
        public int InProcess { get; set; }
        public decimal InProcessPercentage { get; set; }
        public int ApprovedPaid { get; set; }        
        public decimal ApprovePaidPercentage { get; set; }
        public int Denied { get; set; }
        public decimal DeniedPercentage { get; set; }
        public int NotAdjudicated { get; set; }
        public decimal NotAdjudicatedPercentage { get; set; }
        public int ZeroPaid { get; set; }
        public decimal ZeroPaidPercentage { get; set; }
        public decimal AllowedAmount { get; set; }
        public decimal AllowedAmountPercentage { get; set; }
    }
}
