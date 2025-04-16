namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class CustomReportEndpoints
    {
        public static string GetFilterColumnsBasedOnReportType = "/api/v1/tenant/CustomReports/GetCustomReportTabDetails";
        public static string GetPreviewReportForClaimReportType = "/api/v1/tenant/CustomReports/GetPreviewReportForClaimReportType";
        public static string ExportPreviewReportQuery = "/api/v1/tenant/CustomReports/ExportPreviewReportQuery";

    }
}
