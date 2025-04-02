using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum SourceSystemEnum
    {
        /// <summary>
        /// CyFluent
        /// </summary>
        [Description("CyFluent")]
        CyFluent = 1,

        /// <summary>
        /// Test
        /// </summary>
        [Description("Test")]
        Tst = 2,
    }
}
