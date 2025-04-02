
namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    class ClientLevelKpiEndpoints
    {
        public static string Save = "api/v1/tenant/ClientLevelKpis";

        public static string GetByClientId(int id)
        {
            return $"api/v1/tenant/ClientLevelKpis/{id}";
        }
    }
}
