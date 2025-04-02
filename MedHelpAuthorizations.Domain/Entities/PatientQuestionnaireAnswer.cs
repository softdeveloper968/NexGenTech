    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class PatientQuestionnaireAnswer : AuditableEntity<int>//, ITenant
    {
        public PatientQuestionnaireAnswer()
        {

        }
        public bool CustomAnswer { get; set; }
        public int PatientQuestionnaireId { get; set; }
        public int ClientQuestionnaireCategoryQuestionId { get; set; }
        [StringLength(50)]
        public string Answer { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ClientQuestionnaireCategoryQuestionId")]
        public virtual ClientQuestionnaireCategoryQuestion ClientQuestionnaireCategoryQuestion { get; set; }

        [ForeignKey("PatientQuestionnaireId")]
        public virtual PatientQuestionnaire PatientQuestionnaire { get; set; }
    }
}
