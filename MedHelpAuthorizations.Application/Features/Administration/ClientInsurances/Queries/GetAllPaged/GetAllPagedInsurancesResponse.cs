namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged
{
    public class GetAllPagedInsurancesResponse : GetClientInsurancesBaseResponse
    {
        public int? RpaInsuranceId { get; set; }
        public bool RequirePayerIdentifier { get; set; } = false;
    }
}
