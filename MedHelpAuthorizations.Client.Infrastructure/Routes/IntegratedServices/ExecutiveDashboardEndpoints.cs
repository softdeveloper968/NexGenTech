namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public class ExecutiveDashboardEndpoints
    {
        public static string CurrentMonthCharges(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/CurrentMonthCharges?clientId={clientId}";
        }

        public static string CurrentMonthDenials(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/CurrentMonthDenials?clientId={clientId}";
        }

        public static string CurrentMonthPayments(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/CurrentMonthPayments?clientId={clientId}";
        }
        
        public static string GetCurrentAROverPercentageLocation(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/CurrentAROverPercentageLocation?clientId={clientId}";
        }

        public static string GetCurrentAROverNintyDaysPayerByLocation(int clientId, int locationId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/GetCurrentAROverNintyDaysPayer?clientId={clientId}&locationId={locationId}";
        }

        public static string GetCleanClaimRate(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/CleanClaimRate?clientId={clientId}";
        }

        public static string GetExecutiveDenialRate(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/DenialClaimRate?clientId={clientId}";
        }
        public static string GetMonthlyDaysInAR(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/MonthlyDaysInAR?clientId={clientId}";
        }
        
        public static string GetCurrentMonthEmployeeWork(int clientId)
        {
            return $"api/v1/tenant/ExecutiveDashboard/CurrentMonthEmployeeWork?clientId={clientId}";
        }
    }
}
