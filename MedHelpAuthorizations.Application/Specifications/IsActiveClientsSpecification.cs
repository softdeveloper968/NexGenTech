using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class IsActiveClientsSpecification<T> : HeroSpecification<T> where T : class, IClientRelationship, IEntity
    {
        public IsActiveClientsSpecification()
        {
            Criteria = b => true;
            Criteria = Criteria.And(b => b.Client.IsActive);
        }
    }
}
    