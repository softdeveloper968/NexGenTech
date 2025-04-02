using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    internal class InsuranceCardsByCardholderIdFilterSpecification : HeroSpecification<InsuranceCard>
    {
        public InsuranceCardsByCardholderIdFilterSpecification(int cardholderId, int clientId)
        {
            Includes.Add(x => x.Cardholder);
            Includes.Add(x => x.Insurance);
            Includes.Add(x => x.Patient);
            Criteria = p => p.ClientId == clientId && p.CardholderId == cardholderId;
        }
    }
}
