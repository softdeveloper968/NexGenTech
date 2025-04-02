using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Enums;


namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClaimStatusBatchRepository : IRepositoryAsync<ClaimStatusBatch, int>
    {
        IQueryable<ClaimStatusBatch> ClaimStatusBatches { get; }

        Task<List<ClaimStatusBatch>> GetListAsync();

        Task<List<ClaimStatusBatch>> GetUnprocessedByCriteriaAsync(GetClaimStatusUnprocessedBatchesQuery query, ApiIntegrationEnum? apiIntegrationId = null);
        
        Task<List<ClaimStatusBatch>> GetAllUnprocessedAsync(bool isForInitialAnalysis = false);
        Task<List<ClaimStatusBatch>> GetByClientIdAsyncAsync(int clientId);

        Task<List<ClaimStatusBatch>> GetCompletedCleanupByHostName(string hostName);

        Task<List<ClaimStatusBatch>> GetNotCompletedCleanupByHostName(string hostName);

        Task<List<ClaimStatusBatch>> GetUnresolvedBatchesByDaysOldAsync(int daysOld);

        Task<int> InsertAsync(ClaimStatusBatch claimStatusBatch);

        Task RestoreAsync(ClaimStatusBatch claimStatusBatch);

        Task<List<ClaimStatusBatch>> GetAbortedBatchesListAsync();
        Task<List<ClaimStatusBatch>> GetBatchesByClientInsuranceAndAuthTypeAsync(int ClientInsuranceId, int? AuthTypeId);
        Task<List<ClaimStatusBatch>> GetByRpaInsuranceId(GetClaimStatusBatchesByRpaInsuranceIdQuery query, ApiIntegrationEnum? apiIntegrationId = null);
        Task<List<ClaimStatusBatch>> RetrieveValidBatchIdsAsync(); //EN-95
        Task<List<int>> GetNonDeletedBatchIdsAsync();

        Task<ClaimStatusBatchesPaginationModel> GetAbortedClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder);
        Task<ClaimStatusBatchesPaginationModel> GetCompletedClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder);
        Task<ClaimStatusBatchesPaginationModel> GetInProgressClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder);
        Task<ClaimStatusBatchesPaginationModel> GetDeletedClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder);

    }	
}
