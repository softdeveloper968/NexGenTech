namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class ClientKpisEndpoints
    {
        public static string GetByClientId(int clientId)
        {
            return $"api/v1/tenant/clientKpi/getByClientId/{clientId}";
        }

        public static string Save = "api/v1/tenant/clientKpi";

        public static string GetBillingKpiByClientId = "api/v1/tenant/clientKpi/Billing";

    }
}
