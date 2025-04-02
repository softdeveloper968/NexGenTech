using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
	/// <summary>
	/// Enumeration representing different types of write-offs.
	/// </summary>
	public enum WriteOffTypeEnum : int  //AA-231
    {
		/// <summary>
		/// Write-off due to contractual agreement.
		/// </summary>
		[Description("Contractual")]
        Contractual = 1,

		/// <summary>
		/// Other types of write-offs.
		/// </summary>
		[Description("Other")]
        Other = 2,
    }
}
