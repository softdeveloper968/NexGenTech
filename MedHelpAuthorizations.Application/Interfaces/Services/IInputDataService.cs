using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IInputDataService
    {
        Task<IList<ClaimStatusBatchClaim>> DeserializeClaimStatusInputDataAsync(string inputDocumentUrl);
        Task<IList<ClaimStatusWriteOffs>> DeserializeClaimStatusWriteOffDataAsync(byte[] inputDocumentBytes, string fileExtension);
        Task<int?> GetPatientId(ClaimStatusBatchClaimModel patientInfo, CancellationToken cancellationToken);

        Task<Tuple<IList<ClaimStatusBatchClaim>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, int>> DeserializeClaimStatusInputDataAsync(IList<ClaimStatusBatchClaim> batchClaims,
                         IList<ClaimStatusBatchClaimModel> erroredBatchClaims,
                         IList<ClaimStatusBatchClaimModel> unmatchedLocationBatchClaims,
                         IList<ClaimStatusBatchClaimModel> unmatchedProviderBatchClaims,
                         IList<ClaimStatusBatchClaimModel> filesDuplicates,
                         IList<ClaimStatusBatchClaimModel> unsupplantableDuplicates,
						  int attemptedImportCount,
						 int clientInsuranceId,
                         byte[] inputDocumentBytes,
                         string fileExtension,
                         int inputDocumentId
                        );
        Task<IList<ClientFeeScheduleEntryCsvViewModel>> DeserializeClientFeeScheduleEntryDataAsync(byte[] inputDocumentBytes, string fileExtension); // AA-221

        Task<ClaimStatusBatch> ProcessClaimStatusBatchClaims(InputDocument inputDocument, IList<ClaimStatusBatchClaim> claimStatusBatchClaims, int? authTypeId, CancellationToken cancellationToken);

        Task<List<ImportDocumentMessage>> CreateImportDocumentMessages(List<ClaimStatusBatchClaimModel> erroredBatchClaims, List<ClaimStatusBatchClaimModel> unmatchedProviderBatchClaims, List<ClaimStatusBatchClaimModel> unmatchedLocationBatchClaims, int claimStatusBatchId, int inputDocumentId, List<ClaimStatusBatchClaimModel> filesDuplicates, List<ClaimStatusBatchClaimModel> unsupplantableDuplicates);

        Task ProcessClaimStatusBatches(InputDocument inputDocument, CancellationToken cancellationToken);

        Task<int?> GetClientProviderByNpi(string renderingNpi);

        Task<int?> GetClientLocationByName(string locationName);

        List<ClaimStatusBatchClaim> ProcessProcedureCodesAsync(ref List<ClaimStatusBatchClaim> batchClaimsCollection, int clientInsuranceId, IList<ClaimStatusBatchClaimModel> duplicateBatchClaims, IList<ClaimStatusBatchClaimModel> unsupplantableDuplicates, int inputDocumentId);

    }
}