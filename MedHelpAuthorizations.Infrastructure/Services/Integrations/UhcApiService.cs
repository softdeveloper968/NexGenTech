using AutoMapper;
using HttpClientToCurl;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MedHelpAuthorizations.Shared.Constants.IntegratedServices.ExternalApi.UHC;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UhcClaimsApi.Requests.UHC.Claims.V2.ByClaimNumber;
using UhcClaimsApi.Requests.UHC.Claims.V2.ByMemberNumber;
using UhcClaimsApi.Responses.Claims;
using UhcClaimsApi.Responses.UHC.Auth;
using UhcClaimsApi.Responses.UHC.Claims.ByClaimNumber;
using UhcClaimsApi.Responses.UHC.Claims.ByMemberNumber;
using UhcClaimsApi.Routes.UHC;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Infrastructure.Services.Integrations
{
	public class UhcApiService : ClaimsApiService<OAuthTokenResponse, UhcApiService>
	{
        //string csvPath = @"c:\\temp\\ClaimCodes.csv";
        //public void WriteResponseDataToFile(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, GetSummaryByMemberResponse summary = null, GetClaimsDetailByClaimNumberResponse detail = null)
        //{
        //	if(summary == null && detail == null)
        //	{
        //		return;
        //	}

        //	var path = $"c:\\temp\\{batchClaim.ClaimNumber}_{batchClaim.Id}.json";

        //	string newLineSeparators = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
        //	string summaryJson = string.Empty;
        //	string detailJson = string.Empty;
        //	var options = new JsonSerializerOptions { WriteIndented = true };

        //	if (!File.Exists(path))
        //	{
        //		//write batchClaim
        //		File.WriteAllText(path, "***********  INPUT BATCH CLAIM LINE ITEM *****************");

        //		var batchClaimJson = System.Text.Json.JsonSerializer.Serialize(batchClaim, options);
        //		File.AppendAllText(path, batchClaimJson);
        //		// Create a file to write to.
        //		if (summary != null)
        //		{
        //			File.AppendAllText(path, newLineSeparators);
        //			File.AppendAllText(path, "***********  SUMMARY RETURNED FROM API *****************");

        //			summaryJson = System.Text.Json.JsonSerializer.Serialize(summary, options);
        //			File.AppendAllText(path, summaryJson);
        //		}

        //		if (detail != null)
        //		{				
        //			File.AppendAllText(path, newLineSeparators);
        //			File.AppendAllText(path, "***********  DETAIL RETURNED FROM API *****************");

        //			detailJson = System.Text.Json.JsonSerializer.Serialize(detail, options);
        //			File.AppendAllText(path, detailJson);
        //		}
        //	}
        //}

        public UhcApiService(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, ITenantRepositoryFactory tenantRepositoryFactory, ILogger<UhcApiService> logger, IMapper mapper) : base("uhcHttpClient", httpClientFactory, serviceProvider, tenantRepositoryFactory, logger, mapper)
		{
			ApiIntegrationId = ApiIntegrationEnum.UhcClaims;			
		}

		public override async Task<UpsertClaimStatusTransactionCommand> GetClaimStatusFromApi(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier = null)
		{
			if(batchClaim == null)
			{
				return null;
			}

			UpsertClaimStatusTransactionCommand upsertCommand  = null;

			batchClaim = ScrubBatchClaim(batchClaim);
			// If PayerClaimNumber is already recorded.. getDetailByClaimNumber
			// If no Payer Claim Number, get SummaryByMember to get at least a check info if it was paid. Then get DetailByMemberNUmber. 
			if (!string.IsNullOrEmpty(batchClaim.PayerClaimNumber))
			{
				upsertCommand = await ProcessByDetails(batchClaim, tenantIdentifier);

				if (upsertCommand == null || upsertCommand.ExceptionReasonCategoryId == ClaimStatusExceptionReasonCategoryEnum.Duplicate)
				{
					//If UpsertCommand could not be created by ClaimNumber.. or if the result was denied for duplicate, lookup by summary and loop to other claims thatr match
					upsertCommand = await ProcessBySummary(batchClaim, batchClaim.PayerClaimNumber, tenantIdentifier);
				}
			}
			else
			{
				upsertCommand = await ProcessBySummary(batchClaim, batchClaim.PayerClaimNumber, tenantIdentifier);

				if (upsertCommand == null)
				{
					upsertCommand = MapBasicUpsertClaimStatusTransactionCommand("No Claim info not returned from the UHC API", batchClaim.Id, ClaimLineItemStatusEnum.Unavailable, "No Claim Info Returned");
				}
			}
				
			return upsertCommand;			
		}		

		public async Task<UpsertClaimStatusTransactionCommand> ProcessBySummary(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string omitClaimNumber = null, string tenantIdentifier = null)
		{
			UpsertClaimStatusTransactionCommand upsertCommand = null;
			GetSummaryByMemberResponse summary = null;

            summary = await GetClaimsSummaryByMemberNumber(batchClaim);
			if (summary.Error != null && summary.Error.Errors.Any())
			{
                // Fetch the mapped ClaimLineItemStatusId, if available
                var apiClaimsMessageClaimLineitemStatusMapData = await GetApiClaimMessageByCodeAsync(summary.Error.Errors[0].Code, tenantIdentifier);

                // Define the default ClaimLineItemStatusId as ClaimLineItemStatusEnum.Error if no mapping is found
                var claimLineItemStatusId = apiClaimsMessageClaimLineitemStatusMapData?.ClaimLineItemStatusId ?? ClaimLineItemStatusEnum.Error;

                //TODO: Change MapClaimsApiErrorToUpsertClaimStatusTransactionCommand() to accept summary.Error.Errors[0] instead of string errorMessage
                var errorMessage = $"Error Getting Claims Summary By MemberNumber {Environment.NewLine} Code: {summary.Error.Errors[0].Code}   {Environment.NewLine}  Message: {summary.Error.Errors[0].Message}";
				upsertCommand = MapClaimsApiErrorToUpsertClaimStatusTransactionCommand(errorMessage, batchClaim, false, summary.CurlScript, claimLineItemStatusId);
			}
			
			int loopCount = 0;
			
			// If this method called from the detail method for reprocessing.. do not process the same claim number that was just processed in the detail method
			if(!string.IsNullOrWhiteSpace(omitClaimNumber) && summary.Claims.Count > 1)
			{
				summary.Claims = summary.Claims.Where(x => x.ClaimNumber != omitClaimNumber).ToList();
			}
			foreach (var claim in summary.Claims.OrderByDescending(y => y.ClaimSummary.StatusEffectiveOn).ToList())
			{
				loopCount++;
				batchClaim.PayerClaimNumber = claim.ClaimNumber;
				if (claim.DetailsAvailable == "T")
				{
					//Process the detail if available
					upsertCommand = await ProcessByDetails(batchClaim, tenantIdentifier);
					if (upsertCommand != null && upsertCommand.ExceptionReasonCategoryId == ClaimStatusExceptionReasonCategoryEnum.Duplicate)
					{
						if (loopCount < summary.Claims.Count) // If there are more claims to loop through do so. If not - report the duplicate status. 
							continue;
					}
				}
				else
				{
					//If detail not available - Process the high-level Summary Status instead
					upsertCommand = await MapClaimCategoryCodeClaimStatusTransactionCommand(claim, batchClaim, tenantIdentifier);
					break;
				}
			}

			return upsertCommand;
		}   

        public async Task<UpsertClaimStatusTransactionCommand> ProcessByDetails(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier)
		{
			if (string.IsNullOrWhiteSpace(batchClaim?.ClaimNumber))
				return null;

			UpsertClaimStatusTransactionCommand upsertCommand = null;

			var detail = await GetClaimsDetailByClaimNumber(batchClaim);
			if (detail.Error != null && detail.Error.Errors.Any())
			{
                // Fetch the mapped ClaimLineItemStatusId, if available
                var apiClaimsMessageClaimLineitemStatusMapData = await GetApiClaimMessageByCodeAsync(detail.Error.Errors[0].Code, tenantIdentifier);

                // Define the default ClaimLineItemStatusId as ClaimLineItemStatusEnum.Error if no mapping is found
                var claimLineItemStatusId = apiClaimsMessageClaimLineitemStatusMapData?.ClaimLineItemStatusId ?? ClaimLineItemStatusEnum.Error;

                //Create an Error Transaction
                var errorMessage = $"Error Getting Claims Detail By ClaimNumber {Environment.NewLine} Code: {detail.Error.Errors[0].Code}   {Environment.NewLine}  Message: {detail.Error.Errors[0].Message}";
				upsertCommand = MapClaimsApiErrorToUpsertClaimStatusTransactionCommand(errorMessage, batchClaim, false, detail.CurlScript, claimLineItemStatusId);
			}

			var claimLine = detail.ClaimsDetailInfos != null && detail.ClaimsDetailInfos.Any() 
					? detail.ClaimsDetailInfos.SelectMany(x => x.Lines).Where(y => y.ProcedureCode == batchClaim.ProcedureCode.ToUpper().Trim()).FirstOrDefault() 
					: null;

			if (claimLine != null)
			{
				upsertCommand = await MapClaimsApiResponseToUpsertClaimStatusTransactionCommand(detail, batchClaim, tenantIdentifier);
			}			

			return upsertCommand;
		}

		public override async Task<UpsertClaimStatusTransactionCommand> MapClaimsApiResponseToUpsertClaimStatusTransactionCommand<T>(T claimsApiResponse, GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier = null)
		{
			ClaimLineItemStatusEnum lineItemStatusId = ClaimLineItemStatusEnum.Unknown;

			UpsertClaimStatusTransactionCommand upsertCommand = new UpsertClaimStatusTransactionCommand() { ClaimStatusBatchClaimId = batchClaim.Id, ClaimLineItemStatusId = lineItemStatusId };

			var apiResponse = claimsApiResponse as GetClaimsDetailByClaimNumberResponse;
			var claimLine = apiResponse.ClaimsDetailInfos.Any()
									? apiResponse.ClaimsDetailInfos.SelectMany(x => x.Lines)
									.Where(y => y.ProcedureCode == batchClaim.ProcedureCode.ToUpper().Trim()).FirstOrDefault() : null;
			if (claimLine == null) 
			{
				upsertCommand = MapClaimsApiErrorToUpsertClaimStatusTransactionCommand($"Claim Detail not found for Procedure Code: {batchClaim.ProcedureCode}", batchClaim, false, apiResponse.CurlScript, null);
			}
			else
			{
				var _x12ClaimCategoryCodeLineItemStatusRepository = await _tenantRepositoryFactory.GetAsync<IX12ClaimCategoryCodeLineItemStatusRepository>(tenantIdentifier);
				var _x12ClaimCodeLineItemStatusRepository = await _tenantRepositoryFactory.GetAsync<IX12ClaimCodeLineItemStatusRepository>(tenantIdentifier);
				var latestClaimInfo = await GetLatestClaimSummaryInfoFromSummary(apiResponse.ClaimSummaryInfos);
				var claimCategoryCode = await GetClaimCategoryCode507FromSummaryInfo(latestClaimInfo, tenantIdentifier);

				var delimitedClaimCodes = string.Join(",", claimLine.ClaimCodes.Select(x => x.Code.ToUpper().Trim()).ToList());
				var x12ClaimCodes = await _x12ClaimCodeLineItemStatusRepository.GetListByX12DelimitedClaimCodeString(delimitedClaimCodes);

				var highestRankClaimCode = x12ClaimCodes.OrderByDescending(x => x.ClaimLineItemStatus?.Rank ?? 0).FirstOrDefault();

				lineItemStatusId = highestRankClaimCode?.ClaimLineItemStatusId ?? claimCategoryCode?.ClaimLineItemStatusId ?? ClaimLineItemStatusEnum.Unknown;
				var checkInfo = apiResponse?.ClaimSummaryInfos?.Where(x => x.ClaimNumber == batchClaim.PayerClaimNumber)?.FirstOrDefault()?.ClaimSummary?.CheckInfo?.OrderByDescending(y => y?.PaymentIssueDate)?.FirstOrDefault() ?? apiResponse?.ClaimSummaryInfos?.FirstOrDefault()?.ClaimSummary?.CheckInfo?.OrderByDescending(y => y?.PaymentIssueDate)?.FirstOrDefault() ?? null;
				//Check if paid amount or allowed amount > 0. THen look for Paid claim Code. 
				//var isApproved = x12ClaimCodes.Any(x => ReadOnlyObjects.AllPaidClaimLineItemStatuses.ToList().Contains(x.ClaimLineItemStatusId ?? Domain.Entities.Enums.ClaimLineItemStatusEnum.Unknown));
				var isApproved = ReadOnlyObjects.AllPaidClaimLineItemStatuses.ToList().Contains(lineItemStatusId);
				var isPaid = (isApproved && checkInfo != null) || (claimLine?.AllowedAmount ?? 0) > 0;
				var isDenied = !isPaid && ReadOnlyObjects.DeniedClaimLineItemStatuses.ToList().Contains(lineItemStatusId);

				_ = DateTime.TryParse(checkInfo?.PaymentIssueDate, out var checkDate);
				_ = DateTime.TryParse(apiResponse?.ClaimsDetailInfos?.OrderByDescending(x => x.DateReceived)?.FirstOrDefault()?.DateReceived, out var dateReceived);

				upsertCommand = new UpsertClaimStatusTransactionCommand()
				{
					//CheckPaidAmount = checkInfo?.CheckAmount,
					CheckDate = checkDate < DateTime.UtcNow.AddYears(-4) ? null : checkDate,
					CheckNumber = checkInfo?.CheckNumber,
					TotalAllowedAmount = claimLine.AllowedAmount,
					LineItemPaidAmount = claimLine.PaidAmount,
					ClaimLineItemStatusId = isPaid ? ClaimLineItemStatusEnum.Paid : lineItemStatusId,
					ClaimLineItemStatusValue = lineItemStatusId.GetDescription(),
					ClaimNumber = apiResponse.ClaimsDetailInfos.OrderByDescending(x => x.DateReceived).FirstOrDefault().ClaimNumber,
					ClaimStatusBatchClaimId = batchClaim.Id,
					ClaimStatusTransactionBeginDateTimeUtc = DateTime.UtcNow,
					ClaimStatusTransactionEndDateTimeUtc = DateTime.UtcNow,
					CoinsuranceAmount = claimLine.CoInsurance,
					CopayAmount = claimLine.Copay,
					DeductibleAmount = claimLine.Deductible,
					DatePaid = checkDate,
					DateReceived = dateReceived,
					ExceptionReason  = isDenied ? highestRankClaimCode?.Description ?? claimCategoryCode.Description : string.Empty,
					ExceptionReasonCategoryId = isDenied ? highestRankClaimCode?.ClaimStatusExceptionReasonCategoryId : null,
					RemarkCode = x12ClaimCodes != null && x12ClaimCodes.Any() ? string.Join(", ", x12ClaimCodes.Select(x => x.Code).ToList()) : string.Empty,
					RemarkDescription = x12ClaimCodes != null && x12ClaimCodes.Any() ? string.Join(", ", x12ClaimCodes.Select(x => $"{x.Code} - {x.Description}").ToList()) : string.Empty,					
				};				
			}

			return upsertCommand;
		}

		private async Task<UpsertClaimStatusTransactionCommand> MapClaimCategoryCodeClaimStatusTransactionCommand(ClaimSummaryInfo claimSummaryInfo, GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier)
		{
			UpsertClaimStatusTransactionCommand upsertCommand = null;

			if (claimSummaryInfo != null)
			{
				var claimCategoryCode = await GetClaimCategoryCode507FromSummaryInfo(claimSummaryInfo, tenantIdentifier);
				if(claimCategoryCode != null)
				{
					upsertCommand = new UpsertClaimStatusTransactionCommand()
					{
						ClaimStatusBatchClaimId = batchClaim.Id,
						ExceptionReason = ReadOnlyObjects.DeniedClaimLineItemStatuses.ToList().Contains((ClaimLineItemStatusEnum)claimCategoryCode.ClaimLineItemStatusId) ? claimCategoryCode.Description : null,
						ClaimLineItemStatusId = claimCategoryCode.ClaimLineItemStatusId,
						ClaimLineItemStatusValue = claimCategoryCode.ClaimLineItemStatusId.GetDescription(),
						RemarkCode = claimCategoryCode.Code,
						RemarkDescription = claimCategoryCode.Description,	
						ClaimStatusTransactionBeginDateTimeUtc = DateTime.UtcNow,
						ClaimStatusTransactionEndDateTimeUtc = DateTime.UtcNow
					};
				}
				else
				{
					////Process based on the Summary ClaimStatus for entire claim
					switch (claimSummaryInfo.ClaimStatus)
					{
						case UhcClaimsSummaryStatusEnum.Rejected:
							upsertCommand = MapBasicUpsertClaimStatusTransactionCommand("Claim has been rejected.", batchClaim.Id, ClaimLineItemStatusEnum.Rejected, "Rejected");
							break;

						case UhcClaimsSummaryStatusEnum.Finalized:
							upsertCommand = MapBasicUpsertClaimStatusTransactionCommand("Claim has been Finalized. Unknown Status. ", batchClaim.Id, ClaimLineItemStatusEnum.Unknown, "No Claim Category Code");
							break;

						case UhcClaimsSummaryStatusEnum.MultipleStatuses:
							upsertCommand = MapBasicUpsertClaimStatusTransactionCommand("Multiple Statuses", batchClaim.Id, ClaimLineItemStatusEnum.Unknown, "Multiple Statuses");
							break;

						case UhcClaimsSummaryStatusEnum.Denied:
							upsertCommand = MapBasicUpsertClaimStatusTransactionCommand("Claim was denied. No detail returned.", batchClaim.Id, ClaimLineItemStatusEnum.Denied, "Denied");
							break;

						case UhcClaimsSummaryStatusEnum.Acknowledgement:
							upsertCommand = MapBasicUpsertClaimStatusTransactionCommand(string.Empty, batchClaim.Id, ClaimLineItemStatusEnum.Received, "Acknowledged");
							break;
						default:
							break;
					}
				}
			}		

			return upsertCommand;
		}

		public async Task<X12ClaimCategoryCodeLineItemStatus> GetClaimCategoryCode507FromSummaryInfo(ClaimSummaryInfo claimInfo, string tenantIdentifier)
		{
			var _x12ClaimCategoryCodeLineItemStatusRepository = await _tenantRepositoryFactory.GetAsync<IX12ClaimCategoryCodeLineItemStatusRepository>(tenantIdentifier);

			var claimCategoryCode507String = claimInfo.ClaimSummary.ClaimCrossWalkData?.Select(x => x.Clm507Cd)?.FirstOrDefault() ?? string.Empty;
			var claimCategoryCode = await _x12ClaimCategoryCodeLineItemStatusRepository.GetByX12ClaimCategoryCodeString(claimCategoryCode507String.ToUpper().Trim());

			return claimCategoryCode;
		}


		public override async Task<bool> EnsureAuthenticated(bool forceReAuthenticate = false)
		{
			if (_authToken.IsExpiring() || forceReAuthenticate)
			{
				var oAuthTokenResponse = await Authenticate(OAuthTokenEndpoints.Post, UhcApiConstants.TokenBody.BodyDictionary);
				if (oAuthTokenResponse != null && !string.IsNullOrWhiteSpace(((OAuthTokenResponse)oAuthTokenResponse).AccessToken))
				{
					UpdateAccessToken(((OAuthTokenResponse)oAuthTokenResponse).AccessToken, ((OAuthTokenResponse)oAuthTokenResponse).ExpiresIn);
				}
				else
					return false;
			}

			return true;
		}

		public async Task<GetClaimsDetailByClaimNumberResponse> GetClaimsDetailByClaimNumber(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim)
		{
			HttpResponseMessage response = null;
			string data = string.Empty;
			string curlScript = string.Empty;

			if (!await EnsureAuthenticated(false))
			{
				//Log something, alert something
			}

			try
			{
				var headerObject = new ByClaimNumberHeader() 
				{ 
					IncludeSummary = true,
					PayerIdentifier = batchClaim.PayerIdentifier, 
					TaxIdNumber = batchClaim.TaxId ?? string.Empty, 
					ClaimNumber = batchClaim.PayerClaimNumber 
				};
				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.AddObjectAsHttpHeaders(headerObject);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken.Token);
				response = await _httpClient.GetAsync(ClaimDetailEndpoints.ByClaimNumber);
				curlScript = _httpClient.GenerateCurlInString(response.RequestMessage); //Put into a variable
				data = await response.Content.ReadAsStringAsync();

				var claimsDetail = JsonConvert.DeserializeObject<GetClaimsDetailByClaimNumberResponse>(data);
				claimsDetail.CurlScript = curlScript;

				return claimsDetail;
			}
			catch(Exception ex)
			{
				var errorMessage = $"UHC GetClaimsDetailByClaimNumber FAILED: {Environment.NewLine} Status code: {response.StatusCode} -  Reason: {response.ReasonPhrase} {Environment.NewLine} Content: {data} {Environment.NewLine} Curl:  {curlScript}";
				throw new ApiException(errorMessage);
			}
		}

		public async Task<GetClaimsDetailByMemberNumberResponse> GetClaimsDetailByMemberNumber(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim)
		{
			HttpResponseMessage response = null;
			string data = string.Empty;
			string curlScript = string.Empty;

			if (!await EnsureAuthenticated(false))
			{
				//Log something, alert something
			}

			try
			{			
				_httpClient.DefaultRequestHeaders.Clear();
				var headerObject = new ByMemberNumberHeader() { PayerIdentifier = batchClaim.PayerIdentifier, TaxIdNumber = batchClaim.TaxId ?? string.Empty };
				_httpClient.AddObjectAsHttpHeaders(headerObject);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken?.Token);

				var requestBody = new ClaimByMemberCriteria(batchClaim.PolicyNumber, batchClaim.DateOfServiceFrom, batchClaim.DateOfServiceTo, batchClaim.PatientFirstName, batchClaim.PatientLastName, batchClaim.DateOfBirth);
				response = await _httpClient.PostAsJsonAsync(ClaimDetailEndpoints.ByMemberNumber, requestBody);
				curlScript = _httpClient.GenerateCurlInString(response.RequestMessage); //Put into a variable
				data = await response.Content.ReadAsStringAsync();

				var claimsDetail = JsonConvert.DeserializeObject<GetClaimsDetailByMemberNumberResponse>(data);
				claimsDetail.CurlScript = curlScript;

				return claimsDetail;
			}
			catch (Exception ex)
			{
				var errorMessage = $"UHC GetClaimsDetailByMemberNumber FAILED!: {Environment.NewLine} Status code: {response.StatusCode} -  Reason: {response.ReasonPhrase} {Environment.NewLine} Content: {data} {Environment.NewLine} Curl:  {curlScript}";
				throw new Exception(errorMessage, ex);
			}
		}

		public async Task<GetSummaryByClaimNumberResponse> GetClaimsSummaryByClaimNumber(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim)
		{
			GetSummaryByClaimNumberResponse claimSummary = new GetSummaryByClaimNumberResponse();
			HttpResponseMessage response = null;
			string data = string.Empty;
			string curlScript = string.Empty;

			if (!await EnsureAuthenticated(false))
			{
				//Log something, alert something
			}

			try
			{
			
				var headerObject = new ByClaimNumberHeader()
				{
					IncludeSummary = true,
					PayerIdentifier = batchClaim.PayerIdentifier,
					TaxIdNumber = batchClaim.TaxId ?? string.Empty,
					ClaimNumber = batchClaim.PayerClaimNumber
				};
				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.AddObjectAsHttpHeaders(headerObject);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken.Token);

				response = await _httpClient.GetAsync(ClaimSummaryEndpoints.ByClaimNumber);
				curlScript = _httpClient.GenerateCurlInString(response.RequestMessage); //Put into a variable
				data = await response.Content.ReadAsStringAsync();

				claimSummary = JsonConvert.DeserializeObject<GetSummaryByClaimNumberResponse>(data);
				claimSummary.CurlScript = curlScript;

				return claimSummary;
			}
			catch (Exception ex)
			{
				var errorMessage = $"UHC GetClaimsSummaryByClaimNumber FAILED!: {Environment.NewLine} Status code: {response.StatusCode} -  Reason: {response.ReasonPhrase} {Environment.NewLine} Content: {data} {Environment.NewLine} Curl:  {curlScript}";
				throw new Exception(errorMessage, ex);
			}
		}

		private ClaimByMemberCriteria GetClaimByMemberCriteriaForPayerIdentifier(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim)
		{			
			if(batchClaim.PayerIdentifier == "25463")
                return new ClaimByMemberCriteria(null, batchClaim.DateOfServiceFrom, batchClaim.DateOfServiceTo, batchClaim.PatientFirstName, batchClaim.PatientLastName, batchClaim.DateOfBirth);
			else
                return new ClaimByMemberCriteria(batchClaim.PolicyNumber, batchClaim.DateOfServiceFrom, batchClaim.DateOfServiceTo, null, null, batchClaim.DateOfBirth);
        }

        public async Task<GetSummaryByMemberResponse> GetClaimsSummaryByMemberNumber(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim)
		{
			HttpResponseMessage response = null;
			GetSummaryByMemberResponse claimSummary = new GetSummaryByMemberResponse();
			string data = string.Empty;
			string curlScript = string.Empty;

			if (!await EnsureAuthenticated(false))
			{
				//Log something, alert something
				throw new Exception("Unable to Authenticate UhcApiService");
			}

			try
			{
				_httpClient.DefaultRequestHeaders.Clear();
				var headerObject = new ByMemberNumberHeader() { PayerIdentifier = batchClaim.PayerIdentifier, TaxIdNumber = batchClaim.TaxId ?? string.Empty };
				_httpClient.AddObjectAsHttpHeaders(headerObject);
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken.Token);

				var requestBody = GetClaimByMemberCriteriaForPayerIdentifier(batchClaim);
                //var requestBody = new ClaimByMemberCriteria(batchClaim.PolicyNumber, batchClaim.DateOfServiceFrom, batchClaim.DateOfServiceTo, batchClaim.PatientFirstName, batchClaim.PatientLastName, batchClaim.DateOfBirth);


                response = await _httpClient.PostAsJsonAsync(ClaimSummaryEndpoints.ByMemberNumber, requestBody);

				curlScript = _httpClient.GenerateCurlInString(response.RequestMessage); //Put into a variable
																							   //_httpClient.GenerateCurlInConsole(response.RequestMessage); //Show in the console

				data = await response.Content.ReadAsStringAsync();

				claimSummary = JsonConvert.DeserializeObject<GetSummaryByMemberResponse>(data);
				claimSummary.CurlScript = curlScript;

				return claimSummary;
			} 
			catch (Exception ex)
			{
				var errorMessage = $"UHC GetClaimsSummaryByMemberNumber FAILED!: {Environment.NewLine} Status code: {response.StatusCode} -  Reason: {response.ReasonPhrase} {Environment.NewLine} Content: {data} {Environment.NewLine} Curl:  {curlScript}";
				throw new Exception(errorMessage, ex);
			}
		}

		private async Task<ClaimSummaryInfo> GetLatestClaimSummaryInfoFromSummary(List<ClaimSummaryInfo> claimSummaryInfos, bool? isDetailsAvailable = null)
		{
			ClaimSummaryInfo latestClaimInfo;
			if (isDetailsAvailable != null)
			{
				latestClaimInfo  = claimSummaryInfos.OrderByDescending(y => y.ClaimSummary.StatusEffectiveOn).FirstOrDefault(x => x.DetailsAvailable == ((bool)isDetailsAvailable ? "T" : "F"));
			}
			else
			{
				latestClaimInfo = claimSummaryInfos.OrderByDescending(y => y.ClaimSummary.StatusEffectiveOn).FirstOrDefault();
			}

			return latestClaimInfo;
		}

		public override GetClaimStatusBatchClaimsByBatchIdResponse ScrubBatchClaim(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim)
		{
			// remove trailing 0's after 9 characters.  TODO: DO something better with Regex. 
			var policyNumber = batchClaim.PolicyNumber.Trim();

			if (policyNumber.Length > 9)
			{
				int.TryParse(policyNumber.Substring(9), out int trailingInt);
				if (trailingInt == 0)
					batchClaim.PolicyNumber = policyNumber.Substring(0,9);
			}
			return batchClaim;
		}

        /// <summary>
        /// Retrieves the <see cref="ApiClaimsMessageClaimLineitemStatusMap"/> entry associated with the specified error code
        /// for the given tenant identifier.
        /// </summary>
        /// <param name="code">The error code used to look up the corresponding <see cref="ApiClaimsMessageClaimLineitemStatusMap"/>.</param>
        /// <param name="tenantIdentifier">The unique identifier for the tenant to scope the data retrieval.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the 
        /// <see cref="ApiClaimsMessageClaimLineitemStatusMap"/> entry associated with the specified error code, or null 
        /// if no matching entry is found.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="code"/> is null or empty.</exception>
        private async Task<ApiClaimsMessageClaimLineitemStatusMap> GetApiClaimMessageByCodeAsync(string code, string tenantIdentifier)
        {
            var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);

            // Validate input
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code cannot be null or empty.", nameof(code));
            }

            // Fetch the data from the database
            var result = await _unitOfWork.Repository<ApiClaimsMessageClaimLineitemStatusMap>().Entities.FirstOrDefaultAsync(m => m.Code == code);

            // Return the result
            return result;
        }


        //public void WriteClaimCodesToCsv(List<ClaimCode> claimCodes)
        //{
        //	if (!File.Exists(csvPath))
        //	{
        //		using (var writer = new StreamWriter(csvPath))
        //		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //		{
        //			//csv.WriteHeader<ClaimCode>();
        //			csv.WriteRecords(claimCodes);
        //		}
        //	}
        //	else
        //	{
        //		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        //		{
        //			// Don't write the header again.
        //			HasHeaderRecord = false,
        //		};
        //		using (var stream = File.Open(csvPath, FileMode.Append))
        //		using (var writer = new StreamWriter(stream))
        //		using (var csv = new CsvWriter(writer, config))
        //		{
        //			csv.WriteRecords(claimCodes);
        //		}
        //	}
        //}
    }
}
