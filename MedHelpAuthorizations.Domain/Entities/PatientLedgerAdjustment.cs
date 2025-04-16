using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
	public class PatientLedgerAdjustment : AuditableEntity<int>, IClientRelationship, IDataPipe<int>
	{
		public int ClientId { get; set; }

		public int ClientAdjustmentCodeId { get; set; }

		public string DfExternalId { get; set; }

		public int PatientLedgerChargeId { get; set; }

		public string ClaimNumber { get; set; }

		public int? ClientRemittanceId { get; set; }

		public string? Description { get; set; }

		public decimal Amount { get; set; }

		public DateTime? DfCreatedOn{ get; set; }

		public DateTime? DfLastModifiedOn { get; set; }


		[ForeignKey("ClientId")]
		public virtual Client Client { get; set; }


		[ForeignKey("ClientAdjustmentCodeId")]
		public virtual ClientAdjustmentCode ClientAdjustmentCode { get; set; }


		[ForeignKey("PatientLedgerChargeId")]
		public virtual PatientLedgerCharge PatientLedgerCharge { get; set; }


		[ForeignKey("ClientRemittanceId")]
		public virtual ClientRemittance ClientRemittance { get; set; }
	}
}