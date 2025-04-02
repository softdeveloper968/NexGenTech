using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class InsuranceCard : AuditableEntity<int>, IDataPipe<int>, IClientRelationship
	{
        public bool Active { get; set; }                
        public int CardholderId { get; set; }
        public int PatientId { get; set; }
        public int ClientInsuranceId { get; set; }
        public string GroupNumber { get; set; }
        public RelationShipTypeEnum? CardholderRelationshipToPatient { get; set; }
        public string MemberNumber { get; set; }
        public byte? InsuranceCoverageTypes { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
		public string DfExternalId { get; set; }
		public DateTime? DfCreatedOn { get; set; }
		public DateTime? DfLastModifiedOn { get; set; }

		[NotMapped]
        public bool Verified
        {
            get
            {
                if (VerifiedDate == DateTime.MinValue || VerifiedDate == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public DateTime? VerifiedDate { get; set; }
        public int? InsuranceCardOrder { get; set; }
        public decimal CopayAmount { get; set; }
        public int ClientId { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("CardholderId")]
        public virtual Cardholder Cardholder { get; set; }

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance Insurance { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

		//[ForeignKey("ScannedImageId")]
		//public virtual ScannedImage ScannedImage { get; set; }
	}
}
