using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.ClaimStatusBatchesHistories.Models
{
    public class GetClaimStatusBatchesHistoriesQueryResponse
    {
        public int Id { get; set; }
        public int ClaimStatusBatchId { get; set; }

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

        public bool AllClaimStatusesResolvedOrExpired { get; set; }

        public int? ClientId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
