using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetUnresolvedByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetProcedureCodes;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using System.Net.Http.Json;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Commands.Update;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimStatus
{
    public class ClaimStatusBatchClaimManager : IClaimStatusBatchClaimManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        
        public ClaimStatusBatchClaimManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<GetAllClaimStatusBatchClaimsResponse>>> GetAllAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchClaimsEndpoint.GetAll());
            return await response.ToResult<List<GetAllClaimStatusBatchClaimsResponse>>();
        }

        public async Task<IResult<GetClaimStatusBatchClaimByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync($"{ClaimStatusBatchClaimsEndpoint.GetById(id)}");
            return await response.ToResult<GetClaimStatusBatchClaimByIdResponse>();
        }

        public async Task<IResult<List<GetClaimStatusBatchClaimsByBatchIdResponse>>> GetByBatchId(int claimStatusBatchId)
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchClaimsEndpoint.GetByBatchId(claimStatusBatchId));
            return await response.ToResult<List<GetClaimStatusBatchClaimsByBatchIdResponse>>();
        }

        public async Task<IResult<List<GetClaimStatusClientProcedureCodeResponse>>> GetClientClaimStatusProcedureCodesAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchClaimsEndpoint.GetClientProcedureCodes());
            return await response.ToResult<List<GetClaimStatusClientProcedureCodeResponse>>();
        }
        public async Task<IResult<int>> UpdateAsync(UpdateClaimStatusBatchClaimCommand command)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(Routes.IntegratedServices.ClaimStatusBatchClaimsEndpoint.Edit, command);
            return await response.ToResult<int>();
        }
    }
}
