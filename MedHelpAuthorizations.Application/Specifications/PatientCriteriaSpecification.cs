using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class PatientCriteriaSpecification : HeroSpecification<Patient>
    {
        public PatientCriteriaSpecification(GetPatientsByCriteriaQuery query, int clientId)
        {
            Includes.Add(p => p.Person);
            //Includes.Add(p => p.ClientInsurance);
            Criteria = p => p.ClientId == clientId;            
            if (!string.IsNullOrEmpty(query.FirstName))
            {
                Criteria = Criteria.And(p => p.Person.FirstName.ToLower().StartsWith(query.FirstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.LastName))
            {
                Criteria = Criteria.And(p => p.Person.LastName.ToLower().StartsWith(query.LastName.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.ExternalId))
            {
                Criteria = Criteria.And(p => p.ExternalId.ToLower().Contains(query.ExternalId.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.AccountNumber))
            {
                Criteria = Criteria.And(p => p.AccountNumber.ToLower().Contains(query.AccountNumber.ToLower()));
            }
            if (query.BirthDate != DateTime.MinValue && query.BirthDate != null)
            {
                Criteria = Criteria.And(p => p.Person.DateOfBirth == query.BirthDate);
            }
            if (!string.IsNullOrEmpty(query.InsurancePolicyNumber))
            {
                Criteria = Criteria.And(p => p.InsurancePolicyNumber.ToLower().Contains(query.InsurancePolicyNumber.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.InsuranceGroupNumber))
            {
                Criteria = Criteria.And(p => p.InsuranceGroupNumber.ToLower().Contains(query.InsuranceGroupNumber.ToLower()));
            } 
            if (!string.IsNullOrEmpty(query.SocialSecurityNumber)) //AA-218
            {
                Criteria = Criteria.And(p => p.Person.SocialSecurityNumber == query.SocialSecurityNumber);
            }
            if (query.IsAddedThisMonth != null && query.IsAddedThisMonth.Value == true)
            {
                Criteria = Criteria.And(x => x.CreatedOn.Year == DateTime.UtcNow.Year
                        && x.CreatedOn.Month == DateTime.UtcNow.Month);
            }
            if (query.IsActive != null && query.IsActive.Value == true)
            {
                Criteria = Criteria.And(x => x.Authorizations.Any(z => (z.EndDate.Value.Date >= DateTime.Now.Date) || (z.EndDate == null)));
            }
            if (query.BenefitsNotChecked == true && query.IsAddedThisMonth.Value == true)
            {
                Criteria = Criteria.And(x => x.BenefitsCheckedOn == null);
            }
        }
    }
}