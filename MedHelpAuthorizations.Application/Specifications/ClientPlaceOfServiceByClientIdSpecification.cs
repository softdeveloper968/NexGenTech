using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    internal class ClientPlaceOfServiceByClientIdSpecification : HeroSpecification<ClientPlaceOfService>
    {
        public ClientPlaceOfServiceByClientIdSpecification(int clientId)
        {
            Criteria = p => p.ClientId == clientId;
        }
    }
}
