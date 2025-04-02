namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public static class ClaimStatusTransactionsEndpoint
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ClaimStatusTransaction/";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/ClaimStatusTransaction/{id}";
        }

        public static string Save = "api/v1/tenant/ClaimStatusTransaction/Upsert";
        public static string Delete = "api/v1/tenant/ClaimStatusTransaction";
    }
}