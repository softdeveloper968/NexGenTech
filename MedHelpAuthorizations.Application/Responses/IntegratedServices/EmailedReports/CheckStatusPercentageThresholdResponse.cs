namespace MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports
{
    public class CheckStatusPercentageThresholdResponse
    {
        public string ClientCode { get; set; }
        public int BatchId { get; set; }
        public string BatchNumber { get; set; }
        public string PayerName { get; set; }
        public string RpaInsuranceCode { get; set; }
        public string BatchClaimCount { get; set; }
        public int UnavailableCount { get; set; }
        public int NotFoundCount { get; set; }
        public string UnavailablePercentage { get; set; }
        public string NotFoundPercentage { get; set; }
        public int CountNotFoundUnavailable { get; set; }
        public string PercentNotFoundUnavailable { get; set; }
        public double FilterValue {  get; set; }
        public string CreatedOn { get; set; }
    }
}
