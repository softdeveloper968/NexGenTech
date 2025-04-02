using MedHelpAuthorizations.Application.Requests.Administration;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static  class ClientApiKeyEndPoints
    {
        public static string GetByClientId(int clientId)
        {
            return $"api/v1/tenant/ClientApiKey/getByClientId/{clientId}";
        }

        public static string GetBySearchString(string searchString)
        {
            return $"api/v1/tenant/ClientApiKey?searchString={searchString}";
        }

        public static string GetAll()
        {
            return $"api/v1/tenant/ClientApiKey";
        }
        public static string GetAllPaged(int pageNumber, int pageSize, int clientId)
        {
            return $"api/v1/tenant/ClientApiKey/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&clientId={clientId}";
        }

        public static string GetAllPagedByClientId(GetAllPagedClientApiKeyRequest request)
        {
            return $"api/v1/tenant/ClientApiKey/criteria?pageNumber={request.PageNumber}&pageSize={request.PageSize}&clientId={request.ClientId}";
        }

        public static string Save = "api/v1/tenant/ClientApiKey";
        public static string Delete = "api/v1/tenant/ClientApiKey";

    }
}
