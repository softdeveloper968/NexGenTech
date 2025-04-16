using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetAll;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Update;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetCleanupBatches;
using MedHelpAuthorizations.Application.Responses.IntegratedServices;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimStatus
{
    public class ClaimStatusBatchManager : IClaimStatusBatchManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClaimStatusBatchManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<GetAllClaimStatusBatchesResponse>>> GetAllAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchesEndpoint.GetAll());
            return await response.ToResult<List<GetAllClaimStatusBatchesResponse>>();
        }

        public async Task<IResult<GetRecentClaimStatusBatchesByClientIdResponse>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync($"{ClaimStatusBatchesEndpoint.GetById(id)}");
            return await response.ToResult<GetRecentClaimStatusBatchesByClientIdResponse>();
        }

        public async Task<IResult<int>> CreateAsync(CreateClaimStatusBatchCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClaimStatusBatchesEndpoint.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateCompletedAsync(int batchId,string rpaCode,int batchClaimCountProcessed)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClaimStatusBatchesEndpoint.UpdateCompleted(), new UpdateCompletedClaimStatusBatchCommand(){Id = batchId, AssignedToRpaCode = rpaCode,NumberOfClaimsProcessed = batchClaimCountProcessed});
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateAbortedAsync(int batchId,string abortedReason, string rpaCode)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClaimStatusBatchesEndpoint.UpdateAborted(), new UpdateAbortedClaimStatusBatchCommand() { Id = batchId, AbortedReason = abortedReason, AssignedToRpaCode = rpaCode });
            return await response.ToResult<int>();
        }
        
        public async Task<IResult<int>> UpdateAssignmentAsync(UpdateAssignedClaimStatusBatchCommand request)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClaimStatusBatchesEndpoint.UpdateAssignment(), request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UnassignBatchAsync(int id)
        {
            var response = await _tenantHttpClient.PutAsync(ClaimStatusBatchesEndpoint.UnassignBatch(id), null);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ClearRpaLocalProcessIdsAsync(int batchId)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClaimStatusBatchesEndpoint.ClearRpaLocalProcessIds(), new UpdateAssignedClaimStatusBatchCommand() { Id = batchId, AssignedToRpaProcessIds = null });
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetClaimStatusUnprocessedBatchesResponse>>> GetUnprocessedByRpaInsuranceAsync(int rpaInsuranceId, bool isForInitialAnalysis = false)
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchesEndpoint.GetUnprocessedByInsuranceId(rpaInsuranceId, isForInitialAnalysis));
            return await response.ToResult<List<GetClaimStatusUnprocessedBatchesResponse>>();
        }

        //AA-318
		public async Task<IResult<List<GetClaimStatusUnprocessedBatchesResponse>>> GetBatchesByRpaInsuranceIdAsync(int rpaInsuranceId, bool assignedBatches, bool isForInitialAnalysis = false)
		{
			var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchesEndpoint.GetByRpaInsuranceId(rpaInsuranceId, assignedBatches));
			return await response.ToResult<List<GetClaimStatusUnprocessedBatchesResponse>>();
		}

		public async Task<IResult<List<GetClaimStatusUnprocessedBatchesResponse>>> GetAllUnprocessedAsync(bool isForInitialAnalysis = false)
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchesEndpoint.GetAllUnprocessed(isForInitialAnalysis));
            return await response.ToResult<List<GetClaimStatusUnprocessedBatchesResponse>>();
        }

        public async Task<IResult<List<ClaimStatusBatchLastTransactionResponse>>> GetNotCompletedCleanup(string hostname)
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchesEndpoint.GetNotCompletedCleanup(hostname));
            return await response.ToResult<List<ClaimStatusBatchLastTransactionResponse>>();
        }

        public async Task<IResult<List<GetCompletedCleanupByHostnameResponse>>> GetCompletedCleanup(string hostname)
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchesEndpoint.GetCompletedCleanup(hostname));
            return await response.ToResult<List<GetCompletedCleanupByHostnameResponse>>();
        }
        public async Task<IResult<List<GetRecentClaimStatusBatchesByClientIdResponse>>> GetRecentForClientIdAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClaimStatusBatchesEndpoint.GetRecentForClientId());
            return await response.ToResult<List<GetRecentClaimStatusBatchesByClientIdResponse>>();
        }
    }
}
