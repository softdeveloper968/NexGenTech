using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    internal class ClientApiKeyByClientIdSpecification : HeroSpecification<ClientApiIntegrationKey>
    {
        public ClientApiKeyByClientIdSpecification(int clientId)
        {
            Criteria = p => p.ClientId == clientId;
        }
    }
}
