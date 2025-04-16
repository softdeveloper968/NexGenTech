using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.RpaConfigClaims
{
    public class RpaConfigClaimsDto
    {
        public int ClientId { get; set; }
        public int? RPAConfigId { get; set; }
        public int RPAInsuranceId { get; set; }
        public int? AuthTypeId { get; set; }
        public string AuthTypeName { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AlternateUserName { get; set; }
        public string AlternatePassword { get; set; }
        public string URL { get; set; }
        public List<GetClaimStatusBatchClaimsByBatchIdResponse> Claims { get; set; }
    }
}
