using AutoMapper;
using Hangfire;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClaimStatus.Queries.GetEmployeeClaimStatusByEmployeeID;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed;
using MedHelpAuthorizations.Application.Features.Reports.CurrentSummary;
using MedHelpAuthorizations.Application.Features.Reports.DailyClaimReports;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;
using MedHelpAuthorizations.Application.Models.Email.IntegratedServices;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Requests.Mail;
using MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Integrations.HttpClients;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Services.JobsManager;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Template.Views.Emails.DoSendEligibilityReportTemplate;
using MedHelpAuthorizations.Template.Views.Emails.FailedHangfireJobs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;
using OfficeOpenXml.Table.PivotTable;
using RazorEngineCore;
using RazorHtmlEmails.RazorClassLib.Services;
using self_pay_eligibility_api.Application.Features.Eligibility.DiscoveredEligibility.Queries.GetByCriteria;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetAllPaged;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetByCriteria;
using self_pay_eligibility_api.Client.Infrastructure.Routes;
using self_pay_eligibility_api.Shared.Models.Eligibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IClientRepository = MedHelpAuthorizations.Application.Interfaces.Repositories.IClientRepository;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class NightlyJobService : INightlyJobService
    {
        private readonly IMailService _mailService;
        private readonly IChargeEntryQueryService _chargeEntryQueryService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        //private readonly IUhcApiService _uhcApiService;
        private readonly IUiPathApiService _uiPathApiService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;
        private readonly IEmployeeClientService _employeeClientService;
        private readonly ILogger<NightlyJobService> _logger;
        private readonly IClientRepository _clientRespository;
        private readonly IConfiguration _configuration;
        private readonly IExcelService _excelService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        //private readonly IUnitOfWork<int> _unitOfWork;
        private HttpClient _httpClient;
        private readonly ISelfPayInternalClient _selfPayInternalCLient;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITenantManagementService _tenantManagementService;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IClientReportFilterService _clientReportFilterService;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IPatientRepository _patientRepository;
        private readonly IJobCronManager _jobCronManager;

        public NightlyJobService(IMailService mailService,
            IClaimStatusQueryService claimStatusQueryService,
            //IUhcApiService uhcApiService,
            IUiPathApiService uiPathApiService,
            IHttpClientFactory httpClientFactory,
            IUserService userService,
            IEmployeeClientService employeeClientService,
            ILogger<NightlyJobService> logger, IClientRepository clientRespository,
            IConfiguration configuration,
            IExcelService excelService,
            IMediator mediator,
            IMapper mapper,
            //IUnitOfWork<int> unitOfWork,
            IDbContextFactory<ApplicationContext> contextFactory,
            ISelfPayInternalClient selfPayInternalClient,
            UserManager<ApplicationUser> userManager,
            ITenantManagementService tenantManagementService,
            ITenantRepositoryFactory tenantRepositoryFactory,
            IClientReportFilterService clientReportFilterService,
            IRazorViewToStringRenderer razorViewToStringRenderer,
            Application.Interfaces.Repositories.IPatientRepository patientRepository,
            IJobCronManager jobCronManager)
        {
            _mailService = mailService;
            _claimStatusQueryService = claimStatusQueryService;
            //_uhcApiService = uhcApiService;
            _uiPathApiService = uiPathApiService;
            _httpClientFactory = httpClientFactory;
            _userService = userService;
            _employeeClientService = employeeClientService;
            _logger = logger;
            _clientRespository = clientRespository;
            _configuration = configuration;
            _excelService = excelService;
            _mediator = mediator;
            _mapper = mapper;
            //_unitOfWork = unitOfWork;
            _selfPayInternalCLient = selfPayInternalClient;
            _contextFactory = contextFactory;
            _userManager = userManager;
            _tenantManagementService = tenantManagementService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _clientReportFilterService = clientReportFilterService;
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _patientRepository = patientRepository;
            _jobCronManager = jobCronManager ?? throw new ArgumentNullException(nameof(jobCronManager));
        }

        /// <summary>
        /// Performs the re-queuing of unresolved claim status batches based on the specified maximum age.
        /// </summary>
        /// <param name="maxDaysOld">The maximum age of unresolved claim status batches in days (default is 180 days).</param>
        /// <returns>A boolean indicating the success of the operation.</returns>
        public async Task<bool> DoQueueUnresolvedClaimStatusBatches(int maxDaysOld = 180)
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                // Retrieve all tenants
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        // Get the claim status batch repository specific to the current tenant
                        var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);

                        // Retrieve unresolved batches older than the specified age
                        var unresolvedBatches = await _claimStatusBatchRepository.GetUnresolvedBatchesByDaysOldAsync(maxDaysOld) ?? new List<ClaimStatusBatch>();

                        foreach (var batch in unresolvedBatches)
                        {
                            try
                            {
                                // Send a command to unassign the claim status batch
                                await _claimStatusQueryService.UnassignClaimStatusBatchAsync(batch.Id, tenant.Identifier);
                            }
                            catch (Exception ex)
                            {
                                // Log any errors during the process
                                _logger.LogError($"Failed to re-queue (unassign) all unresolved and qualified claims for batch id = {batch.Id} in tenant {tenant.Identifier}. Error - {ex.Message}");
                                exceptions.Add(new Exception($"Batch ID: {batch.Id}, Tenant: {tenant.Identifier}, Error: {ex.Message}", ex));
                                success = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log errors related to tenant-specific processing
                        _logger.LogError($"Failed to process tenant {tenant.Identifier}. Error - {ex.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {ex.Message}", ex));
                        success = false;
                    }
                }
                if (exceptions != null && exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoQueueUnresolvedClaimStatusBatches'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                // Log errors related to retrieving or processing unresolved claim status batches
                _logger.LogError($"Failed getting unresolved claim status batches. Error - {ex.Message}");
                throw;
            }
            return success; // Operation completed successfully
        }

        public async Task<bool> DoSendFailedClientInsuranceRpaConfigurationsEmail()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetEmailBodyTemplateForFailedClientRpaConfiguration());

                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        var _clientInsuranceRpaConfigurationRepository = await _tenantRepositoryFactory.GetAsync<Application.Interfaces.Repositories.IClientInsuranceRpaConfigurationRepository>(tenant.Identifier);
                        var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);
                        var failedCofigurationList = _mapper.Map<List<GetFailedClientInsuranceRpaConfigurationsResponse>>(await _clientInsuranceRpaConfigurationRepository.GetFailedClientInsuranceRpaConfigurationsAsync());

                        List<ClaimStatusBatch> claimStatusFailedBatches = new List<ClaimStatusBatch>();
                        List<ClientInsuranceRpaConfigurationSummary> failedConfigurationSummaryList = new();

                        foreach (var item in failedCofigurationList)
                        {
                            ClientInsuranceRpaConfigurationSummary failedConfigurationSummary = new ClientInsuranceRpaConfigurationSummary
                            {
                                ClientCode = item.ClientCode,
                                ClientInsuranceName = item.ClientInsuranceLookupName,
                                ClientRpaConfigurationId = item.Id,
                                AuthTypeName = item.AuthTypeName,
                                Username = item.Username,
                                Password = item.Password,
                                FailureRecordedOn = item.CreatedOn,
                                TargetUrl = item.TargetUrl,
                                FailureMessage = item.FailureMessage,
                                ExpiryWarningReported = item.ExpiryWarningReported,
                                ReportFailureToEmail = item.ReportFailureToEmail
                            };

                            var abortedBatches = await _claimStatusBatchRepository.GetBatchesByClientInsuranceAndAuthTypeAsync(item.ClientInsuranceId, item.AuthTypeId);
                            if (abortedBatches.Count() > 0)
                                claimStatusFailedBatches.AddRange(abortedBatches);

                            failedConfigurationSummary.EffectedBatchCount = abortedBatches.Count();
                            failedConfigurationSummaryList.Add(failedConfigurationSummary);
                        }

                        string body = template.Run(new
                        {
                            FailedConfigurations = failedConfigurationSummaryList.ToList()
                        });

                        System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
                        builder.ConnectionString = tenant.ConnectionString;
                        string server = builder["Server"] as string;
                        string dbName = builder["Initial Catalog"] as string;

                        string subject = $"Failed Client Insurance Rpa Configurations - Tenant: {tenant.TenantName ?? tenant.Identifier}, DbName {dbName}";

                        // Array of recipient emails
                        string[] recipientEmails = new string[]
                        {
                            "cknight@automatedintegrationtechnologies.com",
                            "jamesnichols@automatedintegrationtechnologies.com",
                            "kmccaffery@automatedintegrationtechnologies.com",
                            "mohit@automatedintegrationtechnologies.com"
                        };

                        foreach (var recipient in recipientEmails)
                        {
                            try
                            {
                                MailRequest request = new MailRequest()
                                {
                                    To = recipient,
                                    Body = body,
                                    Subject = subject
                                };

                                await _mailService.SendFailedRpaConfigEmail(request.Subject, request.To, request.Body, true,
                                    await GetFailedClientRpaConfigurationExcelDataAsync(failedCofigurationList
                                        .OrderBy(x => x.ClientCode).ThenBy(x => x.ClientInsuranceId).ThenBy(x => x.AuthTypeId)
                                        .ToList()),
                                    await GetFailedClaimStatusBatchesExcelDataAsync(claimStatusFailedBatches
                                        .OrderBy(x => x.ClientInsurance.Client.ClientCode)
                                        .ThenBy(x => x.ClientInsurance.RpaInsuranceId).ThenBy(x => x.AuthTypeId)
                                        .ToList()));
                            }
                            catch (Exception mailEx)
                            {
                                _logger.LogError($"General exception while sending mail for tenant {tenant.Identifier}, recipient: {recipient}. Error - {mailEx.Message}");
                                exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {mailEx.Message} for recipient {recipient}", mailEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception fetching data or processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendFailedClientInsuranceRpaConfigurationsEmail'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendFailedClientInsuranceRpaConfigurationsEmail'. Error - {ex.Message}");
                throw;
            }

            return success; // Operation completed successfully
        }



        // GET EMAIL DAILY CLAIM STATUS REPORT
        public async Task<bool> DoSendDailyClaimStatusReport()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetEmailBodyTemplateForDailyClaimStatusReport());
                DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30).Date;
                DateTime todayDate = DateTime.Now;

                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);

                        // Retrieve all clients for the current tenant
                        var clients = await _clientRepository.GetAllActiveClients();
                        clients = RemoveNonProductionClients(clients);
                        if (clients != null && clients.Any())
                        {
                            foreach (var client in clients)
                            {
                                try
                                {
                                    var dailyClaimReportData = await _mediator.Send(new DailyClaimReportDetailsQuery(1, 31, thirtyDaysAgo, todayDate, client.Id, tenant.ConnectionString));
                                    if (dailyClaimReportData.Data != null)
                                    {
                                        List<EmployeeClientDto> employeeClients = await GetAllBillingManagersOrFollowUpEmployeesByClientId(client.Id, tenant.Identifier);
                                        foreach (var employeeClient in employeeClients)
                                        {
                                            //EN-153
                                            string decryptedPin;
                                            if (employeeClient?.Employee?.User != null)
                                            {
                                                // Get the existing PIN from the user or set it to null if it's empty
                                                string userExistingPin = employeeClient.Employee.User?.Pin ?? null;

                                                // Check if the user does not have an existing PIN
                                                if (string.IsNullOrEmpty(userExistingPin))
                                                {
                                                    // Create a new instance of the UserHelper class
                                                    var userHelper = new UserHelper();
                                                    long? phoneNumber = null;

                                                    // Get the phone number string from the user or set it to null if it's empty
                                                    string phoneNumberString = employeeClient.Employee.User?.PhoneNumber ?? null;

                                                    // Check if the phone number string is not empty
                                                    if (!string.IsNullOrEmpty(phoneNumberString))
                                                    {
                                                        // Attempt to parse the phone number string to a long
                                                        if (long.TryParse(phoneNumberString, out long result))
                                                        {
                                                            phoneNumber = result;
                                                        }
                                                    }

                                                    // Generate a new PIN using the UserHelper
                                                    decryptedPin = userHelper.GeneratePin(employeeClient.Employee.User.FirstName, phoneNumber);
                                                }
                                                else
                                                {
                                                    decryptedPin = PinExtensions.DecryptPin(employeeClient.Employee.User.Pin);
                                                }

                                                string body = template.Run(new
                                                {
                                                    DailyClaimReport = dailyClaimReportData.Data
                                                });

                                                string subject = $"Daily Claim Status Report - Client: {client.ClientCode} Tenant: {tenant.TenantName ?? tenant.Identifier}";

                                                MailRequestWithAttachment request = new MailRequestWithAttachment()
                                                {
                                                    To = employeeClient.Employee.User.Email,
                                                    Body = body,
                                                    Subject = subject,
                                                    FileName = $"Daily_Claim_Status_Report{DateTime.Now:MM-dd-yyyy HH:mm:ss}.xlsx",
                                                    Base64Content = await GetDailyClaimStatusExcelDataAsync(dailyClaimReportData.Data, decryptedPin)
                                                };
                                                await _mailService.SendAsync(request);
                                            }
                                        }
                                    }
                                }
                                catch (Exception clientEx)
                                {
                                    _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                    exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                    success = false;
                                }
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendDailyClaimStatusReport'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendDailyClaimStatusReport'. Error - {ex.Message}");
                exceptions.Add(new Exception($"General error: {ex.Message}", ex));
                success = false;
            }

            return success; // Operation completed successfully
        }

        public async Task<bool> DoUpdateDemoDataAuditDates(CancellationToken cancellationToken)
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                // Retrieve the demo tenant
                var demoTenant = await _tenantManagementService.GetByIdentifierAsync("demo");
                //var demoTenant = await _tenantManagementService.GetByIdentifierAsync("medhelp");
                //var demoTenant = await _tenantManagementService.GetByIdentifierAsync("demoTenant");

                if (demoTenant != null && demoTenant.IsActive)
                {
                    try
                    {
                        // Get repository and unit of work specific to the current tenant
                        var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(demoTenant.Identifier);
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(demoTenant.Identifier);
                        var _claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(demoTenant.Identifier);
                        var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(demoTenant.Identifier);
                        var _claimStatusTransactionRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionRepository>(demoTenant.Identifier);
                        var _claimStatusTransactionLineItemStatusChangeRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionLineItemStatusChangeRepository>(demoTenant.Identifier);

                        // Retrieve client for the current tenant
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

                        // Update Batch CreatedOn and LastModifiedOn dates
                        var batches = await _claimStatusBatchRepository.ClaimStatusBatches.Where(x => x.ClientId == client.Id).ToListAsync();
                        foreach (var batch in batches)
                        {
                            batch.CreatedOn = batch.CreatedOn.AddDays(batchAddedDays);
                            batch.LastModifiedOn = batch.CreatedOn;
                        }
                        await _claimStatusBatchRepository.Commit(cancellationToken);

                        // Update BatchClaims CreatedOn and LastModifiedOn dates
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

                        // Update Transactions CreatedOn and LastModifiedOn dates
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

                        // Update ClaimStatusChange CreatedOn and LastModifiedOn dates
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
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {demoTenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {demoTenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }
                else
                {
                    _logger.LogError("Demo tenant not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed updating Demo Data Audit Dates. Error - {ex.Message}");
                exceptions.Add(ex);
                success = false;
            }

            if (exceptions.Any())
            {
                throw new AggregateException("One or more errors occurred during the execution of 'DoUpdateDemoDataAuditDates'.", exceptions);
            }

            return success;
        }

        /// <summary>
        /// EN-37
        /// Sends email notifications to notify employees and managers about unknown claims that require attention. 
        /// This function iterates through tenants and their clients, identifies unknown claims, and sends detailed email notifications to the responsible employees.
        /// In case no required employee is found for a client's unknown claims, additional notifications are sent to designated email addresses.
        /// </summary>
        /// <returns>True if the email notifications are sent successfully, otherwise false.</returns>
        public async Task<bool> DoSendUnknownClaimsEmail()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetUnknownClaimsEmailBodyTemplate());
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                if (tenants != null && tenants.Any())
                {
                    foreach (var tenant in tenants)
                    {
                        try
                        {
                            var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);

                            // Retrieve all clients for the current tenant
                            var clients = await _clientRepository.GetAllActiveClients();
                            clients = RemoveNonProductionClients(clients);

                            if (clients != null && clients.Any())
                            {
                                foreach (var client in clients)
                                {
                                    try
                                    {
                                        var unknownClaims = await GetUnknownwClaimsByClientId(client.Id, tenant.Identifier);
                                        string clientName = client.Name;

                                        if (unknownClaims != null && unknownClaims.Any())
                                        {
                                            string[] defaultEmailToAddresses = new string[]
                                            {
                                            "cknight@medhelpinc.com",
                                            "cknight@automatedintegrationtechnologies.com",
                                            "jamesnichols@automatedintegrationtechnologies.com",
                                            "jamesnichols@medhelpinc.com",
                                            "kmccaffery@automatedintegrationtechnologies.com",
                                            "mohit@automatedintegrationtechnologies.com"
                                            };

                                            var emailBody = new
                                            {
                                                Message = $"Unknown Status Claims : {client.Name}.",
                                                UnknownClaims = unknownClaims,
                                            };

                                            string body = template.Run(emailBody);
                                            foreach (string email in defaultEmailToAddresses)
                                            {
                                                try
                                                {
                                                    MailRequestWithAttachment request = new MailRequestWithAttachment()
                                                    {
                                                        To = email,
                                                        Body = body,
                                                        Subject = $"Unknown Status Claims! - {client.ClientCode}",
                                                        Base64Content = await GetUnknownClaimsExcelDataAsync(unknownClaims),
                                                        FileName = "Unknown_Status"
                                                    };
                                                    await _mailService.SendAsync(request);
                                                }
                                                catch (Exception emailEx)
                                                {
                                                    _logger.LogError($"Failed to send email to {email} for client {client.ClientCode} in tenant {tenant.Identifier}. Error - {emailEx.Message}");
                                                    exceptions.Add(new Exception($"Email: {email}, Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {emailEx.Message}", emailEx));
                                                    success = false;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception clientEx)
                                    {
                                        _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                        exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                        success = false;
                                    }
                                }
                            }
                        }
                        catch (Exception tenantEx)
                        {
                            _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                            exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                            success = false;
                        }
                    }

                    if (exceptions.Any())
                    {
                        throw new AggregateException("One or more errors occurred during the execution of 'DoSendUnknownClaimsEmail'.", exceptions);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendUnknownClaimsEmail'. Error - {ex.Message}");
                exceptions.Add(new Exception($"General error: {ex.Message}", ex));
                success = false;
            }

            return success;
        }


        /// <summary>
        /// Retrieves an email body template from the application's template directory. 
        /// This template is intended for creating email notifications related to unknown claims and their status within a batch of claims.
        /// </summary>
        /// <returns>The content of the email body template as a string.</returns>

        private string GetUnknownClaimsEmailBodyTemplate()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\UnknownStatusClaims.template");
        }

        /// <summary>
        /// Retrieves a list of unknown claims details associated with a specific client and tenant. 
        /// This method queries the repository to find unknown claims within a specified batch, and returns a collection of claim details as UnknownClaimsDetailResponse objects.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client for whom the unknown claims are being retrieved.</param>
        /// <param name="tenantIdentifier">The unique identifier of the tenant within which the claims are managed.</param>
        /// <returns>A list of UnknownClaimsDetailResponse objects representing the unknown claims details or an empty list if no unknown claims are found.</returns>

        public async Task<List<UnknownClaimsDetailResponse>> GetUnknownwClaimsByClientId(int clientId, string tenantIdentifier)
        {
            var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenantIdentifier);
            var claims = await _claimStatusBatchClaimsRepository.ClaimStatusBatchClaims
                .Include(c => c.ClaimStatusTransaction)
                .Include(c => c.ClaimStatusBatch)
                    .ThenInclude(a => a.AuthType)
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientLocation)
                .Include(c => c.ClientProvider)
                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                .Specify(new ClaimStatusBatchClaimNotDeletedSpecification())
                .Specify(new UnknownClaimStatusBatchClaimSpecification())
                .Specify(new Application.Specifications.GenericByClientIdSpecification<ClaimStatusBatchClaim>(clientId))
                .ToListAsync();

            var unknownClaims = new List<UnknownClaimsDetailResponse>();

            foreach (var claim in claims)
            {
                // Split procedure codes by comma and create a separate entry for each one
                var procedureCodes = claim.ProcedureCode.Split(',');

                foreach (var procedureCode in procedureCodes)
                {
                    var unknownClaim = new UnknownClaimsDetailResponse
                    {
                        PatientFirstName = claim.PatientFirstName ?? string.Empty,
                        PatientLastName = claim.PatientLastName ?? string.Empty,
                        DateOfBirth = claim.DateOfBirth.Value,
                        PolicyNumber = claim.PolicyNumber ?? string.Empty,
                        ClientInsuranceId = claim.ClientInsuranceId,
                        InsuranceName = claim.ClientInsurance?.Name ?? string.Empty,
                        OfficeClaimNumber = claim.ClaimNumber ?? string.Empty,
                        InsuranceClaimNumber = claim.ClaimNumber ?? string.Empty,
                        ProcedureCode = procedureCode.Trim(), // Trim to remove any leading/trailing spaces
                        Modifiers = claim.Modifiers ?? string.Empty,
                        Quantity = claim.Quantity,
                        ProviderName = claim.ClientProvider?.Person?.LastCommaFirstName ?? string.Empty,
                        BilledAmount = claim.BilledAmount ?? 0.00m,
                        ClientLocationId = claim.ClientLocationId ?? default(int),
                        ClaimLineItemStatusId = claim.ClaimStatusTransaction?.ClaimLineItemStatusId ?? ClaimLineItemStatusEnum.Unknown,
                        ClientLocationName = claim.ClientLocation?.Name ?? string.Empty,
                        ClientLocationNpi = claim.ClientLocation?.Npi ?? string.Empty,
                        BatchNumber = claim.ClaimStatusBatch?.BatchNumber,
                        ServiceType = claim.ClaimStatusBatch?.AuthType?.Name,
                    };


                    unknownClaims.Add(unknownClaim);
                }
            }

            return unknownClaims;
        }

        /// <summary>
        /// Asynchronously generates an Excel file containing data from a list of unknown claims and returns it as a Base64-encoded string. 
        /// The Excel file includes various details about the claims, such as patient information, claim status, and billing data.
        /// </summary>
        /// <param name="unknownClaims">A list of UnknownClaimsDetailResponse objects containing the claim details to be included in the Excel report.</param>
        /// <returns>A Base64-encoded string representing the generated Excel file.</returns>

        private async Task<string> GetUnknownClaimsExcelDataAsync(List<UnknownClaimsDetailResponse> unknownClaims)
        {
            // Convert UnknownClaimsDetailResponse list to ExportQueryResponse list
            var exportResponses = unknownClaims.Select(item => new ExportQueryResponse
            {
                PatientLastName = item.PatientLastName,
                PatientFirstName = item.PatientFirstName,
                DateOfBirthString = item.DateOfBirth.ToShortDateString(),
                PolicyNumber = item.PolicyNumber,
                InsuranceName = item.InsuranceName,
                OfficeClaimNumber = item.OfficeClaimNumber,
                InsuranceClaimNumber = item.InsuranceClaimNumber,
                ClaimLineItemStatusId = item.ClaimLineItemStatusId,
                ProcedureCode = item.ProcedureCode,
                Modifiers = item.Modifiers,
                Quantity = item.Quantity,
                ProviderName = item.ProviderName,
                BilledAmount = item.BilledAmount.Value,
                ClientLocationName = item.ClientLocationName,
                ClientLocationNpi = item.ClientLocationNpi,
                BatchNumber = item.BatchNumber,
                ServiceType = item.ServiceType
            });

            // Define mappers for ExportQueryResponse
            var mappers = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { NightlyJobStringsHelper.PatientLastName, item => item.PatientLastName },
                { NightlyJobStringsHelper.PatientFirstName, item => item.PatientFirstName },
                { NightlyJobStringsHelper.DateOfBirth, item => item.DateOfBirthString },
                { NightlyJobStringsHelper.PolicyNumber, item => item.PolicyNumber },
                { NightlyJobStringsHelper.InsuranceName, item => item.InsuranceName },
                { NightlyJobStringsHelper.OfficeClaimNumber, item => item.OfficeClaimNumber },
                { NightlyJobStringsHelper.InsuranceClaimNumber, item => item.InsuranceClaimNumber },
                { NightlyJobStringsHelper.Status, item => item.ClaimLineItemStatusId },
                { NightlyJobStringsHelper.ProcedureCode, item => item.ProcedureCode },
                { NightlyJobStringsHelper.Modifiers, item => item.Modifiers },
                { NightlyJobStringsHelper.Quantity, item => item.Quantity },
                { NightlyJobStringsHelper.ProviderName, item => item.ProviderName },
                { NightlyJobStringsHelper.BilledAmount, item => item.BilledAmount },
                { NightlyJobStringsHelper.LocationName, item => item.ClientLocationName },
                { NightlyJobStringsHelper.LocationNpi, item => item.ClientLocationNpi },
                { NightlyJobStringsHelper.BatchNumber, item => item.BatchNumber },
                { NightlyJobStringsHelper.ServiceType, item => item.ServiceType }
            };

            // Call ExportAsync with the converted data and mappers
            var data = await _excelService.ExportReportAsync(exportResponses, mappers, sheetName: "UnknownClaimsReport");

            return data;
        }

        /// <summary>
        /// Sends unchecked claims email to specified recipients for each tenant and client.
        /// </summary>
        /// <returns>A boolean indicating the success of the operation.</returns>
        public async Task<bool> DoSendUncheckedClaimsEmail()
        {
            List<Exception> exceptions = new List<Exception>();
            var success = true;

            try
            {
                // Compile the Razor template for the email body
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetUncheckedClaimsEmailBodyTemplate());

                // Get the base URL from configuration
                var baseUrl = _configuration.GetSection("App:BaseUrl").Value;

                // Retrieve all tenants
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        // Get the client repository specific to the current tenant
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);

                        // Retrieve all clients for the current tenant
                        var clients = await _clientRepository.GetAllActiveClients();
                        clients = RemoveNonProductionClients(clients);

                        foreach (var client in clients)
                        {
                            try
                            {
                                var uncheckedClientClaims = await _claimStatusQueryService.GetUncheckedClaimsDetailsAsync(client.Id, tenant.Identifier) ?? new List<ClaimStatusDashboardInProcessDetailsResponse>();
                                if (uncheckedClientClaims.Count < 1)
                                {
                                    continue;
                                }

                                string body = template.Run(new
                                {
                                    ClientName = client.Name,
                                    UnCheckedClaims = uncheckedClientClaims,
                                });

                                string[] emailToAddresses = new string[]
                                {
                                "cknight@medhelpinc.com",
                                "cknight@automatedintegrationtechnologies.com",
                                "jamesnichols@automatedintegrationtechnologies.com",
                                "kmccaffery@automatedintegrationtechnologies.com",
                                "mohit@automatedintegrationtechnologies.com"
                                };

                                string subject = $"Unchecked Claims! - Client: {client.ClientCode}, Tenant: {tenant.TenantName ?? tenant.Identifier}";

                                foreach (string email in emailToAddresses)
                                {
                                    MailRequestWithAttachment request = new MailRequestWithAttachment()
                                    {
                                        To = email,
                                        Body = body,
                                        Subject = subject,
                                        Base64Content = await GetUncheckedClaimsExcelDataAsync(uncheckedClientClaims),
                                        FileName = "Unchecked_claims"
                                    };
                                    await _mailService.SendAsync(request);
                                }
                            }
                            catch (Exception clientEx)
                            {
                                _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendUncheckedClaimsEmail'.", exceptions);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendUncheckedClaimsEmail'. Error - {ex.Message}");
                throw; // Rethrow the exception to ensure Hangfire captures the failed job
            }
        }

        public async Task<bool> DoSendDaysWaitLapsedClaimsEmail()
        {
            List<Exception> exceptions = new List<Exception>();
            var success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetDaysWaitLapsedClaimsEmailBodyTemplate());

                var baseUrl = _configuration.GetSection("App:BaseUrl").Value;
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        // Get the client repository specific to the current tenant
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
                        var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenant.Identifier);

                        // Retrieve all clients for the current tenant
                        var clients = await _clientRepository.GetAllActiveClients();
                        clients = RemoveNonProductionClients(clients);

                        foreach (var client in clients)
                        {
                            try
                            {
                                var daysWaitLapsedClientClaims = await _claimStatusQueryService.GetDaysWaitLapsedByClientIdAsync(client.Id, tenant.Identifier) ?? new List<ClaimStatusDaysWaitLapsedDetailResponse>();
                                if (daysWaitLapsedClientClaims.Count < 1)
                                {
                                    continue;
                                }

                                string body = template.Run(new
                                {
                                    ClientName = client.Name,
                                    UnCheckedClaims = daysWaitLapsedClientClaims,
                                });

                                string[] emailToAddresses = new string[]
                                {
                                    "cknight@medhelpinc.com",
                                    "cknight@automatedintegrationtechnologies.com",
                                    "jamesnichols@automatedintegrationtechnologies.com",
                                    "jamesnichols@medhelpinc.com",
                                    "kmccaffery@automatedintegrationtechnologies.com",
                                    "mohit@automatedintegrationtechnologies.com"
                                };

                                string subject = $"Days Wait Lapsed Claims! - Client: {client.ClientCode}, Tenant: {tenant.TenantName ?? tenant.Identifier}";

                                foreach (string email in emailToAddresses)
                                {
                                    MailRequestWithAttachment request = new MailRequestWithAttachment()
                                    {
                                        To = email,
                                        Body = body,
                                        Subject = subject,
                                        Base64Content = await GetDaysWaitLapsedClaimsExcelDataAsync(daysWaitLapsedClientClaims),
                                        FileName = "Days_With_Lapsed_Claims"
                                    };
                                    await _mailService.SendAsync(request);
                                }
                            }
                            catch (Exception clientEx)
                            {
                                _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendDaysWaitLapsedClaimsEmail'.", exceptions);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendDaysWaitLapsedClaimsEmail'. Error - {ex.Message}");
                throw; // Rethrow the exception to ensure Hangfire captures the failed job
            }
        }
        private async Task<bool> SendEligibilityReportToEmployees( string tenantIdentifier, Domain.Entities.Client client, string spApiKey, GetEligibilityCheckDetailsByCriteriaQuery getDetailsRequest,List<EmployeeClientDto> employeeClients, string emailSubject = "Eligibility Results",string fileName = "Eligibility Results")
        {
            bool success = true;

            var _employeeRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeRepository>(tenantIdentifier);

            var environmentType = _jobCronManager.GetEnvironmentType();


            var spHttpClient = new SelfPayInternalClient(new HttpClient(), environmentStatus: environmentType,spApiKey);

            var response = await spHttpClient.Client.GetAsync(ExportDataEndpoint.GetDetailsDataByCriteria(getDetailsRequest));
            var eligibilityResponse = await response.ToResult<List<GetEligibilityCheckDetailsByCriteriaResponse>>();

            foreach (var emp in employeeClients)
            {
                var empFilteredEligibilities = await GetFilteredEligibilityCheckDetailModels(emp, eligibilityResponse.Data ?? new List<GetEligibilityCheckDetailsByCriteriaResponse>());

                if (empFilteredEligibilities.Any())
                {
                    var categorizedEligibility = new CategorizedEligibilityDetails(_mapper.Map<List<IEligibilityCheckDetail>>(empFilteredEligibilities));

                    //EN-222
                    var greeting = GreetingHelpers.GetAppropriateGreetingString();

                    var sendEligibilityReportToEmployeesViewModel = new SendEligibilityReportToEmployeesViewModel()
                    {
                        CorrectedInsurances = categorizedEligibility.InsuranceEligibleChangedOrNewTotals
                             .GroupBy(x => x.PayerName)
                             .Select(y => new InsuranceItemViewModel
                             {
                                 PayerName = y.Key,
                                 Count = y.Count()
                             }).ToList(),

                        EligibleInsurances = categorizedEligibility.InsuranceEligibleUnchangedTotals
                             .GroupBy(x => x.PayerName).Select(y => new InsuranceItemViewModel
                             {
                                 PayerName = y.Key,
                                 Count = y.Count()
                             }).ToList(),

                        DiscoveredEligibility = categorizedEligibility.SelfPayEligibilityFound
                             .GroupBy(x => x.PayerName).Select(y => new InsuranceItemViewModel
                             {
                                 PayerName = y.Key,
                                 Count = y.Count()
                             }).ToList(),

                        SelfPayEligibility = categorizedEligibility.AllEligibilityNotFound.GroupBy(x => "Self Pay").Select(y => new InsuranceItemViewModel
                        {
                            PayerName = y.Key,
                            Count = y.Count()
                        }).ToList(),

                        ReviewedInsurancesTotalCount = categorizedEligibility.AllReviewed.Count().ToString(),

                        Greeting = greeting,
                        FirstName = emp.Employee.User?.FirstName
                    };

                    var path = "/Views/Emails/DoSendEligibilityReportTemplate/DoSendEligibilityReportTemplate.cshtml";
                    string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, sendEligibilityReportToEmployeesViewModel);

                    //EN-153
                    string decryptedPin;
                    if (emp?.Employee?.User != null)
                    {
                        // Get the existing PIN from the user or set it to null if it's empty
                        string userExistingPin = emp.Employee.User?.Pin ?? null;

                        // Check if the user does not have an existing PIN
                        if (string.IsNullOrEmpty(userExistingPin))
                        {
                            // Create a new instance of the UserHelper class
                            var userHelper = new UserHelper();
                            long? phoneNumber = null;

                            // Get the phone number string from the user or set it to null if it's empty
                            string phoneNumberString = emp.Employee.User?.PhoneNumber ?? null;

                            // Check if the phone number string is not empty
                            if (!string.IsNullOrEmpty(phoneNumberString))
                            {
                                // Attempt to parse the phone number string to a long
                                if (long.TryParse(phoneNumberString, out long result))
                                {
                                    phoneNumber = result;
                                }
                            }

                            // Generate a new PIN using the UserHelper
                            decryptedPin = userHelper.GeneratePin(emp.Employee.User.FirstName, phoneNumber);
                        }
                        else
                        {
                            decryptedPin = PinExtensions.DecryptPin(emp.Employee.User.Pin);
                        }

                        var mappedDetails = _mapper.Map<List<IEligibilityCheckDetail>>(empFilteredEligibilities);

                        MailRequestWithAttachment request = new MailRequestWithAttachment()
                        {
                            To = emp?.Employee?.User?.Email,
                            Body = body,
                            Subject = emailSubject,
                            Base64Content = EligibilityExcelExportHelpers.CreateExcelFromDetailsData(mappedDetails, decryptedPin),
                            FileName = fileName
                        };

                        await _mailService.SendAsync(request, true);
                    }
                }
            }
            return success;
        }

        public async Task<bool> DoSendCheckCompletedYesterdayEligibilityEmailReport()
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
                        // Get the client repository specific to the current tenant
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
                        var _employeeRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeRepository>(tenant.Identifier);

                        // Retrieve all clients for the current tenant
                        var clients = await _clientRepository.GetAllClientsByEligibilityCriteria();
                        clients = RemoveNonProductionClients(clients);
                        ClientApiIntegrationKey spApiKey = null;

                        foreach (var client in clients)
                        {
                            try
                            {
                                spApiKey = client.ClientApiIntegrationKeys.FirstOrDefault(x => x.ApiIntegrationId == MedHelpAuthorizations.Shared.Enums.ApiIntegrationEnum.SelfPayEligibility);
                                if (string.IsNullOrWhiteSpace(spApiKey?.ApiKey))
                                {
                                    continue;
                                }

                                List<EmployeeClientDto> employeeClients = await GetAllRegistorEmployeesByClientId(tenant.Identifier, client.Id);
                                var getDetailsRequest = new GetEligibilityCheckDetailsByCriteriaQuery()
                                {
                                    ClientId = client.Id,
                                    TransactedDateFrom = DateTime.UtcNow.Date.AddDays(-1),
                                };

                                var emailSubject = $"Tenant: {tenant.TenantName ?? tenant.Identifier}, Client {client.ClientCode} - Eligibility - Results Completed Yesterday: {DateTime.Today.AddDays(-1):MM-dd-yyyy}";
                                var fileName = $"Eligibility_Completed_{DateTime.Today.AddDays(-1):MM-dd-yyyy}";

                                // Call method here
                                var clientSuccess = await SendEligibilityReportToEmployees(tenant.Identifier, client, spApiKey.ApiKey, getDetailsRequest, employeeClients, emailSubject, fileName);

                                // Update the overall success flag
                                success = success && clientSuccess;
                            }
                            catch (Exception clientEx)
                            {
                                _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendCheckCompletedYesterdayEligibilityEmailReport'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendCheckCompletedYesterdayEligibilityEmailReport'. Error - {ex.Message}");
                throw new AggregateException("Failed to execute 'DoSendCheckCompletedYesterdayEligibilityEmailReport'.", ex);
            }

            return success;
        }

        public async Task<bool> DoSendScheduledTomorrowEligibilityEmailReport()
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
                        // Get the client repository specific to the current tenant
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
                        var _employeeRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeRepository>(tenant.Identifier);

                        // Retrieve all clients for the current tenant
                        var clients = await _clientRepository.GetAllClientsByEligibilityCriteria();
                        clients = RemoveNonProductionClients(clients);
                        ClientApiIntegrationKey spApiKey = null;

                        foreach (var client in clients)
                        {
                            try
                            {
                                spApiKey = client.ClientApiIntegrationKeys.FirstOrDefault(x => x.ApiIntegrationId == MedHelpAuthorizations.Shared.Enums.ApiIntegrationEnum.SelfPayEligibility);
                                if (string.IsNullOrWhiteSpace(spApiKey?.ApiKey))
                                {
                                    continue;
                                }

                                List<EmployeeClientDto> employeeClients = await GetAllRegistorEmployeesByClientId(tenant.Identifier, client.Id);
                                var getDetailsRequest = new GetEligibilityCheckDetailsByCriteriaQuery()
                                {
                                    ClientId = client.Id,
                                    ScheduledDateFrom = DateTime.UtcNow.Date.AddDays(1),
                                    ScheduledDateTo = DateTime.UtcNow.Date.AddDays(2).AddSeconds(-1),
                                };

                                var emailSubject = $"Tenant: {tenant.TenantName ?? tenant.Identifier}, Client: {client.ClientCode} - Eligibility - Scheduled Tomorrow: {DateTime.Today.AddDays(-1):MM-dd-yyyy}";
                                var fileName = $"Eligibility_Scheduled_{DateTime.Today.AddDays(1):MM-dd-yyyy}";

                                var clientSuccess = await SendEligibilityReportToEmployees(tenant.Identifier, client, spApiKey.ApiKey, getDetailsRequest, employeeClients, emailSubject, fileName);

                                // Update the overall success flag
                                success = success && clientSuccess;
                            }
                            catch (Exception clientEx)
                            {
                                _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendScheduledTomorrowEligibilityEmailReport'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendScheduledTomorrowEligibilityEmailReport'. Error - {ex.Message}");
                throw new AggregateException("Failed to execute 'DoSendScheduledTomorrowEligibilityEmailReport'.", ex);
            }

            return success;
        }

        public async Task<bool> DoSendScheduledTodayEligibilityEmailReport()
        {
            bool overallSuccess = true;
            List<Exception> exceptions = new List<Exception>();

            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        // Get the client repository specific to the current tenant
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
                        var _employeeRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeRepository>(tenant.Identifier);

                        // Retrieve all clients for the current tenant
                        var clients = await _clientRepository.GetAllClientsByEligibilityCriteria();
                        clients = RemoveNonProductionClients(clients);
                        ClientApiIntegrationKey spApiKey = null;

                        foreach (var client in clients)
                        {
                            try
                            {
                                spApiKey = client.ClientApiIntegrationKeys.FirstOrDefault(x => x.ApiIntegrationId == MedHelpAuthorizations.Shared.Enums.ApiIntegrationEnum.SelfPayEligibility);
                                if (string.IsNullOrWhiteSpace(spApiKey?.ApiKey))
                                {
                                    continue;
                                }

                                List<EmployeeClientDto> employeeClients = await GetAllRegistorEmployeesByClientId(tenant.Identifier, client.Id);
                                var getDetailsRequest = new GetEligibilityCheckDetailsByCriteriaQuery()
                                {
                                    ClientId = client.Id,
                                    ScheduledDateFrom = DateTime.UtcNow.Date,
                                    ScheduledDateTo = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1),
                                };

                                var emailSubject = $"Tenant: {tenant.TenantName ?? tenant.Identifier}, Client: {client.ClientCode} - Eligibility - Scheduled Today: {DateTime.Today.ToString("MM-dd-yyyy")}";
                                var fileName = $"Eligibility_Scheduled_{DateTime.Today.ToString("MM-dd-yyyy")}";

                                var clientSuccess = await SendEligibilityReportToEmployees(tenant.Identifier, client, spApiKey.ApiKey, getDetailsRequest, employeeClients, emailSubject, fileName);

                                // Update the overall success flag
                                overallSuccess = overallSuccess && clientSuccess;
                            }
                            catch (Exception ex)
                            {
                                var errorMessage = $"Failed to process client {client.ClientCode} for tenant {tenant.Identifier}. Error - {ex.Message}";
                                _logger.LogError(errorMessage);
                                exceptions.Add(new Exception(errorMessage, ex));
                                overallSuccess = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorMessage = $"Failed to process tenant {tenant.Identifier}. Error - {ex.Message}";
                        _logger.LogError(errorMessage);
                        exceptions.Add(new Exception(errorMessage, ex));
                        overallSuccess = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendScheduledTodayEligibilityEmailReport'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to execute 'DoSendScheduledTodayEligibilityEmailReport'. Error - {ex.Message}");
                throw; // Rethrow the exception to ensure Hangfire captures the failed job
            }

            return overallSuccess;
        }


        /// <summary>
        /// Send approved claims for more than 6 days report to billing manager or ARFollowUp employees 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DoSendApprovedForMoreThanSixDaysClaimsEmail()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetApprovedForMoreThanSixDaysClaimsEmailBodyTemplate());
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        var assignedClaims = new List<ApprovedClaimsDetailResponse>();
                        var clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
                        var clients = await clientRepository.GetAllActiveClients();
                        clients = RemoveNonProductionClients(clients);

                        foreach (var client in clients)
                        {
                            try
                            {
                                var approvedClaims = await GetApprovedClaimsByClientId(client.Id, tenant.Identifier);
                                string clientName = client.Name;

                                if (approvedClaims.Any())
                                {
                                    List<EmployeeClient> clientEmployees = new List<EmployeeClient>();
                                    clientEmployees = await GetAllBillingManagersOrFollowUpEmployeessByClientId(client.Id, tenant.Identifier);

                                    foreach (var employee in clientEmployees)
                                    {
                                        var employeeAssignedClaims = approvedClaims.Where(x => employee.EmployeeClientLocations.Select(l => l.Id).ToList().Contains((int)x.ClientLocationId));
                                        if (employeeAssignedClaims.Any())
                                        {
                                            var emailBody = new
                                            {
                                                Message = "Please see attached for more details.",
                                                ApprovedClaims = employeeAssignedClaims,
                                            };
                                            string body = template.Run(emailBody);
                                            var user = await _userManager.FindByIdAsync(employee.Employee.UserId);

                                            // EN-153
                                            string decryptedPin;
                                            if (user != null)
                                            {
                                                // Get the existing PIN from the user or set it to null if it's empty
                                                string userExistingPin = user?.Pin ?? null;

                                                // Check if the user does not have an existing PIN
                                                if (string.IsNullOrEmpty(userExistingPin))
                                                {
                                                    // Create a new instance of the UserHelper class
                                                    var userHelper = new UserHelper();
                                                    long? phoneNumber = null;

                                                    // Get the phone number string from the user or set it to null if it's empty
                                                    string phoneNumberString = user?.PhoneNumber ?? null;

                                                    // Check if the phone number string is not empty
                                                    if (!string.IsNullOrEmpty(phoneNumberString))
                                                    {
                                                        // Attempt to parse the phone number string to a long
                                                        if (long.TryParse(phoneNumberString, out long result))
                                                        {
                                                            phoneNumber = result;
                                                        }
                                                    }

                                                    // Generate a new PIN using the UserHelper
                                                    decryptedPin = userHelper.GeneratePin(user.FirstName, phoneNumber);
                                                }
                                                else
                                                {
                                                    decryptedPin = PinExtensions.DecryptPin(user.Pin);
                                                }

                                                string emailSubject = $"Approved Claims! - Client: {client.ClientCode}, Tenant: {tenant.TenantName ?? tenant.Identifier}";

                                                MailRequestWithAttachment request = new MailRequestWithAttachment()
                                                {
                                                To = user.Email,
                                                    Body = body,
                                                    Subject = emailSubject,
                                                    Base64Content = await GetApprovedClaimsExcelDataAsync(employeeAssignedClaims.ToList(), decryptedPin),
                                                    FileName = "Approved_Claims"
                                                };
                                                await _mailService.SendAsync(request);
                                                assignedClaims.AddRange(employeeAssignedClaims);
                                            }
                                        }
                                        
                                    }
                                }

                                string subject = $"Approved Claims! - Client: {client.ClientCode}, Tenant: {tenant.TenantName ?? tenant.Identifier}";

                                var unaccountedForClaimThatGoToTheManager = assignedClaims.Where(p => approvedClaims.All(p2 => p2 != p));
                                if (unaccountedForClaimThatGoToTheManager.Any())
                                {
                                    string[] defaultEmailToAddresses = new string[]
                                    {
                                        "cknight@medhelpinc.com",
                                        "cknight@automatedintegrationtechnologies.com",
                                        "jamesnichols@automatedintegrationtechnologies.com",
                                        "jamesnichols@medhelpinc.com",
                                        "kmccaffery@automatedintegrationtechnologies.com"
                                    };
                                    var emailBody = new
                                    {
                                        Message = $"You are receiving this email because no required employee is found for Client : {client.Name}.",
                                        ApprovedClaims = approvedClaims,
                                    };
                                    string body = template.Run(emailBody);
                                    foreach (string email in defaultEmailToAddresses)
                                    {
                                        MailRequestWithAttachment request = new MailRequestWithAttachment()
                                        {
                                            To = email,
                                            Body = body,
                                            Subject = subject,
                                            Base64Content = await GetApprovedClaimsExcelDataAsync(approvedClaims),
                                            FileName = "Approved_Claims"
                                        };
                                        await _mailService.SendAsync(request);
                                    }
                                }
                            }
                            catch (Exception clientEx)
                            {
                                _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendApprovedForMoreThanSixDaysClaimsEmail'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendApprovedForMoreThanSixDaysClaimsEmail'. Error - {ex.Message}");
                throw new AggregateException("Failed to execute 'DoSendApprovedForMoreThanSixDaysClaimsEmail'.", ex);
            }

            return success;
        }


        /// <summary>
        /// Resets the current day claim count for RPA configurations for all tenants.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to gracefully cancel the operation.</param>
        /// <returns>A boolean indicating the success of the operation.</returns>
        public async Task<bool> DoResetRpaConfigurationCurrentDayClaimCount(CancellationToken cancellationToken)
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                // Retrieve all tenants
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        // Get the client insurance RPA configuration repository specific to the current tenant
                        var _clientInsuranceRpaConfigurationRepository = await _tenantRepositoryFactory.GetAsync<Application.Interfaces.Repositories.IClientInsuranceRpaConfigurationRepository>(tenant.Identifier);

                        // Update records with non-zero current day claim count to reset it to 0
                        _clientInsuranceRpaConfigurationRepository.ExecuteUpdate(
                            b => b.CurrentDayClaimCount != 0,
                            u => { u.CurrentDayClaimCount = 0; });
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoResetRpaConfigurationCurrentDayClaimCount'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoResetRpaConfigurationCurrentDayClaimCount'. Error - {ex.Message}");
                throw new AggregateException("Failed to execute 'DoResetRpaConfigurationCurrentDayClaimCount'.", ex);
            }

            return success; // Operation completed successfully
        }


        public async Task<bool> DoUserAlertsScheduledCleanup()
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
                        // Get the user alerts repository specific to the current tenant
                        var _userAlertsRepository = await _tenantRepositoryFactory.GetAsync<IUserAlertRepository>(tenant.Identifier);

                        // Cleanup UserAlerts for tenant
                        await _userAlertsRepository.DoScheduledCleanup();
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Failed to cleanup UserAlerts for Tenant: {tenant.TenantName}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.TenantName}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoUserAlertsScheduledCleanup'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                success = false;
                _logger.LogError($"General exception in 'DoUserAlertsScheduledCleanup'. Error - {ex.Message}");
                throw new AggregateException("Failed to execute 'DoUserAlertsScheduledCleanup'.", ex);
            }

            return success;
        }



        /// <summary>
        /// Write manual job service that adds or updates employees reports from defaultSystemReportFilters definitions. 
        /// If report is not previously added and the roles match.. add a CliemntUserReport for this employee.. 
        /// Update the report with new definition with any changes if it is already added. 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddUpdateSystemDefaultReportForEmployeeClient()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            var tenants = await _tenantManagementService.GetAllActiveAsync();
            foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
            {
                try
                {
                    var _systemDefaultReportRepository = await _tenantRepositoryFactory.GetAsync<ISystemDefaultReportFilterRepository>(tenant.Identifier);
                    var _employeeClientRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeClientRepository>(tenant.Identifier);

                    List<EmployeeClient> employeeClientDetails = await _employeeClientRepository.GetAllAsync() ?? new List<EmployeeClient>();
                    foreach (EmployeeClient employeeClient in employeeClientDetails)
                    {
                        try
                        {
                            await _clientReportFilterService.AddSystemDefaultReportFiltersForEmployeeClient(employeeClientId: employeeClient.Id, tenantIdentifier: tenant.Identifier);
                        }
                        catch (Exception clientEx)
                        {
                            _logger.LogError($"Failed to add/update system default report for employee client {employeeClient.Id} in tenant {tenant.Identifier}. Error - {clientEx.Message}");
                            exceptions.Add(new Exception($"Employee Client: {employeeClient.Id}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                            success = false;
                        }
                    }
                    if (exceptions.Any())
                    {
                        throw new AggregateException("One or more errors occurred during the execution of 'AddUpdateSystemDefaultReportForEmployeeClient'.", exceptions);
                    }
                }
                catch (Exception tenantEx)
                {
                    _logger.LogError($"Failed to add/update system default report for tenant {tenant.TenantName}. Error - {tenantEx.Message}");
                    exceptions.Add(new Exception($"Tenant: {tenant.TenantName}, Error: {tenantEx.Message}", tenantEx));
                    success = false;
                    throw;
                }
            }

            return success;
        }


        #region Get Email Template Methods
        private string GetEmailBodyTemplateForDailyClaimStatusReport()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\DailyClaimStatusReport.template");
        }

        private string GetUncheckedClaimsEmailBodyTemplate()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\UncheckedClaimsEmail.template");
        }

        private string GetDaysWaitLapsedClaimsEmailBodyTemplate()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\DaysWaitLapsedClaimsEmail.template");
        }

        /// <summary>
        /// get the spreadsheet template for the eligibility email report
        /// </summary>
        /// <returns></returns>
        private string GetEligibilityReportEmailBodyTemplate()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\DoSendEligibilityEmailReport.template");
        }

        private string GetApprovedForMoreThanSixDaysClaimsEmailBodyTemplate()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\ApprovedForMoreThanSixDays.template");
        }

        // GET EMAIL TEMPLATE FOR FAILED CLIENT CONFIGURATIONS
        private string GetEmailBodyTemplateForFailedClientRpaConfiguration()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\FailedClientInsuranceRpaConfigurations.template");
        }

        // Email Employees status changes.
        private string GetEmailBodyTemplateForDoSendYesterdayDeinalClaimsToEmployees()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\DoSendYesterdfaysDeniedClaimsToEmployees.template");
        }

        #endregion

        #region Get Excel Data Methods
        private async Task<string> GetFailedClientRpaConfigurationExcelDataAsync(List<GetFailedClientInsuranceRpaConfigurationsResponse> expiringData)
        {
            var exportResponse = expiringData.Select(item => new ExportQueryResponse
            {
                ConfigurationId = item.Id,
                ClientCode = item.ClientCode,
                ClientInsuranceLookupName = item.ClientInsuranceLookupName,
                AuthTypeName = item.AuthTypeName,
                Username = item.Username,
                Password = item.Password,
                TargetUrl = item.TargetUrl,
                FailureMessage = item.FailureMessage,
                ExpiryWarningReported = item.ExpiryWarningReported,
                LastModifiedOn = item.LastModifiedOn?.ToShortDateString(),
            });
            var data = await _excelService.ExportReportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Configuration Id", item => item.ConfigurationId },
                { "Client Code", item => item.ClientCode },
                { "Client Ins", item => item.ClientInsuranceLookupName },
                { "AuthType Name", item => item.AuthTypeName },
                { "Username", item => item.Username },
                { "Password", item => item.Password },
                { "Target Url", item => item.TargetUrl },
                { "Failure Message", item => item.FailureMessage },
                { "Expiry Warning", item => item.ExpiryWarningReported },
                { "Last Modified On", item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastModifiedOn)},
            }, sheetName: "FailedClientRpaConfigurations");

            return data;
        }
        private async Task<string> GetFailedClaimStatusBatchesExcelDataAsync(List<ClaimStatusBatch> expiringData)
        {
            var exportResponse = expiringData.Select(item => new ExportQueryResponse
            {
                BatchNumber = item.BatchNumber,
                AuthTypeName = item.AuthType?.Name,
                ClientInsurance_ClientCode = item.ClientInsurance.Client.ClientCode,
                ClientInsuranceId = item.ClientInsurance.Id,
                AssignedDateTimeUtc = item.AssignedDateTimeUtc?.ToShortDateString(),
                AssignedToRpaCode = item.AssignedToRpaCode,
                CompletedDateTimeUtc = item.CompletedDateTimeUtc?.ToShortDateString(),
                AbortedOnUtc = item.AbortedOnUtc?.ToShortDateString(),
                AbortedReason = item.AbortedReason,
                CreatedOnString = item.CreatedOn.ToShortDateString(),
                LastModifiedOn = item.LastModifiedOn?.ToShortDateString(),
                AssignedClientRpaConfigurationId = item.AssignedClientRpaConfigurationId,
            });

            var data = await _excelService.ExportReportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Batch #", item => item.BatchNumber },
                { "AuthType Name", item => item.AuthTypeName },
                { "Client Code", item => item.ClientInsurance_ClientCode },
                { "Client InsuranceId", item => item.ClientInsuranceId },
                { "Assigned Date", item => item.AssignedDateTimeUtc },
                { "Assigned To RpaCode", item => item.AssignedToRpaCode },
                { "Completed Date Time Utc", item => item.CompletedDateTimeUtc },
                { "Aborted On", item => item.AbortedOnUtc },
                { "Aborted Reason", item => item.AbortedReason },
                { "Created On", item => item.CreatedOnString },
                { "Last ModifiedOn", item => item.LastModifiedOn },
                { "Assigned Client Rpa Configuration Id", item => item.AssignedClientRpaConfigurationId }
            }, sheetName: "FailedClaimStatusBatches");

            return data;
        }
        private async Task<string> GetDailyClaimStatusExcelDataAsync(List<DailyClaimStatusReportResponse> expiringData, string decryptedPin = null)
        {
            var exportResponse = expiringData.Select(item => new ExportQueryResponse
            {
                ClaimDate = item.ClaimDate,
                Reviewed = item.Reviewed,
                ReviewedPercentage = item.ReviewedPercentage,
                InProcess = item.InProcess,
                InProcessPercentage = item.InProcessPercentage,
                ApprovedPaid = item.ApprovedPaid,
                ApprovePaidPercentage = item.ApprovePaidPercentage,
                Denied = item.Denied,
                DeniedPercentage = item.DeniedPercentage,
                NotAdjudicated = item.NotAdjudicated,
                NotAdjudicatedPercentage = item.NotAdjudicatedPercentage,
                ZeroPaid = item.ZeroPaid,
                ZeroPaidPercentage = item.ZeroPaidPercentage,
            });
            var data = await _excelService.ExportReportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Billed On", item => item.ClaimDate },
                { "Reviewed", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.Reviewed.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { "% Reviewed", item => item.ReviewedPercentage },
                { "In-Process", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.InProcess.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { "% In-Process", item => item.InProcessPercentage },
                { "Paid/Approved", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType,item.ApprovedPaid.ToString("C", new CultureInfo("en-US"))  ?? "$0.00") },
                { "% Paid/Approved", item => item.ApprovePaidPercentage },
                { "Denied", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.Denied.ToString("C", new CultureInfo("en-US")) ?? "$0.00") },
                { "% Denied", item => item.DeniedPercentage },
                { "Not Adjudicated", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.NotAdjudicated.ToString("C", new CultureInfo("en-US")) ?? "$0.00") },
                { "% Not Adjudicated", item => item.NotAdjudicatedPercentage },
                { "Zero Pay", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.ZeroPaid.ToString("C", new CultureInfo("en-US")) ?? "$0.00") },
                { "% Zero Pay", item => item.ZeroPaidPercentage },
            }, sheetName: "DailyClaimStatusReport", passwordString: decryptedPin);

            return data;
        }

        private async Task<string> GetUncheckedClaimsExcelDataAsync(List<ClaimStatusDashboardInProcessDetailsResponse> uncheckedClaimsData)
        {
            var exportResponse = uncheckedClaimsData.Select(z => new ExportQueryResponse
            {
                PatientLastName = z.PatientLastName,
                PatientFirstName = z.PatientFirstName,
                DateOfBirthString = z.DateOfBirth,
                PayerName = z.PayerName,
                ServiceType = z.ServiceType,
                PolicyNumber = z.PolicyNumber,
                OfficeClaimNumber = z.OfficeClaimNumber,
                ClaimBilledOnString = z.ClaimBilledOn,
                DateOfServiceFromString = z.DateOfServiceFrom,
                ProcedureCode = z.ProcedureCode,
                Quantity = z.Quantity,
                BilledAmount = z.BilledAmount,
                BatchNumber = z.BatchNumber,
                ClientLocationName = z.ClientLocationName,
                ClientLocationNpi = z.ClientLocationNpi,
                AitClaimReceivedDate = z.AitClaimReceivedDate,
                AitClaimReceivedTime = z.AitClaimReceivedTime

            });
            var data = await _excelService.ExportReportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Patient Last Name", item => item.PatientLastName },
                { "Patient First Name", item => item.PatientFirstName },
                { "DOB", item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirthString)},
                { "Payer", item => item.PayerName  },
                { "Service Type", item => item.ServiceType },
                { "Policy #", item => item.PolicyNumber },
                { "Office Claim #", item => item.OfficeClaimNumber },
                { "Billed On",  item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOnString)},
                { "DOS", item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFromString) },
                { "Procedure Code", item => item.ProcedureCode },
                { "Quantity", item => item.Quantity },
                { "Billed Amount", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { "Batch #", item => item.BatchNumber },
                { "Ait Received On", item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate)},
                { "Ait Received Time", item =>  _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime) },
                { "Client Location Name", item => item.ClientLocationName },
                { "Client Location Npi", item => item.ClientLocationNpi },
            }, sheetName: "UncheckedClaimsReport");

            return data;
        }

        private async Task<string> GetDaysWaitLapsedClaimsExcelDataAsync(List<ClaimStatusDaysWaitLapsedDetailResponse> daysWaitLapsedClaimsData)
        {
            var exportResponse = daysWaitLapsedClaimsData.Select(z => new ExportQueryResponse
            {
                PatientLastName = z.PatientLastName,
                PatientFirstName = z.PatientFirstName,
                DateOfBirthString = z.DateOfBirth,
                PayerName = z.PayerName,
                ServiceType = z.ServiceType,
                PolicyNumber = z.PolicyNumber,
                OfficeClaimNumber = z.OfficeClaimNumber,
                ClaimBilledOnString = z.ClaimBilledOn,
                DateOfServiceFromString = z.DateOfServiceFrom,
                ProcedureCode = z.ProcedureCode,
                Quantity = z.Quantity,
                BilledAmount = z.BilledAmount,
                ClaimStatusBatchId = z.ClaimStatusBatchId,
                BatchNumber = z.BatchNumber,
                ClaimLineItemStatus = z.ClaimLineItemStatus,
                StatusLastCheckedOn = z.StatusLastCheckedOn, // Formatting DateTime with time
                DaysLapsed = z.DaysLapsed,
                AitClaimReceivedDate = z.AitClaimReceivedDate,
                AitClaimReceivedTime = z.AitClaimReceivedTime

            });
            var data = await _excelService.ExportReportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Patient Last Name", item => item.PatientLastName },
                { "Patient First Name", item => item.PatientFirstName },
                { "DOB", item => item.DateOfBirthString },
                { "Payer", item => item.PayerName  },
                { "Service Type", item => item.ServiceType },
                { "Policy #", item => item.PolicyNumber },
                { "Office Claim #", item => item.OfficeClaimNumber },
                { "Billed On",  item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOnString)},
                { "DOS", item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFromString) },
                { "Procedure Code", item => item.ProcedureCode },
                { "Quantity", item => item.Quantity },
                { "Billed Amount", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00") },
                { "Batch Id", item => item.ClaimStatusBatchId },
                { "Batch #", item => item.BatchNumber },
                { "Status", item => item.ClaimLineItemStatus },
                { "Last Checked", item => item.StatusLastCheckedOn },
                { "Days Lapsed", item => item.DaysLapsed },
                { "Ait Received On", item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate)},
                { "Ait Received Time", item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime) },
            }, sheetName: "DaysWaitLapsedClaimsReport");

            return data;
        }

        ///// <summary>
        ///// get all categorized lists for the discovered eligibilities
        ///// </summary>
        ///// <param name="discoveredEligibilities"></param>
        ///// <returns></returns>
        //private async Task<string> GetEligibilityCheckExcelDataAsync(List<Get> discoveredEligibilities, List<GetEligibilityCheckRequestByCriteriaResponse> selfPayItems, string decryptedPin = null)
        //{
        //    //list of names of all the categorized lists
        //    List<string> sheetsName = new() { "All", "Corrected Insurances", "Unchanged Insurances", "Discovered Eligibility", "Self Pay" };

        //    //lists of all the categories
        //    var allData = discoveredEligibilities;
        //    var changedData = discoveredEligibilities.Where(x => true);//.Where(x => x.MemberNumber != x.EligibilityCheckResult.EligibilityCheckRequest.LkpMemberNumber);
        //    var unchangedData = discoveredEligibilities.Where(x => true);//.Where(x => x.MemberNumber == x.EligibilityCheckResult.EligibilityCheckRequest.LkpMemberNumber);
        //    var discoveredData = discoveredEligibilities.Where(x => true);//.Where(x => x.EligibilityCheckResult.EligibilityCheckRequest.LkpMemberNumber is null);

        //    //excel sheets for all the categories
        //    var allDataSheet = GetExcelData(discoveredEligibilities, "All Discovered Eligibilities");
        //    var changedDataSheet = GetExcelData(changedData.ToList(), "Changed Eligibilities");
        //    var unchangedDataSheet = GetExcelData(unchangedData.ToList(), "Unchanged Eligibilities");
        //    var discoveredDataSheet = GetExcelData(discoveredData.ToList(), "Discovered Self-Pay");
        //    var selfPaySheet = GetSelfPayExcelData(selfPayItems, "Self Pay");

        //    //generate export details and combine into mapper list
        //    var exportDetails = new List<IEnumerable<object>>() { allData, changedData, unchangedData, discoveredData, selfPayItems };
        //    var mapperList = CombineExportSheetModels(allDataSheet, changedDataSheet, unchangedDataSheet, discoveredDataSheet, selfPaySheet);

        //    return await _excelService.ExportMultipleSheetsAsync(exportDetails, mapperList, sheetsName, boldLastRow: false, applyBoldRowInFirstDataModel: false, applyBoldHeader: false, passwordString: decryptedPin).ConfigureAwait(true);

        //}

        //private Dictionary<string, Func<GetEligibilityCheckRequestByCriteriaResponse, object>> GetSelfPayExcelData(List<GetEligibilityCheckRequestByCriteriaResponse> selfPay, string sheetName)
        //{
        //    return new Dictionary<string, Func<GetEligibilityCheckRequestByCriteriaResponse, object>>()
        //   {
        //        { "Provider Name", item => item.ProviderName },
        //        { "Location Name", item => "LOCATION" },
        //        { "Patient Last Name", item => item.PatientLastName },
        //        { "Patient First Name", item => item.PatientFirstName },
        //        { "Patient Middle Name", item => item.PatientMiddleName },
        //        { "Date Of Birth", item => item.PatientDateOfBirthString },
        //        { "Patient Social Security Number", item => item.PatientSocialSecurityNumber },
        //        { "Date Of Service", item => item.DateOfServiceString },
        //        { "Scheduled Date", item => item.ScheduledDateString},
        //        { "LKP Client Insurance Name", item => item.LkpClientInsuranceName},
        //        { "LKP Member Number", item => item.LkpMemberNumber },
        //        { "LKP Group Number", item => item.LkpGroupNumber },
        //        { "LKP Subscriber Last Name", item => item.LkpSubscriberLastName },
        //        { "LKP Subscriber First Name", item => item.LkpSubscriberFirstName },
        //        { "LKP Subscriber Date Of Birth", item => item.LkpSubscriberDateOfBirth },
        //        { "Eligibility Check Completed On", item => item.EligibilityCheckCompletedOn.ToString() },
        //    };
        //}

        /// <summary>
        /// Get report for claims approved form more than 6 days
        /// </summary>
        /// <param name="daysWaitLapsedClaimsData"></param>
        /// <returns></returns>
        private async Task<string> GetApprovedClaimsExcelDataAsync(List<ApprovedClaimsDetailResponse> daysWaitLapsedClaimsData, string descryptedPin = null)
        {
            var exportResponse = daysWaitLapsedClaimsData.Select(item => new ExportQueryResponse
            {
                PatientLastName = item.PatientLastName,
                PatientFirstName = item.PatientFirstName,
                DateOfBirthString = item.DateOfBirth.ToShortDateString(),
                PolicyNumber = item.PolicyNumber,
                InsuranceName = item.InsuranceName,
                OfficeClaimNumber = item.OfficeClaimNumber,
                InsuranceClaimNumber = item.InsuranceClaimNumber,
                ClaimLineItemStatusId = item.ClaimLineItemStatusId,
                ApprovedSinceDate = item.ApprovedSinceDate,
                ClaimBilledOn = item.ClaimBilledOn,
                DateOfService = item.DateOfService,
                DateOfServiceString = item.DateOfService.ToShortDateString(),
                ProcedureCode = item.ProcedureCode,
                Modifiers = item.Modifiers,
                Quantity = item.Quantity,
                ProviderName = item.ProviderName,
                BilledAmount = item.BilledAmount.HasValue ? item.BilledAmount.Value : 0.0m,
                ClientLocationName = item.ClientLocationName,
                ClientLocationNpi = item.ClientLocationNpi,
            });

            var data = await _excelService.ExportReportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Patient Last Name", item => item.PatientLastName },
                { "Patient First Name", item => item.PatientFirstName },
                { "DOB", item => item.DateOfBirthString },
                { "Policy #", item => item.PolicyNumber },
                { "Insurance Name", item => item.InsuranceName },
                { "Office Clailm No", item => item.OfficeClaimNumber },
                { "Insurance Claim No", item => item.InsuranceClaimNumber },
                { "Status", item => item.ClaimLineItemStatusId.ToString() },
                { "Approved Since", item => item.ApprovedSinceDate },
                { "Billed On", item => item.ClaimBilledOn },
                { "DOS", item => item.DateOfServiceString },
                { "Procedure Code", item => item.ProcedureCode },
                { "Modifiers", item => item.Modifiers },
                { "Quantity", item => item.Quantity },
                { "Provider Name", item => item.ProviderName },
                { "Billed Amount", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00") },
                { "Location Name", item => item.ClientLocationName },
                { "Location Npi", item => item.ClientLocationNpi },
            }, sheetName: "ApprovedClaimsReport", passwordString: descryptedPin);

            return data;
        }


        /// <summary>
        /// get excel format data for all the categorized lists
        /// </summary>
        /// <param name="discoveredEligibilities"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private Dictionary<string, Func<GetDiscoveredEligibilitiesByCriteriaResponse, object>> GetExcelData(List<GetDiscoveredEligibilitiesByCriteriaResponse> discoveredEligibilities, string sheetName)
        {
            return new Dictionary<string, Func<GetDiscoveredEligibilitiesByCriteriaResponse, object>>()
           {
                { "Patient Last Name", item => item.PatientLastName },
                { "Patient First Name", item => item.PatientFirstName },
				//{ "ScheduledDate", item => item.EligibilityCheckResult.EligibilityCheckRequest.ScheduledDate.ToString() },
				{ "Payer", item => item.Payer.PayerName },
                { "Policy #", item => item.MemberNumber },
                { "Group #", item => item.GroupNumber},
                { "Cardholder Name/Address", item => $"{item.Cardholder.FirstName} {item.Cardholder.LastName}/{item.Cardholder.Address?.AddressStreetLine1},{item.Cardholder.Address?.AddressStreetLine2},{item.Cardholder.Address?.City},{item.Cardholder.Address?.PostalCode}"},
                { "Copay", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.Copay.Value.ToString("C", new CultureInfo("en-US")) ?? "$0.00") },
                { "Deductible and Deductible Balance", item => $"{item.Deductible}/{item.RemainingDeductible}" },
                { "Primary Provider", item => $"{item.PrimaryCareProvider?.FirstName} {item.PrimaryCareProvider?.LastName }" },
				//{ "Eligibility Date(s)", item => item.EligibilityCheckResult.EligibilityCheckRequest.EligibilityCheckCompletedOn.ToString() },
			};
        }

        /// <summary>
        /// combine all the lists to create mapperList
        /// </summary>
        /// <param name="excelAllDetails"></param>
        /// <param name="excelChangedDetails"></param>
        /// <param name="excelUnchangedDetails"></param>
        /// <param name="excelselfPayDetails"></param>
        /// <returns></returns>
        public List<Dictionary<string, Func<object, object>>> CombineExportSheetModels(Dictionary<string, Func<GetDiscoveredEligibilitiesByCriteriaResponse, object>> excelAllDetails,
            Dictionary<string, Func<GetDiscoveredEligibilitiesByCriteriaResponse, object>> excelChangedDetails, Dictionary<string, Func<GetDiscoveredEligibilitiesByCriteriaResponse, object>> excelUnchangedDetails,
            Dictionary<string, Func<GetDiscoveredEligibilitiesByCriteriaResponse, object>> excelselfPayDetails, Dictionary<string, Func<GetEligibilityCheckRequestByCriteriaResponse, object>> selfPayDetails)
        {
            return new List<Dictionary<string, Func<object, object>>>()
            {
                excelAllDetails.ToDictionary(allKey => allKey.Key, all => (Func<object, object>)(exp => all.Value((GetDiscoveredEligibilitiesByCriteriaResponse)exp))),
                excelChangedDetails.ToDictionary(changedKey => changedKey.Key, changed => (Func<object, object>)(exp => changed.Value((GetDiscoveredEligibilitiesByCriteriaResponse)exp))),
                excelUnchangedDetails.ToDictionary(unchangedKey => unchangedKey.Key, unchanged => (Func<object, object>)(exp => unchanged.Value((GetDiscoveredEligibilitiesByCriteriaResponse)exp))),
                excelselfPayDetails.ToDictionary(selfpayKey => selfpayKey.Key, selfpay => (Func<object, object>)(exp => selfpay.Value((GetDiscoveredEligibilitiesByCriteriaResponse)exp))),
                selfPayDetails.ToDictionary(selfpayKey => selfpayKey.Key, selfpay => (Func<object, object>)(exp => selfpay.Value((GetEligibilityCheckRequestByCriteriaResponse)exp)))
            };
        }

        private async Task<string> GetInProcessClaimsExcelDataAsync(List<ExportQueryResponse> exportResponse)
        {
            var clientName = exportResponse.First().ClientName;
            // List of sheet names for the export
            List<string> sheetsName = new();

            // Create a list containing the data to be exported, including summary and detail items.
            var exportDetails = new List<IEnumerable<ExportQueryResponse>>();

            List<Dictionary<string, Func<ExportQueryResponse, object>>> MappingsList = new List<Dictionary<string, Func<ExportQueryResponse, object>>>();
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Insurance", item => item.ClientInsuranceName },
                { "Claim Count", item => item.ClaimCount },
                { "Failure Reported", item => item.FailureReported },
                { "Expiry Warning Reported", item => item.IsExpiryWarningReported }
            };
            List<IGrouping<string, ExportQueryResponse>> clientInProcessClaims = exportResponse.GroupBy(z => z.ClientName).ToList();


            foreach (var clientInProcessClaim in clientInProcessClaims)
            {
                var statusTitle = clientInProcessClaim.Key;
                sheetsName.Add(statusTitle);
                exportDetails.Add(clientInProcessClaim);
                MappingsList.Add(exportMapper);
            }

            // Export data to Excel with multiple sheets
            return await _excelService.ExportMultipleSheetsAsync(exportDetails, MappingsList, sheetsName, boldLastRow: false, applyBoldRowInFirstDataModel: true, applyBoldHeader: false, groupByKeySelector: x => x.PayerName, hasGroupByKeySelector: false).ConfigureAwait(true);
        }

        #endregion

        #region Helper Methods

        public List<Domain.Entities.Client> RemoveNonProductionClients(List<Domain.Entities.Client> clientList)
        {
            clientList = clientList ?? new List<Domain.Entities.Client>();

            return clientList.Where(c => (c.ClientCode.ToUpper() != "CLIENT123" && !c.ClientCode.ToUpper().Contains("DEFAULT") && c.ClientCode.ToUpper() != "DEMO"))?.ToList() ?? new List<Domain.Entities.Client>();
        }

        public async Task<List<EmployeeClientDto>> GetAllRegistorEmployeesByClientId(string tenantIdentifier, int clientId)
        {
            try
            {
                List<EmployeeLevelEnum> employeeLevelIds = new List<EmployeeLevelEnum>()
                                                            {
                                                                EmployeeLevelEnum.SupervisorLevel,
                                                                EmployeeLevelEnum.ManagerLevel,
                                                                EmployeeLevelEnum.NonManagementLevel,
                                                            };

                List<DepartmentEnum> departmentIds = new List<DepartmentEnum>() { DepartmentEnum.Registor };
                List<EmployeeRoleEnum> employeeRoleIds = new List<EmployeeRoleEnum>(); // { EmployeeRoleEnum.Registor }

                var employeeClientRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeClientRepository>(tenantIdentifier);

                var employeeClients = await _employeeClientService.GetEmployeeClientsByRolesDepartmentsLevels(clientId, employeeLevelIds, departmentIds, employeeRoleIds, employeeClientRepository);

                return employeeClients;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GetEligibilityCheckDetailsByCriteriaResponse>> GetFilteredEligibilityCheckDetailModels(EmployeeClientDto employeeClient, List<GetEligibilityCheckDetailsByCriteriaResponse> eligibilityDetails)
        {
            try
            {
                var query = eligibilityDetails.AsQueryable();

                if (employeeClient.EmployeeClientLocations.Any())
                {
                    //query = query.Where(x => x.ClientLocationId != null ? employeeClient.EmployeeClientLocations.Select(x => x.EligibilityClientLocationId).ToList().Contains(x.ClientLocationId) : true);
                    query = query.Where(x => x.ClientLocationId == null || (x.ClientLocationId != null ? employeeClient.EmployeeClientLocations.Select(x => x.EligibilityClientLocationId).ToList().Contains(x.ClientLocationId) : true));
                }

                if (employeeClient.EmployeeClientAlphaSplits.Any())
                {
                    query = query.Where(model => HasAlphaSplitMatch(employeeClient.EmployeeClientAlphaSplits, model.PatientLastName));
                }
                var filteredStatusChangeDetails = await Task.FromResult(query.ToList());

                return filteredStatusChangeDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GetEligibilityCheckRequestByCriteriaResponse>> GetFilteredEligibilityCheckRequestModels(EmployeeClientDto employeeClient, List<GetEligibilityCheckRequestByCriteriaResponse> eligibilityDetails)
        {
            try
            {
                var query = eligibilityDetails.AsQueryable();

                if (employeeClient.EmployeeClientLocations.Any())
                {
                    //query = query.Where(x => x.ClientLocationId != null ? employeeClient.EmployeeClientLocations.Select(x => x.EligibilityClientLocationId).ToList().Contains(x.ClientLocationId) : true);
                    query = query.Where(x => x.ClientLocationId == null || (x.ClientLocationId != null ? employeeClient.EmployeeClientLocations.Select(x => x.EligibilityClientLocationId).ToList().Contains(x.ClientLocationId) : true));
                }

                if (employeeClient.EmployeeClientAlphaSplits.Any())
                {
                    query = query.Where(model => HasAlphaSplitMatch(employeeClient.EmployeeClientAlphaSplits, model.PatientLastName));
                }
                var filteredStatusChangeDetails = await Task.FromResult(query.ToList());

                return filteredStatusChangeDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<EmployeesClaimStatusResponseModel>> GetFilteredEmployeeClaimStatusResponseModels(EmployeeClientDto employeeClient, List<EmployeesClaimStatusResponseModel> lineItemStatusChangeDetails)
        {
            try
            {
                var query = lineItemStatusChangeDetails.AsQueryable();

                if (employeeClient.EmployeeClientInsurances.Any())
                {
                    query = query.Where(x => x.ClientInsuranceId != null ? employeeClient.EmployeeClientInsurances.Select(x => x.ClientInsuranceId).ToList().Contains((int)x.ClientInsuranceId) : true);
                }

                if (employeeClient.EmployeeClientLocations.Any())
                {
                    query = query.Where(x => x.ClientLocationId != null ? employeeClient.EmployeeClientLocations.Select(x => x.ClientLocationId).ToList().Contains((int)x.ClientLocationId) : true);
                }

                if (employeeClient.AssignedClientEmployeeRoles.Any())
                {
                    query = query.Where(x => employeeClient.AssignedClientEmployeeRoles
                    .Any(cer => cer.EmployeeRole.EmployeeRoleClaimStatusExceptionReasonCategories
                        .Select(cat => cat.ClaimStatusExceptionReasonCategoryId)
                        .Contains(x.ClaimStatusExceptionReasonCategorId)));
                }

                if (employeeClient.EmployeeClientAlphaSplits.Any())
                {
                    query = query.Where(model => HasAlphaSplitMatch(employeeClient.EmployeeClientAlphaSplits, model.PatientLastName));
                }
                var filteredStatusChangeDetails = await Task.FromResult(query.ToList());

                return filteredStatusChangeDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool HasAlphaSplitMatch(List<EmployeeClientAlphaSplitDto> alphaSplits, string compareString)
        {
            if (string.IsNullOrWhiteSpace(compareString))
            {
                return true;
            }

            char compareChar = char.Parse(compareString.Substring(0, 1).ToUpper());

            char beginSplit;
            char endSplit;

            foreach (var split in alphaSplits)
            {
                if (split.AlphaSplitId == AlphaSplitEnum.CustomRange)
                {
                    if (string.IsNullOrWhiteSpace(split.CustomBeginAlpha) || string.IsNullOrWhiteSpace(split.CustomEndAlpha))
                    {
                        return false;
                    }
                    else
                    {
                        beginSplit = char.Parse(split.CustomBeginAlpha);
                        endSplit = char.Parse(split.CustomEndAlpha);
                    }
                }
                else
                {
                    beginSplit = char.Parse(split.AlphaSplit.BeginAlpha);
                    endSplit = char.Parse(split.AlphaSplit.EndAlpha);
                }

                if (compareChar >= beginSplit && compareChar <= endSplit)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<EmployeeClientDto>> GetAllBillingManagersOrFollowUpEmployeesByClientId(int clientId, string tenantIdentifier)
        {
            try
            {
                List<EmployeeLevelEnum> employeeLevelIds = new List<EmployeeLevelEnum>()
                                                            {
                                                                EmployeeLevelEnum.SupervisorLevel,
                                                                EmployeeLevelEnum.ManagerLevel,
                                                                EmployeeLevelEnum.NonManagementLevel
                                                            };

                List<DepartmentEnum> departmentIds = new List<DepartmentEnum>() { DepartmentEnum.Billing };
                List<EmployeeRoleEnum> employeeRoleIds = new List<EmployeeRoleEnum>();
                var employeeClientRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeClientRepository>(tenantIdentifier);

                var employeeClients = await _employeeClientService.GetEmployeeClientsByRolesDepartmentsLevels(clientId, employeeLevelIds, departmentIds, employeeRoleIds, employeeClientRepository);

                return employeeClients;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GetEmployeeDenialCategoriesExportContent(List<EmployeesClaimStatusResponseModel> employeeAssignedClaims, string decryptedPin = null)
        {
            try
            {
                IEnumerable<ExportQueryResponse> exportDetails = employeeAssignedClaims;

                var detailDataExcel = _claimStatusQueryService.GetExcelDenialReport();


                // Create the report options instance
                var options = new ReportCreationOptions
                {
                    Data = exportDetails,
                    MapperFunc = detailDataExcel,
                    SheetName = ReportHelper.Denial_Summary,
                    BoldLastRow = false,
                    ApplyBoldRowInFirstDataModel = true,
                    ApplyBoldHeader = false,
                    PasswordString = null,
                    GroupByKeySelector = x => x.ExceptionReasonCategory,
                    HasGroupByKeySelector = true,
                    PivotTableConfigurations = new List<ExcelPivotTableConfiguration>
                    {
                        new ExcelPivotTableConfiguration(
                            ReportHelper.Summary, // Sheet name where the pivot table will be created
                            ReportHelper.Summary, // Pivot table name
                            ReportHelper.PerStatus, // Data source name
                            async (pivotTable) => await ConfigureDenialSummaryPivotTable(pivotTable) // Method to configure the pivot table
                        )
                    },
                };

                return await _excelService.CreateReport(options);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Configures the pivot table for the denial summary report.
        /// </summary>
        /// <param name="pivotTable">The pivot table object to be configured.</param>
        /// <returns>A task representing the asynchronous operation of configuring the pivot table.</returns>
        private static async Task ConfigureDenialSummaryPivotTable(ExcelPivotTable pivotTable)
        {
            // Add fields to the pivot table
            var rowField = pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Exception_Reason]);

            var quantityField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Quantity]);
            var billedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Billed_Amt]);

            // Specify the format for the fields
            quantityField.Format = "#,##0";
            billedAmtField.Format = "$#,##0.00";

            // Set DataOnRows as needed
            pivotTable.DataOnRows = false;

            // Ensure all row fields are collapsed by default
            rowField.Items.Refresh();  // Load the pivot items from the source data
            rowField.Items.ShowDetails(false); // Collapse all items

            // As this is asynchronous, return a completed task
            await Task.CompletedTask;
        }

        private string GetEmployeeStatusByDenialCategoriesEmailBody(List<EmployeesClaimStatusResponseModel> employeeAssignedClaims, IRazorEngineCompiledTemplate template)
        {
            try
            {
                // Render the template with the dynamic model
                string result = template.Run(new
                {
                    EmployeeAssignedClaims = employeeAssignedClaims
                });

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetEmployeeClaimStatusResponseForExport()
        {
            return new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Exception Reason", item => item.ExceptionReason },
                { "Patient Last Name", item => item.PatientLastName },
                { "Patient First Name", item => item.PatientFirstName },
                { "DOB", item => item.DateOfBirthString },
                { "Payer Name", item => item.InsuranceName },
                { "Policy Number", item => item.PolicyNumber },
                { "Date of Service", item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfService?.ToString("MM/dd/yyyy") ?? null) },
                { "Procedure Code", item => item.ProcedureCode },
                { "Billed Amount", item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { "Claim Number", item => item.OfficeClaimNumber },
                { "Provider Name", item => item.ProviderName },
                { "Exception Remark", item => item.ExceptionRemark },
                { "Exception Category", item => item.ClaimStatusExceptionReasonCategorId },
                { "Location Name", item => item.ClientLocationName },
                { "Location Npi", item => item.ClientLocationNpi },
           };
        }

        public async Task<List<EmployeeClient>> GetAllBillingManagersOrFollowUpEmployeessByClientId(int clientId, string tnantIdentifier)
        {
            try
            {
                var _employeeRepository = await _tenantRepositoryFactory.GetAsync<IEmployeeRepository>(tnantIdentifier);

                var employeeClients = await _employeeRepository.GetAllBillingManagersOrFollowUpEmployeessByClientId(clientId);
                return employeeClients;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// get approved claims by clientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<List<ApprovedClaimsDetailResponse>> GetApprovedClaimsByClientId(int clientId, string tenantIdentifier)
        {
            List<ApprovedClaimsDetailResponse> approvedClaims = new List<ApprovedClaimsDetailResponse>();
            try
            {
                var _claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenantIdentifier);

                if (_claimStatusBatchClaimsRepository == null)
                {
                    throw new Exception($"Cound not initialize ClaimStatusBatchClaimsRepository for tenantIdentifier: {tenantIdentifier}");
                }

                approvedClaims = await _claimStatusBatchClaimsRepository.ClaimStatusBatchClaims
                                    .Include(c => c.ClaimStatusTransaction)
                                    .Include(c => c.ClientInsurance)
                                    .Include(c => c.ClientLocation)
                                    .Include(c => c.ClientProvider)
                                        .ThenInclude(cp => cp.Person)
                                    .Where(x => x.ClaimStatusTransaction != null)
                                    .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                                    .Specify(new ClaimStatusBatchClaimNotDeletedSpecification())
                                    .Specify(new ApprovedForMoreThanSixDaysSpecification())
                                    .Specify(new Application.Specifications.GenericByClientIdSpecification<ClaimStatusBatchClaim>(clientId))
                                    .Select(c => new ApprovedClaimsDetailResponse()
                                    {
                                        PatientFirstName = c.PatientFirstName ?? string.Empty,
                                        PatientLastName = c.PatientLastName ?? string.Empty,
                                        DateOfBirth = c.DateOfBirth.Value,
                                        PolicyNumber = c.PolicyNumber ?? string.Empty,
                                        ClientInsuranceId = c.ClientInsuranceId,
                                        InsuranceName = c.ClientInsurance != null ? c.ClientInsurance.Name : string.Empty,
                                        OfficeClaimNumber = c.ClaimNumber ?? string.Empty,
                                        InsuranceClaimNumber = c.ClaimNumber ?? string.Empty,
                                        ApprovedSinceDate = c.ClaimStatusTransaction.LastModifiedOn.Value,
                                        ClaimBilledOn = c.ClaimBilledOn.Value,
                                        DateOfService = c.DateOfServiceFrom.Value,
                                        ProcedureCode = c.ProcedureCode ?? string.Empty,
                                        Modifiers = c.Modifiers ?? string.Empty,
                                        Quantity = c.Quantity,
                                        ProviderName = c.ClientProvider != null && c.ClientProvider.Person != null ? c.ClientProvider.Person.LastCommaFirstName ?? string.Empty : string.Empty,
                                        BilledAmount = c.BilledAmount ?? 0.00m,
                                        ClientLocationId = c.ClientLocationId ?? default(int),
                                        ClaimLineItemStatusId = c.ClaimStatusTransaction.ClaimLineItemStatusId ?? ClaimLineItemStatusEnum.Approved,
                                        ClientLocationName = c.ClientLocation != null ? c.ClientLocation.Name : string.Empty,
                                        ClientLocationNpi = c.ClientLocation != null ? c.ClientLocation.Npi : string.Empty
                                    })
                                    .ToListAsync() ?? new List<ApprovedClaimsDetailResponse>();
            }
            catch (Exception ex)
            {
                throw;
            }

            return approvedClaims;
        }

        public async Task<bool> DoSendYesterdayDenialClaimsToEmployees()
        {
            var success = true;
            List<Exception> exceptions = new List<Exception>(); 

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                var templateContent = GetEmailBodyTemplateForDoSendYesterdayDeinalClaimsToEmployees();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(templateContent);

                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);

                        // Retrieve all clients for the current tenant
                        var clients = await _clientRepository.GetAllActiveClients();
                        clients = RemoveNonProductionClients(clients);
                        if (clients != null && clients.Any())
                        {
                            foreach (var client in clients)
                            {
                                try
                                {
                                    List<EmployeeClientDto> employeeClients = new List<EmployeeClientDto>();
                                    employeeClients = await GetAllBillingManagersOrFollowUpEmployeesByClientId(client.Id, tenant.Identifier);
                                    var alldeniedClaimsForClientList = await _claimStatusQueryService.GetEmployeeClaimStatusDataAsync(client.Id, tenant.ConnectionString);


                                    foreach (var emp in employeeClients)
                                    {
                                        try
                                        {
                                            var employeeAssignedClaims = await GetFilteredEmployeeClaimStatusResponseModels(emp, alldeniedClaimsForClientList);

                                            // Handle cases where no assigned claims exist
                                            if (employeeAssignedClaims.Count() > 0)
                                            {
                                                string decryptedPin;
                                                if (emp?.Employee?.User != null)
                                                {
                                                    string userExistingPin = emp.Employee.User?.Pin ?? null;

                                                    if (string.IsNullOrEmpty(userExistingPin))
                                                    {
                                                        var userHelper = new UserHelper();
                                                        long? phoneNumber = null;
                                                        string phoneNumberString = emp.Employee.User?.PhoneNumber ?? null;

                                                        if (!string.IsNullOrEmpty(phoneNumberString))
                                                        {
                                                            if (long.TryParse(phoneNumberString, out long result))
                                                            {
                                                                phoneNumber = result;
                                                            }
                                                        }

                                                        decryptedPin = userHelper.GeneratePin(emp.Employee.User.FirstName, phoneNumber);
                                                    }
                                                    else
                                                    {
                                                        decryptedPin = PinExtensions.DecryptPin(emp.Employee.User.Pin);
                                                    }
                                                    string body = GetEmployeeStatusByDenialCategoriesEmailBody(employeeAssignedClaims, template);

                                                    string base64MultiExportContent = await GetEmployeeDenialCategoriesExportContent(employeeAssignedClaims, decryptedPin);

                                                    MailRequestWithAttachment request = new()
                                                    {
                                                        To = emp.Employee.User.Email,
                                                        Body = body,
                                                        Subject = $"{emp.Employee.User.LastName}_Yesterday's Denials - {client.ClientCode} - {tenant.TenantName ?? tenant.Identifier}",
                                                        FileName = $"{emp.Employee.User.LastName}_Yesterdays_Denials_{DateTime.Now:MM-dd-yyyy HH:mm:ss}.xlsx",
                                                        Base64Content = base64MultiExportContent
                                                    };
                                                    await _mailService.SendAsync(request);
                                                }
                                            }

                                        }
                                        catch (Exception empEx)
                                        {
                                            _logger.LogError($"Exception processing employee {emp.Employee.UserId} for client {client.ClientCode} in tenant {tenant.Identifier}. Error - {empEx.Message}");
                                            exceptions.Add(new Exception($"Employee: {emp.Employee.UserId}, Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {empEx.Message}", empEx));
                                            success = false;
                                        }
                                    }
                                }
                                catch (Exception clientEx)
                                {
                                    _logger.LogError($"Exception processing client {client.ClientCode} for tenant {tenant.Identifier}. Error - {clientEx.Message}");
                                    exceptions.Add(new Exception($"Client: {client.ClientCode}, Tenant: {tenant.Identifier}, Error: {clientEx.Message}", clientEx));
                                    success = false;
                                }
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendYesterdayDenialClaimsToEmployees'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'DoSendYesterdayDenialClaimsToEmployees'. Error - {ex.Message}");
                exceptions.Add(new Exception($"General error: {ex.Message}", ex));
                throw new AggregateException("Failed to execute 'DoSendYesterdayDenialClaimsToEmployees'.", ex);
            }

            return success;
        }


        #endregion

        #region EN-131
        private string GetEmailBodyTemplateForExceedingDenial()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\ExcedingDenial.template");
        }

        /// <summary>
        /// Checks the percentage threshold of unavailable and not-on-file claims for each tenant and sends reports to specified email addresses if the threshold is exceeded.
        /// </summary>
        /// <returns>
        /// A boolean indicating the success of the operation. Returns true if the reports are successfully sent; otherwise, false.
        /// </returns>
        public async Task<bool> CheckStatusPercentageThreshold()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                var templateContent = GetEmailBodyTemplateForExceedingDenial();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(templateContent);

                // Retrieve all tenants
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                foreach (var tenant in tenants ?? Enumerable.Empty<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
                        var claimStatusBatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);
                        var claimStatusBatchClaimsRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenant.Identifier);
                        var claimStatusTransactionRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionRepository>(tenant.Identifier);

                        // Get batches created in the last 30 Days that have claims in Statuses: (Unavailable or NotOnFile)
                        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                        var claimStatusBatchData = await _unitOfWork.Repository<ClaimStatusBatch>().Entities
                                                        .Include(x => x.ClientInsurance)
                                                            .ThenInclude(y => y.RpaInsurance)
                                                        .Include(x => x.Client)
                                                        .Include(x => x.ClaimStatusBatchClaims)
                                                            .ThenInclude(x => x.ClaimStatusTransaction)
                                                        .Where(x => x.CreatedOn > thirtyDaysAgo
                                                                && x.ClaimStatusBatchClaims
                                                                    .Any(y => y.ClaimStatusTransaction != null
                                                                        && (y.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Unavailable
                                                                            || y.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.NotOnFile)))
                                                        .OrderByDescending(x => x.CreatedOn)
                                                        .ToListAsync() ?? new List<ClaimStatusBatch>();

                        List<CheckStatusPercentageThresholdResponse> thresholdList = new List<CheckStatusPercentageThresholdResponse>();
                        foreach (var batch in claimStatusBatchData)
                        {
                            var claimCount = batch.ClaimStatusBatchClaims.Count();
                            var unavailableCount = batch.ClaimStatusBatchClaims.Where(c => c.ClaimStatusTransaction != null && c.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Unavailable).Count();
                            var notOnFileCount = batch.ClaimStatusBatchClaims.Where(c => c.ClaimStatusTransaction != null && c.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.NotOnFile).Count();
                            var unavailableOrNotOnFileCount = unavailableCount + notOnFileCount;
                            var filterValue = ((double)unavailableOrNotOnFileCount / claimCount);

                            if (filterValue > 0.05)
                            {
                                thresholdList.Add(new CheckStatusPercentageThresholdResponse()
                                {
                                    BatchId = batch.Id,
                                    BatchNumber = batch.BatchNumber ?? "",
                                    ClientCode = batch.Client?.ClientCode ?? "",
                                    RpaInsuranceCode = batch.ClientInsurance?.RpaInsurance?.Name ?? "",
                                    PayerName = batch.ClientInsurance?.Name ?? "",
                                    CreatedOn = batch.CreatedOn.ToString() ?? "",
                                    BatchClaimCount = claimCount.ToString(),
                                    UnavailableCount = unavailableCount,
                                    NotFoundCount = notOnFileCount,
                                    CountNotFoundUnavailable = unavailableOrNotOnFileCount,
                                    UnavailablePercentage = ((double)unavailableCount / claimCount).ToString("p"),
                                    NotFoundPercentage = ((double)notOnFileCount / claimCount).ToString("p"),
                                    PercentNotFoundUnavailable = ((double)unavailableOrNotOnFileCount / claimCount).ToString("p"),
                                    FilterValue = ((double)unavailableOrNotOnFileCount / claimCount)
                                });
                            }
                        }

                        if (thresholdList.Any())
                        {
                            string[] emailToAddresses = new string[]
                            {
                        "cknight@medhelpinc.com",
                        "cknight@automatedintegrationtechnologies.com",
                        "jamesnichols@automatedintegrationtechnologies.com",
                        "jamesnichols@medhelpinc.com",
                        "kmccaffery@automatedintegrationtechnologies.com",
                        "Mohit@automatedintegrationtechnologies.com"
                            };

                            string body = template.Run(new
                            {
                                NotFoundUnavailableClaims = thresholdList
                            });

                            string subject = $"Claim Not Found Threshold Report - Tenant: {tenant.TenantName ?? tenant.Identifier}";

                            foreach (string email in emailToAddresses)
                            {
                                MailRequestWithAttachment request = new MailRequestWithAttachment()
                                {
                                    To = email,
                                    Body = body,
                                    Subject = subject,
                                    Base64Content = await GetNotFoundUnavailableClaimsExcelDataAsync(thresholdList),
                                    FileName = "NotFoundUnavailableClaimsReport"
                                };
                                await _mailService.SendAsync(request);
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Exception processing tenant {tenant.Identifier}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'CheckStatusPercentageThreshold'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General exception in 'CheckStatusPercentageThreshold'. Error - {ex.Message}");
                exceptions.Add(new Exception($"General error: {ex.Message}", ex));
                throw new AggregateException("Failed to execute 'CheckStatusPercentageThreshold'.", ex);
            }

            return success;
        }


        private async Task<string> GetNotFoundUnavailableClaimsExcelDataAsync(List<CheckStatusPercentageThresholdResponse> notFoundUnavailableClaimsData)
        {
            var exportResponse = notFoundUnavailableClaimsData.Select(item => new ExportQueryResponse
            {
                ClientCode = item.ClientCode,
                BatchId = item.BatchId,
                BatchNumber = item.BatchNumber,
                CreatedOnString = item.CreatedOn,
                PayerName = item.PayerName,
                RpaInsuranceCode = item.RpaInsuranceCode,
                BatchClaimCount = item.BatchClaimCount,
                UnavailableCount = item.UnavailableCount,
                UnavailablePercentage = item.UnavailablePercentage,
                NotFoundCount = item.NotFoundCount,
                NotFoundPercentage = item.NotFoundPercentage,
                CountNotFoundUnavailable = item.CountNotFoundUnavailable,
                PercentNotFoundUnavailable = item.PercentNotFoundUnavailable,
            });

            var data = await _excelService.ExportReportAsync(exportResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { "Client Code", item => item.ClientCode },
                { "BatchId", item => item.BatchId.ToString() },
                { "Batch Number", item => item.BatchNumber },
                { "Created On", item => item.CreatedOnString },
                { "PayerName", item => item.PayerName },
                { "RpaInsuranceCode", item => item.RpaInsuranceCode },
                { "BatchClaim Count", item => item.BatchClaimCount.ToString() },
                { "Unavailable Count", item => item.UnavailableCount.ToString() },
                { "Unavailable Percentage", item => item.UnavailablePercentage },
                { "NotOnFile Count", item => item.NotFoundCount.ToString() },
                { "NotOnFile Percentage", item => item.NotFoundPercentage },
                { "CountNotFoundUnavailable", item => item.CountNotFoundUnavailable },
                { "PercentNotFoundUnavailable", item => item.PercentNotFoundUnavailable },

            }, sheetName: "NotFoundUnavailableClaimsReport");

            return data;
        }

        #endregion

        #region EN-359

        /// <summary>
        /// Retrieves information about failed Hangfire jobs and sends an email notification to designated recipients.
        /// </summary>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// </returns>
        public async Task<bool> RetrieveFailedJobs()
        {
            var success = true;
            var failedJobsList = new List<FailedHangfireJobs>(); // List to store failed job models

            try
            {
                var storage = JobStorage.Current;
                var monitoringApi = storage.GetMonitoringApi();
                var failedJobs = monitoringApi.FailedJobs(0, int.MaxValue);

                foreach (var failedJob in failedJobs)
                {
                    // Create a new FailedHangfireJobs instance for each failed job
                    var failedJobModel = new FailedHangfireJobs
                    {
                        JobId = failedJob.Key,
                        JobType = failedJob.Value.Job.ToString(),
                        FailedAt = failedJob.Value.FailedAt.ToString(),
                        Exception = failedJob.Value.ExceptionMessage
                    };

                    failedJobsList.Add(failedJobModel); // Add the model to the list
                }

                var viewModel = new FailedHangfireJobsViewModel
                {
                    Greeting = GreetingHelpers.GetAppropriateGreetingString(), // Add your greeting message here
                    FailedHangfireJobs = failedJobsList // Set the list of failed jobs in the view model
                };

                if (viewModel.FailedHangfireJobs.Count > 0)
                {
                    var path = "/Views/Emails/FailedHangfireJobs/FailedHangfireJobEmailTemplate.cshtml";
                    string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, viewModel);

                    string[] emailToAddresses = new string[]
                    {
                        "cknight@medhelpinc.com",
                        "cknight@automatedintegrationtechnologies.com",
                        "jamesnichols@automatedintegrationtechnologies.com",
                        "jamesnichols@medhelpinc.com",
                        "kmccaffery@automatedintegrationtechnologies.com",
                        "mohit@automatedintegrationtechnologies.com"
                    };

                    foreach (string email in emailToAddresses)
                    {
                        var mailRequest = new MailRequest()
                        {
                            To = email,
                            Body = body ?? string.Empty,
                            Subject = "Hangfire Jobs Failed List",
                        };
                        await _mailService.SendAsync(mailRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                var failedJobModel = new FailedHangfireJobs
                {
                    JobId = "Failed Jobs",
                    JobType = "RetrieveFailedJobs",
                    FailedAt = DateTime.UtcNow.ToString(),
                    Exception = ex.Message
                };

                failedJobsList.Add(failedJobModel);
                await ReportFailure(failedJobsList); // Report failure with details
            }

            return success;
        }

        private async Task ReportFailure(List<FailedHangfireJobs> failedJobsList)
        {
            // Create and send a failure report email
            var viewModel = new FailedHangfireJobsViewModel
            {
                Greeting = GreetingHelpers.GetAppropriateGreetingString(), // Add your greeting message here
                FailedHangfireJobs = failedJobsList // Set the list of failed jobs in the view model
            };

            var path = "/Views/Emails/FailedHangfireJobs/FailedHangfireJobEmailTemplate.cshtml";
            string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, viewModel);

            string[] emailToAddresses = new string[]
            {
                "cknight@medhelpinc.com",
                "cknight@automatedintegrationtechnologies.com",
                "jamesnichols@automatedintegrationtechnologies.com",
                "jamesnichols@medhelpinc.com",
                "kmccaffery@automatedintegrationtechnologies.com",
                "mohit@automatedintegrationtechnologies.com"
            };

            foreach (string email in emailToAddresses)
            {
                var mailRequest = new MailRequest()
                {
                    To = email,
                    Body = body ?? string.Empty,
                    Subject = "Hangfire Jobs Failed List",
                };
                await _mailService.SendAsync(mailRequest);
            }
        }
        #endregion

        #region in process claims report
        // GET EMAIL TEMPLATE FOR IN PROCESS CLAAIMS
        private string GetEmailBodyTemplateForInProcessClaims()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(buildDir + "\\Templates\\InProcessClaimsReport.template");
        }
        public async Task<bool> DoSendInProcessClaimsReport()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetEmailBodyTemplateForInProcessClaims());

                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        // Retrieve in-process details
                        var inProcessDetails = await _claimStatusQueryService.GetInProcessClaimsReportAsync(tenant);

                        string subject = $"In Process Claims! - Tenant: {tenant.TenantName ?? tenant.Identifier}";

                        if (inProcessDetails.Any())
                        {
                            string[] defaultEmailToAddresses = new string[]
                            {
                                "cknight@medhelpinc.com",
                                "cknight@automatedintegrationtechnologies.com",
                                "jamesnichols@automatedintegrationtechnologies.com",
                                "jamesnichols@medhelpinc.com",
                                "kmccaffery@automatedintegrationtechnologies.com",
                                "mohit@automatedintegrationtechnologies.com"
                            };

                            var emailBody = new
                            {
                                InProcessReport = inProcessDetails.ToList()
                            };
                            string body = template.Run(emailBody);
                            foreach (string email in defaultEmailToAddresses)
                            {
                                MailRequestWithAttachment request = new MailRequestWithAttachment()
                                {
                                    To = email,
                                    Body = body,
                                    Subject = subject,
                                    Base64Content = await GetInProcessClaimsExcelDataAsync(inProcessDetails),
                                    FileName = "InProcess_Claims"
                                };
                                await _mailService.SendAsync(request);
                            }
                        }
                    }
                    catch (Exception tenantEx)
                    {
                        _logger.LogError($"Failed to send in-process claims report for tenant {tenant.TenantName}. Error - {tenantEx.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.TenantName}, Error: {tenantEx.Message}", tenantEx));
                        success = false;
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'DoSendInProcessClaimsReport'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception sending mail on nightly job. Error - " + ex.Message);
                exceptions.Add(ex);
                success = false;
                throw;
            }

            return success;
        }

        #endregion


        #region Month Close services
        public async Task<bool> FillMonthCashCollection()
        {
            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                // Check if tenants is null or empty
                if (tenants == null || !tenants.Any())
                {
                    // Optionally log or handle the case where there are no tenants
                    return false; // No active tenants
                }

                foreach (var tenant in tenants)
                {
                    // Check for a valid connection string
                    if (string.IsNullOrEmpty(tenant?.ConnectionString))
                    {
                        // Optionally log or handle the case where the connection string is null or empty
                        continue; // Skip to the next tenant
                    }

                    SqlConnection conn = null;
                    try
                    {
                        conn = new SqlConnection(tenant.ConnectionString);

                        // Open connection
                        await conn.OpenAsync();

                        // Ensure that the stored procedure exists before attempting execution
                        SqlCommand cmd = new SqlCommand("spGetMonthlyCashCollectionData", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        // Execute stored procedure and handle any errors gracefully
                        await cmd.ExecuteReaderAsync();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log SQL exceptions (e.g., issues with connection or command execution)
                        // Example: _logger.LogError(sqlEx, "SQL Error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    catch (Exception ex)
                    {
                        // Log any other types of exceptions
                        // Example: _logger.LogError(ex, "Unexpected error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    finally
                    {
                        // Close the connection if it's not already closed
                        if (conn?.State == ConnectionState.Open)
                        {
                            await conn.CloseAsync();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the error, if needed
                // _logger.LogError(ex, "Error occurred while processing monthly cash collection.");
                return false;
            }
        }

        public async Task<bool> FillMonthlyDenialData()
        {
            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                // Check if tenants is null or empty
                if (tenants == null || !tenants.Any())
                {
                    // Optionally log or handle the case where there are no tenants
                    return false; // No active tenants
                }

                foreach (var tenant in tenants)
                {
                    // Check for a valid connection string
                    if (string.IsNullOrEmpty(tenant?.ConnectionString))
                    {
                        // Optionally log or handle the case where the connection string is null or empty
                        continue; // Skip to the next tenant
                    }

                    SqlConnection conn = null;
                    try
                    {
                        conn = new SqlConnection(tenant.ConnectionString);

                        // Open connection
                        await conn.OpenAsync();

                        // Ensure that the stored procedure exists before attempting execution
                        SqlCommand cmd = new SqlCommand("spGetMonthlyDenialData", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        // Execute stored procedure and handle any errors gracefully
                        await cmd.ExecuteReaderAsync();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log SQL exceptions (e.g., issues with connection or command execution)
                        // Example: _logger.LogError(sqlEx, "SQL Error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    catch (Exception ex)
                    {
                        // Log any other types of exceptions
                        // Example: _logger.LogError(ex, "Unexpected error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    finally
                    {
                        // Close the connection if it's not already closed
                        if (conn?.State == ConnectionState.Open)
                        {
                            await conn.CloseAsync();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the error, if needed
                // _logger.LogError(ex, "Error occurred while processing monthly cash collection.");
                return false;
            }
        }

        public async Task<bool> FillGetMonthlyReceivablesData()
        {
            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                // Check if tenants is null or empty
                if (tenants == null || !tenants.Any())
                {
                    // Optionally log or handle the case where there are no tenants
                    return false; // No active tenants
                }

                foreach (var tenant in tenants)
                {
                    // Check for a valid connection string
                    if (string.IsNullOrEmpty(tenant?.ConnectionString))
                    {
                        // Optionally log or handle the case where the connection string is null or empty
                        continue; // Skip to the next tenant
                    }

                    SqlConnection conn = null;
                    try
                    {
                        conn = new SqlConnection(tenant.ConnectionString);

                        // Open connection
                        await conn.OpenAsync();

                        // Ensure that the stored procedure exists before attempting execution
                        SqlCommand cmd = new SqlCommand("spGetMonthlyReceivablesData", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        // Execute stored procedure and handle any errors gracefully
                        await cmd.ExecuteReaderAsync();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log SQL exceptions (e.g., issues with connection or command execution)
                        // Example: _logger.LogError(sqlEx, "SQL Error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    catch (Exception ex)
                    {
                        // Log any other types of exceptions
                        // Example: _logger.LogError(ex, "Unexpected error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    finally
                    {
                        // Close the connection if it's not already closed
                        if (conn?.State == ConnectionState.Open)
                        {
                            await conn.CloseAsync();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the error, if needed
                // _logger.LogError(ex, "Error occurred while processing monthly cash collection.");
                return false;
            }
        }

        public async Task<bool> FillGetMonthlyARData()
        {
            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                // Check if tenants is null or empty
                if (tenants == null || !tenants.Any())
                {
                    // Optionally log or handle the case where there are no tenants
                    return false; // No active tenants
                }

                foreach (var tenant in tenants)
                {
                    // Check for a valid connection string
                    if (string.IsNullOrEmpty(tenant?.ConnectionString))
                    {
                        // Optionally log or handle the case where the connection string is null or empty
                        continue; // Skip to the next tenant
                    }

                    SqlConnection conn = null;
                    try
                    {
                        conn = new SqlConnection(tenant.ConnectionString);

                        // Open connection
                        await conn.OpenAsync();

                        // Ensure that the stored procedure exists before attempting execution
                        SqlCommand cmd = new SqlCommand("spGetMonthlyARData", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        // Execute stored procedure and handle any errors gracefully
                        await cmd.ExecuteReaderAsync();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log SQL exceptions (e.g., issues with connection or command execution)
                        // Example: _logger.LogError(sqlEx, "SQL Error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    catch (Exception ex)
                    {
                        // Log any other types of exceptions
                        // Example: _logger.LogError(ex, "Unexpected error occurred while processing tenant {TenantId}", tenant?.Id);
                    }
                    finally
                    {
                        // Close the connection if it's not already closed
                        if (conn?.State == ConnectionState.Open)
                        {
                            await conn.CloseAsync();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the error, if needed
                // _logger.LogError(ex, "Error occurred while processing monthly cash collection.");
                return false;
            }
        }

        #endregion

        public async Task<bool> UpdateClaimStatusExceptionReasonCategory()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetEmailBodyTemplateForInProcessClaims());

                var tenants = await _tenantManagementService.GetAllActiveAsync();
                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    if (tenant.IsProductionTenant)
                    {
                        try
                        {
                            await Task.Run(() => _claimStatusQueryService.UpdateClaimStatusExceptionReasonCategoryForTenant(tenant));
                        }
                        catch (Exception tenantEx)
                        {
                            _logger.LogError($"Failed to UpdateClaimStatusExceptionReasonCategory for tenant {tenant.TenantName}. Error - {tenantEx.Message}");
                            exceptions.Add(new Exception($"Tenant: {tenant.TenantName}, Error: {tenantEx.Message}", tenantEx));
                            success = false;
                        }
                    }
                }
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'UpdateClaimStatusExceptionReasonCategory'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception on nightly job. Error - " + ex.Message);
                exceptions.Add(ex);
                success = false;
                throw;
            }

            return success;
        }

        public async Task<bool> GetMonthlyDaysInARorAllTenantsAsync()
        {
            List<Exception> exceptions = new List<Exception>();
            bool success = true;

            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                foreach (var tenant in tenants ?? Enumerable.Empty<Application.Multitenancy.TenantDto>())
                {
                    try
                    {
                        string connStr = tenant.ConnectionString;
                        using (SqlConnection conn = new SqlConnection(connStr))
                        {
                            try
                            {
                                // Open the connection
                                await conn.OpenAsync();

                                // Create the command for executing the stored procedure (no parameters)
                                SqlCommand cmd = CreateCurrentARSpCommand(StoreProcedureTitle.spGetMonthlyDaysInAR, conn);

                                // Execute the stored procedure without expecting a return value
                                await cmd.ExecuteNonQueryAsync();
                            }
                            catch (Exception tenantEx)
                            {
                                // Log the exception and add it to the collection
                                _logger.LogError($"Failed to execute stored procedure for tenant {tenant.Identifier}. Error: {tenantEx.Message}");
                                exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {tenantEx.Message}", tenantEx));
                                success = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and add it to the collection
                        _logger.LogError($"Error for tenant {tenant.Identifier}: {ex.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {ex.Message}", ex));
                        success = false;
                    }
                }

                // If any exceptions were encountered, throw an aggregate exception
                if (exceptions.Any())
                {
                    throw new AggregateException("One or more errors occurred during the execution of 'GetMonthlyDaysInARorAllTenantsAsync'.", exceptions);
                }
            }
            catch (Exception ex)
            {
                // Log or handle global exceptions
                _logger.LogError($"Exception on GetMonthlyDaysInARorAllTenantsAsync. Error: {ex.Message}");
                throw;
            }

            return success;
        }


        private SqlCommand CreateCurrentARSpCommand(string spName, SqlConnection conn)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn)
                {
                    // Set the command type to stored procedure.
                    CommandType = CommandType.StoredProcedure
                };

                return cmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateOutstandingBalancesForAllTenantsAsync()
        {
            List<Exception> exceptions = new List<Exception>();
            bool isSuccess = true;

            try
            {
                // Retrieve all active tenants
                var activeTenants = await _tenantManagementService.GetAllActiveAsync();

                foreach (var tenant in activeTenants ?? Enumerable.Empty<Application.Multitenancy.TenantDto>())
                {
                    var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
                    var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
                    try
                    {
                        var clients = await _clientRepository.GetAllActiveClients();
                         clients = RemoveNonProductionClients(clients);

                        foreach (var client in clients)
                        {
                            string connStr = tenant.ConnectionString ?? _configuration.GetConnectionString("DefaultConnection");

                            // Initialize SQL connection for the current tenant
                            using (SqlConnection conn = new SqlConnection(connStr))
                            {
                                await conn.OpenAsync();

                                // Create the stored procedure command for UpdateOutstandingBalance
                                SqlCommand cmd = new SqlCommand(StoreProcedureTitle.spUpdateOutstandingBalance, conn)
                                {
                                    CommandType = CommandType.StoredProcedure
                                };

                                // Add the ClientId parameter
                                cmd.Parameters.AddWithValue("@ClientId", client.Id);

                                // Execute the stored procedure
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log tenant-specific errors
                        _logger.LogError($"Error processing tenant {tenant.Identifier}: {ex.Message}");
                        exceptions.Add(new Exception($"Tenant: {tenant.Identifier}, Error: {ex.Message}", ex));
                        isSuccess = false;
                    }
                }

                // If exceptions occurred, throw an aggregate exception
                if (exceptions.Any())
                {
                    throw new AggregateException("Errors occurred while updating outstanding balances for tenants.", exceptions);
                }
            }
            catch (Exception ex)
            {
                // Log global exceptions
                _logger.LogError($"Exception in UpdateOutstandingBalancesForAllTenantsAsync: {ex.Message}");
                throw;
            }

            return isSuccess;
        }


    }
}