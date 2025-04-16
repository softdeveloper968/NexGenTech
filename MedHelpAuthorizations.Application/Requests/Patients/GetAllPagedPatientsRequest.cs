using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Patients
{
    public class GetAllPagedPatientsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}