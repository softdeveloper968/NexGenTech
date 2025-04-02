using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class SearchUnmappedFeeScheduleCptSpecification : HeroSpecification<UnmappedFeeScheduleCpt>
    {
        public SearchUnmappedFeeScheduleCptSpecification(string searchString)
        {
            IncludeStrings.Add("ClientCptCode");
            IncludeStrings.Add("ClientInsurance");

            Criteria = p => true;

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = Criteria.And(p =>
                         (p.ClientCptCode != null && !string.IsNullOrEmpty(p.ClientCptCode.Code) && p.ClientCptCode.Code.Contains(searchString)) ||
                         (p.ClientInsurance != null && !string.IsNullOrEmpty(p.ClientInsurance.Name) && p.ClientInsurance.Name.Contains(searchString)));
            }
        }
    }
}
