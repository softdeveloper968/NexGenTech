using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Specifications.ReportSpecifications
{
    public class ExpiringAuthorizationFilterSpecification : HeroSpecification<Authorization>
    {
        public ExpiringAuthorizationFilterSpecification(GetPagedExpiringAuthorizationsQuery query)
        {
            Includes.Add(a => a.AuthType);
            Criteria = p => true;

            if (query.ClientId > 0)
                Criteria = Criteria.And(p => p.ClientId == query.ClientId);
            //throw new ArgumentException("clientId must be greater than 0", "clientId");


            //Filter out auths where concurrent auth has been obtained. 
            Criteria = Criteria.And(p => !p.InitialAuthorizations.Any());

            Criteria = Criteria.And(p => p.CompleteDate == null 
                && (p.StartDate != null)
                && (p.EndDate != null && p.EndDate < query.RelativeDate.AddDays(query.ExpiringDays))
                && (p.DischargedOn == null || p.DischargedOn > query.RelativeDate));            
        }
    }
}
