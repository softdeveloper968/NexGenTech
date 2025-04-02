using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
