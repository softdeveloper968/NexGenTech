using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Questions in a ClientQuestionnaireCategory that is assigned to a questionnaire.
    /// Shows what order the question should be listed in the questioncategory 
    /// </summary>
    public class ClientQuestionnaireCategoryQuestion : AuditableEntity<int>//, ITenant
    {
        public ClientQuestionnaireCategoryQuestion()
        {
            ClientQuestionnaireCategoryQuestionOptions = new HashSet<ClientQuestionnaireCategoryQuestionOption>();
        }

        [Required]
        public string QuestionContent { get; set; }
        [Required]
        public int CategoryQuestionOrder { get; set; }
        public int ClientQuestionnaireCategoryId { get; set; }
       // public string TenantId { get; set; }

        [ForeignKey("ClientQuestionnaireCategoryId")]
        public virtual ClientQuestionnaireCategory ClientQuestionnaireCategory { get; set; }

        public virtual ICollection<ClientQuestionnaireCategoryQuestionOption> ClientQuestionnaireCategoryQuestionOptions { get; set; }
    }
}
