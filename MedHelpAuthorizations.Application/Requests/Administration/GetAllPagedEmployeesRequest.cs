using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedEmployeesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
