using MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAddresses;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class AddressByCriteriaSpecification : HeroSpecification<Address>
    {
        public AddressByCriteriaSpecification(GetAddressesByCriteriaQuery query, int clientId)
        {
        }
    }
}
