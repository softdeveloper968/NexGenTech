using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientCptCodesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
