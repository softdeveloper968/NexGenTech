using CsvHelper.Configuration.Attributes;
using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ClaimLineItemStatusEnum : int
    {
		[Name("Unknown")]
		[Description("Unknown")]
        Unknown = 0,

		[Name("Paid")]
		[Description("Paid")]
        Paid = 1,

		[Name("Approved")]
		[Description("Approved")]
        Approved = 2,

		[Name("Rejected")]
		[Description("Rejected")]
        Rejected = 3,

		[Name("Voided")]
		[Description("Voided")]
        Voided = 4,

		[Name("Received")]
		[Description("Received")]
        Received = 5,

		[Name("NotAdjudicated")]
		[Description("Not-Adjudicated")]
        NotAdjudicated = 6,

		[Name("Denied")]
		[Description("Denied")]
        Denied = 7,

		[Name("Pended")]
		[Description("Pended")]
        Pended = 8,

		[Name("UnMatchedProcedureCode")]
		[Description("UnMatched-ProcedureCode")]
        UnMatchedProcedureCode = 9,

		[Name("Error")]
		[Description("Error / Exception")]
        Error = 10,

		[Name("Unavailable")]
		[Description("Unavailable for review")]
        Unavailable = 11,

		[Name("MemberNotFound")]
		[Description("Member Not Found")]
        MemberNotFound = 12,

		[Name("Ignored")]
		[Description("Ignored")]
        Ignored = 13,

		[Name("ZeroPay")]
		[Description("Zero Pay")]
        ZeroPay = 14,

		[Name("BundledFqhc")]
		[Description("Bundled Fqhc")]
        BundledFqhc = 15,

		[Name("NeedsReview")]
		[Description("Needs Review")]
        NeedsReview = 16,

		[Name("TransientError")]
		[Description("Transient Error")]
        TransientError = 17,

		[Name("CallPayer")]
		[Description("Call Payer")]
        CallPayer = 18,

		[Name("Returned")]
		[Description("Returned")]
        Returned = 19,

		[Name("Writeoff")]
		[Description("Write-off")]
        Writeoff = 20,

		[Name("Rebilled")]
		[Description("Rebilled")]
        Rebilled = 21,

		[Name("Contractual")]
		[Description("Contractual")]  //EN-79
		Contractual = 22,

		[Name("NotOnFile")]
		[Description("Not On File")] //EN-151
        NotOnFile = 23,

		[Name("MemberNotEligible")]
		[Description("Member Not Eligible")] 
        MemberNotEligible = 24,

		[Name("RetryMemberNotFound")]
		[Description("Retry Member not found")]
        RetryMemberNotFound = 25,
    }
}
