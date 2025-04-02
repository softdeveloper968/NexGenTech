using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ChargeEntryBatch : AuditableEntity<int>, ISoftDelete//, ITenant
    {

        public ChargeEntryBatch()
        {
            #region Navigational Property Init

            ChargeEntryTransactions = new List<ChargeEntryTransaction>();
            ChargeEntryBatchHistories = new HashSet<ChargeEntryBatchHistory>();

            #endregion
        }

        [StringLength(6)]
        public string BatchNumber { get; set; }


        [Required]
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
        //public string TenantId { get; set; }

        // public int? ClientId { get; set; }

        //public string TenantId { get; set; }

        #region Navigational Property Access

        //[ForeignKey("ClientId")]
        //public virtual Client Client { get; set; }


        [ForeignKey("ChargeEntryRpaConfigurationId")]
        public virtual ChargeEntryRpaConfiguration ChargeEntryRpaConfiguration { get; set; }


        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }

        public virtual ICollection<ChargeEntryTransaction> ChargeEntryTransactions { get; set; }

        public virtual ICollection<ChargeEntryBatchHistory> ChargeEntryBatchHistories { get; set; }

        #endregion
    }
}

