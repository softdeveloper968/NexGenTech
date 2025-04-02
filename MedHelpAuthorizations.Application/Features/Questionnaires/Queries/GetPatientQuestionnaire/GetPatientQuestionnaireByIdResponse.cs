using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetPatientQuestionnaire
{
    public class GetPatientQuestionnaireByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ClientQuestionnaire ClientQuestionnaire { get; set; }
    }
}
