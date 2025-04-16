using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ProviderLevelEnum
	{
		///// <summary>
		///// Provider Level Not Specified
		///// </summary>
		//[Description("Not Specified")]
		//Not Specified = 0,

		/// <summary>
		/// Medical Doctor
		/// </summary>
		[Description("Medical Doctor")]
        MD = 1,

        /// <summary>
        /// Nurse Practitioner
        /// </summary>
        [Description("Nurse Practitioner")]
        NP = 2,

		/// <summary>
		/// Physician Assistant
		/// </summary>
		[Description("Physician Assistant")]
        PA = 3,

        /// <summary>
		/// Doctor of Osteopathic Medicine
		/// </summary>
        [Description("Doctor of Osteopathic Medicine")]
        DO = 4,

        /// <summary>
		/// Licensed Clinical Social Worker
		/// </summary>
        [Description("Licensed Clinical Social Worker")]
        LCSW = 5,

        /// <summary>
		/// Doctor of Dental Medicine
		/// </summary>
        [Description("Doctor of Dental Medicine")]
        DMD = 6,

        /// <summary>
		/// Family Nurse Practitioner
		/// </summary>
        [Description("Family Nurse Practitioner")]
        FNP = 7,

        /// <summary>
		/// Advanced Practice Registered Nurse
		/// </summary>
        [Description("Advanced Practice Registered Nurse")]
        APRN = 8,

        /// <summary>
		/// Family Nurse Practitioner - Certified
		/// </summary>
        [Description("Family Nurse Practitioner - Certified")]
        FNPC = 9,

        /// <summary>
		/// Adult Nurse Practitioner
		/// </summary>
        [Description("Adult Nurse Practitioner")]
        ANP = 10,

        /// <summary>
		/// Advanced Registered Nurse Practitioner
		/// </summary>
        [Description("Advanced Registered Nurse Practitioner")]
        ARNP = 11,

        /// <summary>
		/// Doctor of Osteopathic Medicine
		/// </summary>
        [Description("Doctor of Osteopathic Medicine")]
        OB = 12,
    }
}
