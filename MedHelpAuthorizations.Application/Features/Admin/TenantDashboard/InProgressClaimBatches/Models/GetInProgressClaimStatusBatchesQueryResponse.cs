using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.InProgressClaimBatches.Models
{
    public class GetInProgressClaimStatusBatchesQueryResponse
    {
        public int Id { get; set; }
        public string ClientCode { get; set; }
        public string BatchNumber { get; set; }
        public int LineItems { get; set; }
        public string Payer { get; set; }
        public string RPA { get; set; }
        public DateTime? AssignedDateTimeUtc { get; set; }
        public string AssignedToIpAddress { get; set; }
        public string AssignedToHostName { get; set; }
        public string AssignedToRpaLocalProcessIds { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
