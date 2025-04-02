using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetById
{
    public class GetClientQuestionnaireByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ClientQuestionnaire ClientQuestionnaire { get; set; }
    }
}
