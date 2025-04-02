using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class GenericByClientIdSpecification<T> : HeroSpecification<T> 
        where T : class, IClientRelationship, IEntity
    {
        public GenericByClientIdSpecification(int clientId)
        {
            Criteria = p => p.ClientId == clientId;
        }
    }
}
