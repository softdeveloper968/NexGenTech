using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class PatientWorkstationFilterSpecification : HeroSpecification<ClaimStatusTransaction>
    {
        public PatientWorkstationFilterSpecification(IClaimWorkstationDetailQuery query)
        {
            Includes.Add(t => t.ClaimStatusBatchClaim.Patient);
            Includes.Add(t => t.ClaimStatusBatchClaim.Patient.Person);

            ///Filter based on patient Id
            if (query.PatientId.HasValue && query.PatientId > 0)
            {
                Criteria = Criteria.And(claimStatusTransaction => claimStatusTransaction.ClaimStatusBatchClaim != null && claimStatusTransaction.ClaimStatusBatchClaim.Patient != null && claimStatusTransaction.ClaimStatusBatchClaim.Patient.Id > 0 && claimStatusTransaction.ClaimStatusBatchClaim.Patient.Id == query.PatientId);
            }
        }
    }
}
