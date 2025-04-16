using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class UnmappedFeeScheduleSpecification : HeroSpecification<UnmappedFeeScheduleCpt>
    {
        public UnmappedFeeScheduleSpecification(string searchString)
        {
            IncludeStrings.Add("ClientCptCode");

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = Criteria.And(p =>
                        (p.ClientCptCode != null && !string.IsNullOrEmpty(p.ClientCptCode.Code) && p.ClientCptCode.Code.Contains(searchString)));
            }
        }
    }
}
