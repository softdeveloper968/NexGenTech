namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class AvgDayToPaySummary
    {
        public string Payer { get; set; }
        public double AvgDaysToBill { get; set; }
        public double AvgDaysToPay { get; set; }
    }
}