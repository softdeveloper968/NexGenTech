using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class CardholderCriteriaSpecification : HeroSpecification<Cardholder>
    {
        public CardholderCriteriaSpecification(GetCardholdersByCriteriaQuery query, int clientId)
        {
            Includes.Add(c => c.Person);
            Includes.Add(c => c.Person.Address);
            Includes.Add(c => c.InsuranceCards);

            Criteria = p => p.Person.ClientId == clientId;
            if (!string.IsNullOrEmpty(query.FirstName))
            {
                Criteria = Criteria.And(p => p.Person.FirstName.ToLower().Contains(query.FirstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.LastName))
            {
                Criteria = Criteria.And(p => p.Person.LastName.ToLower().Contains(query.LastName.ToLower()));
            }
           
            if (query.BirthDate != DateTime.MinValue && query.BirthDate != null)
            {
                Criteria = Criteria.And(p => p.Person.DateOfBirth== query.BirthDate);
            }            
        }
    }
}
