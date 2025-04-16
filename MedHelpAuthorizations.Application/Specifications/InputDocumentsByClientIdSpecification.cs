using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class InputDocumentsByClientIdSpecification : HeroSpecification<InputDocument>
    {
        public InputDocumentsByClientIdSpecification(int clientId)
        {
            Includes.Add(x => x.ClientInsurance);
            Includes.Add(x => x.InputDocumentType);
            Includes.Add(x => x.ClaimStatusBatches);

            Criteria = p => p.ClientId == clientId;
        }
    }
}
