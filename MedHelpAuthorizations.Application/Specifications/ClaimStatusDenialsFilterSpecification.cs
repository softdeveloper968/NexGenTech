using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusDenialsFilterSpecification : HeroSpecification<ClaimStatusTransaction>
    {
        public ClaimStatusDenialsFilterSpecification()
        {
            Criteria = c => true;
            Criteria = Criteria.And(c => !c.IsDeleted);
            //Criteria = Criteria.And(d => !d.ExceptionReason.Contains("Voided"));
            Criteria = Criteria.And(d => d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Denied 
                || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Rejected
                || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.MemberNotFound
                || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.NotOnFile
                || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.UnMatchedProcedureCode
                || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Error
                || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Ignored);
        }
    }
}
