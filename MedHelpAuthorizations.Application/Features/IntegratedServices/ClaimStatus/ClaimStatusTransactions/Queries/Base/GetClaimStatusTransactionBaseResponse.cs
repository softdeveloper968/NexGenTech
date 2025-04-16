using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Queries.Base
{
    public class GetClaimStatusTransactionBaseResponse
    {
        public int Id { get; set; }

        public int ClaimStatusBatchClaimId { get; set; }

        public DateTime ClaimStatusTransactionBeginDateTimeUtc { get; set; }

        public DateTime ClaimStatusTransactionEndDateTimeUtc { get; set; }

        public int ClientInsuranceCredentialId { get; set; }

        public DateTime? DateOfServiceFrom { get; set; }

        public DateTime? DateOfServiceTo { get; set; }

        public string LineItemControlNumber { get; set; }

        public string ProcedureCode { get; set; }

        public ClaimStatusEnum TotalClaimStatusId { get; set; }

        public ClaimLineItemStatusEnum ClaimLineItemStatusId { get; set; }

        public string ExceptionReason { get; set; }

        public string ExceptionRemark { get; set; }

        public string RemarkCode { get; set; }

        public string ReasonCode { get; set; }

        public decimal LineItemChargeAmount { get; set; }

        public decimal LineItemPaidAmount { get; set; }

        public decimal LineItemApprovedAmount { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string PatientMiddleName { get; set; }

        public DateTime? PatientDateOfBirth { get; set; }

        public string PolicyNumber { get; set; }

        public string ClaimNumber { get; set; }

        public DateTime? DateReceived { get; set; }
        public DateTime? DateEntered { get; set; }
        public string? DiagnosisDescription { get; set; }
        public DateTime? DatePaid { get; set; }
        public string ServiceLineDenialReason { get; set; }
        public string CheckNumber { get; set; }

        public DateTime? CheckDate { get; set; }

        public decimal CheckPaidAmount { get; set; }

        public bool? AuthorizationFound { get; set; }

        public bool IsDeleted { get; set; } = false;

        public AuthorizationStatusEnum AuthorizationStatusId { get; set; }

        public string AuthorizationNumber { get; set; }

        public string EligibilityInsurance { get; set; }

        public string EligibilityPolicyNumber { get; set; }

        public DateTime? EligibilityFromDate { get; set; }

        public string LastActiveEligibleDateRange { get; set; }

        public string EligibilityStatus { get; set; }

        public string EligibilityPhone { get; set; }

        public string EligibilityUrl { get; set; }

        public DateTime? PartA_EligibilityFromDate { get; set; }

        public DateTime? PartA_EligibilityToDate { get; set; }

        public DateTime? PartA_DeductibleFromDate { get; set; }

        public DateTime? PartA_DeductibleToDate { get; set; }

        public decimal? PartA_RemainingDeductible { get; set; }

        public DateTime? PartB_EligibilityFromDate { get; set; }

        public DateTime? PartB_EligibilityToDate { get; set; }

        public DateTime? PartB_DeductibleFromDate { get; set; }

        public DateTime? PartB_DeductibleToDate { get; set; }

        public decimal? PartB_RemainingDeductible { get; set; }

        public DateTime? OtCapYearFrom { get; set; }

        public DateTime? OtCapYearTo { get; set; }

        public decimal? OtCapUsedAmount { get; set; }

        public DateTime? PtCapYearFrom { get; set; }

        public DateTime? PtCapYearTo { get; set; }

        public decimal? PtCapUsedAmount { get; set; }

        public int InputDataListIndex { get; set; }

        public string InputDataFileName { get; set; }
    }
}
