using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientInsuranceFilterCriteriaSpecification : HeroSpecification<ClientInsurance>
    {
        public ClientInsuranceFilterCriteriaSpecification(GetByCriteriaPagedInsurancesQuery query)
        {
            //Includes.Add(a => a.AuthType);
            //Includes.Add(a => a.Documents);
            Criteria = p => true;
            Criteria = Criteria.And(p => p.ClientId == query.ClientId);            
        }
    }
}
