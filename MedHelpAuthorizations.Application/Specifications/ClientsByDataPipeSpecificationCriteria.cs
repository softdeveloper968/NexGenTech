using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Shared.Enums;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientsByDataPipeSpecificationCriteria : HeroSpecification<Domain.Entities.Client>
    {
        public ClientsByDataPipeSpecificationCriteria()
        {
            Includes.Add(c => c.ClientApplicationFeatures);
			Criteria = c => true;

            Criteria = Criteria.And(c => c.ClientApplicationFeatures.Any(cf => cf.ApplicationFeatureId == ApplicationFeatureEnum.DataPipe));
            //Criteria = Criteria.And(c => c.ClientCode != "Client123" && c.ClientCode != "default" && c.ClientCode != "Demo");
        }
    }
}
