namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientLocationInsuranceIdentifierEndpoints
    {
        public static string GetAllByClientLocationId(int locationId)
        {
            return $"api/v1/tenant/ClientLocationInsuranceIdentifier/GetByLocationId?locationId={locationId}";
        }
        public static string GetById(int id)
        {
            return $"api/v1/tenant/ClientLocationInsuranceIdentifier/{id}";
        }
        public static string Save = "api/v1/tenant/ClientLocationInsuranceIdentifier";
        public static string Delete = "api/v1/tenant/ClientLocationInsuranceIdentifier";
    }
}
