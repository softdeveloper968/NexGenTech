using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Questionnaire : AuditableEntity
    {

        public Questionnaire()
        {
            ClientQuestionnaireCategories = new HashSet<ClientQuestionnaireCategory>();
        }
        //TODo: Perhaps relate it to a Payer eventually
        public string Name { get; set; }
        public string Description { get; set; }
        public StateEnum RelatedState { get; set; } = StateEnum.UNK; // Unknown
        public virtual ICollection<ClientQuestionnaireCategory> ClientQuestionnaireCategories { get; set; }
    }
}
