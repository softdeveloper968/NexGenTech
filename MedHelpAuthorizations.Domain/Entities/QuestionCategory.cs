using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Indicates what section of a questionnaire (grouping of questions) the question is a part of on a questionnaire
    /// </summary>
    public class QuestionCategory : AuditableEntity<QuestionCategoryEnum>
    {
        public QuestionCategory()
        {
            ClientQuestionnaireCategories = new HashSet<ClientQuestionnaireCategory>();
            ClientQuestionnaireQuestions = new HashSet<ClientQuestionnaireCategoryQuestion>();
        }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        //public string TenantId { get; set; }

        public virtual ICollection<ClientQuestionnaireCategory> ClientQuestionnaireCategories { get; set; }
        public virtual ICollection<ClientQuestionnaireCategoryQuestion> ClientQuestionnaireQuestions { get; set; }
    }
}
