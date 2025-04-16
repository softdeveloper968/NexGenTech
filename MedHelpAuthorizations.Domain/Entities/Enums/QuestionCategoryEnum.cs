using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{

    /// <summary>
    /// Indicates what section of a questionnaire (grouping of questions) the question is a part of on a questionnaire
    /// </summary>

    public enum QuestionCategoryEnum
    {
        /// <summary>
        /// General Patient Questions
        /// </summary>
        [Description("PatientGeneral")]
        PatientGeneral = 0,

        /// <summary>
        /// Questions regarding patients gambling experience
        /// </summary>
        [Description("PatientGambling")]
        PatientGambling = 1,

        /// <summary>
        /// Questions regarding patients substance use experience
        /// </summary>
        [Description("PatientSubstanceUse")]
        PatientSubstanceUse = 2,
    }
}