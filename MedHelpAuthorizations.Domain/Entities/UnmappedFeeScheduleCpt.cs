using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class UnmappedFeeScheduleCpt : AuditableEntity<int>, IClientRelationship
	{
		//public string ProcedureCode { get; set; }  //Include in lookup
		public int ClientInsuranceId { get; set; } //Include in lookup
		public int DateOfServiceYear { get; set; } //Include in lookup
		public int ClientId { get; set; }
        public decimal BilledAmount { get; set; }
        public int? ClientCptCodeId { get; set; }
        public DateTime ReferencedDateOfServiceFrom { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }

        [ForeignKey("ClientCptCodeId")]
        public virtual ClientCptCode ClientCptCode { get; set; }
    }
}
