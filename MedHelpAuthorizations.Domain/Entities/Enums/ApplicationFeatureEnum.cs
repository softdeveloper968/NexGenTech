using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    /// <summary>
    ///  Values that indicate what areas a client would have access to. 
    /// </summary>
    public enum ApplicationFeatureEnum : int
    {

        /// <summary>
        ///  Aceess to Authorizations Features
        /// </summary>
        [Description("Authorizations")]
        Authorizations = 1,

        /// <summary>
        ///  Aceess to ClaimStatus Features
        /// </summary>
        [Description("ClaimStatus")]
        ClaimStatus = 2,

        /// <summary>
        ///  Aceess to Charge Entry Features
        /// </summary>
        [Description("ChargeEntry")]
        ChargeEntry = 3
    }
}