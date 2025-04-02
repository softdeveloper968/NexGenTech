using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using MedHelpAuthorizations.Domain.Common.Contracts;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ClaimStatusTransactionLineItemStatusChangẹ : AuditableEntity<int>, IClientRelationship//, ITenant
    {
        public ClaimStatusTransactionLineItemStatusChangẹ() { }

        public ClaimStatusTransactionLineItemStatusChangẹ(int clientId, ClaimLineItemStatusEnum updatedClaimLineItemStatusId, DbOperationEnum dbOperationId)
        {
            this.ClientId = clientId;
            this.UpdatedClaimLineItemStatusId = updatedClaimLineItemStatusId;
            this.DbOperationId = dbOperationId;
        }

        public ClaimStatusTransactionLineItemStatusChangẹ(int clientId, ClaimLineItemStatusEnum? previousClaimLineItemStatusId, ClaimLineItemStatusEnum updatedClaimLineItemStatusId, DbOperationEnum dbOperationId)
        {
            this.ClientId = clientId;
            this.PreviousClaimLineItemStatusId = previousClaimLineItemStatusId;
            this.UpdatedClaimLineItemStatusId = updatedClaimLineItemStatusId;
            this.DbOperationId = dbOperationId;
        }

        public int? ClaimStatusTransactionId { get; set; }

        [Required]
        public int ClientId { get; set; }

        public ClaimLineItemStatusEnum? PreviousClaimLineItemStatusId { get; set; }

        [Required]
        public ClaimLineItemStatusEnum UpdatedClaimLineItemStatusId { get; set; }
        public DbOperationEnum DbOperationId { get; set; }

        public decimal? WriteoffAmount { get; set; }
        //public string TenantId { get; set; }

        #region Navigational Property Access

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        //[ForeignKey("ClaimStatusTransactionId")]
        //public virtual ClaimStatusTransaction ClaimStatusTransaction { get; set; }

        [ForeignKey("PreviousClaimLineItemStatusId")]
        public virtual ClaimLineItemStatus PreviousClaimLineItemStatus { get; set; }

        [ForeignKey("UpdatedClaimLineItemStatusId")]
        public virtual ClaimLineItemStatus UpdatedClaimLineItemStatus { get; set; }

        [ForeignKey("DbOperationId")]
        public virtual DbOperation DbOperation { get; set; }

        #endregion
    }
}
