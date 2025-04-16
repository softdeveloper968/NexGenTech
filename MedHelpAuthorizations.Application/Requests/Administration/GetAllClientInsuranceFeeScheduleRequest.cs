using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllClientInsuranceFeeScheduleRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int ClientInsuranceId { get; set; }
    }
}
