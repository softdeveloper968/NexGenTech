using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetById
{
    public class GetInsuranceByIdResponse : GetClientInsurancesBaseResponse
    {
        public int? RpaInsuranceId { get; set; }
    }
}