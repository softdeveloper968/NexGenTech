using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientProviderBySearchStringSpecification : HeroSpecification<ClientProvider>
    {
        public ClientProviderBySearchStringSpecification(string searchString, int clientId)
        {
            Criteria = p => p.ClientId == clientId;

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = Criteria.And(p =>(p.Npi != null && p.Npi.Contains(searchString)) ||(p.Person != null && p.Person.LastName != null && p.Person.LastName.Contains(searchString)) || (p.Person != null && p.Person.FirstName != null && p.Person.FirstName.Contains(searchString)));

            }
        }
    }
}
