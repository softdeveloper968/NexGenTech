using MedHelpAuthorizations.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Contracts
{
    public class PersonLinkedEntity : AuditableEntity<int>, IPersonLinkedEntity, IClientRelationship
    {
        public int PersonId { get; set; }
        public int ClientId { get; set; }
        
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        [ForeignKey("ClientId")]
        public virtual Entities.Client Client { get; set; }
    }
}
