using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchRecentByClientIdSpecification : HeroSpecification<ClaimStatusBatch>
    {
        public ClaimStatusBatchRecentByClientIdSpecification(int clientId)
        {
            if (clientId > 0)
            {
                Criteria = x => x.ClientId == clientId;
                Criteria = Criteria.And(x => x.CreatedOn.Date >= System.DateTime.Today.AddYears(-1) && x.CreatedOn.Date <= System.DateTime.Today);
            }
        }
    }
}
