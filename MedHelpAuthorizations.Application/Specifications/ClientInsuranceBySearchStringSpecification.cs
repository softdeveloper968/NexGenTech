using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetBySearchString;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientInsuranceBySearchStringSpecification : HeroSpecification<ClientInsurance>
    {
        public ClientInsuranceBySearchStringSpecification(GetClientInsurancesBySearchStringQuery query, int clientId)
        {
            if (!string.IsNullOrWhiteSpace(query.SearchString))
            {
                Criteria = p => false;
            }
            else
            {
                query.SearchString = query.SearchString.ToUpper().Trim();
                Criteria = p => true;
                Criteria = Criteria.And(p => p.ClientId == clientId);
                Criteria = Criteria.And(p => p.Name.ToUpper().Contains(query.SearchString) || p.LookupName.ToUpper().Contains(query.SearchString));               
            }          
        }
    }
}
