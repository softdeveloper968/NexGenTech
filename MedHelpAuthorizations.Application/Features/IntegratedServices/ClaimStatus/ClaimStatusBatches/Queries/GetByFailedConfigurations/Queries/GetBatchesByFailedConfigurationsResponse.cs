using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.Base;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByFailedConfigurations.Queries
{
    public  class GetBatchesByFailedConfigurationsResponse : GetClaimStatusBatchesBaseResponse
    {
        public string ClientName { get; set; }
        public string ClientInsuranceId { get; set; }
        public string AbortedReason { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
