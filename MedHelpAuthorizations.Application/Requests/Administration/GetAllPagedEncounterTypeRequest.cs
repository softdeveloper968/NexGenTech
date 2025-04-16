using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedEncounterTypeRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
