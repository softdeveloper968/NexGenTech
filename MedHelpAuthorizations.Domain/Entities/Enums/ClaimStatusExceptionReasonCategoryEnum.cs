using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ClaimStatusExceptionReasonCategoryEnum : int
    {

        [Description("Other")]
        Other = 0,


        [Description("COB")]
        COB = 1,


        [Description("Coding Issue")]
        CodingIssue = 2,


        [Description("Coverage")]
        Coverage = 3,


        [Description("Credentialing")]
        Credentialing = 4,


        [Description("Insurance Term")]
        InsuranceTermed = 5,


        [Description("Invalid Insurance")]
        InvalidInsurance = 6,


        [Description("MR Needed")]
        MRNeeded = 7,


        [Description("No Claim/CPT")]
        NoClaimMissingProcedure = 8,


        [Description("Policy Number")]
        PolicyNumber = 9,


        [Description("Provider Type")]
        ProviderType = 10,

        //[Description("Rendering Prov.")]
        //RenderingProvider = 11,

        [Description("Internal Review")]
        Review = 12,

        [Description("Wrong Payer")]
        WrongPayer = 13,

        [Description("Authorization")]
        AuthorizationDenial = 14,

        [Description("Contractual")]
        Contractual = 15,

        [Description("Duplicate")]
        Duplicate = 16,

        [Description("Secondary Missing EOB")]
        SecondaryMissingEob = 17,

        [Description("Medicare Advantage Coverage")]
        MedicareAdvCoverage = 18,

        [Description("Timely Filing")]
        TimelyFiling = 19,

        [Description("Medical Necessity")]
        MedicalNecessity = 20,

        [Description("Claim Issue")]
        ClaimIssue = 21,

        [Description("Demographics Issue")]
        DemographicsIssue = 22,

        [Description("Max Benefits")]
        MaxBenefits = 23,

    }
}
    