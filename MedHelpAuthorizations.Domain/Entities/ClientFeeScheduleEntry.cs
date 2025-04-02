using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientFeeScheduleEntry : AuditableEntity<int>, IClientRelationship
	{
        public ClientFeeScheduleEntry()
        {
            ClaimStatusBatchClaims = new HashSet<ClaimStatusBatchClaim>();
        }

        public int ClientId { get; set; }

        public int ClientCptCodeId { get; set; }

        public int ClientFeeScheduleId { get; set; }

        public decimal Fee { get; set; }

        public decimal? AllowedAmount { get; set; }
        public decimal? ReimbursablePercentage { get; set; } //EN-70

        public bool IsReimbursable { get; set; }


        [ForeignKey("ClientCptCodeId")]
        public virtual ClientCptCode ClientCptCode { get; set; }


        [ForeignKey("ClientFeeScheduleId")]
        public virtual ClientFeeSchedule ClientFeeSchedule { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        public virtual ICollection<ClaimStatusBatchClaim> ClaimStatusBatchClaims { get; set; }
        
    }
}
