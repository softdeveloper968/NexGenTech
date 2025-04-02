using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientNoteRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
