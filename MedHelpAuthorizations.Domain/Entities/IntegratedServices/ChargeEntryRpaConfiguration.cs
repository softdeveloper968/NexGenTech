using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ChargeEntryRpaConfiguration : AuditableEntity<int>, ISoftDelete
    {
        public ChargeEntryRpaConfiguration(int clientId, RpaTypeEnum rpaTypeId, string userName, string password, string targetUrl, int authTypeId, TransactionTypeEnum transactionTypeId = TransactionTypeEnum.ChargeEntry)
        {
            ClientId = clientId;
            RpaTypeId = rpaTypeId;
            Username = userName;
            Password = password;
            TargetUrl = targetUrl;
            TransactionTypeId = transactionTypeId;

            #region Navigational Property Init

            //TransactionBatches = new HashSet<TransactionBatch>();

            #endregion
        }

        public ChargeEntryRpaConfiguration()
        {

            #region Navigational Property Init

            //TransactionBatches = new HashSet<TransactionBatch>();

            #endregion
        }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public TransactionTypeEnum TransactionTypeId { get; set; }

        [Required]
        public RpaTypeEnum RpaTypeId { get; set; } // ICANOTES, et Cetera

        public int? AuthTypeId { get; set; } = null;

        [Required]
        public RelativeDateRangeEnum RelativeDateRangeId { get; set; } = RelativeDateRangeEnum.FirstOfMonthToUtcNow;

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string TargetUrl { get; set; }

        public string LocationName { get; set; }

        public bool FailureReported { get; set; } = false;

        public string FailureMessage { get; set; }

        public bool IsDeleted { get; set; } = false;


        #region Navigational Property Access

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("RpaTypeId")]
        public virtual RpaType RpaType { get; set; }


        [ForeignKey("RelativeDateRangeId")]
        public virtual RelativeDateRange RelativeDateRange { get; set; }


        [ForeignKey("TransactionTypeId")]
        public virtual TransactionType TransactionType { get; set; }


        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }

        #endregion
    }
}
