using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;


namespace MedHelpAuthorizations.Application.Specifications
{
    public class GetClaimsByFeeScheduleCriteriaSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public GetClaimsByFeeScheduleCriteriaSpecification(List<int> clientInsuranceIds,
        int clientCptCodeId,
        DateTime? clientFeeScheduleStartDate,
        DateTime? clientFeeScheduleEndDate,
        List<SpecialtyEnum> specialtyIds = null,
        List<ProviderLevelEnum> providerLevelIds = null)
        {
            IncludeStrings.Add("ClientProvider");

            Criteria = bc => true;

            Criteria = Criteria.And(c => c.ClientInsuranceId != null && clientInsuranceIds.Contains(c.ClientInsuranceId ?? 0) && c.DateOfServiceFrom >= clientFeeScheduleStartDate
                                                   && c.DateOfServiceFrom <= clientFeeScheduleEndDate && c.ClientCptCodeId == clientCptCodeId && c.ClientFeeScheduleEntryId == null);

            if (providerLevelIds != null && providerLevelIds.Any())
            {
                Criteria = Criteria.And(c => providerLevelIds.Contains(c.ClientProvider.ProviderLevelId ?? new ProviderLevelEnum()));
            }

            if (specialtyIds != null && specialtyIds.Any())
            {
                Criteria = Criteria.And(c => specialtyIds.Contains(c.ClientProvider.SpecialtyId));
            }
        }
    }
}
