namespace MedHelpAuthorizations.Application.Features.Reports.ARAgingReport
{
    public class ARAgingInsurance
    {
        public string InsuranceTitle { get; set; }
        public int InsuranceId { get; set; }
        public int? LocationId { get; set; }
        public int? ProviderId { get; set; }
        public decimal AgeGroup_0_30 { get; set; } = 0.0m;
        public decimal AgeGroup_31_60 { get; set; } = 0.0m;
        public decimal AgeGroup_61_90 { get; set; } = 0.0m;
        public decimal AgeGroup_91_120 { get; set; } = 0.0m;
        public decimal AgeGroup_121_150 { get; set; } = 0.0m;
        public decimal AgeGroup_151_180 { get; set; } = 0.0m;
        public decimal AgeGroup_181_Plus { get; set; } = 0.0m;
        public decimal InsuranceTotal { get; set; } = 0.0m;
        public decimal TotalInsurancePercent { get; set; } = 0.0m;
    }
}
