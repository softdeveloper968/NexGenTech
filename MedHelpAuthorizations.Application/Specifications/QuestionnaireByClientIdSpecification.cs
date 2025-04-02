using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class QuestionnaireByClientIdSpecification : HeroSpecification<ClientQuestionnaire>
    {
        public QuestionnaireByClientIdSpecification(int clientId)
        {
            IncludeStrings.Add("ClientQuestionnaireCategories.ClientQuestionnaireCategoryQuestions.ClientQuestionnaireCategoryQuestionOptions");
            //Includes.Add(q => q.ClientQuestionnaireCategories);
            //QuestionCategory
            IncludeStrings.Add("ClientQuestionnaireCategories.QuestionCategory");

            Criteria = q => q.ClientId == clientId;
        }
    }
}
