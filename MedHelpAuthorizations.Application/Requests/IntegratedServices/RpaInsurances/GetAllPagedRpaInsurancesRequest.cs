namespace MedHelpAuthorizations.Application.Requests.IntegratedServices.RpaInsurances
{
    public class GetAllPagedRpaInsurancesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}