using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ChargeEntryBatchRecentByClientIdSpecification : HeroSpecification<ChargeEntryBatch>
    {
        public ChargeEntryBatchRecentByClientIdSpecification(int clientId)
        {
            Criteria = x => true;

            Includes.Add(x => x.ChargeEntryRpaConfiguration);
            Includes.Add(x => x.ChargeEntryRpaConfiguration.AuthType);

            Criteria = Criteria.And(x => x.CreatedOn.Date >= System.DateTime.Today.AddDays(-30) && x.CreatedOn.Date <= System.DateTime.Today);
            if (clientId > 0)
            {
                Criteria = x => x.ChargeEntryRpaConfiguration.ClientId == clientId;
            }
        }
    }
}
