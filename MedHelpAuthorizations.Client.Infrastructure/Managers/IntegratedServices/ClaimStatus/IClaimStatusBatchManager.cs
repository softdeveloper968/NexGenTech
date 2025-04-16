using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetCleanupBatches;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimStatus
{
    public interface IClaimStatusBatchManager : IManager
    {
        Task<IResult<List<GetAllClaimStatusBatchesResponse>>> GetAllAsync();

        Task<IResult<GetRecentClaimStatusBatchesByClientIdResponse>> GetByIdAsync(int id);

        Task<IResult<int>> CreateAsync(CreateClaimStatusBatchCommand command);

        //Task<IResult<int>> UpdateAsync(UpdateClaimStatusBatchCommand command);

        Task<IResult<int>> UpdateCompletedAsync(int batchId,string rpaCode,int totalClaimsProcssed);
        Task<IResult<int>> UpdateAbortedAsync(int batchId,string abortedReason, string rpaCode);

        //Task<IResult<int>> UpdateDeletedAsync(UpdateClaimStatusBatchCommand command);

        //Task<IResult<int>> DeleteAsync(int batchId);

        Task<IResult<int>> UpdateAssignmentAsync(UpdateAssignedClaimStatusBatchCommand command);

        Task<IResult<List<GetClaimStatusUnprocessedBatchesResponse>>> GetUnprocessedByRpaInsuranceAsync(int rpaInsuranceId, bool isForInitialAnalysis = false);

        Task<IResult<List<GetClaimStatusUnprocessedBatchesResponse>>> GetAllUnprocessedAsync(bool isForInitialAnalysis = false);

        Task<IResult<List<GetRecentClaimStatusBatchesByClientIdResponse>>> GetRecentForClientIdAsync();
        Task<IResult<List<GetClaimStatusUnprocessedBatchesResponse>>> GetBatchesByRpaInsuranceIdAsync(int rpaInsuranceId, bool assignedBatches, bool isForInitialAnalysis = false);


	}
}
