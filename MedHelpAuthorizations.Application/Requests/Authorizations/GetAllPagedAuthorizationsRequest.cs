using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Authorizations
{
    public class GetAllPagedAuthorizationsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}