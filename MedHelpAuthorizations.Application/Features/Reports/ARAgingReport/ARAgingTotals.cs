namespace MedHelpAuthorizations.Application.Features.Reports.ARAgingReport
{
    public class ARAgingTotals
    {
        public string InsuranceTitle { get; set; }
        public int InsuranceId { get; set; }
        public int? LocationId { get; set; }
        public int? ProviderId { get; set; }
        public decimal InsuranceTotal { get; set; } = 0.0m;
        public decimal TotalInsurancePercent { get; set; } = 0.0m;
    }
}
