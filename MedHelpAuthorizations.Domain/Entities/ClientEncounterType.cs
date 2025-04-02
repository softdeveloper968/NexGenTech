using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientEncounterType : AuditableEntity<int>, IClientRelationship
    {

        public ClientEncounterType()
        {
            ClientInsuranceRpaConfigurations = new HashSet<ClientInsuranceRpaConfiguration>();
            ClaimStatusBatches = new HashSet<ClaimStatusBatch>();
        }

        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? OldClientAuthTypeId { get; set; }
        public int? OldAuthTypeId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public virtual ICollection<ClientInsuranceRpaConfiguration> ClientInsuranceRpaConfigurations { get; set; }
        public virtual ICollection<ClaimStatusBatch> ClaimStatusBatches { get; set; }

    }
}
