namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class AuditEndpoints
    {
        public static string GetCurrentUserTrails = "api/v1/tenant/audits";
        public static string DownloadFile = "api/v1/tenant/audits/export";
    }
}