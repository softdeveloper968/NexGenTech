using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public class ClaimStatusBatchClaimsEndpoint
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ClaimStatusBatchClaim";
        }

        public static string GetById(int id)
        {
            //api/v1/tenant/ClaimStatusBatchClaim/3
            return $"api/v1/tenant/ClaimStatusBatchClaim/{id}";
        }

        public static string GetByBatchId(int claimStatusBatchId)
        {
            return $"api/v1/tenant/ClaimStatusBatchClaim/BatchId/{claimStatusBatchId}";
        }

        public static string GetClientProcedureCodes()
        {
            return $"api/v1/tenant/ClaimStatusBatchClaim/clientProcedureCodes";
        }

        //public static string GetQualifiedUnresolvedByBatchId(int claimStatusBatchId)
        //{
        //    return $"api/v1/tenant/ClaimStatusBatchClaim/QualifiedUnresolved/{claimStatusBatchId}";
        //}

        //public static string Save = "api/v1/tenant/ClaimStatusBatchClaim";
        public static string Edit = "api/v1/tenant/ClaimStatusBatchClaim";
        //public static string Delete = "api/v1/tenant/ClaimStatusBatchClaim";
    }
}
