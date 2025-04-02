using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{

    /// <summary>
    /// The Authorization Status Enumeration
    /// Authorization Status is the Current status of the Authorization.    
    /// </summary>
    public enum AuthorizationStatusEnum
    {
        [Description("Auth requested")]
        ClientRequestAdded = 1,

        [Description("Questionnaire not complete or other info required")]
        InformationNeeded,

        [Description("Nurse to review questionnaire for completeness")]
        NurseReview,

        [Description("Request ready for robot")]
        RFR,

        [Description("Auth approved on insurance website")]
        AuthApproved,

        [Description("Auth pending by insurance company")]
        AuthPended,

        [Description("Auth has been discharged in Insurance website")]
        AuthDischarged,

        [Description("Auth denied by insurance company")]
        AuthDenied,

        [Description("Auth end date in the next 30 days or less")]
        AuthExpiring,

        [Description("New auth request for this service type")]
        AuthConcurrent,

        [Description("Auth end date is past the current date")]
        AuthExpired,
        
        [Description("In-Process")]
        AuthInProcess,
    }
}
