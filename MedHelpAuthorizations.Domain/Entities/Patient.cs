using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Helpers;

#nullable disable

namespace MedHelpAuthorizations.Domain.Entities
{
    [CustomReportTypeEntityHeaderAttribute(entityName: CustomReportHelper._ClaimStatusBatch, CustomTypeCode.Empty, false)]
    public class Patient : AuditableEntity<int>, IDataPipe<int>, IClientRelationship
	{
        public Patient()
        {
            Authorizations = new HashSet<Authorization>();
            Documents = new HashSet<Document>();
        }

        [StringLength(25)]
        public string AccountNumber { get; private set; }

        [StringLength(25)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._Patient, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.PatientExternalId, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ExternalId { get; set; }

        public AdministrativeGenderEnum AdministrativeGenderId { get; set; } = AdministrativeGenderEnum.Unknown;

        //public string Insurance { get; set; }
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._Patient, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.PatientInsurancePolicyNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]

        public string InsurancePolicyNumber { get; set; }

        public string InsuranceGroupNumber { get; set; }

        public DateTime? BenefitsCheckedOn { get; set; }

        public RelationShipTypeEnum ResponsiblePartyRelationshipToPatient { get; set; } = RelationShipTypeEnum.Self;

        public int ClientId { get; set; }
        public int? PersonId { get; set; }
        public int? ResponsiblePartyId { get; set; }
        
        public int? PrimaryProviderId { get; set; }
        
        public int? ReferringProviderId { get; set; }

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }


		[ForeignKey("ResponsiblePartyId")]
        public virtual ResponsibleParty ResponsibleParty { get; set; }

        [ForeignKey("PersonId")]
        [CustomTypeSubEntityAttribute(entityName:CustomReportHelper._Patient, subEntityName:CustomReportHelper._Person,customTypeCode:CustomTypeCode.FKRelationSubEntity)]
        public virtual Person Person { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("PrimaryProviderId")]
        public virtual ClientProvider PrimaryProvider { get; set; }
        

        [ForeignKey("ReferringProviderId")]
        public virtual ReferringProvider ReferringProvider { get; set; }
        

        [ForeignKey("AdministrativeGenderId")]
        public virtual AdministrativeGender AdministrativeGender { get; set; }
        
        public virtual ICollection<Authorization> Authorizations { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
	}
}
