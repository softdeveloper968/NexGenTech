namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public class ChargeEntryBatchesEndpoint
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ChargeEntryBatch";
        }

        public static string GetById(int id)
        {
            //api/v1/tenant/ChargeEntryBatch/3
            return $"api/v1/tenant/ChargeEntryBatch/{id}";
        }

        public static string GetAllUnprocessed()
        {
            return $"api/v1/tenant/ChargeEntryBatch/Unprocessed";
        }

        public static string GetRecentForClientId()
        {
            return $"api/v1/tenant/ChargeEntryBatch/GetRecent";
        }

        public static string GetNotCompleted()
        {
            return $"api/v1/tenant/ChargeEntryBatch/notCompleted";
        }
        public static string GetCompleted()
        {
            return $"api/v1/tenant/ChargeEntryBatch/completedCleanup";
        }

        public static string UpdateProcessStartDateTime()
        {
            return $"api/v1/tenant/ChargeEntryBatch/UpdateProcessStartDateTime";
        }
        public static string UpdateCompleted()
        {
            return $"api/v1/tenant/ChargeEntryBatch/updateCompleted";
        }
        public static string UpdateAborted()
        {
            return $"api/v1/tenant/ChargeEntryBatch/updateAborted";
        }

        public static string Save = "api/v1/tenant/claimStatusBatch";
        public static string Edit = "api/v1/tenant/claimStatusBatch";
        public static string Delete = "api/v1/tenant/claimStatusBatch";
    }
}
