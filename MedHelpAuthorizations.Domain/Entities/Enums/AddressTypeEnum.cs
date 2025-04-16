using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    /// <summary>
    ///  Commercial or Residential Address Type
    /// </summary>
    public enum AddressTypeEnum : int
    {

        /// <summary>
        ///  Residential Address Type
        /// </summary>
        [Description("Residential")]
        Residential = 1,

        /// <summary>
        ///  Commercial Address Type
        /// </summary>
        [Description("Commercial")]
        Commercial = 2       
    }
}