using AutoMapper;
using MedHelpAuthorizations.Application;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.GetClaimsData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Requests.Mail;
using MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RazorEngineCore;
using self_pay_eligibility_api.Domain.Entities.Enums.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Specifications;
using System.Security.Claims;


namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ManuallyRunJobService : IManuallyRunJobService
    {
        private readonly IMailService _mailService;
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
        private readonly IClaimStatusTransactionRepository _claimStatusTransactionRepository;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository; //AA-231
        private readonly IChargeEntryBatchRepository _chargeEntryBatchRepository;
        private readonly IChargeEntryQueryService _chargeEntryQueryService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;
        private readonly ILogger<ManuallyRunJobService> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;
        private readonly IExcelService _excelService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IClientCptCodeRepository _clientCptCodeRepository;
        private readonly IClientFeeScheduleService _clientFeeScheduleService;
        private readonly IClaimNumberNormalizationService _claimNumberNormalizationService; //EN-35
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory; //EN-95
        private readonly ITenantManagementService _tenantManagementService;
        private HttpClient _httpClient;
        //private readonly ITenantInfo _tenantInfo;


        public ManuallyRunJobService(IMailService mailService,
            IAuthorizationRepository authorizationRepository,
            IClaimStatusBatchRepository claimStatusBatchRepository,
            IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, //AA-231
            IClaimStatusTransactionRepository claimStatusTransactionRepository,
            IChargeEntryQueryService chargeEntryQueryService,
            IClaimStatusQueryService claimStatusQueryService,
            IHttpClientFactory httpClientFactory,
            IUserService userService,
            ILogger<ManuallyRunJobService> logger,
            IClientRepository clientRespository,
            IConfiguration configuration,
            IExcelService excelService,
            IMediator mediator,
            IMapper mapper,
            IUnitOfWork<int> unitOfWork,
            ICurrentUserService currentUserService,
            IClientFeeScheduleService clientFeeScheduleService,
            IClaimNumberNormalizationService claimNumberNormalizationServices,
            ITenantRepositoryFactory tenantRepositoryFactory,
            IClientCptCodeRepository cptCodeRepository,
            ITenantManagementService tenantManagementService)
        //ITenantInfo tenantInfo)
        {
            _mailService = mailService;
            _authorizationRepository = authorizationRepository;
            _claimStatusBatchRepository = claimStatusBatchRepository;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository; //AA-231
            _claimStatusTransactionRepository = claimStatusTransactionRepository;
            _chargeEntryQueryService = chargeEntryQueryService;
            _claimStatusQueryService = claimStatusQueryService;
            _httpClientFactory = httpClientFactory;
            _userService = userService;
            _logger = logger;
            _clientRepository = clientRespository;
            _configuration = configuration;
            _excelService = excelService;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _clientFeeScheduleService = clientFeeScheduleService; //AA-315
            _claimNumberNormalizationService = claimNumberNormalizationServices;
            _tenantRepositoryFactory = tenantRepositoryFactory; //EN-95
            _tenantManagementService = tenantManagementService;
            _clientCptCodeRepository = cptCodeRepository;
            //_tenantInfo = tenantInfo;
        }

        /// <summary>
        /// Asynchronously updates the normalized claim numbers for all clients' claim status batch claims.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to stop the operation if needed.</param>
        /// <returns>A boolean indicating the success of the operation.</returns>
        public async Task<bool> UpdateNormalizedClaimsAsync(CancellationToken cancellationToken)
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        // Get the claim status batch repository specific to the current tenant
                        var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);
                        var batchIds = await _claimStatusBatchRepository.GetNonDeletedBatchIdsAsync();

                        foreach (var batchId in batchIds)
                        {
                            try
                            {
                                var _claimStatusBatchClaimRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenant.Identifier);
                                var claimsToUpdate = await _claimStatusBatchClaimRepository.GetClaimsToUpdateAsync(batchId);

                                foreach (var claim in claimsToUpdate)
                                {
                                    claim.NormalizedClaimNumber = await _claimNumberNormalizationService.GetNormalizedClaimNumber(claim.ClaimNumber);
                                    await _claimStatusBatchClaimRepository.UpdateAsync(claim);

                                    await _unitOfWork.Commit(cancellationToken);
                                }
                            }
                            catch (Exception batchEx)
                            {
                                _logger.LogError($"Failed to update claims for batch {batchId} in tenant {tenant.TenantName}. Error - {batchEx.Message}");
                                exceptions.Add(new Exception($"Tenant: {tenant.TenantName}, Batch: {batchId}, Error: {batchEx.Message}", batchEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Failed to process tenant {tenant.TenantName}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.TenantName}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'UpdateNormalizedClaimsAsync'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred during UpdateNormalizedClaimsAsync. Error - " + ex.Message);
                exceptions.Add(ex);
                success = false;
                throw;
            }

            return success;
        }



        #region DaysWaitLapsedResetToUnknown
        public async Task<bool> DoResetUnknownDaysWaitLapsedClaims()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                if (tenants != null)
                {
                    foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                    {
                        var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
                        var clients = unitOfWork.Repository<Domain.Entities.Client>()
                                             .Entities
                                             .Where(c => c.ClientCode != "Client123" && c.ClientCode != "default")
                                             .ToList();

                        if (clients != null)
                        {
                            foreach (var client in clients)
                            {
                                try
                                {
                                    var daysWaitLapsedClientClaims = await _claimStatusQueryService.GetDaysWaitLapsedByClientIdAsync(client.Id) ?? new List<ClaimStatusDaysWaitLapsedDetailResponse>();
                                    if (daysWaitLapsedClientClaims.Count < 1)
                                    {
                                        continue;
                                    }

                                    _unitOfWork.Repository<ClaimStatusTransaction>().ExecuteUpdate(
                                        c => daysWaitLapsedClientClaims.Select(x => x.ClaimStatusTransactionId).Contains(c.Id),
                                        u =>
                                        {
                                            u.ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unknown;
                                            u.ClaimLineItemStatusValue = ClaimLineItemStatusEnum.Unknown.GetDescription();
                                        });

                                    _unitOfWork.Repository<ClaimStatusTransactionLineItemStatusChangẹ>().ExecuteUpdate(
                                        sc => daysWaitLapsedClientClaims.Select(x => x.ClaimStatusTransactionLineItemStatusChangẹId).Contains(sc.Id),
                                        u =>
                                        {
                                            u.UpdatedClaimLineItemStatusId = ClaimLineItemStatusEnum.Unknown;
                                        });

                                    await _unitOfWork.Commit(new CancellationToken());
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError($"Failed to reset 'Days Wait Lapsed' claims to Unknown Status for Client: {client.ClientCode}. Error - {ex.Message}");
                                    exceptions.Add(new Exception($"Client: {client.ClientCode}, Error: {ex.Message}", ex));
                                    success = false;
                                }
                            }
                        }
                        if (exceptions.Any())
                        {
                            throw new AggregateException("One or more errors occurred during the execution of 'DoResetUnknownDaysWaitLapsedClaims'.", exceptions);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to reset 'Days Wait Lapsed' claims to Unknown Status Job. Error - " + ex.Message);
                exceptions.Add(ex);
                success = false;
                throw;
            }

            return success;
        }

        #endregion
        //public async Task<bool> DoAddMissingClientIdReferencesOnTables()
        //{
        //    try
        //    {            

        //        var clients = await _unitOfWork.Repository<Domain.Entities.Client>().Entities.ToListAsync();

        //        foreach(var cl in clients)
        //        {
        //            try
        //            {
        //                var claimStatusBatches = await _claimStatusBatchRepository.GetByClientIdAsyncAsync(cl.Id);

        //                foreach(var csb in claimStatusBatches)
        //                {
        //                    csb.ClientId = cl.Id;
        //                    foreach(var h in csb.ClaimStatusBatchHistories)
        //                    {
        //                        h.ClientId = cl.Id;
        //                    }
        //                    foreach (var bc in csb.ClaimStatusBatchClaims)
        //                    {
        //                        bc.ClientId = cl.Id;
        //                        if (bc.ClaimStatusTransactionId != null)
        //                        {
        //                            var trx = await _claimStatusTransactionRepository.GetByIdAsync((int)bc.ClaimStatusTransactionId);
        //                            trx.ClientId = cl.Id;
        //                            foreach (var hx in trx.ClaimStatusTransactionHistories)
        //                            {
        //                                hx.ClientId = cl.Id;
        //                            }
        //                        }
        //                    }
        //                }

        //                //var chargeEntryBatches = _unitOfWork.Repository<ChargeEntryBatch>()
        //                //   .Entities
        //                //   .Include(x => x.ChargeEntryRpaConfiguration)
        //                //   .Include(x => x.ChargeEntryBatchHistories)
        //                //   .Include(x => x.ChargeEntryTransactions)
        //                //        .ThenInclude(y => y.ChargeEntryTransactionHistories)
        //                //   .Where(x => x.ChargeEntryRpaConfiguration.ClientId == cl.Id)
        //                //   .ToList();

        //                //foreach (var ceb in chargeEntryBatches)
        //                //{
        //                //    ceb.ClientId = cl.Id;
        //                //    foreach (var h in ceb.ChargeEntryBatchHistories)
        //                //    {
        //                //        h.ClientId = cl.Id;
        //                //    }
        //                //    foreach (var tx in ceb.ChargeEntryTransactions)
        //                //    {
        //                //        tx.ClientId = cl.Id;
        //                //        foreach (var hx in tx.ChargeEntryTransactionHistories)
        //                //        {
        //                //            hx.ClientId = cl.Id;
        //                //        }
        //                //    }
        //                //}

        //                //await _unitOfWork.Commit(new CancellationToken());
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError("Failed to update ClientId References.  Error - " + ex.Message);
        //            }
        //        }

        //        await _unitOfWork.Commit(new CancellationToken());
        //    }

        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Failed to update ClientId References.  Error - " + ex.Message);
        //    }
        //    return true;
        //}
        //

        /// <summary>
        /// Processes claims manually by checking and updating their status based on fee schedule information.
        /// </summary>
        /// <remarks>
        /// This method iterates through all batches of claims, retrieves claims that are not mapped to fee schedules,
        /// and checks if they require a write-off based on fee schedule conditions. If a claim requires a write-off,
        /// it either creates a new transaction or updates an existing one accordingly, and then updates the claim status.
        /// </remarks>
        public async Task<bool> ProcessClaimsManually()
        {
            var success = true;
            var exceptions = new List<Exception>();

            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                tenants = tenants.Where(x => x.Identifier != "demo" && x.Identifier != "initial").ToList();

                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);
                        var batches = await _claimStatusBatchRepository.RetrieveValidBatchIdsAsync();

                        foreach (var batch in batches)
                        {
                            try
                            {
                                var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenant.Identifier);
                                var claims = await _claimStatusBatchClaimsRepository.GetClaimsUnMappedToFeeScheduleByBatchIdAsync(batch.Id) ?? new List<ClaimStatusBatchClaim>();

                                foreach (var claim in claims)
                                {
                                    try
                                    {
                                        await _clientFeeScheduleService.ProcessFeeScheduleMatchedClaim(claim, tenant.Identifier);
                                    }
                                    catch (Exception claimEx)
                                    {
                                        _logger.LogError($"Failed to process claim {claim.ClaimNumber} in batch {batch.Id} for tenant {tenant.Identifier}. Error - {claimEx.Message}");
                                        success = false;
                                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Batch: {batch.Id}, Claim: {claim.ClaimNumber}, Error: {claimEx.Message}", claimEx));
                                    }
                                }
                            }
                            catch (Exception batchEx)
                            {
                                _logger.LogError($"Failed to process batch {batch.Id} for tenant {tenant.Identifier}. Error - {batchEx.Message}");
                                success = false;
                                exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Batch: {batch.Id}, Error: {batchEx.Message}", batchEx));
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Failed to process tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        success = false;
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'ProcessClaimsManually'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Process Claims Manually. Error - " + ex.Message);
                success = false;
                exceptions.Add(ex);
            }

            return success;
        }


        //job for fetching data
        public async Task<bool> DoUpdateDevDemoDataAuditDates(CancellationToken cancellationToken)
        {
            try
            {
                //run in only one tenants
                // Retrieve all tenants
                var demoTenant = await _tenantManagementService.GetByIdentifierAsync("devTenant");
                //var demoTenant = await _tenantManagementService.GetByIdentifierAsync("medhelp");
                //var demoTenant = await _tenantManagementService.GetByIdentifierAsync("demoTenant");
                if (demoTenant != null)
                {
                    // Get the client repository specific to the current tenant
                    var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(demoTenant.Identifier);
                    var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(demoTenant.Identifier);
                    var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(demoTenant.Identifier);
                    var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(demoTenant.Identifier);
                    var _claimStatusTransactionRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionRepository>(demoTenant.Identifier);
                    var _claimStatusTransactionLineItemStatusChangeRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionLineItemStatusChangeRepository>(demoTenant.Identifier);

                    // Retrieve all clients for the current tenant
                    var client = await _clientRepository.GetByClientCode("DEMO");
                    if (client == null)
                    {
                        _logger.LogError("DEMO client not found in database");
                        return false;
                    }
                    // Get latest batch created on date
                    var lastBatchCreated = await _claimStatusBatchRepository.ClaimStatusBatches.Where(x => x.ClientId == client.Id).OrderByDescending(x => x.CreatedOn).FirstAsync();
                    var lastBatchClaimCreated = await _claimStatusBatchClaimsRepository.ClaimStatusBatchClaims.Where(x => x.ClientId == client.Id).OrderByDescending(x => x.CreatedOn).FirstAsync();
                    var lastTransactionCreated = await _claimStatusTransactionRepository.ClaimStatusTransactions.Where(x => x.ClientId == client.Id).OrderByDescending(x => x.CreatedOn).FirstAsync();
                    var lastCheckDate = await _claimStatusTransactionRepository.ClaimStatusTransactions.Where(x => x.ClientId == client.Id).OrderByDescending(x => x.CheckDate).FirstAsync();
                    var lastStatusChangeCreated = await _claimStatusTransactionLineItemStatusChangeRepository.ClaimStatusTransactionLineItemStatusChangẹs
                        //.Include(x => x.ClaimStatusTransaction)
                        .Where(x => x.ClientId == client.Id)
                        .OrderByDescending(x => x.CreatedOn)
                        .FirstAsync();

                    int batchAddedDays = (DateTime.Now - lastBatchCreated.CreatedOn.Date).Days;
                    int batchClaimAddedDays = (DateTime.Now - lastBatchClaimCreated.CreatedOn.Date).Days;
                    int transactionAddedDays = (DateTime.Now - lastTransactionCreated.CreatedOn.Date).Days;
                    int checkDateAddedDays = (DateTime.Now - lastCheckDate.CheckDate.Value.Date).Days;
                    int statusChangeCreatedOn = (DateTime.Now - lastStatusChangeCreated.CreatedOn.Date).Days;

                    //TO BE DISCUSSED

                    //Update Batch CreatedOn and LastModifiedOn dates
                    var batches = await _claimStatusBatchRepository.ClaimStatusBatches.Where(x => x.ClientId == client.Id).ToListAsync();
                    foreach (var batch in batches)
                    {
                        batch.CreatedOn = batch.CreatedOn.AddDays(batchAddedDays);
                        batch.LastModifiedOn = batch.CreatedOn;
                    }
                    await _claimStatusBatchRepository.Commit(cancellationToken);

                    //Update BatchClaims CreatedOn and LastModifiedOn dates
                    var claims = await _claimStatusBatchClaimsRepository.ClaimStatusBatchClaims.Where(x => x.ClientId == client.Id).ToListAsync();
                    foreach (var claim in claims)
                    {
                        claim.CreatedOn = claim.CreatedOn.AddDays(batchClaimAddedDays);
                        claim.LastModifiedOn = claim.CreatedOn;
                        claim.DateOfServiceFrom = claim.DateOfServiceFrom.Value.AddDays(batchClaimAddedDays);
                        claim.DateOfServiceTo = claim.DateOfServiceTo.Value.AddDays(batchClaimAddedDays);
                        claim.ClaimBilledOn = claim.ClaimBilledOn.Value.AddDays(batchClaimAddedDays);
                    }
                    await _claimStatusBatchClaimsRepository.Commit(cancellationToken);

                    //Update Transactions CreatedOn and LastModifiedOn dates
                    var transactions = await _claimStatusTransactionRepository.ClaimStatusTransactions.Where(x => x.ClientId == client.Id).ToListAsync();
                    foreach (var tx in transactions)
                    {
                        if (tx.CheckDate != null)
                        {
                            tx.CheckDate = tx.CheckDate.Value.AddDays(checkDateAddedDays);
                        }
                        tx.CreatedOn = tx.CreatedOn.AddDays(transactionAddedDays);
                        tx.LastModifiedOn = tx.CreatedOn;
                    }
                    await _claimStatusTransactionRepository.Commit(cancellationToken);

                    //update ClaimStatusChange CreatedOn and LastModifiedOn dates
                    var statusChanges = await _claimStatusTransactionLineItemStatusChangeRepository.Entities
                                                //.Include(x => x.ClaimStatusTransaction)
                                                .Where(x => x.ClientId == client.Id)
                                                .ToListAsync();
                    foreach (var sc in statusChanges)
                    {
                        sc.CreatedOn = sc.CreatedOn.AddDays(statusChangeCreatedOn);
                        sc.LastModifiedOn = sc.CreatedOn;
                    }
                    await _claimStatusTransactionLineItemStatusChangeRepository.Commit(cancellationToken);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed updating Demo Data Audit Dates.  Error - " + ex.Message);
                throw; // Rethrow the exception to ensure Hangfire captures the failed job
            }

            return true;
        }

        /// <summary>
        /// Updates the data in ClaimStatusBatchClaim entities by associating them with ClientCptCode entries. EN-214
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for asynchronous operations.</param>
        /// <returns>True if the update operation is successful, otherwise false.</returns>
        public async Task<bool> UpdateClaimStatusBatchClaimsByCptCode(CancellationToken cancellationToken)
        {
            var success = true;
            var exceptions = new List<Exception>();

            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                foreach (var tenant in tenants ?? Enumerable.Empty<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
                        var batchClaims = await unitOfWork.Repository<ClaimStatusBatchClaim>().Entities
                            .Where(x => x.ClientCptCodeId == null)
                            .GroupBy(x => new { x.ProcedureCode, x.ClientId })
                            .Select(grp => new ClaimStatusBatchClaim
                            {
                                ProcedureCode = grp.Key.ProcedureCode,
                                ClientId = grp.Key.ClientId
                            })
                            .ToListAsync() ?? new();

                        if (batchClaims.Any())
                        {
                            foreach (var claim in batchClaims)
                            {
                                try
                                {
                                    var matchedClientCptCode = await unitOfWork.Repository<ClientCptCode>().Entities
                                        .FirstOrDefaultAsync(x => x.Code == claim.ProcedureCode && x.ClientId == claim.ClientId);

                                    if (matchedClientCptCode == null)
                                    {
                                        var matchedCptCode = await unitOfWork.Repository<CptCode>().Entities
                                            .FirstOrDefaultAsync(x => x.Code == claim.ProcedureCode);

                                        var clientCptCode = new ClientCptCode
                                        {
                                            ClientId = claim.ClientId,
                                            Code = claim.ProcedureCode,
                                            LookupName = matchedCptCode?.Description ?? "Missing Reference",
                                            Description = matchedCptCode?.Description ?? "Missing Reference",
                                            ScheduledFee = claim.BilledAmount
                                        };

                                        await unitOfWork.Repository<ClientCptCode>().AddAsync(clientCptCode);
                                        await unitOfWork.Commit(cancellationToken);

                                        matchedClientCptCode = clientCptCode;
                                    }

                                    unitOfWork.Repository<ClaimStatusBatchClaim>().ExecuteUpdate(
                                        ec => ec.ClientId == claim.ClientId && ec.ProcedureCode == claim.ProcedureCode,
                                        ec => ec.ClientCptCodeId = matchedClientCptCode.Id
                                    );
                                }
                                catch (Exception claimEx)
                                {
                                    _logger.LogError($"Failed to update claim {claim.ProcedureCode} for tenant {tenant.Identifier}. Error - {claimEx.Message}");
                                    success = false;
                                    exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Claim: {claim.ProcedureCode}, Error: {claimEx.Message}", claimEx));
                                }
                            }

                            await unitOfWork.Commit(cancellationToken);
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Failed to update claims for tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        success = false;
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'UpdateClaimStatusBatchClaimsByCptCode'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update ClaimStatusBatchClaims by CPT Code. Error - " + ex.Message);
                success = false;
                exceptions.Add(ex);
                throw;
            }

            return success;
        }


        public async Task ProcessFeeScheduleEntriesForAllTenants()
        {
            var exceptions = new List<Exception>();

            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                if (tenants != null && tenants.Any())
                {
                    foreach (var tenant in tenants)
                    {
                        try
                        {
                            var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
                            var clientFeeScheduleList = await unitOfWork.Repository<ClientFeeSchedule>().GetAllAsync();
                            if (clientFeeScheduleList != null && clientFeeScheduleList.Count > 0)
                            {
                                foreach (var feeSchedule in clientFeeScheduleList)
                                {
                                    try
                                    {
                                        await ProcessFeeScheduleEntriesForSingleTenant(tenant.Identifier, feeSchedule.Id);
                                    }
                                    catch (Exception feeScheduleEx)
                                    {
                                        _logger.LogError($"Failed to process fee schedule {feeSchedule.Id} for tenant {tenant.Identifier}. Error - {feeScheduleEx.Message}");
                                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, FeeSchedule: {feeSchedule.Id}, Error: {feeScheduleEx.Message}", feeScheduleEx));
                                    }
                                }
                            }
                        }
                        catch (Exception tenantEx)
                        {
                            _logger.LogError($"Failed to process fee schedules for tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                            exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        }
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'ProcessFeeScheduleEntriesForAllTenants'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to process FeeScheduleEntries for all tenants. Error - " + ex.Message);
                exceptions.Add(ex);
                throw;
            }
        }


        /// <summary>
        /// Processes a fee schedule entry for a single tenant.
        /// </summary>
        /// <param name="tenantIdentifier">The identifier of the tenant for which the fee schedule entry will be processed.</param>
        /// <param name="clientFeeScheduleEntryId">The unique identifier of the fee schedule entry to be processed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ProcessFeeScheduleEntriesForSingleTenant(string tenantIdentifier, int clientFeeScheduleId)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenantIdentifier) && clientFeeScheduleId > 0)
                {

                    var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
                    var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenantIdentifier);
                    var feeScheduleEntries = await unitOfWork.Repository<ClientFeeScheduleEntry>()
                                                                            .Entities
                                                                            .Where(x => x.ClientFeeScheduleId == clientFeeScheduleId)
                                                                            .ToListAsync();

                    if (feeScheduleEntries != null && feeScheduleEntries.Any())
                    {
                        foreach (var feeScheduleEntry in feeScheduleEntries)
                        {
                            if (!feeScheduleEntry.IsReimbursable)
                            {
                                var feeSchedule = await unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().Entities
                                                                                .Include(x => x.ClientInsuranceFeeSchedules)
                                                                                .Include(x => x.ClientFeeScheduleProviderLevels)
                                                                                .Include(x => x.ClientFeeScheduleSpecialties)
                                                                                .FirstOrDefaultAsync(x => x.Id == clientFeeScheduleId);

                                if (feeSchedule != null)
                                {
                                    var claimStatusBatchClaimData = await _claimStatusBatchClaimsRepository.GetClaimsByFeeScheduleCriteriaAsync(
                                                                        feeSchedule.ClientInsuranceFeeSchedules.Select(x => x.ClientInsuranceId).ToList(),
                                                                        feeScheduleEntry.ClientCptCodeId,
                                                                        feeSchedule.StartDate,
                                                                        feeSchedule.EndDate,
                                                                        feeSchedule.ClientFeeScheduleSpecialties.Select(x => x.SpecialtyId).ToList(),
                                                                        feeSchedule.ClientFeeScheduleProviderLevels.Select(x => x.ProviderLevelId).ToList());

                                    if (claimStatusBatchClaimData != null && claimStatusBatchClaimData.Any())
                                    {
                                        foreach (var claim in claimStatusBatchClaimData)
                                        {
                                            await _clientFeeScheduleService.ProcessFeeScheduleMatchedClaim(claim, tenantIdentifier, feeScheduleEntry.Id);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to process fee schedule entry for tenant '{tenantIdentifier}' and fee schedule ID '{clientFeeScheduleId}'. Error - {ex.Message}");
                throw; // Rethrow the exception to ensure Hangfire captures the failed job
            }
        }

        #region EN-231

        private string GetEmailBodyTemplateForClaimStatusTotalReport()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\ClaimStatusTotalReport.template");
        }

        public async Task PreSummarizedClaimsDataRefreshService(CancellationToken cancellationToken)
        {
            var exceptions = new List<Exception>();
            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                var templateContent = GetEmailBodyTemplateForClaimStatusTotalReport();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(templateContent);
                var tenants = await _tenantManagementService.GetAllActiveAsync();


                if (tenants != null && tenants.Any())
                {
                    foreach (var tenant in tenants)
                    {
                        try
                        {
                            var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
                            var existingClaims = await unitOfWork.Repository<ClaimStatusTotalResult>().Entities.Where(x => !x.IsDeleted).ToListAsync() ?? new List<ClaimStatusTotalResult>();

                            //for each client inside the tenant 
                            // set isDeleted = true all for current client 
                            // populate claimstatustotalsresult for current client 4 years worht of data grouped and summed

                            //if sucessful delete all rows where isDeleted = true for that clientID. 
                            //if exception  throws update isDeleted = false for that client and send an error report to AIT 

                            // We want to run  sp something almost identitcal to the old spClaimStatusTotalsTask from end of february 2024

                            if (existingClaims != null && existingClaims.Any())
                            {
                                List<ClaimStatusTotalResultExceptionModel> claimStatusTotalResultExceptionModels = new List<ClaimStatusTotalResultExceptionModel>();
                                var clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
                                var clients = await clientRepository.GetAllActiveClients();

                                foreach (var client in clients)
                                {
                                    try
                                    {
                                        var claimStatusTotalsByClientId = await unitOfWork.Repository<ClaimStatusTotalResult>()
                                            .Entities
                                            .Where(x => x.ClientId == client.Id && !x.IsDeleted)
                                            .ToListAsync() ?? new List<ClaimStatusTotalResult>();

                                        if (claimStatusTotalsByClientId != null && claimStatusTotalsByClientId.Any())
                                        {
                                            Action<ClaimStatusTotalResult> updateAction = claim =>
                                            {
                                                claim.IsDeleted = true;
                                            };

                                            // Execute the update action on existing claims
                                            unitOfWork.Repository<ClaimStatusTotalResult>().ExecuteUpdate(claim => claimStatusTotalsByClientId.Contains(claim), updateAction);

                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        claimStatusTotalResultExceptionModels.Add(new ClaimStatusTotalResultExceptionModel()
                                        {
                                            ClientId = client.Id,
                                            ClientName = client.Name,
                                            ExceptionMessage = ex.Message,
                                        });
                                    }
                                }

                                if (claimStatusTotalResultExceptionModels.Any())
                                {
                                    string[] emailToAddresses =
                                    [
                                    "cknight@medhelpinc.com",
                                        "cknight@automatedintegrationtechnologies.com",
                                        "jamesnichols@automatedintegrationtechnologies.com",
                                        "jamesnichols@medhelpinc.com",
                                        "kmccaffery@automatedintegrationtechnologies.com",
                                        "mohit@automatedintegrationtechnologies.com"
                                    ];

                                    string body = template.Run(new
                                    {
                                        ClaimStatusTotalResultExceptionModels = claimStatusTotalResultExceptionModels
                                    });

                                    foreach (string email in emailToAddresses)
                                    {
                                        MailRequestWithAttachment request = new MailRequestWithAttachment()
                                        {
                                            To = email,
                                            Body = body,
                                            Subject = $"tenant:{tenant.Identifier} - ClaimStatusTotalReport",
                                            Base64Content = await ClaimStatusTotalReportExcelDataAsync(claimStatusTotalResultExceptionModels),
                                            FileName = "ClaimStatusTotalReport"
                                        };
                                        await _mailService.SendAsync(request);
                                    }
                                }
                            }

                            var claimByProcedureSummaryDetails = await _claimStatusQueryService.GetLastFourYearClaims(tenant.ConnectionString) ?? new List<GetClaimByProcedureSummaryResponse>();

                            if (claimByProcedureSummaryDetails != null && claimByProcedureSummaryDetails.Any())
                            {
                                var claimStatusTotalResult = claimByProcedureSummaryDetails
                                    .Where(item => item.ClientCptCodeId.HasValue) // Ensure ClientCptCodeId is not null
                                    .Select(item => _mapper.Map<ClaimStatusTotalResult>(item))
                                    .Where(item => item.ClientCptCodeId > 0) // Optional: Ensure a valid ID
                                    .ToList();

                                if (claimStatusTotalResult.Any())
                                {
                                    unitOfWork.Repository<ClaimStatusTotalResult>().AddRange(claimStatusTotalResult);
                                    await unitOfWork.Commit(cancellationToken);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Failed to PreSummarizedClaimsDataRefreshService for tenant {tenant.Identifier}. Error - {ex.Message}");
                            exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {ex.Message}", ex));
                        }

                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'PreSummarizedClaimsDataRefreshService'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while refreshing pre-summarized claims data.", ex);
            }
        }

        private async Task<string> ClaimStatusTotalReportExcelDataAsync(List<ClaimStatusTotalResultExceptionModel> notFoundUnavailableClaimsData)
        {
            var exportResponse = notFoundUnavailableClaimsData.Select(z => new ExportQueryResponse
            {
                ClientId = z.ClientId,
                ClientName = z.ClientName,
                ExceptionMessage = z.ExceptionMessage
            });
            var data = await _excelService.ExportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Client Id", item => item.ClientId.ToString() },
                { "Client Name", item => item.ClientName },
                { "Error Message", item => item.ExceptionMessage },

            }, sheetName: "ClaimStatusTotalReport");

            return data;
        }

        #endregion

        #region insert claims for the unpaids from the df data

        public class RpaConfigurationClaimStatusBatchClaims()
        {
            public int ClientId { get; set; }
            public int RPAConfigId { get; set; }
            public int RPAInsuranceId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string URL { get; set; }
            public List<GetClaimStatusBatchClaimsByBatchIdResponse> Claims { get; set; }
        }


        /// <summary>
        /// Creates claim line items for unpaid claims across all active tenants.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to handle operation cancellation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetClaimLineItemForUnPaidClaims(CancellationToken cancellationToken)
        {
            try
            {
                int returnQuantityCap = 1000;
                // Get all active tenants
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                // Process claims for each tenant
                foreach (var tenant in tenants)
                {
                    await ProcessTenantClaimsAsync(tenant.Identifier, returnQuantityCap, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw;
            }
        }

        /// <summary>
        /// Processes claims for a specific tenant.
        /// </summary>
        /// <param name="tenantIdentifier">Identifier of the tenant.</param>
        /// <param name="returnQuantityCap">Maximum number of claims to process.</param>
        /// <param name="cancellationToken">Cancellation token to handle operation cancellation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessTenantClaimsAsync(string tenantIdentifier, int returnQuantityCap, CancellationToken cancellationToken)
        {
            var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
            var clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenantIdentifier);
            var clients = await clientRepository.GetAllActiveClients();

            // Process claims for each client of the tenant
            foreach (var client in clients)
            {
                await ProcessClientClaimsAsync(tenantIdentifier, client, unitOfWork, returnQuantityCap, cancellationToken);
            }
        }

        /// <summary>
        /// Processes claims for a specific client.
        /// </summary>
        /// <param name="tenantIdentifier">Identifier of the tenant.</param>
        /// <param name="client">Client entity.</param>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="returnQuantityCap">Maximum number of claims to process.</param>
        /// <param name="cancellationToken">Cancellation token to handle operation cancellation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessClientClaimsAsync(string tenantIdentifier, Domain.Entities.Client client, IUnitOfWork<int> unitOfWork, int returnQuantityCap, CancellationToken cancellationToken)
        {
            var rpaConfigs = await GetRpaConfigurationsAsync(client, unitOfWork);

            // Process claims for each RPA configuration of the client
            foreach (var rpaConfig in rpaConfigs)
            {
                await ProcessRpaConfigClaimsAsync(tenantIdentifier, client, rpaConfig, unitOfWork, returnQuantityCap, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves RPA configurations for a specific client.
        /// </summary>
        /// <param name="client">Client entity.</param>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <returns>List of RPA configurations.</returns>
        private async Task<List<ClientInsuranceRpaConfiguration>> GetRpaConfigurationsAsync(Domain.Entities.Client client, IUnitOfWork<int> unitOfWork)
        {
            return await unitOfWork.Repository<ClientInsuranceRpaConfiguration>()
                                   .Entities
                                   .Include(c => c.ClientInsurance)
                                   .Include(c => c.ClientLocation)
                                   .Include(c => c.ClientInsurance.RpaInsurance)
                                   .ThenInclude(d => d.RpaInsuranceGroup)
                                   .Include(c => c.ClientRpaCredentialConfiguration)
                                   .ThenInclude(d => d.RpaInsuranceGroup)
                                   .Where(c => c.ClientInsurance.ClientId == client.Id
                                       && c.TransactionTypeId == TransactionTypeEnum.ClaimStatus
                                       && !c.IsDeleted)
                                   .ToListAsync();
        }

        /// <summary>
        /// Processes claims for a specific RPA configuration.
        /// </summary>
        /// <param name="tenantIdentifier">Identifier of the tenant.</param>
        /// <param name="client">Client entity.</param>
        /// <param name="rpaConfig">RPA configuration entity.</param>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="returnQuantityCap">Maximum number of claims to process.</param>
        /// <param name="cancellationToken">Cancellation token to handle operation cancellation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessRpaConfigClaimsAsync(string tenantIdentifier, Domain.Entities.Client client, ClientInsuranceRpaConfiguration rpaConfig, IUnitOfWork<int> unitOfWork, int returnQuantityCap, CancellationToken cancellationToken)
        {
            var claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenantIdentifier);
            var claimStatusBatchClaims = claimStatusBatchClaimsRepository.ClaimStatusBatchClaims;

            // Filter and retrieve claims based on specifications
            var result = await GetFilteredClaimStatusBatchClaimsAsync(claimStatusBatchClaims, rpaConfig, returnQuantityCap);

            // Create an object to store processed claims and RPA configuration details
            var rpaConfigurationClaimStatusBatchClaims = new RpaConfigurationClaimStatusBatchClaims
            {
                ClientId = client.Id,
                RPAConfigId = rpaConfig.Id,
                RPAInsuranceId = rpaConfig.ClientInsurance.RpaInsuranceId ?? 0,
                UserName = rpaConfig?.ClientRpaCredentialConfiguration?.Username ?? string.Empty,
                Password = rpaConfig?.ClientRpaCredentialConfiguration?.Password ?? string.Empty,
                URL = !string.IsNullOrEmpty(rpaConfig.ClientInsurance.RpaInsurance.TargetUrl)
                    ? rpaConfig.ClientInsurance.RpaInsurance.TargetUrl
                    : rpaConfig.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl,
                Claims = result,
            };

            // Process the rpaConfigurationClaimStatusBatchClaims further as needed
        }

        /// <summary>
        /// Retrieves and filters claim status batch claims based on the specifications.
        /// </summary>
        /// <param name="claimStatusBatchClaims">Queryable of claim status batch claims.</param>
        /// <param name="rpaConfig">RPA configuration entity.</param>
        /// <param name="returnQuantityCap">Maximum number of claims to retrieve.</param>
        /// <returns>List of filtered claim status batch claims responses.</returns>
        private async Task<List<GetClaimStatusBatchClaimsByBatchIdResponse>> GetFilteredClaimStatusBatchClaimsAsync(IQueryable<ClaimStatusBatchClaim> claimStatusBatchClaims, ClientInsuranceRpaConfiguration rpaConfig, int returnQuantityCap)
        {
            try
            {
                return await claimStatusBatchClaims
                .AsNoTracking()
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(t => t.ClaimStatusTransactionLineItemStatusChangẹ)
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(cs => cs.ClaimLineItemStatus)
                .Include(c => c.ClientLocation)
                .Include(c => c.Patient)
                    .ThenInclude(p => p.Person)
                .Specify(new ApprovedClaimLineItemStatusWaitPeriodSpecification())
                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                .Specify(new ClaimStatusClaimBilledOnQualificationFilterSpecification())
                .Specify(new ClaimStatusMaxDaysPipelineQualificationFilterSpecification())
                .Specify(new ClaimStatusDaysBetweenAttemptsQualificationFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedWrongPayerFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedPolicyNumberFilterSpecification())
                .Specify(new ClaimStatusBatchClaimRPAConfigurationSpecification(rpaConfig.ClientInsurance.RpaInsuranceId ?? 0, rpaConfig.AuthTypeId ?? 0, rpaConfig.ClientLocationId ?? 0))
                .OrderBy(c => c.ClaimStatusTransactionId.HasValue)
                .ThenBy(c => c.ClaimStatusTransaction != null && c.ClaimStatusTransaction.ClaimLineItemStatus != null
                    ? c.ClaimStatusTransaction.ClaimLineItemStatus.Rank : 0)
                .Take(returnQuantityCap).
                Select(c => GetResult(c))
                .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            } 
        }

        private static GetClaimStatusBatchClaimsByBatchIdResponse GetResult(ClaimStatusBatchClaim c)
        {
            var result = new GetClaimStatusBatchClaimsByBatchIdResponse
            {
                Id = c.Id,
                CurrentLineItemStatusId = c?.ClaimStatusTransaction?.ClaimLineItemStatusId,
                CurrentExceptionReason = c?.ClaimStatusTransaction?.ExceptionReason,
                LastStatusChangedOn = c?.ClaimStatusTransaction?.ClaimStatusTransactionLineItemStatusChangẹ?.LastModifiedOn,
                ClaimStatusBatchId = c.ClaimStatusBatchId,
                ClaimStatusTransactionId = c?.ClaimStatusTransactionId,
                ClaimNumber = c?.ClaimNumber,
                PayerClaimNumber = c?.ClaimStatusTransaction?.ClaimNumber,
                PatientLastName = c?.Patient?.Person?.LastName ?? string.Empty,
                PatientFirstName = c?.Patient?.Person?.FirstName ?? string.Empty,
                DateOfBirth = c?.DateOfBirth,
                RenderingNpi = c?.ClientProvider?.Npi ?? string.Empty,
                GroupNpi = c?.GroupNpi,
                PolicyNumber = c?.PolicyNumber,
                PolicyNumberUpdatedOn = c?.PolicyNumberUpdatedOn,
                EligibilityPolicyNumber = c?.ClaimStatusTransaction?.EligibilityPolicyNumber ?? string.Empty,
                DateOfServiceFrom = c?.DateOfServiceFrom,
                DateOfServiceTo = c?.DateOfServiceTo,
                ProcedureCode = c?.ProcedureCode,
                Modifiers = c?.Modifiers,
                ClaimBilledOn = c?.ClaimBilledOn,
                BilledAmount = c?.BilledAmount,
                Quantity = c.Quantity,
                IsDeleted = c.IsDeleted,
                ClientLocationNpi = c?.ClientLocation?.Npi ?? string.Empty,
                ClientLocationInsuranceIdentifierString = c?.ClientLocation?.GetLocationIdentifierStringForInsuranceId(c.ClientLocation, c.ClientInsuranceId) ?? string.Empty,
                PayerIdentifier = c?.ClientInsurance?.PayerIdentifier ?? string.Empty,
                TaxId = c?.Client?.TaxId.ToString().PadLeft(9, '0') ?? string.Empty,
            };
            return result;
        }

        #endregion

    }
}

