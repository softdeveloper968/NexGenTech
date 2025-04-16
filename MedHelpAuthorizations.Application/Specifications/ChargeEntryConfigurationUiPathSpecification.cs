using System.Linq;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Application.ReadOnlyObjects;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ChargeEntryConfigurationUiPathSpecification : HeroSpecification<ChargeEntryRpaConfiguration>
    {
        public ChargeEntryConfigurationUiPathSpecification()
        {
            Criteria = x => true;

            Includes.Add(x => x.Client);
            Includes.Add(x => x.Client.ClientApplicationFeatures);
            Includes.Add(x => x.RpaType);
            Includes.Add(x => x.TransactionType);
            Includes.Add(x => x.AuthType);

            Criteria = Criteria.And(x => ReadOnlyObjects.ReadOnlyObjects.UiPathRpaTypes.Contains(x.RpaTypeId));
        }
    }
}
