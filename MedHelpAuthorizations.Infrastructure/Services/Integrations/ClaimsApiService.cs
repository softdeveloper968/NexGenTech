using AutoMapper;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Interfaces.Services.ExternalApis;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services.Integrations
{
    public abstract class ClaimsApiService<TTokenResponse, TClaimsApiService> : IClaimsApiService
	{
		private readonly IMapper _mapper;
		protected readonly IServiceProvider _serviceProvider;
		protected readonly IHttpClientFactory _httpClientFactory;
		protected readonly string _httpClientName;
		protected HttpClient _httpClient;
		protected ILogger<TClaimsApiService> _logger { get; }
		protected AuthToken _authToken { get;  private set; }
		public ApiIntegrationEnum ApiIntegrationId { get; protected set; }

		protected readonly ITenantRepositoryFactory _tenantRepositoryFactory;

		public ClaimsApiService(string httpClientName, IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, ITenantRepositoryFactory tenantRepositoryFactory, ILogger<TClaimsApiService> logger, IMapper mapper)
		{
			_mapper = mapper;
			_tenantRepositoryFactory = tenantRepositoryFactory;
			_httpClientName = httpClientName;
			_httpClientFactory = httpClientFactory;
			_httpClient = _httpClientFactory.CreateClient(_httpClientName);
			_serviceProvider = serviceProvider;
			_logger = logger;
			_authToken = new AuthToken();
		}

		public void UpdateAccessToken(string token, long? expiresInSeconds)
		{
			if(token == null)
				throw new ArgumentNullException("token");

			if(_authToken == null)
			{
				_authToken = new AuthToken(token, expiresInSeconds);
			}
			else
			{
				_authToken.Token = token; 
				_authToken.ExpiresInSeconds = expiresInSeconds;
				_authToken.SetCreatedOn(DateTime.UtcNow);
			}
		}
		public AuthToken GetStoredAuthToken()
		{
			return _authToken;
		}

		public virtual async Task<object> Authenticate(string requestUri, Dictionary<string, string> requestBody)
		{
			try
			{
				HttpResponseMessage response =
					await _httpClient.PostAsJsonAsync(requestUri, requestBody);

				if (!response.IsSuccessStatusCode)
				{
					var errorMessage = $"{_httpClientName} - Authentication FAILED!, Status code: {response.StatusCode} -  Reason: {response.ReasonPhrase} ";
					_logger.LogError(errorMessage);
					//TODO:  EnQueue a hangfire job to SEND Email To AIT indicating that we cannot authenticate. 
					//BackgroundJob.Enqueue(() => _manuallyRunJobService.SomeEmailService
					throw new ApiException(errorMessage);
				}

				//deserialize response. 
				var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TTokenResponse>(await response.Content.ReadAsStringAsync());

				return tokenResponse;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				throw new Exception($"Error Authenticating using httpClient: {_httpClientName}.{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
			}
		}

		public abstract Task<bool> EnsureAuthenticated(bool forceReAuthenticate = false);

		public abstract Task<UpsertClaimStatusTransactionCommand> GetClaimStatusFromApi(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier = null);

		public UpsertClaimStatusTransactionCommand MapClaimsApiErrorToUpsertClaimStatusTransactionCommand(string errorMessage, GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, bool isTransientError, string curlScript, ClaimLineItemStatusEnum? claimLineItemStatusEnum)
		{
            // Check if claimLineItemStatusEnum is null; if so, default to ClaimLineItemStatusEnum.Error
            ClaimLineItemStatusEnum statusId = claimLineItemStatusEnum ?? ClaimLineItemStatusEnum.Error;

            // Create the upsert command
            UpsertClaimStatusTransactionCommand upsertCommand = new UpsertClaimStatusTransactionCommand()
            {
                ClaimStatusBatchClaimId = batchClaim.Id,
                ExceptionReason = errorMessage, // make this a friendlyErrorMessage, //Could also be the errorMessage if we do not know exactly what is wrong. 
                ExceptionRemark = errorMessage,
                ClaimLineItemStatusId = isTransientError ? ClaimLineItemStatusEnum.TransientError : statusId, 
                ClaimLineItemStatusValue = isTransientError ? ClaimLineItemStatusEnum.TransientError.GetDescription() : statusId.GetDescription(), 
                ClaimStatusTransactionBeginDateTimeUtc = DateTime.UtcNow,
                ClaimStatusTransactionEndDateTimeUtc = DateTime.UtcNow,
                ErrorMessage = errorMessage,
                CurlScript = curlScript
            };

            return upsertCommand;
        }

		public UpsertClaimStatusTransactionCommand MapBasicUpsertClaimStatusTransactionCommand(string exceptionReason, int batchClaimId, ClaimLineItemStatusEnum statusId = ClaimLineItemStatusEnum.Unknown, string statusValueText = "", ClaimStatusExceptionReasonCategoryEnum? exceptionReasonCategoryId = null)
		{

			UpsertClaimStatusTransactionCommand upsertCommand = new UpsertClaimStatusTransactionCommand()
			{
				ClaimStatusBatchClaimId = batchClaimId,
				ExceptionReason = exceptionReason,
				ClaimLineItemStatusId = statusId,
				ClaimLineItemStatusValue = statusValueText,
				ExceptionReasonCategoryId = exceptionReasonCategoryId,
				ClaimStatusTransactionBeginDateTimeUtc = DateTime.UtcNow,
				ClaimStatusTransactionEndDateTimeUtc = DateTime.UtcNow
			};

			return upsertCommand;
		}


		public abstract Task<UpsertClaimStatusTransactionCommand> MapClaimsApiResponseToUpsertClaimStatusTransactionCommand<T>(T claimsApiResponse, GetClaimStatusBatchClaimsByBatchIdResponse batchClaim, string tenantIdentifier = null);

		public abstract GetClaimStatusBatchClaimsByBatchIdResponse ScrubBatchClaim(GetClaimStatusBatchClaimsByBatchIdResponse batchClaim);

		public class AuthToken
		{
			public string Token { get; internal set; }

			private DateTime? _createdOn;

			internal DateTime? GetCreatedOn()
			{
				return _createdOn;
			}

			internal void SetCreatedOn(DateTime? value)
			{
				_createdOn = value;
			}

			private double? _expiresInSeconds {  get; set; }
			public double? ExpiresInSeconds 
			{
				get { return _expiresInSeconds; }
				set 
				{ 					
					_expiresInSeconds = value;
					ExpiresOn = DateTime.UtcNow.AddSeconds(_expiresInSeconds ?? 3600); // Default to 1 hour
				} 
			}

			public DateTime ExpiresOn 
			{ 
				get; 
				private set;
			}

			internal AuthToken()
			{

			}
			public AuthToken(string token, long? expiresInSeconds) 
			{
				Token = token;
				ExpiresInSeconds = expiresInSeconds; // The setter will make this 1hr equivalent if null
				SetCreatedOn(DateTime.UtcNow);
			}

			public bool IsExpiring()
			{
				if (string.IsNullOrWhiteSpace(this.Token))
					return true;

				//if expire in less then 10 seconds
				var tsDateDifference = this.ExpiresOn.Subtract(DateTime.UtcNow);
				if (tsDateDifference.TotalMinutes < 3)
					return true;

				return false;
			}
		}
	}
}
