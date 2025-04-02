using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Providers.GetByCriteria;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ProviderByCriteriaSpecification : HeroSpecification<ClientProvider>
    {
        public ProviderByCriteriaSpecification(GetProvidersByCriteriaQuery request)
        {
            AddInclude(x => x.Person);

            Criteria = q => true;

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.Person.FirstName) && x.Person.FirstName.Trim().ToLower().StartsWith(request.FirstName.ToString()));

            }
            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.Person.LastName) && x.Person.LastName.Trim().ToLower().StartsWith(request.LastName.ToString()));
            }           
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.Person.Email) && x.Person.Email.ToLower().Trim().StartsWith(request.Email.ToLower().Trim()));
            }          
            if (!string.IsNullOrWhiteSpace(request.OfficePhoneNumber))
            {
                Criteria = Criteria.And(x => x.Person.OfficePhoneNumber != null && x.Person.OfficePhoneNumber.ToString().StartsWith(request.OfficePhoneNumber));
            }
            if (!string.IsNullOrWhiteSpace(request.ExternalId))
            {
                Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.ExternalId) && x.ExternalId.Trim().StartsWith(request.ExternalId.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.AddressStreetLine1))
            {
                Criteria = Criteria.And(x => x.Person.Address != null && !string.IsNullOrWhiteSpace(x.Person.Address.AddressStreetLine1) && x.Person.Address.AddressStreetLine1.Trim().ToLower().StartsWith(request.AddressStreetLine1.Trim()));
            }
            if (request.SpecialtyId != null)
            {
                Criteria = Criteria.And(x => x.SpecialtyId == request.SpecialtyId);
            }
            if (!string.IsNullOrWhiteSpace(request.Npi))
            {
                Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.Npi) && x.Npi.Trim().StartsWith(request.Npi.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.TaxId))
            {
                Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.TaxId) && x.TaxId.Trim().StartsWith(request.TaxId.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.Upin))
            {
                Criteria = Criteria.And(x => !string.IsNullOrWhiteSpace(x.Upin) && x.Upin.Trim().StartsWith(request.ExternalId.Trim()));
            }
        }
    }
}
