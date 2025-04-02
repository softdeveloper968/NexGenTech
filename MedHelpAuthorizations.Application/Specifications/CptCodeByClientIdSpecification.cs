using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class CptCodeByClientIdSpecification : HeroSpecification<ClientCptCode>
    {

        public CptCodeByClientIdSpecification(int clientId)
        {
            Criteria = bc => true;

            Criteria = Criteria.And(bc => bc.ClientId == clientId);
        }
    }
}
