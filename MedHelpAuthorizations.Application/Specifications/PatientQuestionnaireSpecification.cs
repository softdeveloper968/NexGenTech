using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class PatientQuestionnaireSpecification : HeroSpecification<PatientQuestionnaire>
    {
        public PatientQuestionnaireSpecification(int patientId)
        {
            //Includes.Add(q => q.ClientQuestionnaire);
            //AddInclude("ClientQuestionnaire.ClientQuestionnaireCategories.ClientQuestionnaireCategoryQuestions.ClientQuestionnaireCategoryQuestionOptions");
            AddInclude("PatientQuestionnairesAnswers.ClientQuestionnaireCategoryQuestion");
            //PatientQuestionnairesAnswers
            Criteria = q => q.PatientId == patientId;
        }
    }
}
