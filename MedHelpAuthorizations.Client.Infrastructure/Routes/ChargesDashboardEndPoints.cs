namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ChargesDashboardEndPoints
    {
        public static string ExportCashProjectionByDay = "api/v1/tenant/Charges/exportCashProjection"; //AA-343
        public static string GetCashProjectionByDay = "api/v1/tenant/Charges";
        public static string GetCashValueForRevenueByDay = "api/v1/tenant/Charges/cashValue";
        public static string ExportCashValueForRevenueByDay = "api/v1/tenant/Charges/exportCashValueForRevenue";
    }
}
