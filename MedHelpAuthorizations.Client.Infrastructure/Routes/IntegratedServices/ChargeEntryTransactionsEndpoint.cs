namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public static class ChargeEntryTransactionsEndpoint
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ChargeEntryTransaction/";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/ChargeEntryTransaction/{id}";
        }

        public static string GetByChargeEntryBatchId(int chargeEntryBatchId)
        {
            return $"api/v1/tenant/ChargeEntryTransaction/ChargeEntryBatchId{chargeEntryBatchId}";
        }

        public static string Save = "api/v1/tenant/ChargeEntryTransaction/Upsert";
        public static string Delete = "api/v1/tenant/ChargeEntryTransaction";
    }
}