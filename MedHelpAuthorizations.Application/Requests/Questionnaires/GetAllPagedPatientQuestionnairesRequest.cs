using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Questionnaires
{
    public class GetAllPagedPatientQuestionnairesRequest : PagedRequest
    {
        public int patientId { get; set; }
    }
}