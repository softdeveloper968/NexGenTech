using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    internal class ClientLocationsByClientIdSpecification : HeroSpecification<ClientLocation>
    {
        public ClientLocationsByClientIdSpecification(int clientId)
        {
            Criteria = p => p.ClientId == clientId;
        }
    }

}
