using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.RpaConfigClaims;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IRpaConfigClaimService : IService
    {
        Task<List<RpaConfigClaimsDto>> GetRpaConfigClaimsListAsync();
        Task<RpaConfigClaimsDto> ProcessClientClaimsAsync(CancellationToken cancellationToken, int RpaConfigId, string tenantIdentifier = "");
        Task<List<ClientInsuranceRpaConfiguration>> GetRpaConfigurationsAsync();
        Task<ClientInsuranceRpaConfiguration> GetRpaConfigurationByIdAsync(int rpaConfigId);
        Task<RpaConfigClaimsDto> ProcessRpaConfigClaimsAsync(string tenantIdentifier, ClientInsuranceRpaConfiguration rpaConfig, CancellationToken cancellationToken);
        Task<List<GetClaimStatusBatchClaimsByBatchIdResponse>> GetFilteredClaimStatusBatchClaimsAsync(IQueryable<ClaimStatusBatchClaim> claimStatusBatchClaims, ClientInsuranceRpaConfiguration rpaConfig);

    }
}
