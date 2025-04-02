using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.Base
{
    public class GetClaimStatusBatchesBaseResponse
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; }
        public string ClientCode { get; set; }
        public int ClientId { get; set; }
        public string ClientInsuranceLookupName { get; set; }
        public int RpaInsuranceId { get; set; }
        public string RpaInsuranceCode { get; set; }
        public int? AssignedClientRpaConfigurationId { get; set; }
        public int InputDocumentId { get; set; }
        public string AuthTypeName { get; set; }
        public int? AuthTypeId { get; set; }
        public DateTime? AssignedDateTimeUtc { get; set; }
        public string AssignedToIpAddress { get; set; }
        public string AssignedToHostName { get; set; }
        public string AssignedToRpaCode { get; set; }
        public string AssignedToRpaLocalProcessIds { get; set; }
        public DateTime? CompletedDateTimeUtc { get; set; }
        public DateTime? AbortedOnUtc { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int? Priority { get; set; }
        public int BatchAssignmentCount { get; set; }
        //public List<DateTime> BatchAssignmentDates = new();
        public bool IsDeleted { get; set; } = false;
        public string RpaGroupName { get; set; } //AA-316
        public int? ClientLocationId { get; set; }
        public List<int?> ClientLocationInsuranceIdentifier { get; set; }
    }
}
