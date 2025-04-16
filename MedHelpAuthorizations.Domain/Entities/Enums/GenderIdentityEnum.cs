using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{

    /// <summary>
    /// The GenderIdentity Enumeration
    /// Defines a set of codes that can be used to indicate a patient's gender identity.
    /// </summary>
    ///<seealso href="https://www.hl7.org/fhir/codesystem-gender-identity.html"/>
    public enum GenderIdentityEnum
    {
        /// <summary>
        /// It is unknown what gender the patient identifies as. 
        /// </summary>
        [Description("Unknown")]
        Unknown = 0,

        /// <summary>
        /// The patient identifies as transgender male-to-female
        /// </summary>
        [Description("Transgenderer Female")]
        TransgenderFemale = 1,

        /// <summary>
        /// The patient identifies as transgender female-to-male
        /// </summary>
        [Description("Transgenderer Male")]
        TransgenderMale = 2,

        /// <summary>
        /// The patient identifies with neither/both female and male
        /// </summary>
        [Description("Non-Binary")]
        NonBinary = 3,

        /// <summary>
        /// The patient identifies as male
        /// </summary>
        [Description("Male")]
        Male = 4,

        /// <summary>
        /// The patient identifies as female
        /// </summary>
        [Description("Female")]
        Female = 5,

        /// <summary>
        /// Other gender identity
        /// </summary>
        [Description("Other")]
        Other = 6,

        /// <summary>
        /// The patient does not wish to disclose his gender identity
        /// </summary>
        [Description("Does not wish to disclose")]
        NonDisclose = 7        
    }
}