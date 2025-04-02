using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedLocationsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
