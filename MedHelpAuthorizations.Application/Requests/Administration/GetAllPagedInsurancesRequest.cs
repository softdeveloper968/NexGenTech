using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedInsurancesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}