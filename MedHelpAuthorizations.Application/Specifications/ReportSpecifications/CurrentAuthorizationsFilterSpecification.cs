
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq;
using MedHelpAuthorizations.Application.Features.Reports.GetCurrentAuthorizations;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Specifications.ReportSpecifications
{
    public class CurrentAuthorizationsFilterSpecification : HeroSpecification<Authorization>
    {
        public CurrentAuthorizationsFilterSpecification(GetPagedCurrentAuthorizationsQuery query)
        {
            Includes.Add(a => a.AuthType);
            Includes.Add(a => a.Patient);
            //Includes.Add(a => a.Patient.ClientInsurance);
            Includes.Add(a => a.Notes);

            Criteria = p => true;

            if (query.ClientId > 0)
                Criteria = Criteria.And(p => p.ClientId == query.ClientId);

            //Filter only auths where a patient is active

            //Filter out auths where concurrent auth has been obtained. 
            Criteria = Criteria.And(p => !p.InitialAuthorizations.Any());

            Criteria = Criteria.And(p => p.CompleteDate == null
                && (p.StartDate != null)
                && (p.EndDate != null && p.EndDate <= System.DateTime.UtcNow)
                && (p.DischargedOn == null));
        }
    }
}
