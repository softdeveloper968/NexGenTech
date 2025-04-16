using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class AuthorizationFilterSpecification : HeroSpecification<Authorization>
    {
        public AuthorizationFilterSpecification(GetAllAuthorizationsQuery query)
        {
            Includes.Add(a => a.AuthType);
            Includes.Add(a => a.Documents);
            Criteria = p => true;
            if (query.ClientId > 0)
                Criteria = Criteria.And(p => p.ClientId == query.ClientId);

            if (!string.IsNullOrEmpty(query.SearchString))
            {
                Criteria = Criteria.And(p => p.AuthNumber.ToLower().Contains(query.SearchString));
            }           
        }
    }
}