namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class AuthTypesEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/AuthType/paginated?pageNumber={pageNumber}&pageSize={pageSize}";
        }
        public static string GetById(int id)
        {
            return $"api/v1/tenant/AuthType/{id}";
        }

        public static string GetAll = "api/v1/tenant/AuthType";
        public static string GetCount = "api/v1/tenant/AuthType/count";  
        public static string GetAllClientAuthTypeByAuthType = "api/v1/tenant/AuthType/GetAllClientAuthTypeByAuthType";  
        public static string Save = "api/v1/tenant/AuthType";
        public static string Delete = "api/v1/tenant/AuthType";
        public static string Export = "api/v1/tenant/AuthType/export";
    }
}