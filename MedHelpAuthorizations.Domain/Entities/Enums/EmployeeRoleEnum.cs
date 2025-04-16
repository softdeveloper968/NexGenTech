using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{

    public enum EmployeeRoleEnum
    {
        /// <summary>
        /// Registration Manager
        /// </summary>
        [Description("Registration Manager")]
        RegistrationManager = 1,

        /// <summary>
        /// Billing Manager
        /// </summary>
        [Description("Billing Manager")]
        BillingManager,

        /// <summary>
        /// Registration
        /// </summary>
        [Description("Registor")]
        Registor,

        /// <summary>
        /// Medical Assistance
        /// </summary>
        [Description("Medical Assistance")]
        MedicalAssistance,
        
        [Description("CEO")]
        CEO,
            
        [Description("CFO")]
        CFO,
        
        [Description("COO")]
        COO,
        
        [Description("CIO")]
        CIO,
        
        [Description("Director of Patient Financial Services")]
        DirectorOfPatientFinancialServices,
        
        [Description("Vice President")]
        VicePresident,
        
        [Description("Medical Director")]
        MedicalDirector,
        
        [Description("Billing Supervisor")]
        BillingSupervisor,
        
        [Description("Cash Posting Manager")]
        CashPostingManager,
        
        [Description("Biller")]
        Biller,
        
        [Description("Cash Poster")]
        CashPoster,
        
        [Description("Charge Enrty")]
        ChargeEnrty,
        
        [Description("Insurance Contractor")]
        InsuranceContractor,
    }
}
