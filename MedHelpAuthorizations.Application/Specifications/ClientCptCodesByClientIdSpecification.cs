using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Extensions;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientCptCodesByClientIdSpecification : HeroSpecification<ClientCptCode>
    {
        public ClientCptCodesByClientIdSpecification(string searchString, int clientId)
        {
            
            Includes.Add(x => x.TypeOfService);
            Criteria = p => p.ClientId == clientId;

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = Criteria.And(p => p.Code.Contains(searchString) || p.Description.Contains(searchString) || p.ShortDescription.Contains(searchString) || p.ShortDescription.Contains(searchString));
            }
        }
    }
}
