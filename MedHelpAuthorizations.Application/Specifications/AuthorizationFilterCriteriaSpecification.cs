using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class AuthorizationFilterCriteriaSpecification : HeroSpecification<Authorization>
    {
        public AuthorizationFilterCriteriaSpecification(GetByCriteriaPagedAuthorizationsQuery query)
        {
            Includes.Add(a => a.AuthType);
            Includes.Add(a => a.Documents);
            Criteria = p => true;
            if (query.ClientId > 0)
                Criteria = Criteria.And(p => p.ClientId == query.ClientId);

            if (query.AuthTypeNames != null && query.AuthTypeNames.Length > 0)
            {
                if (query.AuthTypeNamesFilterType == Enums.AuthTypeNameFilterType.ThreeLetter)
                {
                    //TODO: THis looks kinda ugly. Perhaps split out the SUD IOP and the SUD OP into 2 types instead of combining so we dont have to do substrings and hope anything named in the future adheres to the first 4 matching.
                    Criteria = Criteria.And(p => p.AuthType.Name.StartsWith("MH") || query.AuthTypeNames.Contains(p.AuthType.Name.Substring(0, 4).TrimEnd().ToUpper()));
                }
                else
                {
                    //fullname filter for auth dashboard
                    Criteria = Criteria.And(p => query.AuthTypeNames.Contains(p.AuthType.Name.Trim().ToUpper()));
                }
            }

            if (query.AuthorizationStatuses != null && query.AuthorizationStatuses.Length > 0)
            {
                Criteria = Criteria.And(p => query.AuthorizationStatuses.Contains(p.AuthorizationStatusId));

            }

            if(query.QueryStateType == QueryStateTypeEnum.Active)
            {
                Criteria = Criteria.And(p => p.EndDate.Value.Date >= DateTime.Now.Date || (p.EndDate == null));
            }

            if (query.QueryStateType == Shared.Enums.QueryStateTypeEnum.Discharged)
            {
                Criteria = Criteria.And(p => (p.DischargedOn != null
                            && p.DischargedOn.Value.Year == DateTime.UtcNow.Year)
                        && (p.DischargedOn.Value.Month == DateTime.UtcNow.Month));
            }

            if (query.QueryStateType == Shared.Enums.QueryStateTypeEnum.NotCompleted)
            {
                Criteria = Criteria.And(p => p.CompleteDate == null && p.DischargedOn == null && p.EndDate > DateTime.UtcNow );
            }            
        }
    }
}
