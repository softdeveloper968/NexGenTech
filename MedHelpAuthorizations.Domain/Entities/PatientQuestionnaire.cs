using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Acceptable answers for a given question in the Questionnaire
    /// </summary>
    public class PatientQuestionnaire : AuditableEntity<int>//, ITenant
    {
        public PatientQuestionnaire()
        {
            //PatientQuestionnairesAnswers = new HashSet<PatientQuestionnaireAnswer>();
        }

        public int ClientQuestionnaireId { get; set; }        
        public int? AuthorizationId { get; set; }
        public int PatientId { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("AuthorizationId")]
        public virtual Authorization Authorization { get; set; }

        [ForeignKey("ClientQuestionnaireId")]
        public virtual ClientQuestionnaire ClientQuestionnaire { get; set; }
        public virtual ICollection<PatientQuestionnaireAnswer> PatientQuestionnairesAnswers { get; set; }

        

    }
}
