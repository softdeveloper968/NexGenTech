using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class GetClaimsByUnmappedFeeScheduleSpecification : HeroSpecification<ClaimStatusBatchClaim>
	{
        /// <summary> //EN-264
        /// Specification to filter claims by client insurance, procedure code, client ID, and date of service year.
        /// </summary>
        /// <param name="clientInsuranceId">The ID of the client insurance.</param>
        /// <param name="procedurecode">The procedure code.</param>
        /// <param name="clientId">The ID of the client.</param>
        /// <param name="DateOfServiceYear">The year of the date of service.</param>
        public GetClaimsByUnmappedFeeScheduleSpecification(int clientInsuranceId, int? clientCptCodeId, int clientId, DateTime DateOfService) //cptCode ID
		{
			IncludeStrings.Add("ClaimStatusTransaction");
            IncludeStrings.Add("ClientCptCode");

            Criteria = bc => true;

			Criteria = Criteria.And(c => (c.ClientInsuranceId.HasValue && c.ClientInsuranceId == clientInsuranceId) &&
							 (c.DateOfServiceFrom.HasValue && c.DateOfServiceFrom.Value.Month == DateOfService.Month && c.DateOfServiceFrom.Value.Year == DateOfService.Year) &&
							 (c.ClientId > 0 &&c.ClientId == clientId) &&
							 (c.ClientCptCode != null && c.ClientCptCode.Id == clientCptCodeId) &&
							 ((c.ClaimStatusTransaction != null && c.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue) && c.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Paid));
		}
	}
}
