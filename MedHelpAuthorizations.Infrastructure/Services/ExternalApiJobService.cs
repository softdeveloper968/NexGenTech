using AutoMapper;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.ExternalApis;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Services.Integrations;
using MedHelpAuthorizations.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
	public class ExternalApiJobService : IExternalApiJobService
	{
        private readonly IMailService _mailService;
        private readonly ILogger<ExternalApiJobService> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
		private readonly IClaimStatusTransactionService _claimStatusTransactionService;
		private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ITenantManagementService _tenantManagementService;
		private readonly IEnumerable<IClaimsApiService> _claimsApiServices;

		public ExternalApiJobService(IEnumerable<IClaimsApiService> claimsApiServices,
			IMailService mailService,
			ILogger<ExternalApiJobService> logger,
			IMediator mediator,
            IMapper mapper,
			IClaimStatusTransactionService claimStatusTransactionService,
			ITenantRepositoryFactory tenantRepositoryFactory,
            ITenantManagementService tenantManagementService)
        {
			_claimStatusTransactionService = claimStatusTransactionService;
			_claimsApiServices = claimsApiServices;
            _mailService = mailService;
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _tenantManagementService = tenantManagementService;
		}
		public async Task<bool> ProcessApiClaimStatus()
		{
			try
			{
				List<ErroredClaim> erroredClaims = new List<ErroredClaim>();

				//Loop through all ApiServices 
				foreach (var apiService in _claimsApiServices)
				{
					// Retrieve all tenants
					// loop through each tenant 
					var tenants = await _tenantManagementService.GetAllActiveAsync();
					foreach (var tenant in tenants ?? new List<TenantDto>())
					{
						var isAuthenticated = await apiService.EnsureAuthenticated();
						if (!isAuthenticated)
						{
							//TODO: SEND Alert email to AIT stating that we cannot authenticate to the current ClaimsApiServiceImplementation
							continue;
						}

						//Get Repositories for tenant
						var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
						var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);
						var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenant.Identifier);
						var _claimStatusTransactionRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionRepository>(tenant.Identifier);
						var _claimStatusTransactionLineItemStatusChangeRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionLineItemStatusChangeRepository>(tenant.Identifier);
						var _x12ClaimCategoryCodeLineItemStatusRepository = _tenantRepositoryFactory.GetAsync<IX12ClaimCategoryCodeLineItemStatusRepository>(tenant.Identifier);
						var _x12ClaimCodeLineItemStatusRepository = _tenantRepositoryFactory.GetAsync<IX12ClaimCodeLineItemStatusRepository>(tenant.Identifier);


						//Get RpaInsurances the apiService processes
						var rpaInsurances = await _unitOfWork.Repository<RpaInsurance>().Entities.Include(x => x.ApiIntegration).Include(x => x.ApiIntegration).Where(y => y.ApiIntegrationId == apiService.ApiIntegrationId).ToListAsync();
						foreach (var rpa in rpaInsurances)
						{
							//Get Batches that need processing and reference the RpaInsurance
							var claimStatusBatchList = await _claimStatusBatchRepository.GetByRpaInsuranceId(new GetClaimStatusBatchesByRpaInsuranceIdQuery() { RpaInsuranceId = rpa.Id }, rpa.ApiIntegrationId);
							foreach (var batch in claimStatusBatchList) 
							{
								if ((rpa.ApiIntegration.RequirePayerIdentifier && string.IsNullOrWhiteSpace(batch.ClientInsurance.PayerIdentifier)) || (rpa.ApiIntegration.RequireTaxId && batch.Client.TaxId == null))
								{
									//TODO: If PayerIdentifier of the batch.ClientInsurance or Client TaxID is required and missing... Do Something
									//Send email stating what needs to be configured properly
									continue;
								}

								//Get Claims that are able to be processed for the current batch
								var batchClaims = await _claimStatusBatchClaimsRepository.GetUnresolvedByBatchIdAsync(batch.Id);
								//var notransactionsList = batchClaims.Where(x => x.ClaimStatusTransactionId == null).ToList();
								//Loop through the claims and send info to UHC api, 
								foreach (var claim in batchClaims) //batchClaims.Where(x => x.ClaimStatusTransactionId == null))
								{
									//if (claim.ClaimStatusTransactionId != null )
									//	continue;
									//TODO: Get all claim lines for the claim number and process them all at once. Remove from batchClaims foreach collection after processed so do not process again .
									//List<GetClaimStatusBatchClaimsByBatchIdResponse> groupedLInes = batchClaims.Where(x => x.NormalizedClaimNumber == claim.NormalizedClaimNumber).ToList();
									UpsertClaimStatusTransactionCommand upsertCommand = null;
									int? transactionId;
									if ((rpa.ApiIntegration.RequirePolicyNumber && string.IsNullOrWhiteSpace(claim.PolicyNumber)) || (rpa.ApiIntegration.RequireDateOfBirth && string.IsNullOrWhiteSpace(claim.DateOfBirthString)))
									{
										var errorMessage = $"Policy or DatOfBirth is missing:  Policy: {claim.PolicyNumber}    DatOfBirth: {claim.DateOfBirthString}";
										//Console.WriteLine(errorMessage);
										erroredClaims.Add(new ErroredClaim() { Claim = claim, ErrorMessage = errorMessage, TenantIdentifier = tenant.Identifier });
										upsertCommand = apiService.MapBasicUpsertClaimStatusTransactionCommand(errorMessage, claim.Id, ClaimLineItemStatusEnum.Ignored, "Ignored - Missing Lookup Info", ClaimStatusExceptionReasonCategoryEnum.DemographicsIssue);
										//await _mediator.Send(apiService.MapClaimsApiErrorToUpsertClaimStatusTransactionCommand(errorMessage, claim, false));
										transactionId = await _claimStatusTransactionService.UpsertClaimStatusTransaction(upsertCommand, tenant.Identifier);
										continue;
									}
									try
									{
										//Get UpsertClaimStatustransactionCommand from Api Methods
										upsertCommand = await apiService.GetClaimStatusFromApi(claim, tenant.Identifier);
										if (upsertCommand == null)
											throw new Exception("apiService.GetClaimStatusFromApi returned null");

										if(!string.IsNullOrWhiteSpace(upsertCommand.ErrorMessage))
											erroredClaims.Add(new ErroredClaim() { Claim = claim, ErrorMessage = upsertCommand.ErrorMessage, TenantIdentifier = tenant.Identifier });

										//TODO: Bring Back
										transactionId = await _claimStatusTransactionService.UpsertClaimStatusTransaction(upsertCommand, tenant.Identifier);
			
									}
									catch (Exception ex)
									{
										var errorMessage = $"Get claim status failed for PayerIdentifier:  {claim.PayerIdentifier} {Environment.NewLine} {ex.GetBaseException()}";
										
										Console.WriteLine(errorMessage);
										erroredClaims.Add(new ErroredClaim() { Claim = claim, ErrorMessage = errorMessage, TenantIdentifier = tenant.Identifier });
										upsertCommand = apiService.MapClaimsApiErrorToUpsertClaimStatusTransactionCommand(errorMessage, claim, true, string.Empty, null);
										transactionId = await _claimStatusTransactionService.UpsertClaimStatusTransaction(upsertCommand, tenant.Identifier);                                       
										//TODO:  pickup here . send emails. 
									}
								}
							}
						}
					}

					var foo = "";
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed Processing API Claim Status. Error - " + ex.Message);
				return false;
			}

			return true;
		}

		public class ErroredClaim
		{
			public ErroredClaim() { }
			public string TenantIdentifier { get; set; }
			public GetClaimStatusBatchClaimsByBatchIdResponse Claim { get; set; }

			//public ResponseError responseError { get; set; }
			public string ErrorMessage { get; set; }
		}

		#region UHC Claims Api Test Methods

		public async Task<bool> TestUhcServiceGetClaimSummaryByMember()
		{
			UhcApiService _uhcApiService = (UhcApiService)_claimsApiServices.Where(x => x.ApiIntegrationId == ApiIntegrationEnum.UhcClaims).First();

			GetClaimStatusBatchClaimsByBatchIdResponse batchClaim = new GetClaimStatusBatchClaimsByBatchIdResponse()
			{
				PayerIdentifier = "87726",// "74227",
				TaxId = "237267007",
				PolicyNumber = "0217158025", //"997413880",
				DateOfServiceFrom = new DateTime(2024, 1, 25),
				PatientFirstName = "SARAH",
				PatientLastName = "BROWN",
				DateOfBirth = new DateTime(1986, 10, 18)
			};
			try
			{
				Console.WriteLine($"Getting claim summary by MemberNUmber for PayerIdentifier:  {batchClaim.PayerIdentifier}");
				var claimDetail = await _uhcApiService.GetClaimsSummaryByMemberNumber(batchClaim);
				Console.WriteLine($"Successful claim summary by MemberNUmber for PayerIdentifier:  {batchClaim.PayerIdentifier}");
				//continue; 

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Get claim summary by MemberNUmber failed for PayerIdentifier:  {batchClaim.PayerIdentifier} {Environment.NewLine} {ex.GetBaseException()}");
			}

			return true;
		}

		public async Task<bool> TestUhcServiceGetClaimSummaryByClaimNumber()
		{
			UhcApiService _uhcApiService = (UhcApiService)_claimsApiServices.Where(x => x.ApiIntegrationId == ApiIntegrationEnum.UhcClaims).First();

			GetClaimStatusBatchClaimsByBatchIdResponse batchClaim = new GetClaimStatusBatchClaimsByBatchIdResponse()
			{
				PayerIdentifier = "87726",// "74227",
				PayerClaimNumber = "EH36815697",
				TaxId = "237267007",
				PolicyNumber = "0217158025", //"997413880",
				DateOfServiceFrom = new DateTime(2024, 1, 25),
				PatientFirstName = "SARAH",
				PatientLastName = "BROWN",
				DateOfBirth = new DateTime(1986, 10, 18)
			};
			try
			{
				Console.WriteLine($"Getting summary by claimNumber for PayerIdentifier:  {batchClaim.PayerIdentifier}");
				var claimDetail = await _uhcApiService.GetClaimsSummaryByClaimNumber(batchClaim);
				Console.WriteLine($"Successful summary by claimNumber for PayerIdentifier:  {batchClaim.PayerIdentifier}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Get summary by claimNumber failed for PayerIdentifier:  {batchClaim.PayerIdentifier} {Environment.NewLine} {ex.GetBaseException()}");
			}

			return true;
		}
		public async Task<bool> TestUhcServiceGetClaimDetailByMember()
		{
			UhcApiService _uhcApiService = (UhcApiService)_claimsApiServices.Where(x => x.ApiIntegrationId == ApiIntegrationEnum.UhcClaims).First();

			GetClaimStatusBatchClaimsByBatchIdResponse batchClaim = new GetClaimStatusBatchClaimsByBatchIdResponse()
			{
				PayerIdentifier = "87726",// "74227", // 87726 Fails, but works for all the other api calls. 
				TaxId = "237267007",
				PolicyNumber = "0217158025", //"997413880",
				DateOfServiceFrom = new DateTime(2024, 1, 25),
				PatientFirstName = "SARAH",
				PatientLastName = "BROWN",
				DateOfBirth = new DateTime(1986, 10, 18)
			};
			try
			{
				Console.WriteLine($"Getting detail by Member for PayerIdentifier:  {batchClaim.PayerIdentifier}");
				var claimDetail = await _uhcApiService.GetClaimsDetailByMemberNumber(batchClaim);
				Console.WriteLine($"Successful detail by member for PayerIdentifier:  {batchClaim.PayerIdentifier}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Get detail by Member failed for PayerIdentifier:  {batchClaim.PayerIdentifier} {Environment.NewLine} {ex.GetBaseException()}");
			}
			return true;
		}

		public async Task<bool> TestUhcServiceGetClaimDetailByClaimNumber()
		{
			UhcApiService _uhcApiService = (UhcApiService)_claimsApiServices.Where(x => x.ApiIntegrationId == ApiIntegrationEnum.UhcClaims).First();

			GetClaimStatusBatchClaimsByBatchIdResponse batchClaim = new GetClaimStatusBatchClaimsByBatchIdResponse()
			{
				PayerIdentifier = "87726",// "74227",
				PayerClaimNumber = "EH36815697",
				TaxId = "237267007",
				PolicyNumber = "0217158025", //"997413880",
				DateOfServiceFrom = new DateTime(2024, 1, 25),
				PatientFirstName = "SARAH",
				PatientLastName = "BROWN",
				DateOfBirth = new DateTime(1986, 10, 18)
			};
			try
			{
				Console.WriteLine($"Getting detail by claimNumber for PayerIdentifier:  {batchClaim.PayerIdentifier}");
				var claimDetail = await _uhcApiService.GetClaimsDetailByClaimNumber(batchClaim);
				Console.WriteLine($"Successful detail by claimNumber for PayerIdentifier:  {batchClaim.PayerIdentifier}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Get detail by claimNumber failed for PayerIdentifier:  {batchClaim.PayerIdentifier} {Environment.NewLine} {ex.GetBaseException()}");
			}

			return true;
		}
		#endregion
	}
}

