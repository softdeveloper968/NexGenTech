using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public partial class ChargeEntryBatchHistory : AuditableEntity<int>
    {
        public ChargeEntryBatchHistory()
        {
            #region Navigational Property Init


            #endregion        
        }

        public int ChargeEntryBatchId { get; set; }

        public int ChargeEntryRpaConfigurationId { get; set; }

        public DateTime DateOfServiceFrom { get; set; }

        public DateTime DateOfServiceTo { get; set; }

        public DateTime? ProcessStartDateTimeUtc { get; set; }

        public string ProcessStartedByHostIpAddress { get; set; }

        public string ProcessStartedByRpaCode { get; set; }

        public DateTime? CompletedDateTimeUtc { get; set; }

        public DateTime? AbortedOnUtc { get; set; }

        public string AbortedReason { get; set; }

        public DateTime? ReviewedOnUtc { get; set; }

        public string ReviewedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DbOperationEnum DbOperationId { get; set; }

        public bool AllClaimStatusesResolvedOrExpired { get; set; }


         public int? ClientId { get; set; }


        #region Navigational Property Access


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("ChargeEntryRpaConfigurationId")]
        public virtual ChargeEntryRpaConfiguration ChargeEntryRpaConfiguration { get; set; }


        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }


        [ForeignKey("ChargeEntryBatchId")]
        public virtual ChargeEntryBatch ChargeEntryBatch { get; set; }


        [ForeignKey("DbOperationId")]
        public virtual DbOperation DbOperation { get; set; }

        #endregion
    }
}
