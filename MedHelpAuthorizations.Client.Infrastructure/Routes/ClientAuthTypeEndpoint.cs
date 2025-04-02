
namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientAuthTypeEndpoint
    {
        public static string GetById(int id)
        {
            return $"{Get}/{id}";
        }
        public static string GetClientLocationServiceTypes(int locationId)
        {
            return $"api/v1/tenant/ClientAuthType/GetClientLocationServiceTypes?locationId={locationId}";
        }
        public static string GetByClientId()
        {
            return $"{Get}/ByClientId";
        }
        public static string Get = "api/v1/tenant/ClientAuthType";
        public static string Save = "api/v1/tenant/ClientAuthType";
        public static string Delete = "api/v1/tenant/ClientAuthType";
    }
}
