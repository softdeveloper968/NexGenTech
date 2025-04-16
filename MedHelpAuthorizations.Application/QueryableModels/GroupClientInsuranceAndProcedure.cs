using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.QueryableModels
{
    public class GroupInsLookupNameProcedureCode
    {
        public string LookupName { get; set; }
        public string ProcedureCode { get; set; }
    }

    public class GroupInsLookupNameProcedureCodeLineItemStatus : GroupInsLookupNameProcedureCode
    {
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
    }

    public class GroupInsLookupNameProcedureCodeLineItemStatusDenialCategory : GroupInsLookupNameProcedureCodeLineItemStatus
    {
        public string ClaimStatusExceptionReasonCategory { get; set; }
    }
}
