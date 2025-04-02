using System.Linq;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Enums;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ActiveChargeEntryConfigurationSpecification : HeroSpecification<ChargeEntryRpaConfiguration>
    {
        public ActiveChargeEntryConfigurationSpecification()
        {
            Criteria = x => true;

            Includes.Add(x => x.Client);
            Includes.Add(x => x.Client.ClientApplicationFeatures);
            Includes.Add(x => x.RpaType);
            Includes.Add(x => x.TransactionType);
            Includes.Add(x => x.AuthType);

            Criteria = Criteria.And(x => x.IsDeleted == false && x.Client.ClientApplicationFeatures.Any(y => y.ApplicationFeatureId == ApplicationFeatureEnum.ChargeEntry));
        }
    }
}
