using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.SortDefinitions
{
    public static partial class SortDefinitions
    {
        public static string ClaimStatusBatchesPrefix = "ClaimStatusBatches";
        public static Dictionary<string, string> ClaimStatusBatchesDefinitions = new Dictionary<string, string>()
        {
            {"clientCode","ClientCode" },
            {"batchNumber","BatchNumber" },
            { "payer","Payer" },
            { "rpa","RPA"},
            {"AssignedDateTimeUtc","AssignedDateTimeUtc" },
            {"AssignedToIpAddress","AssignedToIpAddress" },
            {"AssignedToHostName","AssignedToHostName" },
            {"AssignedToRpaLocalProcessIds","AssignedToRpaLocalProcessIds" },
            {"createdOn","CreatedOn" },
            {"lastModifiedOn","LastModifiedOn" },
            {"allClaimStatusesResolvedOrExpired","AllClaimStatusesResolvedOrExpired" },
            { "lineItems","LineItems"},
            {"completedDateTimeUtc","CompletedDateTimeUtc" },
            {"abortedOnUtc","AbortedOnUtc" },
            {"abortedReason","AbortedReason" }
        };
    }
}
