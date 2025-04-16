using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Acceptable answers for a given question in the Questionnaire
    /// </summary>
    public class ClientDefaultQuestionnaireAnswer : AuditableEntity
    {
        public ClientDefaultQuestionnaireAnswer()
        {

        }
        public int QuestionnaireQuestionId { get; set; }
        [StringLength(50)]
        public string Answer { get; set; }
        //public bool IsDefaultAnswer { get; set; } = false;

        [ForeignKey("QuestionnaireQuestionId")]
        public virtual QuestionnaireQuestion QuestionnaireQuestion { get; set; }
    }
}
