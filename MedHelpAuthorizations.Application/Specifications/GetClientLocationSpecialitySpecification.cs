using MedHelpAuthorizations.Application.Features.Administration.ClientLocationSpeciality.Queries.GetClientLocationSpecilality;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class GetClientLocationSpecialitySpecification : HeroSpecification<ClientLocationSpeciality>
    {
        public GetClientLocationSpecialitySpecification(GetClientLocationSpecilalityQuery filter, int clientId)
        {
           // Includes.Add(x => x.Specialty);
            Includes.Add(z => z.ClientLocation);

            Criteria = p => p.ClientId == clientId;

            if (filter.LocationId > 0)
            {
                Criteria = p => p.ClientLocationId == filter.LocationId;
            }

        }
    }
}
