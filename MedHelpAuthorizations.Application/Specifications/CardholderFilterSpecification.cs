using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class CardholderFilterSpecification : HeroSpecification<Cardholder>
    {
        public CardholderFilterSpecification(string searchString, int clientId)
        {
            searchString = !string.IsNullOrWhiteSpace(searchString) ? searchString.ToUpper().Trim() : string.Empty;
            
            Includes.Add(x => x.Person);
            Includes.Add(x => x.Person.Address);
            Includes.Add(x => x.Person.Address.State);
            
            Criteria = p => p.Person.ClientId == clientId;

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (!string.IsNullOrWhiteSpace(p.Person.FirstName) && p.Person.FirstName.ToUpper().Trim().Contains(searchString)) 
                                    || (!string.IsNullOrWhiteSpace(p.Person.LastName) && p.Person.LastName.ToUpper().Trim().Contains(searchString));
            }
            else
            {
                Criteria = p => false;
            }
        }
    }
}
