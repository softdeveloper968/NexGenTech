using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientApiKeyRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int ClientId { get; set; }
    }
}
