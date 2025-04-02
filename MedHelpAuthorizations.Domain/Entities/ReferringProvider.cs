using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ReferringProvider : AuditableEntity<int>, IProvider, IClientRelationship//, ITenant
    {
        public ReferringProvider()
        {
            //PatientLedgerCharge = new HashSet<PatientLedgerCharge>();
            Patients = new HashSet<Patient>();
        }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int PersonId { get; set; }

        [Required]
        public SpecialtyEnum SpecialtyId { get; set; }

        [StringLength(20)]
        public string Credentials { get; set; }

        [StringLength(10)]
        public string Npi { get; set; }

        [StringLength(6)]
        public string Upin { get; set; }

        [StringLength(9)]
        public string TaxId { get; set; }

        [StringLength(10)]
        public string TaxonomyCode { get; set; }

        [StringLength(25)]
        public string License { get; set; }

        [StringLength(25)]
        public string ExternalId { get; set; }

        //public string TenantId { get; set; }


        #region Navigation Objects

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }


        [ForeignKey("SpecialtyId")]
        public virtual Specialty Specialty { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }

        #endregion
    }
}
