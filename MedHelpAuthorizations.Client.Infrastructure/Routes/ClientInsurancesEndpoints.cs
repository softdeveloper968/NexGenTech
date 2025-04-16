namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class ClientInsurancesEndpoints
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ClientInsurance";
        }
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/ClientInsurance/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}";
        }
        public static string GetBySearchString(string searchString)
        {
            return $"api/v1/tenant/ClientInsurance?searchString={searchString}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/ClientInsurance/{id}";
        }

        public static string GetByClientId(int clientId)
        {
            return $"api/v1/tenant/ClientInsurance/getByClientId/{clientId}";
        }
        public static string GetRpaAssigned= "api/v1/tenant/ClientInsurance/rpaAssigned";
        public static string GetCount = "api/v1/tenant/ClientInsurance/count";  
        public static string Save = "api/v1/tenant/ClientInsurance";
        public static string Delete = "api/v1/tenant/ClientInsurance";
        public static string Export = "api/v1/tenant/ClientInsurance/export";
        public static string GetProviderTotalsByPayer = "api/v1/tenant/ClientInsurance/GetProviderTotalsByPayer";
        public static string GetPaymentTotalsByPayer = "api/v1/tenant/ClientInsurance/GetPaymentTotalsByPayer";
        public static string GetDenialTotalsByPayer = "api/v1/tenant/ClientInsurance/GetDenialTotalsByPayer";
        public static string GetPayerTotal = "api/v1/tenant/ClientInsurance/GetPayerTotal";
    }
}