using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetByCriteria;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    internal class GetByCritieriaResponsiblePartyQuerySpec : HeroSpecification<ResponsibleParty>
    {
        public GetByCritieriaResponsiblePartyQuerySpec(GetByCritieriaResponsiblePartyQuery request)
        {
            AddInclude(x => x.Person);

            Criteria = q => true;

            if (!string.IsNullOrWhiteSpace(request.PatientFirstName))
            {
                Criteria = Criteria.And(x => x.Patients.Any(y => y.Person.FirstName.Trim().ToLower().StartsWith(request.PatientFirstName.ToString())));

            }
            if (!string.IsNullOrWhiteSpace(request.PatientLastName))
            {
                Criteria = Criteria.And(x => x.Patients.Any(y => y.Person.LastName.Trim().ToLower().StartsWith(request.PatientLastName.ToString())));
                //query = query.SelectMany(r => r.Patients.Select(p => p.Person).Where(p.LastName.Trim().StartsWith(request.PatientLastName.Trim().ToLower())));
            }
            if (!string.IsNullOrWhiteSpace(request.ResponsiblePartyFirstName))
            {
                Criteria = Criteria.And(x => x.Person.FirstName.Trim().ToLower().StartsWith(request.ResponsiblePartyFirstName.Trim().ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(request.ResponsiblePartyLastName))
            {
                Criteria = Criteria.And(x => x.Person.LastName.Trim().ToLower().StartsWith(request.ResponsiblePartyLastName.Trim().ToLower()));
            }
            if (request.PatientAccountNumber != null)
            {
                Criteria = Criteria.And(x => x.Patients.Any(y => y.AccountNumber.StartsWith(request.PatientAccountNumber.ToString())));
            }
            if (request.ResponsiblePartyAccountNumber != null)
            {
                Criteria = Criteria.And(x => x.AccountNumber.StartsWith(request.ResponsiblePartyAccountNumber.ToString()));
            }
            if (request.DOB != null)
            {
                Criteria = Criteria.And(x => x.Person.DateOfBirth.ToString().StartsWith(request.DOB.ToString().Trim()));
            }
            if (request.PhoneNumber != null)
            {
                Criteria = Criteria.And(x => x.Person.MobilePhoneNumber.ToString().StartsWith(request.PhoneNumber) 
                        || x.Person.HomePhoneNumber.ToString().StartsWith(request.PhoneNumber) 
                        || x.Person.OfficePhoneNumber.ToString().StartsWith(request.PhoneNumber));
            }

            if (request.ExternalId != null)
            {
                Criteria = Criteria.And(x => x.ExternalId.StartsWith(request.ExternalId.ToString()));
            }
        }
    }
}
