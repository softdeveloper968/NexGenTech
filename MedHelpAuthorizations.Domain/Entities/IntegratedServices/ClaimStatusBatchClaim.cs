using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MedHelpAuthorizations.Shared.Constants.Permission.Permissions;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    [CustomReportTypeEntityHeader(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.Empty, hasSubEntityExist: true)]
    public class ClaimStatusBatchClaim : AuditableEntity<int>, IEquatable<ClaimStatusBatchClaim>, ISoftDelete, IClientRelationship
    {
        public ClaimStatusBatchClaim()
        {
            //ClaimStatusTransactions = new HashSet<ClaimStatusTransaction>();
        }

        public ClaimStatusBatchClaim(int clientId, int claimStatusBatchId, string policyNumber, DateTime dateOfServiceFrom, DateTime dateOfServiceTo, DateTime? claimBilledOn, string procedureCode, int clientLocationId, int clientProviderId, int? patientId, int clientCptCodeId)
        {
            //Required
            ClientId = clientId;
            ClaimStatusBatchId = claimStatusBatchId;
            PolicyNumber = policyNumber;
            DateOfServiceFrom = dateOfServiceFrom;
            DateOfServiceTo = dateOfServiceTo;
            ProcedureCode = procedureCode;
            ClientProviderId = clientProviderId;
            ClientLocationId = clientLocationId;
            PatientId = patientId;
            ClaimBilledOn = claimBilledOn;
            ClientCptCodeId = clientCptCodeId;
        }
        public int ClientId { get; set; }

        public int? ClaimStatusBatchClaimRootId { get; set; }

        [Required]
        public int ClaimStatusBatchId { get; set; }

        public int? ClaimStatusTransactionId { get; set; }

        //[StringLength(64)]
        //public string? PatientLastCommaFirstName => $"{Patient?.Person?.LastName}, {Patient?.Person?.FirstName}";

        [Required]
        [StringLength(36)]
        //[CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.PatientFirstName)]
        public string PatientLastName { get; set; }

        [Required]
        [StringLength(36)]
        //[CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.PatientLastName)]
        public string PatientFirstName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        //public string DateOfBirthString { get; set; }

        //Claim Rendering Provider id
        public int? ClientInsuranceId { get; set; }

        //Claim Rendering Provider id
        public int? ClientProviderId { get; set; }

        //client location id
        public int? ClientLocationId { get; set; }

        [Required]
        [StringLength(32)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.PolicyNumber)]
        public string PolicyNumber { get; set; }

		public DateTime? PolicyNumberUpdatedOn { get; set; }
        

		[CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.DateRangeTypeCombined, propertyName: CustomReportHelper.DateOfServiceFrom,hasCustomDateRange:true, hasRelativeDateRange:true,hasDateRangeCombined:true)]
        public DateTime? DateOfServiceFrom { get; set; }

        public string DateOfServiceFromString { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.DateRangeTypeCombined, propertyName: CustomReportHelper.DateOfServiceTo, hasCustomDateRange: true, hasRelativeDateRange: true, hasDateRangeCombined: true)]
        public DateTime? DateOfServiceTo { get; set; }

        public string DateOfServiceToString { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.DateRangeType, propertyName: CustomReportHelper.ClaimBilledOn, hasCustomDateRange: true, hasRelativeDateRange: true, hasDateRangeCombined: false)]
        public DateTime? ClaimBilledOn { get; set; }

        public string ClaimBilledOnString { get; set; }

        [Required]
        [StringLength(16)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ProcedureCode, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ProcedureCode { get; set; }

        [StringLength(30)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.Modifiers, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string Modifiers { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.BilledAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? BilledAmount { get; set; }
        public decimal? WriteOffAmount { get; set; }

        public decimal? BilledAmountString { get; set; }

        public int Quantity { get; set; } = 1;

        public bool IsDeleted { get; set; } = false;

        public bool IsSupplanted { get; set; } = false;

        //rendering NPI - all numeric
        //group NPI
        //  
        [StringLength(10)]
        public string RenderingNpi { get; set; }


        [StringLength(10)]
        public string GroupNpi { get; set; }


        [StringLength(16)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatchClaim, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ClaimNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ClaimNumber { get; set; }


        [StringLength(34)]
        public string EntryMd5Hash { get; set; }

        //public string TenantId { get; set; }

        public int? PatientId { get; set; } //AA-167

        public string CalculatedLookupHash { get; set; }

        public string CalculatedLookupHashInput { get; set; }

        [StringLength(34)]
        public string ClaimLevelMd5Hash { get; set; }

        public int? ClientFeeScheduleEntryId { get; set; } //AA-231

        public string NormalizedClaimNumber { get; set; } //EN-35

        public int? ClientCptCodeId { get; set; }

        public int? PatientLedgerChargeId { get; set; }

        public DateTime? OriginalClaimBilledOn { get; set; } //EN-247

        //public bool? WasSupplantedByClaimNumber { get; set; } = false;


        #region Navigational Property Access


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("ClaimStatusBatchClaimRootId")]
        public virtual ClaimStatusBatchClaimRoot ClaimStatusBatchClaimRoot { get; set; }


        [ForeignKey("ClaimStatusTransactionId")]
        [CustomTypeSubEntityAttribute(CustomReportHelper._ClaimStatusBatchClaim, CustomReportHelper._ClaimStatusTransaction,CustomTypeCode.FKRelationSubEntity)]
        public virtual ClaimStatusTransaction ClaimStatusTransaction { get; set; }


        [ForeignKey("ClaimStatusBatchId")]
        [CustomTypeSubEntityAttribute(CustomReportHelper._ClaimStatusBatchClaim, CustomReportHelper._ClaimStatusBatch, CustomTypeCode.FKRelationSubEntity)]
        public virtual ClaimStatusBatch ClaimStatusBatch { get; set; }


        [ForeignKey("ClientInsuranceId")]
        [CustomTypeSubEntityAttribute(CustomReportHelper._ClaimStatusBatchClaim, CustomReportHelper._ClientInsurance, CustomTypeCode.FKRelationSubEntity)]
        public virtual ClientInsurance ClientInsurance { get; set; }

        [ForeignKey("ClientProviderId")]
        [CustomTypeSubEntityAttribute(CustomReportHelper._ClaimStatusBatchClaim, CustomReportHelper._ClientProvider, CustomTypeCode.FKRelationSubEntity)]
        public virtual ClientProvider ClientProvider { get; set; }


        [ForeignKey("ClientLocationId")]
        [CustomTypeSubEntityAttribute(CustomReportHelper._ClaimStatusBatchClaim, CustomReportHelper._ClientLocation, CustomTypeCode.FKRelationSubEntity)]
        public virtual ClientLocation ClientLocation { get; set; }

        [ForeignKey("PatientId")]
        [CustomTypeSubEntityAttribute(CustomReportHelper._ClaimStatusBatchClaim, CustomReportHelper._Patient, CustomTypeCode.FKRelationSubEntity)]
        public virtual Patient Patient { get; set; } //AA-167

		[ForeignKey("ClientFeeScheduleEntryId")]
		public virtual ClientFeeScheduleEntry ClientFeeScheduleEntry { get; set; } //AA-231

        [ForeignKey("ClientCptCodeId")]
        public virtual ClientCptCode ClientCptCode { get; set; }

        [ForeignKey("PatientLedgerChargeId")]
        public virtual PatientLedgerCharge PatientLedgerCharge { get; set; }


        public bool Equals(ClaimStatusBatchClaim other)
        {
            bool flag = true;

            if (this.ClientInsuranceId != other.ClientInsuranceId)
                flag = false;
            //if (this.PatientFirstName?.ToUpper().Trim() != other.PatientFirstName?.ToUpper().Trim())
            ///   flag = false;
            //if (this.PatientLastName?.ToUpper().Trim() != other.PatientLastName?.ToUpper().Trim())
            //    flag = false;
            if (this.PolicyNumber?.ToUpper().Trim() != other.PolicyNumber?.ToUpper().Trim())
                flag = false;
            //if (this.DateOfBirth.Value != other.DateOfBirth.Value)
            //    flag = false;
            if (this.DateOfServiceFrom.Value != other.DateOfServiceFrom.Value)
                flag = false;
            if (this.DateOfServiceTo.Value != other.DateOfServiceTo.Value)
                flag = false;
            if (this.ProcedureCode?.ToUpper().Trim() != other.ProcedureCode?.ToUpper().Trim())
                flag = false;
            if (this.Modifiers?.ToUpper().Trim() != other.Modifiers?.ToUpper().Trim())
                flag = false;
            if (this.ClaimNumber?.ToUpper().Trim() != other.ClaimNumber?.ToUpper().Trim())
                flag = false;
            if (this.GroupNpi?.ToUpper().Trim() != other.GroupNpi?.ToUpper().Trim())
                flag = false;
            if (this.RenderingNpi?.ToUpper().Trim() != other.RenderingNpi?.ToUpper().Trim())
                flag = false;
            if (this.BilledAmount.Value != other.BilledAmount.Value)
                flag = false;
            if (this.ClaimBilledOn.Value != other.ClaimBilledOn.Value)
                flag = false;
            if (this.ClientProviderId != other.ClientProviderId)
                flag = false;
            if (this.ClientLocationId != other.ClientLocationId)
                flag = false;
            if (this.PatientId != other.PatientId)
                flag = false;

            return flag;
        }
        #endregion
    }
}
