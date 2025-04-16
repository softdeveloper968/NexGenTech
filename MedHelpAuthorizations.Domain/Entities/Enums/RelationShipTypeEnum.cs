using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    /// <summary>
    ///  The relationship type a patient has between themselves and a responsible Party
    /// ex. Self, Parent, Other.
    /// </summary>
    public enum RelationShipTypeEnum : int
    {
        /// <summary>
        ///  The related party is themselves
        /// </summary>
        //[Description("Unassigned")]
        //Unassigned = 0,
        
        /// <summary>
        ///  The related party is themselves
        /// </summary>
        [Description("Self")]
        Self = 1,

        /// <summary>
        ///  The related party is their Parent
        /// </summary>
        [Description("Parent")]
        Parent = 2,

        /// <summary>
        ///  The related party is someone other than a parent or themselves
        /// </summary>
        [Description("Other")]
        Other = 3
    }
}
