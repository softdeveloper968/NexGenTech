using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.SortDefinitions
{
    public static partial class SortDefinitions
    {
        public static string ClaimStatusBatchHistoriesPrefix = "ClaimStatusBatchHistories";
        public static Dictionary<string, string> ClaimStatusBatchHistoriesDefinitions = new Dictionary<string, string>()
        {
            {"id","Id" },
             { "claimStatusBatchId","ClaimStatusBatchId" },
            { "batchNumber","ClientInsuranceId" },
            { "authTypeId","AuthTypeId" },
            { "assignedClientRpaConfigurationId","AssignedClientRpaConfigurationId"},
            { "assignedDateTimeUtc","AssignedDateTimeUtc" },
            { "assignedToIpAddress","AssignedToIpAddress" },
            { "assignedToHostName","AssignedToHostName" },
            { "assignedToRpaCode","AssignedToRpaCode" },
            { "assignedToRpaLocalProcessIds","AssignedToRpaLocalProcessIds" },
            { "completedDateTimeUtc","CompletedDateTimeUtc" },
            { "abortedOnUtc","AbortedOnUtc" },
            { "abortedReason","AbortedReason" },
            { "reviewedOnUtc","ReviewedOnUtc" },
            { "reviewedBy","ReviewedBy" },
            { "allClaimStatusesResolvedOrExpired","AllClaimStatusesResolvedOrExpired" },
            { "clientId","ClientId"},
            { "createdBy","CreatedBy"},
            { "createdOn","CreatedOn"},
            { "lastModifiedBy","LastModifiedBy" },
            { "lastModifiedOn","LastModifiedOn" },
        };
    }
}
