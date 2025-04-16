using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientUserNotificationRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
