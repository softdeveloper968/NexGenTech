using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    [CustomReportTypeEntityHeader(entityName: CustomReportHelper._ClaimStatusBatch, CustomTypeCode.Empty, false)]
    public partial class ClaimStatusBatch : AuditableEntity<int>, ISoftDelete , IClientRelationship
    {
        public ClaimStatusBatch()
        {
            #region Navigational Property Init

            ClaimStatusBatchClaims = new HashSet<ClaimStatusBatchClaim>();
            ClaimStatusBatchHistories = new HashSet<ClaimStatusBatchHistory>();

            #endregion        
        }
        public ClaimStatusBatch(int clientId, int rpaInsuranceId, int authTypeId, int inputDocumentId)
        {
            //Required
            //ClientId = clientId;
            ClientInsuranceId = rpaInsuranceId;
            AuthTypeId = authTypeId;
            InputDocumentId = inputDocumentId;

            #region Navigational Property Init

            ClaimStatusBatchClaims = new HashSet<ClaimStatusBatchClaim>();
            ClaimStatusBatchHistories = new HashSet<ClaimStatusBatchHistory>();

            #endregion
        }

        [StringLength(6)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusBatch, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.BatchNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string BatchNumber { get; set; }

        public int ClientId { get; set; }

        [Required]
        public int ClientInsuranceId { get; set; }

        //[Required]
        //public int RpaInsuranceId { get; set; }

        [Required]
        public int InputDocumentId { get; set; }

        //[Required]
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

        public bool AllClaimStatusesResolvedOrExpired { get; set; } = false;

        public int? Priority { get; set; } = null;

        public bool IsDeleted { get; set; } = false;

        //public string TenantId { get; set; }


        #region Navigational Property Access

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }


        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("InputDocumentId")]
        public virtual InputDocument InputDocument { get; set; }


        [ForeignKey("AssignedClientRpaConfigurationId")]
        public virtual ClientInsuranceRpaConfiguration AssignedClientRpaConfiguration { get; set; }


        public virtual ICollection<ClaimStatusBatchClaim> ClaimStatusBatchClaims { get; set; }

        public virtual ICollection<ClaimStatusBatchHistory> ClaimStatusBatchHistories { get; set; }

        #endregion
    }
}
