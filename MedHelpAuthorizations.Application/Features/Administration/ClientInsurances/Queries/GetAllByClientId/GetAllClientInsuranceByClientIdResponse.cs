using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId
{
    public class GetAllClientInsurancesByClientIdResponse : GetClientInsurancesBaseResponse
    {
        public int? RpaInsuranceGroupId { get; set; } //AA-23
    }
}
