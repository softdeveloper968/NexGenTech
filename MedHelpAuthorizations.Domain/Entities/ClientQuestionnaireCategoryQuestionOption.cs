using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Acceptable answers for a given question in the Questionnaire
    /// </summary>
    public class ClientQuestionnaireCategoryQuestionOption : AuditableEntity<int>//, ITenant
    {
        public ClientQuestionnaireCategoryQuestionOption()
        {

        }
        public int ClientQuestionnaireCategoryQuestionId { get; set; }
        [StringLength(50)]
        public string Answer { get; set; }
        public bool IsDefaultAnswer { get; set; } = false;
        //public string TenantId { get; set; }

        [ForeignKey("ClientQuestionnaireCategoryQuestionId")]
        public virtual ClientQuestionnaireCategoryQuestion ClientQuestionnaireCategoryQuestion { get; set; }
    }
}
