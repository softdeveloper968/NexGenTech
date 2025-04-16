using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetById;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimStatus
{
    public class ClaimStatusTransactionManager : IClaimStatusTransactionManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        
        public ClaimStatusTransactionManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> UpsertAsync(UpsertClaimStatusTransactionCommand command)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.ClaimStatusTransactionsEndpoint.Save, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllClaimStatusBatchClaimsResponse>>> GetAllAsync()
        {
            //var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchClaimsEndpoint.GetAll());
            //return await response.ToResult<List<GetAllClaimStatusBatchClaimsResponse>>();
            throw new NotImplementedException();
        }

        public async Task<IResult<GetClaimStatusBatchClaimByIdResponse>> GetByIdAsync(int id)
        {
            //var response = await _tenantHttpClient.GetAsync($"{ClaimStatusBatchClaimsEndpoint.GetById(id)}");
            //return await response.ToResult<GetClaimStatusBatchClaimByIdResponse>();
            throw new NotImplementedException();
        }

        public async Task<IResult<List<GetClaimStatusBatchClaimsByBatchIdResponse>>> GetByBatchId(int claimStatusBatchId)
        {
            throw new NotImplementedException();
        }
    }
}
