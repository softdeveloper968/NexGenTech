using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public partial class ClaimStatusBatchHistory : AuditableEntity<int>
    {
        public ClaimStatusBatchHistory()
        {
            #region Navigational Property Init


            #endregion        
        }

        [Required]
        public int ClaimStatusBatchId { get; set; }

        [Required]
        public int ClientInsuranceId { get; set; }

        public int? AuthTypeId { get; set; }

        public int? AssignedClientRpaConfigurationId { get; set; }

        public DateTime? AssignedDateTimeUtc { get; set; }

        public string AssignedToIpAddress { get; set; }

        public string AssignedToHostName { get; set; }

        public string AssignedToRpaCode { get; set; }

        public string AssignedToRpaLocalProcessIds { get; set; }

        public DateTime? CompletedDateTimeUtc { get; set; }

        public DateTime? AbortedOnUtc { get; set; }

        public string AbortedReason { get; set; }

        public DateTime? ReviewedOnUtc { get; set; }

        public string ReviewedBy { get; set; }

        public DbOperationEnum DbOperationId { get; set; }

        public bool AllClaimStatusesResolvedOrExpired { get; set; }

        public int? ClientId { get; set; }

        #region Navigational Property Access

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }


        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }


        [ForeignKey("ClaimStatusBatchId")]
        public virtual ClaimStatusBatch ClaimStatusBatch { get; set; }


        [ForeignKey("AssignedClientRpaConfigurationId")]
        public virtual ClientInsuranceRpaConfiguration AssignedClientRpaConfiguration { get; set; }


        [ForeignKey("DbOperationId")]
        public virtual DbOperation DbOperation { get; set; }

        #endregion
    }
}
