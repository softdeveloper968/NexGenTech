using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
	public class PatientLedgerCharge : AuditableEntity<int>, IClientRelationship, IDataPipe<int>
	{
		public int ClientId { get; set; }

		public string DfExternalId { get; set; }

		public string ClaimNumber { get; set; }

		public int PatientId { get; set; }

		public int ResponsiblePartyId { get; set; }

		//public string? ProcedureCode { get; set; }

		//public string? PlaceOfServiceCode { get; set; }
		public int ClientCptCodeId { get; set; }

		public int? ClientPlaceOfServiceId { get; set; }

		public int Quantity { get; set; }

		public decimal ChargeAmount { get; set; }

		public string? Description { get; set; }

		public int? InsuranceCard1Id { get; set; }

		//public int? ClientInsurance1Id { get; set; }
		//public int? ClientInsurance2Id { get; set; }
		//public int? ClientInsurance3Id { get; set; }

		public int? InsuranceCard2Id { get; set; }

		public int? InsuranceCard3Id { get; set; }

		public int? BilledToInsuranceCardId { get; set; }

		public string? PatientFirstBillDate { get; set; }

		public string? PatientLastBillDate { get; set; }

		public int? BilledToClientInsuranceId { get; set; }

        public DateTime? InsuranceFirstBilledOn { get; set; }

        public DateTime? InsuranceLastBilledOn { get; set; }


        public int? RenderingProviderId { get; set; }

		//TODO: Build out Modifier Reference Table 
		public string? Modifier1 { get; set; }

		public string? Modifier2 { get; set; }

		public string? Modifier3 { get; set; }

		public string? Modifier4 { get; set; }

		//TODO: Build out ICD10 Reference Table 
		public string? IcdCode1 { get; set; }

		public string? IcdCode2 { get; set; }

		public string? IcdCode3 { get; set; }

		public string? IcdCode4 { get; set; }

		public DateTime? DateOfServiceFrom { get; set; }

		public DateTime? DateOfServiceTo { get; set; }

		public int? ClientLocationId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }

		public decimal? OutstandingBalance { get; set; }

        [ForeignKey("ClientId")]
		public virtual Client Client { get; set; }


		[ForeignKey("PatientId")]
		public virtual Patient Patient { get; set; }


		[ForeignKey("ResponsiblePartyId")]
		public virtual ResponsibleParty ResponsibleParty { get; set; }


		[ForeignKey("ClientCptCodeId")]
		public virtual ClientCptCode ClientCptCode { get; set; }


		[ForeignKey("ClientPlaceOfServiceId")]
		public virtual ClientPlaceOfService ClientPlaceOfService { get; set; }


		[ForeignKey("InsuranceCard1Id")]
		public virtual InsuranceCard InsuranceCard1 { get; set; }


		[ForeignKey("InsuranceCard2Id")]
		public virtual InsuranceCard InsuranceCard2 { get; set; }


		[ForeignKey("InsuranceCard3Id")]
		public virtual InsuranceCard InsuranceCard3 { get; set; }

		[ForeignKey("BilledToInsuranceCardId")]
		public virtual InsuranceCard BilledToInsuranceCard { get; set; }


		[ForeignKey("RenderingProviderId")]
		public virtual ClientProvider RenderingProvider { get; set; }


		[ForeignKey("ClientLocationId")]
		public virtual ClientLocation ClientLocation { get; set; }


        // InsuranceCard Can take the place of these. THese are redundant
        //[ForeignKey("ClientInsurance1Id")]
        //public virtual ClientInsurance ClientInsurance1 { get; set; }

        //[ForeignKey("ClientInsurance2Id")]
        //public virtual ClientInsurance ClientInsurance2 { get; set; }

        //[ForeignKey("ClientInsurance3Id")]
        //public virtual ClientInsurance ClientInsurance3 { get; set; }

        //[ForeignKey("BilledToClientInsuranceId")]
        //public virtual ClientInsurance BilledToClientInsurance { get; set; }



    }
}