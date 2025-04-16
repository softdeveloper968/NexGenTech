using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Administrative Gender
    /// This attribute does not include terms related to clinical gender. 
    /// Gender is a complex physiological, genetic and sociological concept that requires multiple observations in order to be
    /// comprehensively described.The purpose of this attribute is to provide a high level classification that can additionally
    /// be used for the appropriate allocation of inpatient bed assignment.
    /// </summary>
    /// <seealso>
    ///     href="https://github.com/HL7/C-CDA-Examples/blob/master/Header/Patient%20Demographic%20Information/Patient%20Demographic%20Information(C-CDA2.1).xml"
    /// </seealso>
    /// 
    public class AdministrativeGender : AuditableEntity<AdministrativeGenderEnum>
    {
        public AdministrativeGender() { }
        public AdministrativeGender(AdministrativeGenderEnum id, string name) 
        {
            Id = id;
            Name = name;
        }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
