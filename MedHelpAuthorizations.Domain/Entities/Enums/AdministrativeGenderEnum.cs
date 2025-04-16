using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{

    /// <summary>
    /// The Administrative Gender Enumeration
    /// Administrative Gender is the gender of a person used for administrative purposes.
    /// Within the HITSP specifications, this data element is used to identify the sex structure of a person,
    /// and excludes a detailed clinical description of the person´s sex.
    /// </summary>
    ///<seealso
    /// href="https://www.hl7.org/fhir/codesystem-administrative-gender.html">
    /// https://ushik.ahrq.gov/ViewItemDetails?&system=hitsp&itemKey=168394000
    /// </seealso>

    public enum AdministrativeGenderEnum
    {
        /// <summary>
        /// Sex structure of a person identified as Male
        /// </summary>
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// Sex structure of a person identified as Male
        /// </summary>
        [Description("Male")]
        Male = 1,

        /// <summary>
        /// Sex structure of a person identified as Female
        /// </summary>
        [Description("Female")]
        Female = 2,

        /// <summary>
        ///  The gender of a person could not be uniquely defined as male or female, such as hermaphrodite 
        /// </summary>
        [Description("Undifferentiated")]
        Undifferentiated = 3
    }
}