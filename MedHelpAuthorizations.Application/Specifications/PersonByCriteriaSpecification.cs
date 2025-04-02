using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsByCriteria;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class PersonByCriteriaSpecification : HeroSpecification<Person>
    {
        public PersonByCriteriaSpecification(GetPersonsByCriteriaQuery query, int clientId)
		{
			AddInclude(x => x.Address);

			Criteria = q => true;

			Criteria = Criteria.And(x => x.ClientId == clientId);

			if (!string.IsNullOrWhiteSpace(query.FirstName))
			{
				Criteria = Criteria.And(x => x.FirstName.Trim().ToLower() == query.FirstName.Trim().ToLower());
			}
			if (!string.IsNullOrWhiteSpace(query.LastName))
			{
				Criteria = Criteria.And(x => x.LastName.Trim().ToLower() == query.LastName.Trim().ToLower());
			}
			if (query.DateOfBirth != null)
			{
				Criteria = Criteria.And(x => x.DateOfBirth != null ? x.DateOfBirth.Value.Date == query.DateOfBirth.Value.Date : false);
			}
			//if (query.PhoneNumber != null)
			//{
			//	Criteria = Criteria.And(x => x.MobilePhoneNumber == query.PhoneNumber
			//			|| x.HomePhoneNumber == query.PhoneNumber
			//			|| x.OfficePhoneNumber == query.PhoneNumber);
			//}
			if (!string.IsNullOrWhiteSpace(query.Email))
			{
				Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.Email) ? x.Email.Trim().ToLower() == query.Email.Trim().ToLower() : false);
			}
			//if (query.AddressStreetLine1 != null)
			//{
			//	Criteria = Criteria.And(x => x.Address != null && x.Address.AddressStreetLine1 != null ? x.Address.AddressStreetLine1.Trim().ToLower() == query.AddressStreetLine1.Trim().ToLower() : false);
			//}
		}
    }
}
