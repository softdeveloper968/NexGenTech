namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientReportFilterEndpoints
    {
        ///AA-157
        public static string GetClientReportFiltersByReportIdAndFilterName = "/api/v1/tenant/ClientReportFilter/GetClientReportFiltersByReportIdAndFilterName";
        public static string GetClientReportFiltersByReportId = "/api/v1/tenant/ClientReportFilter/GetClientReportFiltersByReportId";
        public static string GetCustomClientReportFilterDetailsByReportId = "/api/v1/tenant/ClientReportFilter/GetCustomClientReportFilterDetailsByReportId";
        public static string SaveClientReportFilter = "/api/v1/tenant/ClientReportFilter/Save";
        public static string SaveCustomClientReportFilters = "/api/v1/tenant/ClientReportFilter/SaveCustomClientReportFilters";
        public static string GetClientReportFilter = "/api/v1/tenant/ClientReportFilter/GetClientReportFiltersByClientId";
        public static string DeleteClientReportFilterById(int reportFilterId)
        {
            return $"/api/v1/tenant/ClientReportFilter/{reportFilterId}";
        }
        public static string DeleteSharedClientReportFilterById(int employeClientUserReportFilterId)
        {
            return $"/api/v1/tenant/ClientReportFilter/DeleteSharedClientReportFilterById/{employeClientUserReportFilterId}";
        }

    }
}
