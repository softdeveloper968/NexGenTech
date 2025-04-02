using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusTotalReportSpecification : HeroSpecification<ClaimStatusTotalResult>
    {
        public ClaimStatusTotalReportSpecification(int clientId)
        {
            Includes.Add(c => c.ClientInsurance);
            Includes.Add(c => c.ClientLocation);
            Includes.Add(c => c.ClientProvider);
            Includes.Add(p => p.ClientProvider.Person);
            Includes.Add(c => c.ClientCptCode);

            Criteria = t => true;
            Criteria = Criteria.And(c => c.ClientId == clientId && !c.IsDeleted);
        }
    }
}
