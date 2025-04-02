using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientInsurancesRpaAssignedSpecification : HeroSpecification<ClientInsurance>
    {
        public ClientInsurancesRpaAssignedSpecification()
        {
            Criteria = i => true;
            Criteria = Criteria.And(i => i.RpaInsuranceId != null && i.RpaInsuranceId != 0);
        }
    }
}
