using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClaimStatusBatchClaimsRepository : IRepositoryAsync<ClaimStatusBatchClaim, int>
    {
        IQueryable<ClaimStatusBatchClaim> ClaimStatusBatchClaims { get; }

        Task<List<ClaimStatusBatchClaim>> GetInitialClaimStatusByBatchIdAsync(int batchId);        

        Task<int> GetUnresolvedCountByBatchIdAsync(int batchId);

        Task<int> InsertAsync(ClaimStatusBatchClaim claimStatusBatchClaim);

        //Task<List<ClaimStatusBatchClaim>> GetByClaimStatusBatchClaimRootIdAsync(int claimStatusBatchClaimRootId);
        Task<ClaimStatusBatchClaim> GetActiveByClaimNumberAndPatientIdAsync(string claimNumber, int? patientId);

        Task<List<ClaimStatusBatchClaim>> GetByEntryMd5HashAsync(string EntryMd5HashAsync);

        //Task UpdateSupplantedByRootIdAsync(int claimStatusBatchClaimRootId);

        Task UpdateSupplantedByEntryMd5HashAsync(string EntryMd5HashAsync);

        Task UpdateSupplantedByIdAsync(int Id);

        Task<bool> IsSupplantableAsync(ClaimStatusBatchClaim originalClaim, ClaimStatusBatchClaim replacementClaim);

        Task<ClaimStatusBatchClaim> GetActiveByEntryMd5HashAsync(string EntryMd5HashAsync);
        Task<List<ClaimStatusDashboardInProcessDetailsResponse>> GetUncheckedClaims(int ClientId);

        //Task<ClaimStatusBatchClaim> GetActiveByRootIdAsync(int claimStatusBatchClaimRootId);
        Task<List<ClaimStatusBatchClaim>> GetClaimsUnMappedToFeeScheduleByBatchIdAsync(int batchId); //AA-231

        Task<List<GetClaimStatusBatchClaimsByBatchIdResponse>> GetUnresolvedByBatchIdAsync(int batchId, int returnQuantityCap = 5000);//, int pageNumber = 1, int pageSize = 100000);//AA-321

        Task<List<ClaimStatusBatchClaim>> GetClaimsToUpdateAsync(int batchId); //EN-95
        Task<List<ClaimStatusBatchClaim>> GetClaimsByCriteriaAsync(
                                                                          List<int> clientInsuranceIds,
                                                                          int clientCptCodeId,
                                                                          DateTime clientFeeScheduleStartDate,
                                                                          DateTime? clientFeeScheduleEndDate,
                                                                          List<SpecialtyEnum> SpecialtyId = null,
                                                                          List<ProviderLevelEnum> ProviderLevelId = null
                                                                          );


        Task<List<ClaimStatusBatchClaim>> GetClaimStatusByBatchIdAsync(int batchId);
        Task<List<ClaimStatusBatchClaim>> GetClaimsByFeeScheduleCriteriaAsync(
                                                                          List<int> clientInsuranceIds,
                                                                          int clientCptCodeId,
                                                                          DateTime? clientFeeScheduleStartDate,
                                                                          DateTime? clientFeeScheduleEndDate,
                                                                          List<SpecialtyEnum> SpecialtyId = null,
                                                                          List<ProviderLevelEnum> ProviderLevelId = null
                                                                          ); //EN-232

	}
}
