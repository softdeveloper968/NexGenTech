using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Documents
{
    public class GetAllPagedDocumentsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}