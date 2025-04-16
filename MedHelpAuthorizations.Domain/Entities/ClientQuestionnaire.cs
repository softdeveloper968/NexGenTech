using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientQuestionnaire : AuditableEntity<int>//, ITenant
    {

        public ClientQuestionnaire()
        {
            ClientQuestionnaireCategories = new HashSet<ClientQuestionnaireCategory>();
            PatientQuestionnaires = new HashSet<PatientQuestionnaire>();
        }
        //TODo: Perhaps relate it to a Payer eventually
        public string Name { get; set; }
        public string Description { get; set; }
        public StateEnum RelatedState { get; set; } = StateEnum.UNK; // Unknown
        public int ClientId { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public virtual ICollection<ClientQuestionnaireCategory> ClientQuestionnaireCategories { get; set; }
        public virtual ICollection<PatientQuestionnaire> PatientQuestionnaires { get; set; }
    }
}
