using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientProvidersRequest: PagedRequest
    {
        public string SearchString { get; set; } = string.Empty;
    }
}
