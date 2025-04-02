using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Net.Http;
using UhcClaimsApi.Responses.UHC.Auth;
using UhcClaimsApi.Responses.UHC.Claims.ByClaimNumber;
using UhcClaimsApi.Responses.UHC.Claims.ByMemberNumber;

namespace MedHelpAuthorizations.Application.Interfaces.Services.ExternalApis
{
	public interface IUhcApiService //: IClaimsApiService
	{
		Task<Tuple<HttpClient, OAuthTokenResponse>> Authenticate();

		// Api V2
		Task<GetClaimsDetailByMemberNumberResponse> GetClaimsDetailByMemberNumber(ClaimStatusBatchClaim batchClaim, string accessToken = null);
		Task<GetClaimsDetailByClaimNumberResponse> GetClaimsDetailByClaimNumber(ClaimStatusBatchClaim batchClaim, string accessToken = null);
	}
}
