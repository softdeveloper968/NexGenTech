using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    [CustomReportTypeEntityHeader(CustomReportHelper._ClaimLineItemStatus, CustomTypeCode.Empty, false)]
    public class ClaimLineItemStatus : AuditableEntity<ClaimLineItemStatusEnum>
    {
        public ClaimLineItemStatus() { }
        public ClaimLineItemStatus(ClaimLineItemStatusEnum id, string code, string description, int daysWaitBetweenAttempts, int maximumPipelineDays, int minimumResolutionAttempts, int maximumResolutionAttempts,  int rank, ClaimStatusTypeEnum? claimStatusTypeId) 
        { 
            Id = id;
            Code = code;
            Description = description;
            MaximumPipelineDays = maximumPipelineDays;
            MinimumResolutionAttempts = minimumResolutionAttempts;
            MaximumResolutionAttempts = maximumResolutionAttempts;
            DaysWaitBetweenAttempts = daysWaitBetweenAttempts;
            Rank = rank;
            ClaimStatusTypeId = claimStatusTypeId;
        }


        [StringLength(25)]
        public string Code { get; set; }

        [StringLength(80)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimLineItemStatus, CustomTypeCode.String, propertyName: CustomReportHelper.ClaimLineItemStatusDescription)]
        public string Description { get; set; }

        //Maximum Number of days AIT will attempt to resolve claim Status for a claim
        public int MaximumPipelineDays { get; set; }

        //Minimum Number of times status resolution must be attempted
        public int MinimumResolutionAttempts { get; set; }

        //Maximum Number of times status resolution can be attempted
        public int MaximumResolutionAttempts { get; set; }

        //Minimum number of days to wait between claim status resolution attempts
        public int DaysWaitBetweenAttempts { get; set; }

        //[ForeignKey("ClaimStatusResolutionIntervalId")]
        //public virtual ClaimStatusResolutionInterval ClaimStatusResolutionInterval { get; set; }

        /// <summary>
        /// Rank will be an integer type used to determine if a new status should be recorded over the old status.
        /// </summary>
        public int Rank { get; set; }//TAPI-118
        public ClaimStatusTypeEnum? ClaimStatusTypeId { get; set; }

        public virtual ICollection<ClaimStatusTransaction> ClaimStatusTransactions { get; set; }

        [ForeignKey("ClaimStatusTypeId")]
        public virtual ClaimStatusType ClaimStatusType { get; set; }

    }
}

