using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Assigns sections (categories) that make up the questionnaire.
    /// Shows what order the category should be listed in the questionnaire 
    /// </summary>
     
    public class ClientQuestionnaireCategory : AuditableEntity<int>//, ITenant
    {
        public ClientQuestionnaireCategory()
        {
            ClientQuestionnaireCategoryQuestions = new HashSet<ClientQuestionnaireCategoryQuestion>();
        }
        public int ClientQuestionnaireId { get; set; }
        public QuestionCategoryEnum QuestionCategoryId { get; set; }
        public int CategoryOrder { get; set; }
        //public int ClientId { get; set; }
        //[ForeignKey("ClientId")]
        //public virtual Client Client { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ClientQuestionnaireId")]
        public virtual ClientQuestionnaire ClientQuestionnaire { get; set; }

        [ForeignKey("QuestionCategoryId")]
        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual ICollection<ClientQuestionnaireCategoryQuestion> ClientQuestionnaireCategoryQuestions { get; set; }

    }
}
