using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Shared.Enums;
using System.Linq;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ClientByEligibilitySpecificationCriteria : HeroSpecification<Domain.Entities.Client>
    {
        public ClientByEligibilitySpecificationCriteria() 
        {
            Includes.Add(c => c.ClientApiIntegrationKeys);

            Criteria = c => true;

            Criteria = Criteria.And(c => c.ClientApiIntegrationKeys.Any(ck => ck.ApiIntegrationId == ApiIntegrationEnum.SelfPayEligibility && ck.IsActive));
            //Criteria = Criteria.And(c => c.ClientCode != "Client123" && c.ClientCode != "default" && c.ClientCode != "Demo");
        }
    }
}
