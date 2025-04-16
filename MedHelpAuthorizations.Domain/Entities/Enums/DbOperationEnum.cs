using System.ComponentModel;
namespace MedHelpAuthorizations.Domain.Entities.Enums
{

    /// <summary>
    /// Describes a Database operation.
    /// Original purpose is to be used to indicate what operation on a related table 
    /// triggered the record to be written in another such as a history table.  
    /// </summary>
    public enum DbOperationEnum : int
    {
        [Description("Insert")]
        Insert = 1,

        [Description("Update")]
        Update = 2,
    }
}
