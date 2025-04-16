using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class CptCode : AuditableEntity<int>
    {
        public CptCode()
        {
            //PatientLedgerCharge = new HashSet<PatientLedgerCharge>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string CodeVersion { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string RecId { get; set; }
        public TypeOfServiceEnum? TypeOfServiceId { get; set; }

        #region Navigation Objects

        //[ForeignKey("TypeOfServiceId")]
        //public virtual TypeOfService TypeOfService { get; set; }
        //public virtual ICollection<PatientLedgerCharge> PatientLedgerCharge { get; set; }

        #endregion
    }
}
