using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Enums;
using System;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusClaimByClientIdFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {

        public ClaimStatusClaimByClientIdFilterSpecification(int clientId)
        {
            Criteria = bc => true;

            Criteria = Criteria.And(bc => bc.ClientId == clientId); 
        }
    }
}
