using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class GenericNameSearchSpecification<T> : HeroSpecification<T> 
        where T : class, IName, IEntity
    {
        public GenericNameSearchSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = n => n.Name != null && ( n.Name.Contains(searchString));
            }
            else
            {
                Criteria = i => true;
            }
        }
    }
}
