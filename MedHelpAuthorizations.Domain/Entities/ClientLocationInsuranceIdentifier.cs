using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientLocationInsuranceIdentifier : AuditableEntity<int>, IClientRelationship
    {
        public ClientLocationInsuranceIdentifier() { }

        public int ClientId { get; set; }

        public int ClientLocationId { get; set; } 

        public int ClientInsuranceId { get; set; }

        public string Identifier { get; set; }


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }
    }
}
