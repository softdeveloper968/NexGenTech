namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ExportSummaryReportResponse
    {
        public string ExceptionReasonCategory { get; set; }
        public int ExceptionCount { get; set; } = 0;
        public decimal SumBilledAmount { get; set; } = 0.0m;
    }
}
