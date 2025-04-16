using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedAuthTypesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}