using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientFeeSchduleRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
