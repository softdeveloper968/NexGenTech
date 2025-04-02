using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
	public class ClientRemittance : AuditableEntity<int>, IClientRelationship, IDataPipe<int>
	{
		public ClientRemittance() 
		{
			ClientClaimPayments = new HashSet<PatientLedgerPayment>();
		}

		public int? ClientInsuranceId { get; set; }

		public int ClientId { get; set; }

		public decimal? UndistributedAmount { get; set; }

		public decimal? PaymentAmount { get; set; }

		public string? CheckNumber { get; set; }

		public int? PatientId { get; set; }

		//TODO: Build out Enum 
		public string? RemittanceFormType { get; set; }

		//TODO: Build out Enum 
		public string? RemittanceSource { get; set; }

		public int? ClientLocationId { get; set; }

		public DateTime? CheckDate { get; set; }

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }

		[ForeignKey("ClientId")]
		public virtual Client Client { get; set; }


		[ForeignKey("ClientInsuranceId")]
		public virtual ClientInsurance ClientInsurance { get; set; }


		[ForeignKey("ClientLocationId")]
		public virtual ClientLocation ClientLocation { get; set; }


		[ForeignKey("PatientId")]
		public virtual Patient Patient { get; set; }

		public virtual ICollection<PatientLedgerPayment> ClientClaimPayments { get; set; }

	}
}
