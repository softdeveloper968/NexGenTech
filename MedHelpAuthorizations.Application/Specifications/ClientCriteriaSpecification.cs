using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetClientsByCriteria;
using MedHelpAuthorizations.Application.Specifications.Base;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientCriteriaSpecification : HeroSpecification<Domain.Entities.Client>
    {
        public ClientCriteriaSpecification(GetClientsByCriteriaQuery query)
        {
            Includes.Add(a => a.ClientAuthTypes);
            Includes.Add(a => a.ClientApplicationFeatures);
            Criteria = p => !string.IsNullOrWhiteSpace(p.Name);
            if (!string.IsNullOrEmpty(query.Name))
            {
                Criteria = p => p.Name.ToLower().Contains(query.Name.ToLower());
            }
        }
    }
}