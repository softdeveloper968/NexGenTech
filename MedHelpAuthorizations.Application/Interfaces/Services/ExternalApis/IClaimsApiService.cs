using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services.ExternalApis
{
	public interface IClaimsApiService
	{
		public ApiIntegrationEnum ApiIntegrationId { get; }
		Task<object> Authenticate(string requestUri, Dictionary<string, string> requestBody);
		Task<bool> EnsureAuthenticated(bool forceReAuthenticate = false);
		Task<UpsertClaimStatusTransactionCommand> GetClaimStatusFromApi(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier = null);
		UpsertClaimStatusTransactionCommand MapClaimsApiErrorToUpsertClaimStatusTransactionCommand(string errorMessage, GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, bool isTransientError, string curlScript, ClaimLineItemStatusEnum? claimLineItemStatusEnum);
		Task<UpsertClaimStatusTransactionCommand> MapClaimsApiResponseToUpsertClaimStatusTransactionCommand<T>(T claimsApiResponse, GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier = null);
		UpsertClaimStatusTransactionCommand MapBasicUpsertClaimStatusTransactionCommand(string exceptionReason, int batchClaimId, ClaimLineItemStatusEnum statusId = ClaimLineItemStatusEnum.Unknown, string statusValueText = "", ClaimStatusExceptionReasonCategoryEnum? exceptionReasonCategoryId = null);
		GetClaimStatusBatchClaimsByBatchIdResponse ScrubBatchClaim(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim);

	}
}
