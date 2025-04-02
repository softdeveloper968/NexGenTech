using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class PatientFilterSpecification : HeroSpecification<Patient>
    {
        public PatientFilterSpecification(string searchString, int clientId)
        {
            //string ssn;
            //Int32.TryParse(searchString, out ssn);
            Criteria = p => p.ClientId == clientId;
            Includes.Add(x => x.Person);

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Person.LastName != null && (p.Person.SocialSecurityNumber.Contains(searchString) || p.Person.FirstName.Contains(searchString) || p.InsurancePolicyNumber.Contains(searchString) || p.InsuranceGroupNumber.Contains(searchString) || p.Person.LastName.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Person.LastName != null;
            }
        }
    }
}