using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ResponsibleParty : AuditableEntity<int>, IClientRelationship, IDataPipe<int>
	{
        public ResponsibleParty()
        {
            //InvoiceHistory = new HashSet<InvoiceHistory>();
            Patients = new HashSet<Patient>();
            PatientLedgerCharges = new HashSet<PatientLedgerCharge>();
        }

        [StringLength(7)]
        public string AccountNumber { get; private set; }
        
        [StringLength(25)]
        public string ExternalId { get; set; }
        
        [MaxLength(9)]
        public decimal? SocialSecurityNumber { get; set; }
        
        public int PersonId { get; set; }

        //public string TenantId { get; set; }

        public int ClientId { get; set; }

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }


		#region Navigation Objects

		[ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }

		public virtual ICollection<PatientLedgerCharge> PatientLedgerCharges { get; set; }

		//public virtual ICollection<InvoiceHistory> InvoiceHistory { get; set; }

		#endregion
	}
}
