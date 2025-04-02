//using MedHelpAuthorizations.Application.Interfaces.Services;
//using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
//using MedHelpAuthorizations.Shared.Enums;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria;
//using MedHelpAuthorizations.Application.Interfaces.Repositories;
//using UhcClaimsApi.Requests.Claims;
//using Azure.Core;
//using System.Net.Http;
//using UhcClaimsApi.Responses.Auth;

//namespace MedHelpAuthorizations.Infrastructure.Services
//{
//	public class ClaimApiError
//	{
//		public ClaimApiError() { }

//	}
//    public partial class NightlyJobService : INightlyJobService
//    {
//		public async Task<bool> ProcessUhcApiClaimStatus()
//		{
//			//try
//			//{
//				// Retrieve all tenants
//				// loop through each tenant 

//			//	Tuple<HttpClient, OAuthTokenResponse> authenticateObjects = await _uhcApiService.Authenticate();				
//			//	var oAuthTokenResponse = authenticateObjects.Item2 as OAuthTokenResponse;
//			//	var accessToken = oAuthTokenResponse.AccessToken;

//			//	var tenants = await _tenantManagementService.GetAllActiveAsync();
//			//	foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
//			//	{
//			//		List<ClaimApiError> errored = new List<ClaimApiError>();
//			//		//Get all UHC Batches for RpaId's that have UHC Claims Api ApiType
//			//		var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
//			//		var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);
//			//		var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenant.Identifier);
//			//		var _claimStatusTransactionRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionRepository>(tenant.Identifier);
//			//		var _claimStatusTransactionLineItemStatusChangeRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionLineItemStatusChangeRepository>(tenant.Identifier);

//			//		var uhcRpaInsurances = await _unitOfWork.Repository<RpaInsurance>().Entities.Where(x => x.ApiIntegrationId == ApiIntegrationEnum.UhcClaims).ToListAsync();
//			//		// Loop through batches that have insurances that have the InsuranceIdentifier that is appropriate for the UHC endpoint. 
//			//		foreach(var uhc in uhcRpaInsurances)
//			//		{
//			//			var claimStatusBatchList = await _claimStatusBatchRepository.GetByRpaInsuranceId(new GetClaimStatusBatchesByRpaInsuranceIdQuery() { RpaInsuranceId = uhc.Id });
//			//			foreach(var batch in claimStatusBatchList)
//			//			{
//			//				//IF PayerIdentifier is null Do Something


//			//				// Get Claims for the batches
//			//				var batchClaims = await _claimStatusBatchClaimsRepository.GetUnresolvedByBatchIdAsync(batch.Id);

//			//				//Loop through the claims and send info to UHC api, 
//			//				foreach (var claim in batchClaims)
//			//				{
//			//					// may need to check payer id and call 2 different methods depending on the payer ID .
//			//					//Note: By ClaimNumber - Allowed payerIds are 87726, 03432, 96385, 95467, 86050, 86047, 95378, 06111, 39026, 37602.

//			//					// By MemberNumber - *  This api operation works only with payerId 06111, 74227, 25463
//			//					// *In case of payer Id - 74227 only Member ID and Patient DOB search combination will work.
//			//					//*In case of payer Id - 25463 only Patient First Name, Last Name and Patient DOB search combination will work
//			//					//*This api will take more than expected time if there are multiple claims for the criteria.
//			//					try
//			//					{
//			//						if(string.IsNullOrWhiteSpace(claim.TaxId) || string.IsNullOrWhiteSpace(claim.PayerIdentifier) || string.IsNullOrWhiteSpace(claim.PolicyNumber) || string.IsNullOrWhiteSpace(claim.DateOfBirthString))
//			//						{
//			//							continue;
//			//						}
//			//						var claimsDetail = await _uhcApiService.GetClaimsDetailByMemberNumberObsolete(new UhcClaimsApi.Requests.Claims.V1.GetClaimsDetailByMemberNumber() 
//			//						{ 
//			//							TaxIdNumber = claim.TaxId,
//			//							PatientDateOfBirth = claim.DateOfBirthString,
//			//							MemberId = claim.PolicyNumber, 
//			//							PayerIdentifier = claim.PayerIdentifier,
//			//							FirstServiceDate =  claim.DateOfServiceFromString,
//			//							LastServiceDate = claim.DateOfServiceFromString,
//			//						}, accessToken);

//			//						//var claimsDetail = await _uhcApiService.GetClaimsDetailByClaimNumber(new GetClaimsDetailByClaimNumber() { TaxIdNumber = claim.TaxId, ClaimNumber = claim.ClaimNumber, PayerIdentifier = claim.PayerIdentifier });
//			//						if (claimsDetail?.ClaimsDetails != null)
//			//						{
//			//							var foo = string.Empty;
//			//						}
//			//						else
//			//						{
//			//							var foo = string.Empty;
//			//						}
//			//					}
//			//					catch (Exception ex)
//			//					{
//			//						Console.WriteLine(ex.ToString());
//			//					}

								
//			//					/// Process the responses. Make transaction - et cetera. 

//			//				}
//			//			}
//			//		}
//			//	}
//			//}
//			//catch (Exception ex)
//			//{
//			//	// Log errors related to retrieving or processing unresolved claim status batches
//			//	_logger.LogError("Failed getting unresolved claim status batches. Error - " + ex.Message);
//			//	return false;
//			//}
			
//			return true;
//		}

//		public async Task<bool> ProcessApiClaimStatus(string tenantIdentifier, ApiIntegrationEnum apiIntegration)
//		{
//			//try
//			//{
//			//	// Retrieve all tenants
//			//	// loop through each tenant 

//			//	Tuple<HttpClient, OAuthTokenResponse> authenticateObjects = await _uhcApiService.Authenticate();
//			//	var oAuthTokenResponse = authenticateObjects.Item2 as OAuthTokenResponse;
//			//	var accessToken = oAuthTokenResponse.AccessToken;
//			//	List<ClaimApiError> errored = new List<ClaimApiError>();

//			//	var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
//			//	var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenantIdentifier);
//			//	var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenantIdentifier);
//			//	var _claimStatusTransactionRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionRepository>(tenantIdentifier);
//			//	var _claimStatusTransactionLineItemStatusChangeRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionLineItemStatusChangeRepository>(tenantIdentifier);

//			//	var rpaInsurances = await _unitOfWork.Repository<RpaInsurance>().Entities.Where(x => x.ApiIntegrationId == apiIntegration).ToListAsync();
//			//	// Loop through batches that have insurances that have the InsuranceIdentifier that is appropriate for the UHC endpoint. 
//			//	foreach (var rpa in rpaInsurances)
//			//	{
//			//		var claimStatusBatchList = await _claimStatusBatchRepository.GetByRpaInsuranceId(new GetClaimStatusBatchesByRpaInsuranceIdQuery() { RpaInsuranceId = rpa.Id });
//			//		foreach (var batch in claimStatusBatchList)
//			//		{
//			//			//IF PayerIdentifier is null Do Something


//			//			// Get Claims for the batches
//			//			var batchClaims = await _claimStatusBatchClaimsRepository.GetUnresolvedByBatchIdAsync(batch.Id);

//			//			//Loop through the claims and send info to UHC api, 
//			//			foreach (var claim in batchClaims)
//			//			{
//			//				// may need to check payer id and call 2 different methods depending on the payer ID .
//			//				//Note: By ClaimNumber - Allowed payerIds are 87726, 03432, 96385, 95467, 86050, 86047, 95378, 06111, 39026, 37602.

//			//				// By MemberNumber - *  This api operation works only with payerId 06111, 74227, 25463
//			//				// *In case of payer Id - 74227 only Member ID and Patient DOB search combination will work.
//			//				//*In case of payer Id - 25463 only Patient First Name, Last Name and Patient DOB search combination will work
//			//				//*This api will take more than expected time if there are multiple claims for the criteria.
//			//				try
//			//				{
//			//					if (string.IsNullOrWhiteSpace(claim.TaxId) || string.IsNullOrWhiteSpace(claim.PayerIdentifier) || string.IsNullOrWhiteSpace(claim.PolicyNumber) || string.IsNullOrWhiteSpace(claim.DateOfBirthString))
//			//					{
//			//						continue;
//			//					}
//			//					var claimsDetail = await _uhcApiService.GetClaimsDetailByMemberNumberObsolete(new UhcClaimsApi.Requests.Claims.V1.GetClaimsDetailByMemberNumber()
//			//					{
//			//						TaxIdNumber = claim.TaxId,
//			//						PatientDateOfBirth = claim.DateOfBirthString,
//			//						MemberId = claim.PolicyNumber,
//			//						PayerIdentifier = claim.PayerIdentifier,
//			//						FirstServiceDate = claim.DateOfServiceFromString,
//			//						LastServiceDate = claim.DateOfServiceFromString,
//			//					}, accessToken);

//			//					//var claimsDetail = await _uhcApiService.GetClaimsDetailByClaimNumber(new GetClaimsDetailByClaimNumber() { TaxIdNumber = claim.TaxId, ClaimNumber = claim.ClaimNumber, PayerIdentifier = claim.PayerIdentifier });
//			//					if (claimsDetail?.ClaimsDetails != null)
//			//					{
//			//						var foo = string.Empty;
//			//					}
//			//					else
//			//					{
//			//						var foo = string.Empty;
//			//					}
//			//				}
//			//				catch (Exception ex)
//			//				{
//			//					Console.WriteLine(ex.ToString());
//			//				}


//			//				/// Process the responses. Make transaction - et cetera. 

//			//			}
//			//		}
//			//	}				
//			//}
//			//catch (Exception ex)
//			//{
//			//	// Log errors related to retrieving or processing unresolved claim status batches
//			//	_logger.LogError("Failed getting unresolved claim status batches. Error - " + ex.Message);
//			//	return false;
//			//}

//			return true;
//		}
//	}
//}



