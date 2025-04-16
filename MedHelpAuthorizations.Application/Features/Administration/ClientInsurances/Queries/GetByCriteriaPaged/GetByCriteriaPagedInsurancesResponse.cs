using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteriaPaged
{
    public class GetByCriteriaPagedInsurancesResponse : GetClientInsurancesBaseResponse
    {
        public int? RpaInsuranceId { get; set; }
    }
}