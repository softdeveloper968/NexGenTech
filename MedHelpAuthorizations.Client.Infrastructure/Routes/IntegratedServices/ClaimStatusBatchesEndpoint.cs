
namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public class ClaimStatusBatchesEndpoint
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ClaimStatusBatch";
        }

        public static string GetById(int id)
        {
            //api/v1/tenant/ClaimStatusBatch/3
            return $"api/v1/tenant/ClaimStatusBatch/{id}";
        }

        public static string GetUnprocessedByInsuranceId(int rpaInsuranceId, bool isForInitialAnalysis = false)
        {
            return $"api/v1/tenant/ClaimStatusBatch/UnprocessedByInsurance?id={rpaInsuranceId}&isForInitialAnalysis={isForInitialAnalysis}";
        }
            
        public static string GetByRpaInsuranceId(int rpaInsuranceId, bool includeAssignedBatches, bool isForInitialAnalysis = false)
        {
            return $"api/v1/tenant/ClaimStatusBatch/GetByRpaInsuranceId?id={rpaInsuranceId}&includeAssignedBatches={includeAssignedBatches}&isForInitialAnalysis={isForInitialAnalysis}";
        }

        public static string GetAllUnprocessed(bool isForInitialAnalysis = false)
        {
            return $"api/v1/tenant/ClaimStatusBatch/Unprocessed?isForInitialAnalysis={isForInitialAnalysis}";
        }

        public static string GetRecentForClientId()
        {
            return $"api/v1/tenant/ClaimStatusBatch/GetRecent";
        }

        public static string GetNotCompletedCleanup(string hostname)
        {
            return $"api/v1/tenant/ClaimStatusBatch/notCompletedCleanup/{hostname}";
        }
        public static string GetCompletedCleanup(string hostname)
        {
            return $"api/v1/tenant/ClaimStatusBatch/completedCleanup/{hostname}";
        }

        public static string UpdateAssignment()
        {
            return $"api/v1/tenant/ClaimStatusBatch/updateAssignment";
        }

        public static string UnassignBatch(int id)
        {
            return $"api/v1/tenant/ClaimStatusBatch/unassignBatch/{id}";
        }

        public static string ClearRpaLocalProcessIds()
        {
            return $"api/v1/tenant/ClaimStatusBatch/clearProcessIds";
        }

        public static string UpdateCompleted()
        {
            return $"api/v1/tenant/ClaimStatusBatch/updateCompleted";
        }
        public static string UpdateAborted()
        {
            return $"api/v1/tenant/ClaimStatusBatch/updateAborted";
        }

        public static string Save = "api/v1/tenant/claimStatusBatch";
        public static string Edit = "api/v1/tenant/claimStatusBatch";
        public static string Delete = "api/v1/tenant/claimStatusBatch";
    }
}
