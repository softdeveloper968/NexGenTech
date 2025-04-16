using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientLocationServiceTypesByClientIdSpecification : HeroSpecification<ClientLocationTypeOfService>
    {
        public ClientLocationServiceTypesByClientIdSpecification(GetClientLocationServiceTypesQuery filter, int clientId)
        {
            Includes.Add(x => x.TypeOfService);
            Includes.Add(z => z.ClientLocation);
            
            Criteria = p => p.ClientId == clientId;
            
            if (filter.LocationId > 0)
            {
                Criteria = p => p.ClientLocationId == filter.LocationId;
            }

        }
    }
}
