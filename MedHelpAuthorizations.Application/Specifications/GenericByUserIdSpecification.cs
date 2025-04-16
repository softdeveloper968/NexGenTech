using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class GenericByUserIdSpecification<T> : HeroSpecification<T> 
        where T : class, IUserRelationship, IEntity
    {
        public GenericByUserIdSpecification(string userId)
        {
            Criteria = p => p.UserId == userId;
        }
    }
}
