using MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsByCriteria;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class PersonByIdFilterSpecification : HeroSpecification<Person>
    {
        public PersonByIdFilterSpecification(GetPersonsByCriteriaQuery query, int clientId)
        {
            
        }        
    }
}
