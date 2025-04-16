using AutoMapper;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.Client_ApplicationFeatures;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClaimStatus.Queries.GetEmployeeClaimStatusByEmployeeID;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.Dashboards.GetClaimsData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetClaimStatusTotalReort;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Common;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Services.JobsManager;
using MedHelpAuthorizations.Shared.Extensions;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Shared.Models.Exports;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class ClaimStatusQueryService : IClaimStatusQueryService
    {
        private readonly ApplicationContext _context;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;
        private IUnitOfWork<int> _unitOfWork;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IStringLocalizer<ClaimStatusQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITenantInfo _tenantInfo;
        private readonly IExcelService _excelService;
        private readonly IJobCronManager _jobCronManager;

        private int _clientId => _currentUserService.ClientId;

        public ClaimStatusQueryService(
            ApplicationContext context,
            IDbContextFactory<ApplicationContext> contextFactory,
            IUnitOfWork<int> unitOfWork,
            IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository,
            IMapper mapper,
            IStringLocalizer<ClaimStatusQueryService> localizer,
            ICurrentUserService currentUserService,
            IConfiguration configuration,
            ITenantInfo tenantInfo,
            ITenantRepositoryFactory tenantRepositoryFactory,
            IExcelService excelService,
            IJobCronManager jobCronManager)
        {
            _context = context;
            _contextFactory = contextFactory;
            _unitOfWork = unitOfWork;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _mapper = mapper;
            _configuration = configuration;
            _tenantInfo = tenantInfo;
            _localizer = localizer;
            this._currentUserService = currentUserService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _excelService = excelService;
            _jobCronManager = jobCronManager;
        }

        private async Task<List<ClaimStatusTotal>> ExecuteClaimsTotalsSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ClaimStatusTotal> claimStatusTotals = new List<ClaimStatusTotal>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var claimStatusUploadTotal = new ClaimStatusTotal();
                        claimStatusUploadTotal.AllowedAmountSum = reader[StoredProcedureColumnsHelper.AllowedAmountSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmountSum];
                        claimStatusUploadTotal.NonAllowedAmountSum =
                            reader[StoredProcedureColumnsHelper.NonAllowedAmountSum] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.NonAllowedAmountSum];
                        ;
                        claimStatusUploadTotal.ChargedSum = reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum];
                        claimStatusUploadTotal.PaidAmountSum = reader[StoredProcedureColumnsHelper.PaidAmountSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.PaidAmountSum];
                        claimStatusUploadTotal.Quantity =
                            reader[StoredProcedureColumnsHelper.Quantity] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.Quantity];
                        claimStatusUploadTotal.WriteOffAmountSum = reader[StoredProcedureColumnsHelper.WriteOffAmountSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.WriteOffAmountSum];
                        claimStatusUploadTotal.ClaimLineItemStatusId =
                            reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId] == DBNull.Value ? default(int?) : (int?)reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId]; //AA-317

                        claimStatusUploadTotal.ClaimLevelMd5Hash = reader[StoredProcedureColumnsHelper.ClaimLevelMd5Hash] as string;

                        claimStatusTotals.Add(claimStatusUploadTotal);
                    }

                    reader.Close();
                }

                return claimStatusTotals;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<ClaimStatusTotal>> GetClaimsStatusUploadedTotalsAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null)
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }

            List<ClaimStatusTotal> claimStatusTotalsTask = new();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimStatusTotalsTask, conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId);
                    var claimStatusotalsTask = ExecuteClaimsTotalsSpCommand(cmd);

                    await Task.WhenAll(claimStatusotalsTask);
                    claimStatusTotalsTask = claimStatusotalsTask.Result;

                }
                return claimStatusTotalsTask;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// To get the data for the dashboard
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="clientId"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public async Task<ClaimStatusDashboardResponse> GetClaimsStatusTotalsAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null)
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }
            if (string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }
            ClaimStatusDashboardResponse claimStatusDashboardData = new ClaimStatusDashboardResponse()
            {
                ClaimStatusUploadedTotals = new List<ClaimStatusTotal>(),
                ClaimStatusDashboardData = new ClaimStatusDashboardData()
            };

            try
            {
                //Get all
                //Task<List<ClaimStatusTotal>> ClaimStatusUploadedTotals = GetClaimsStatusUploadedTotalsAsync(filters, clientId, connStr);
                ClaimStatusDashboardData claimStatusDashboardTilesData = await GetClaimStatusDashbaordDataAsync(filters, connStr);
                //Wait for all asynchronous tasks to complete;
                //await Task.WhenAll(/*ClaimStatusUploadedTotals,*/ claimStatusDashboardTilesData);

                //claimStatusDashboardData.ClaimStatusUploadedTotals = ClaimStatusUploadedTotals.Result;
                claimStatusDashboardData.ClaimStatusDashboardData = claimStatusDashboardTilesData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return claimStatusDashboardData;
        }

        public async Task<List<ExportQueryResponse>> GetClaimsStatusDetailsAsync(IClaimStatusDashboardDetailsQuery filters, int clientId, string conn)
        {
            try
            {
                //if (!string.IsNullOrWhiteSpace(filters.CommaDelimitedLineItemStatusIds))
                //{
                //    filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName(filters.FlattenedLineItemStatus);
                //}

                //.GroupBy(i => new { i.ClaimStatusBatchClaim.ClaimLevelMd5Hash})
                var claimsStatusDetails = await GetFilteredReportAsync(filters, clientId, connStr: conn);


                return claimsStatusDetails;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //private DateTime? GetClaimStatusTransactionHistoryLastCreationDate(ICollection<ClaimStatusTransactionHistory> claimStatusTransactionHistories, int id)
        //{
        //    return claimStatusTransactionHistories.Where(h => h.ClaimStatusTransactionId == id)?.OrderByDescending(o => o.CreatedOn)?.FirstOrDefault().CreatedOn ?? null;
        //}

        public async Task<List<ExportQueryResponse>> GetDenialDetailsAsync(IClaimStatusDashboardDetailsQuery filters, int clientId = 0, string connStr = "")
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }
            if (string.IsNullOrEmpty(connStr) || string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }
            List<ExportQueryResponse> response = new();

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new(connStr);

            filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName("Denied");

            ///We no longer need bewlo code as we are filter data based on statusTypeinstead of status Ids.

            //if (string.IsNullOrWhiteSpace(filters.CommaDelimitedLineItemStatusIds))
            //{
            //    filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName((string.IsNullOrEmpty(filters.FlattenedLineItemStatus) ? "Denied" : filters.FlattenedLineItemStatus));
            //}

            string dashboardType = null;
            if (!string.IsNullOrEmpty(filters.DashboardType)) { dashboardType = filters.DashboardType; }

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spDynamicExportDashboardQuery,
                        conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds,
                        filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds,
                        filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom,
                        filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom,
                        filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId,
                        filters.ClaimStatusBatchId, claimStatusType: (int?)filters.ClaimStatusType,
                        claimStatusTypeStatus: filters.ClaimStatusTypeValue, dashboardType: dashboardType);

                    var claimStatusDashboardInProcessDetailsResponse = await ExecuteClaimDynamicExportSpCommand(cmd);

                    response = claimStatusDashboardInProcessDetailsResponse ?? new();

                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }


        }

        public async Task<List<ExportQueryResponse>> GetInProcessDetailsAsync(IClaimStatusDashboardDetailsQuery filters, int clientId, string conn)
        {
            try
            {

                var claimsStatusDetailsData = await GetFilteredInProcessDetailsReportAsync(filters, clientId: clientId, conn);

                return claimsStatusDetailsData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ClaimStatusDashboardInProcessDetailsResponse>> GetUncheckedClaimsDetailsAsync(int clientId, string tenantIdentifier = null)
        {

            if (!string.IsNullOrWhiteSpace(tenantIdentifier))
            {
                _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
            }
            var claimsStatusDetailsData = await _unitOfWork.Repository<ClaimStatusBatchClaim>().Entities
               .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
               .Specify(new ClaimStatusBatchClaimNotDeletedSpecification())
               .Specify(new ClaimStatusUncheckedClaimsSpecification())
               .Specify(new GenericByClientIdSpecification<ClaimStatusBatchClaim>(clientId))
               .Select(c => new ClaimStatusDashboardInProcessDetailsResponse()
               {
                   PatientLastName = c.Patient.Person.LastName,
                   PatientFirstName = c.Patient.Person.FirstName,
                   DateOfBirth = c.Patient.Person.DateOfBirth != null ? c.Patient.Person.DateOfBirth.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                   PolicyNumber = c.PolicyNumber,
                   ServiceType = c.ClaimStatusBatch.AuthType.Name,
                   PayerName = c.ClaimStatusBatch.ClientInsurance.LookupName,
                   OfficeClaimNumber = c.ClaimNumber,
                   ProcedureCode = c.ProcedureCode,
                   DateOfServiceFrom = c.DateOfServiceFrom != null ? c.DateOfServiceFrom.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                   ClaimBilledOn = c.ClaimBilledOn != null ? c.ClaimBilledOn.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                   BilledAmount = c.BilledAmount ?? 0.00m,
                   BatchNumber = c.ClaimStatusBatch.BatchNumber,
                   AitClaimReceivedDate = c.CreatedOn.ToLocalTime().ToShortDateString(),
                   AitClaimReceivedTime = c.CreatedOn.ToLocalTime().ToShortTimeString(),
                   ClientLocationName = c.ClientLocation.Name,
                   ClientLocationNpi = c.ClientLocation.Npi
               })
               .OrderBy(c => c.PayerName)
               .ToListAsync();

            return claimsStatusDetailsData;
        }

        public async Task<List<ClaimStatusDaysWaitLapsedDetailResponse>> GetDaysWaitLapsedByClientIdAsync(int clientId, string tenantIdentifier = null)
        {
            if (!string.IsNullOrWhiteSpace(tenantIdentifier))
            {
                _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
            }
            var claimsStatusDetailsData = await _unitOfWork.Repository<ClaimStatusBatchClaim>().Entities
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(t => t.ClaimStatusTransactionLineItemStatusChangẹ)
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(cs => cs.ClaimLineItemStatus)
                .Include(c => c.Patient)
                    .ThenInclude(p => p.Person)
                .Specify(new ApprovedClaimLineItemStatusWaitPeriodSpecification())
                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                .Specify(new ClaimStatusNotDeletedSpecification())
                .Specify(new GenericByClientIdSpecification<ClaimStatusBatchClaim>(clientId))
                .Specify(new ClaimStatusClaimBilledOnQualificationFilterSpecification())
                .Specify(new ClaimStatusMaxDaysPipelineQualificationFilterSpecification())
                //.Specify(new ClaimStatusAttemptsQualificationFilterSpecification())
                .Specify(new ClaimStatusDaysBetweenAttemptsQualificationFilterSpecification())
                .Specify(new ClaimStatusDaysWaitLapsedFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedWrongPayerFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedPolicyNumberFilterSpecification())
                .Select(c => new ClaimStatusDaysWaitLapsedDetailResponse()
                {
                    ClaimStatusTransactionId = c.ClaimStatusTransactionId,
                    ClaimStatusTransactionLineItemStatusChangẹId = c.ClaimStatusTransaction != null ? c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId : null,
                    PatientLastName = c.Patient.Person.LastName,
                    PatientFirstName = c.Patient.Person.FirstName,
                    DateOfBirth = c.Patient.Person.DateOfBirth != null ? c.Patient.Person.DateOfBirth.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                    PolicyNumber = c.PolicyNumber,
                    ServiceType = c.ClaimStatusBatch.AuthType.Name,
                    PayerName = c.ClaimStatusBatch.ClientInsurance.LookupName,
                    OfficeClaimNumber = c.ClaimNumber,
                    ProcedureCode = c.ProcedureCode,
                    DateOfServiceFrom = c.DateOfServiceFrom != null ? c.DateOfServiceFrom.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                    ClaimBilledOn = c.ClaimBilledOn != null ? c.ClaimBilledOn.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                    BilledAmount = c.BilledAmount ?? 0.00m,
                    ClaimStatusBatchId = c.ClaimStatusBatchId,
                    BatchNumber = c.ClaimStatusBatch.BatchNumber,
                    ClaimLineItemStatus = GetClaimLineItemStatusString(c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatusId),
                    StatusLastCheckedOn = c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn == null
                                            ? c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.CreatedOn.ToString(ClaimFiltersHelpers._dateFormat)
                                            : c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.Value.ToString(ClaimFiltersHelpers._dateFormat),
                    DaysLapsed = String.Format("{0:0.##}", (DateTime.UtcNow - (c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn == null
                                            ? c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.CreatedOn
                                            : c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.Value)
                                            .AddDays(c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatus.DaysWaitBetweenAttempts))
                                            .TotalDays),
                    AitClaimReceivedDate = c.CreatedOn.ToLocalTime().ToShortDateString(),
                    AitClaimReceivedTime = c.CreatedOn.ToLocalTime().ToShortTimeString(),
                })
                .OrderBy(c => c.PayerName)
                .ToListAsync();

            return claimsStatusDetailsData;
        }

        //public static string GetTotalClaimStatusString(ClaimStatusEnum? claimStatusEnum)
        //{
        //    if (claimStatusEnum != null)
        //        return claimStatusEnum.ToString();

        //    return string.Empty;
        //}

        public static string GetClaimLineItemStatusString(ClaimLineItemStatusEnum? claimLineItemStatusEnum)
        {
            if (claimLineItemStatusEnum != null)
                return claimLineItemStatusEnum.ToString();

            return string.Empty;
        }

        //public decimal GetSumOfDecimals(IEnumerable<decimal> decimalToSum)
        //{
        //    return decimalToSum.Sum();
        //}

        public async Task<PaginatedResult<ClaimWorkstationDetailsResponse>> GetClaimsWorkstationDetailsAsync(IClaimWorkstationDetailQuery filters, int pageNumber, int pageSize, ClaimWorkstationSearchOptions? claimWorkstationSearchOptions)
        {
            try
            {

                var insuranceIds = ClaimFiltersHelpers.ConvertStringToList(filters.ClientInsuranceIds, true);
                var locationIds = ClaimFiltersHelpers.ConvertStringToList(filters.ClientLocationIds, true);
                var providerIds = ClaimFiltersHelpers.ConvertStringToList(filters.ClientProviderIds, true);
                List<ClaimStatusExceptionReasonCategoryEnum> exceptionReasonIds = ClaimFiltersHelpers.ConvertStringToExceptionReasonCategoryEnumList(filters.ExceptionReasonCategoryIds, false);
                var serviceTypeIds = ClaimFiltersHelpers.ConvertStringToList(filters.AuthTypeIds, true);
                var procedureCodes = ClaimFiltersHelpers.ConvertProcedureCodesToList(filters.ProcedureCodes);
                //.Specify(new PatientWorkstationFilterSpecification(filters))

                var claimsStatusDetails = await _unitOfWork.Repository<ClaimStatusTransaction>().Entities
                       .Specify(new ClaimStatusTransactionNotSupplantedFilterSpecification())
                       .Specify(new ClaimWorkstationFilterSpecification(filters, _currentUserService.ClientId, claimWorkstationSearchOptions))
                       .Specify(new ClaimWorkstationLocationProviderFilterSpecification(filters, _currentUserService.ClientId, insuranceIds, exceptionReasonIds, serviceTypeIds, procedureCodes, locationIds, providerIds))
                       .Select(t => new ClaimWorkstationDetailsResponse()
                       {
                           ExceptionReason = t.ExceptionReason,
                           Id = t.Id,
                           PatientLastName = t.ClaimStatusBatchClaim.Patient.Person.LastName,
                           PatientFirstName = t.ClaimStatusBatchClaim.Patient.Person.FirstName,
                           DateOfBirth = t.ClaimStatusBatchClaim.Patient.Person.DateOfBirth,
                           PolicyNumber = t.ClaimStatusBatchClaim.PolicyNumber,
                           ServiceType = t.ClaimStatusBatchClaim.ClaimStatusBatch.AuthType.Name,
                           PayerName = t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance.LookupName,
                           OfficeClaimNumber = t.ClaimStatusBatchClaim.ClaimNumber,
                           PayerClaimNumber = t.ClaimNumber,
                           PayerLineItemControlNumber = t.LineItemControlNumber,
                           ProcedureCode = t.ClaimStatusBatchClaim.ProcedureCode,
                           DateOfServiceFrom = t.ClaimStatusBatchClaim.DateOfServiceFrom,
                           DateOfServiceTo = t.ClaimStatusBatchClaim.DateOfServiceTo,
                           ClaimLineItemStatus = t.ClaimLineItemStatus.Description,
                           ClaimLineItemStatusValue = t.ClaimLineItemStatusValue,
                           ExceptionRemark = t.ExceptionRemark,
                           ReasonCode = t.ReasonCode,
                           ClaimBilledOn = t.ClaimStatusBatchClaim.ClaimBilledOn,
                           BilledAmount = t.ClaimStatusBatchClaim.BilledAmount ?? 0.00m,
                           LineItemPaidAmount = t.LineItemPaidAmount,
                           TotalAllowedAmount = t.TotalAllowedAmount ?? 0.00m,
                           NonAllowedAmount = t.TotalNonAllowedAmount ?? 0.00m,
                           CheckPaidAmount = t.CheckPaidAmount,
                           CheckDate = t.CheckDate,
                           CheckNumber = t.CheckNumber,
                           ReasonDescription = t.ReasonDescription,
                           RemarkCode = t.RemarkCode,
                           RemarkDescription = t.RemarkDescription,
                           CoinsuranceAmount = t.CoinsuranceAmount,
                           CopayAmount = t.CopayAmount,
                           DeductibleAmount = t.DeductibleAmount,
                           CobAmount = t.CobAmount,
                           PenalityAmount = t.PenalityAmount,
                           EligibilityStatus = t.EligibilityStatus,
                           EligibilityInsurance = t.EligibilityInsurance,
                           EligibilityPolicyNumber = t.EligibilityPolicyNumber,
                           EligibilityFromDate = t.EligibilityFromDate,
                           VerifiedMemberId = t.VerifiedMemberId,
                           CobLastVerified = t.CobLastVerified,
                           LastActiveEligibleDateRange = t.LastActiveEligibleDateRange,
                           PrimaryPayer = t.PrimaryPayer,
                           PrimaryPolicyNumber = t.PrimaryPolicyNumber,

                           BatchNumber = t.ClaimStatusBatchClaim.ClaimStatusBatch.BatchNumber,
                           AitClaimReceivedDate = t.ClaimStatusBatchClaim.CreatedOn.ToLocalTime().ToShortDateString(),
                           AitClaimReceivedTime = t.ClaimStatusBatchClaim.CreatedOn.ToLocalTime().ToShortTimeString(),
                           TransactionDate = t.LastModifiedOn == null ? t.CreatedOn.ToLocalTime().ToShortDateString() : t.LastModifiedOn.Value.ToLocalTime().ToShortDateString(),
                           TransactionTime = t.LastModifiedOn == null ? t.CreatedOn.ToLocalTime().ToShortTimeString() : t.LastModifiedOn.Value.ToLocalTime().ToShortTimeString(),
                           ExceptionReasonCategoryId = t.ClaimStatusExceptionReasonCategoryId,
                           Modifiers = t.ClaimStatusBatchClaim.Modifiers,
                           WriteoffAmount = t.WriteoffAmount
                       })
                       .ToPaginatedListAsync(pageNumber, pageSize);
                return claimsStatusDetails;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        //AA-142
        #region provider comparison services
        public async Task<List<ProviderComparisonResponse>> GetProviderComparisonDataAsync(ProviderComparisonQuery filters, int clientId = 0)
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }

            List<ProviderComparisonResponse> response = new List<ProviderComparisonResponse>();


            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetProvidersDetails, conn, clientId, delimitedLineItemStatusIds: null, filters.ClientInsuranceIds, filters.ClientAuthTypeIds, filters.ClientProcedureCodes, filters.ClientExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo);
                    var providerDetails = ExecuteProviderComparisonSpCommand(cmd);

                    await Task.WhenAll(providerDetails);
                    response = providerDetails.Result;

                }
                return response;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<List<ProviderComparisonResponse>> ExecuteProviderComparisonSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ProviderComparisonResponse> providerDetails = new List<ProviderComparisonResponse>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    var rows = reader.GetSchemaTable().Rows;
                    while (reader.Read())
                    {
                        var provider = new ProviderComparisonResponse();
                        provider.ClientProviderId = reader["ProviderId"] == DBNull.Value ? default(int) : (int)reader["ProviderId"];
                        provider.ProcedureCode = reader["ProcedureCode"] as string;
                        provider.ChargedSum = reader["ChargedSum"] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader["ChargedSum"];
                        provider.Quantity =
                            reader["Quantity"] == DBNull.Value ? default(int) : (int)reader["Quantity"];
                        //provider.ClaimLineItemStatus = reader["ClaimLineItemStatus"] as string;
                        //provider.ClaimLineItemStatusId = reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId]; //AA-317
                        provider.ProviderName = reader["ProviderName"] as string;
                        //provider.ClaimStatusExceptionReasonCategory =
                        //    reader["ClaimStatusExceptionReasonCategory"] as string;
                        provider.Denial = GetIntValue(reader, rows, StoredProcedureColumnsHelper.DenialVisits);
                        provider.DenialAmt = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.DenialTotals);
                        providerDetails.Add(provider);
                    }

                    reader.Close();
                }

                return providerDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        public async Task<List<EmployeesClaimStatusResponseModel>> GetEmployeeClaimStatusDataAsync(int clientId = 0, string connStr = null)
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }
            if (string.IsNullOrWhiteSpace(connStr) || string.IsNullOrEmpty(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }

            List<EmployeesClaimStatusResponseModel> response = new List<EmployeesClaimStatusResponseModel>();

            //if (string.IsNullOrEmpty(connStr))
            //    connStr = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spDynamicExportDashboardQuery, conn, clientId, claimStatusType: 2, hasIncludeClaimStatusTransactionLineItemStatusChange: true);
                    List<ExportQueryResponse> claimResponse = await ExecuteClaimDynamicExportSpCommand(cmd);

                    var mappingData = _mapper.Map<List<EmployeesClaimStatusResponseModel>>(claimResponse);
                    response = mappingData;
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region AA-255 Dashboard Rredesign

        /// <summary>
        /// Get data for the Revenue totals chart
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<List<ExportQueryResponse>> GetClaimStatusRevenueTotalsAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null)
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            List<ExportQueryResponse> claimStatusRevenueTotals = new();

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //TODO : Update filter dates with last One Year date
                    DateTime today = DateTime.Now;
                    DateTime last12MonthsStartDate = today.AddMonths(-12); // Subtract 12 months to get the start date

                    // Set the start date to the first day of the month
                    last12MonthsStartDate = new DateTime(last12MonthsStartDate.Year, last12MonthsStartDate.Month, 1);

                    DateTime last12MonthsEndDate = today;

                    // Set the end date to the last day of the current month
                    last12MonthsEndDate = new DateTime(last12MonthsEndDate.Year, last12MonthsEndDate.Month, DateTime.DaysInMonth(last12MonthsEndDate.Year, last12MonthsEndDate.Month));
                    filters.ClaimBilledFrom = last12MonthsStartDate;
                    filters.ClaimBilledTo = last12MonthsEndDate;

                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimStatusRevenueTotals, conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId);

                    var claimStatusRevenueTotalsTask = ExecuteClaimStatusRevenueTotalsSpCommand(cmd);

                    await Task.WhenAll(claimStatusRevenueTotalsTask);
                    claimStatusRevenueTotals = claimStatusRevenueTotalsTask.Result;

                }
                return claimStatusRevenueTotals;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Execute sp for revenue totals chart
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private async Task<List<ExportQueryResponse>> ExecuteClaimStatusRevenueTotalsSpCommand(SqlCommand cmd)
        {
            try
            {
                var ClaimStatusRevenueTotals = new List<ExportQueryResponse>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var ClaimStatusRevenueTotal = new ExportQueryResponse
                        {
                            AllowedAmountSum = reader[StoredProcedureColumnsHelper.AllowedAmountSum] == System.DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmountSum],
                            NonAllowedAmountSum = reader[StoredProcedureColumnsHelper.NonAllowedAmountSum] == System.DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.NonAllowedAmountSum],
                            ChargedSum = reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum],
                            PaidAmountSum = reader[StoredProcedureColumnsHelper.PaidAmountSum] == System.DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.PaidAmountSum],
                            Quantity = reader[StoredProcedureColumnsHelper.Quantity] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.Quantity],
                            ClaimLineItemStatus = reader[StoredProcedureColumnsHelper.ClaimLineItemStatus] as string,
                            ClaimLineItemStatusId = reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId] == DBNull.Value ? default(int) : (ClaimLineItemStatusEnum?)((int)(reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId])),
                            WriteOffAmountSum = reader[StoredProcedureColumnsHelper.WriteOffAmountSum] == System.DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.WriteOffAmountSum],
                            DateOfServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.DOS_From) ? (reader[StoredProcedureColumnsHelper.DOS_From] == DBNull.Value ? default : ((DateTime?)reader[StoredProcedureColumnsHelper.DOS_From])) : default,
                            ClaimBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.Billed_On) ? (reader[StoredProcedureColumnsHelper.Billed_On] == DBNull.Value ? default : ((DateTime?)reader[StoredProcedureColumnsHelper.Billed_On])) : default
                        };

                        ClaimStatusRevenueTotals.Add(ClaimStatusRevenueTotal);
                    }

                    reader.Close();
                }

                return ClaimStatusRevenueTotals;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Sp to get ClientFeeScheduleData

        /// <summary>
        /// Retrieves a client fee schedule entry for a given procedure code, client insurance ID, and date of service.
        /// </summary>
        /// <param name="procedureCode">The procedure code to query.</param>
        /// <param name="clientInsuranceId">The ID of the client's insurance.</param>
        /// <param name="dateOfService">The date of service for the fee schedule entry.</param>
        /// <returns>
        /// An instance of the GetClientFeeScheduleEntryDataByClaimsModel representing the client fee schedule entry, or null if not found.
        /// </returns>
        public async Task<GetClientFeeSschedulelEntryDataByClaimsModel> GetClientFeeScheduleEntry(string procedureCode,
            int clientInsuranceId,
            DateTime dateOfServiceFrom,
            ProviderLevelEnum? providerLevelId = null,
            SpecialtyEnum? specialtyId = null,
            string connStr = null)
        {
            GetClientFeeSschedulelEntryDataByClaimsModel clientFeeScheduleEntry = null;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }
            if (string.IsNullOrEmpty(connStr))
                connStr = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = GetClientFeeScheduleEntrySqlCommand(StoreProcedureTitle.spGetClientFeeScheduleEntryByClaims, conn, procedureCode, clientInsuranceId, dateOfServiceFrom, providerLevelId, specialtyId);

                    clientFeeScheduleEntry = await ExecuteClientFeeScheduleSpCommand(cmd);
                }
                return clientFeeScheduleEntry;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private SqlCommand GetClientFeeScheduleEntrySqlCommand(string spName, SqlConnection conn, string procedureCode, int clientInsuranceId, DateTime dateOfService, ProviderLevelEnum? providerLevelId, SpecialtyEnum? specialtyId)
        {
            SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ProcedureCode, procedureCode);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceId, clientInsuranceId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, dateOfService);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ProviderLevelId, providerLevelId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.SpecialtyId, specialtyId);

            return cmd;
        }

        private async Task<GetClientFeeSschedulelEntryDataByClaimsModel> ExecuteClientFeeScheduleSpCommand(SqlCommand cmd)
        {
            try
            {
                GetClientFeeSschedulelEntryDataByClaimsModel clientFeeScheduleEntry = null;

                if (cmd.Connection.State != ConnectionState.Open)
                    await cmd.Connection.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    clientFeeScheduleEntry = new GetClientFeeSschedulelEntryDataByClaimsModel
                    {
                        ClientFeeScheduleEntryId = reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.ClientFeeScheduleEntryId)),
                        IsReimbursable = reader.GetBoolean(reader.GetOrdinal(StoredProcedureColumnsHelper.IsReimbursable))
                    };
                }

                reader.Close();

                return clientFeeScheduleEntry;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region AA-326 : Charges Dashboard 

        /// <summary>
        /// Get next seven days' cash projection for the client
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExportQueryResponse>> GetCashProjectionByDayAsync(GetCashProjectionByDayQuery query) //Updated AA-343
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ExportQueryResponse> response = new();

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetCashProjectionByDay, conn, clientId, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, filterForDays: query.FilterForDays);

                    //execute the sp to get the cash projection data for last 7 days
                    var cashProjectionByDayTask = await ExecuteGetCashProjectionByDaySpCommand(cmd);

                    response = cashProjectionByDayTask;

                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Execute spGetCashProjectionByDay to get cash projection by day
        /// and read the output into the response
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private async Task<List<ExportQueryResponse>> ExecuteGetCashProjectionByDaySpCommand(SqlCommand cmd)
        {
            try
            {
                List<ExportQueryResponse> cashProjectionByDayResponse = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var cashProjectionByDay = new ExportQueryResponse//GetCashProjectionByDayResponse
                        {
                            ClaimCount = reader[StoredProcedureColumnsHelper.ClaimCount] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ClaimCount],
                            CheckNumber = reader[StoredProcedureColumnsHelper.CheckNumber] as string,
                            CheckDate = HasColumn(rows, StoredProcedureColumnsHelper.CheckDate) ? (reader[StoredProcedureColumnsHelper.CheckDate] == DBNull.Value ? default(DateTime?) : ((DateTime?)reader[StoredProcedureColumnsHelper.CheckDate])) : default(DateTime?),
                            CheckDateString = HasColumn(rows, StoredProcedureColumnsHelper.CheckDate) ? (reader[StoredProcedureColumnsHelper.CheckDate] == DBNull.Value ? default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.CheckDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            PaidTotals = reader[StoredProcedureColumnsHelper.PaidTotals] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.PaidTotals],
                            RevenueTotals = reader[StoredProcedureColumnsHelper.RevenueTotals] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.RevenueTotals],
                            ClaimLevelMd5Hash = reader[StoredProcedureColumnsHelper.ClaimLevelMd5Hash] as string,
                            ClientInsuranceId = reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId],
                            PayerName = reader[StoredProcedureColumnsHelper.PayerName] as string,
                            AccountNumber = reader[StoredProcedureColumnsHelper.ClaimCount] as string,
                            ExternalId = reader[StoredProcedureColumnsHelper.ClaimCount] as string,
                            PatientLastCommaFirst = reader[StoredProcedureColumnsHelper.PatientLastCommaFirst] as string
                        };

                        cashProjectionByDayResponse.Add(cashProjectionByDay);
                    }
                    reader.Close();
                }
                return cashProjectionByDayResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get next 7 days' cash value for Revenue by day for the client
        /// </summary>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        public async Task<List<GetCashValueForRevenueByDayResponse>> GetCashValueForRevenueByDayAsync(GetCashValueForRevenueByDayQuery query, int clientId, string connStr)
        {
            //get client id from the current user service 
            if (clientId != null && clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }

            List<GetCashValueForRevenueByDayResponse> response = new List<GetCashValueForRevenueByDayResponse>();

            //initialize sql connection that will be used to execute the stored procedure
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 

                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetCashValueForRevenueByDay, conn, clientId: clientId, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, filterForDays: query.FilterForDays, filterBy: query.FilterBy);

                    //execute the sp
                    var cashValueForRevenueByDayTask = ExecuteGetCashValueForRevenueByDaySpCommand(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(cashValueForRevenueByDayTask);
                    response = cashValueForRevenueByDayTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Execute spGetCashValueForRevenueByDay to get cash projection by day
        /// and read the output into the response
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private async Task<List<GetCashValueForRevenueByDayResponse>> ExecuteGetCashValueForRevenueByDaySpCommand(SqlCommand cmd)
        {
            try
            {
                List<GetCashValueForRevenueByDayResponse> cashValueForRevenueByDayResponse = new List<GetCashValueForRevenueByDayResponse>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var cashVavlueForRevenueByDay = new GetCashValueForRevenueByDayResponse();
                        cashVavlueForRevenueByDay.ClaimCount = reader[StoredProcedureColumnsHelper.ClaimCount] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ClaimCount];
                        cashVavlueForRevenueByDay.CashValue = reader[StoredProcedureColumnsHelper.CashValue] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.CashValue];
                        cashVavlueForRevenueByDay.RevenueTotals = reader[StoredProcedureColumnsHelper.RevenueTotals] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.RevenueTotals];
                        cashVavlueForRevenueByDay.ClaimLevelMd5Hash = reader[StoredProcedureColumnsHelper.ClaimLevelMd5Hash] as string;
                        cashVavlueForRevenueByDay.ClientInsuranceId = reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId];
                        cashVavlueForRevenueByDay.PayerName = reader[StoredProcedureColumnsHelper.PayerName] as string;
                        cashVavlueForRevenueByDay.ServiceDate = HasColumn(rows, StoredProcedureColumnsHelper.ServiceDate) ?
                            (reader[StoredProcedureColumnsHelper.ServiceDate] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.ServiceDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string);
                        cashVavlueForRevenueByDay.BilledDate = HasColumn(rows, StoredProcedureColumnsHelper.BilledDate) ?
                            (reader[StoredProcedureColumnsHelper.BilledDate] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.BilledDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string);
                        //cashVavlueForRevenueByDay.CheckDate = HasColumn(rows, StoredProcedureColumnsHelper.CheckDate) ?
                        //    (reader[StoredProcedureColumnsHelper.CheckDate] == DBNull.Value ?
                        //    default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.CheckDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string);
                        cashValueForRevenueByDayResponse.Add(cashVavlueForRevenueByDay);
                    }
                    reader.Close();
                }
                return cashValueForRevenueByDayResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get cash value details to be exported in the additional details sheet in the cash value for revenue component
        /// </summary>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        public async Task<List<CashValueForRevenueDetails>> GetCashValueForRevenueDetails(GetCashValueForRevenueByDayQuery query, int clientId, string connStr = null) //AA-331
        {
            //get client id from the current user service 
            if (clientId == null || clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }

            List<CashValueForRevenueDetails> response = new List<CashValueForRevenueDetails>();
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spExportCashValueOfRevenue, conn, clientId: clientId, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, filterForDays: query.FilterForDays, filterBy: query.FilterBy); //clientInsuranceIds: query.ClientInsuranceId.ToString(), 

                    //execute the sp
                    var cashValueForRevenueDetailsTask = ExecuteCashValueForRevenueDetailsSpCommand(cmd);

                    //wait for the task to complete and then map the result into response
                    await cashValueForRevenueDetailsTask;
                    response = cashValueForRevenueDetailsTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Execute the spExportCashValueOfRevenue to get cash value for revenue details to export in the charges dashbaord
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private async Task<List<CashValueForRevenueDetails>> ExecuteCashValueForRevenueDetailsSpCommand(SqlCommand cmd) //AA-331
        {
            try
            {
                List<CashValueForRevenueDetails> cashValueForRevenueDetails = new List<CashValueForRevenueDetails>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var cashVavlueForRevenue = new CashValueForRevenueDetails
                        {
                            CashValue = reader[StoredProcedureColumnsHelper.CashValue] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.CashValue],
                            PayerName = reader[StoredProcedureColumnsHelper.PayerName] as string,
                            ServiceDate = HasColumn(rows, StoredProcedureColumnsHelper.ServiceDate) ?
                            (reader[StoredProcedureColumnsHelper.ServiceDate] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.ServiceDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            BilledDate = HasColumn(rows, StoredProcedureColumnsHelper.BilledDate) ?
                            (reader[StoredProcedureColumnsHelper.BilledDate] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.BilledDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            PatientLastName = reader[StoredProcedureColumnsHelper.PatientLastName] as string,
                            PatientFirstName = reader[StoredProcedureColumnsHelper.PatientFirstName] as string,
                            ProcedureCode = reader[StoredProcedureColumnsHelper.ProcedureCode] as string,
                            Location = reader[StoredProcedureColumnsHelper.LocationName] as string,
                            Provider = reader[StoredProcedureColumnsHelper.ProviderName] as string
                        };
                        cashValueForRevenueDetails.Add(cashVavlueForRevenue);
                    }
                    reader.Close();
                }
                return cashValueForRevenueDetails;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region export cash value response AA-331
        /// <summary>
        /// Generates a dictionary for mapping columns when exporting Cash Value for Revenue data.
        /// </summary>
        /// <param name="FilterBy">The selected filter type, either "ServiceDate" or "Billed Date."</param>
        /// <returns>A dictionary mapping column names to functions that extract data from GetCashValueForRevenueByDayResponse objects.</returns>
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelCashValueForRevenue(string FilterBy)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                {_localizer[StoredProcedureColumnsHelper.LineItems], item => item.ClaimCount },
                {_localizer[StoredProcedureColumnsHelper.Payer_Name], item => item.PayerName },
                // Add column and value mappings based on the FilterBy property selected
                {FilterBy == "ServiceDate" ? _localizer[StoredProcedureColumnsHelper.ServiceDateCol] : _localizer[StoredProcedureColumnsHelper.BilledDateCol], item =>  FilterBy == "ServiceDate" ? item.ServiceDate : item.BilledDate},
                {_localizer[StoredProcedureColumnsHelper.RevenueTotalsCol], item =>  item.RevenueTotals.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.CashValueCol], item =>  item.CashValue.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.CollectionPercentageCol], item => (item.RevenueTotals > 0 && item.CashValue > 0) ? Math.Abs(item.CashValue/item.RevenueTotals).ToString("P") : $"0%" }
            };
            //if (!_jobCronManager.IsProductionEnvironment)
            //{
            //    exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            //}
            return exportMapper;
        }

        /// <summary>
        /// Generates a dictionary for mapping columns when exporting detailed Cash Value for Revenue data.
        /// </summary>
        /// <param name="FilterBy">The selected filter type, either "ServiceDate" or "Billed Date."</param>
        /// <returns>A dictionary mapping column names to functions that extract data from CashValueForRevenueDetails objects.</returns>
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelCashValueForRevenueDetails(string FilterBy)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                {_localizer[StoredProcedureColumnsHelper.Last_Name], item =>  item.PatientLastName },
                {_localizer[StoredProcedureColumnsHelper.First_Name], item =>  item.PatientFirstName },
                // Add column and value mappings based on the FilterBy property selected
                {FilterBy == "ServiceDate" ? _localizer[StoredProcedureColumnsHelper.ServiceDateCol] : _localizer[StoredProcedureColumnsHelper.BilledDateCol], item =>  FilterBy == "ServiceDate" ? item.ServiceDate : item.BilledDate},
                {_localizer[StoredProcedureColumnsHelper.Payer_Name], item => item.PayerName },
                {_localizer[StoredProcedureColumnsHelper.ProcedureCodeCol], item => item.ProcedureCode },
                {_localizer[StoredProcedureColumnsHelper.CashValueCol], item =>  item.CashValue.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.LocationNameCol], item => item.Location },
                {_localizer[StoredProcedureColumnsHelper.ProviderNameCol], item => item.Provider }
            };
            //if (!_jobCronManager.IsProductionEnvironment)
            //{
            //    exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            //}
            return exportMapper;
        }

        /// <summary>
        /// Combine sheets to get the report and the additional details in a single export
        /// </summary>
        /// <param name="excelReport"></param>
        /// <param name="excelReportDetails"></param>
        /// <returns></returns>
        public List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineTwoExportReportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails) //AA-331
        {
            return new List<Dictionary<string, Func<ExportQueryResponse, object>>>()
            {
                excelReport.ToDictionary(summaryKey => summaryKey.Key, summary => (Func<ExportQueryResponse, object>)(exp => summary.Value((ExportQueryResponse)exp))),
                excelReportDetails.ToDictionary(detailKey => detailKey.Key, detail => (Func<ExportQueryResponse, object>)(exp => detail.Value((ExportQueryResponse)exp)))
            };
        }

        #endregion

        #region export cash projection
        /// <summary>
        /// Create excel sheet for the cash projection data
        /// </summary>
        /// <param name="FilterBy"></param>
        /// <returns></returns>
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelCashProjection(string FilterBy = "ServiceDate") //AA-343
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                {_localizer[StoredProcedureColumnsHelper.LineItems], item => item.ClaimCount },
                // Add column and value mappings based on the FilterBy property selected
                //{FilterBy == "ServiceDate" ? _localizer[StoredProcedureColumnsHelper.ServiceDateCol] : _localizer[StoredProcedureColumnsHelper.BilledDateCol], item =>  FilterBy == "ServiceDate" ? item.ServiceDate : item.BilledDate},
                {_localizer[StoredProcedureColumnsHelper.CheckNumberCol], item => item.CheckNumber },
                {_localizer[StoredProcedureColumnsHelper.Check_Date], item => item.CheckDateString },
                {_localizer[StoredProcedureColumnsHelper.PaidTotalsCol], item =>  item.PaidTotals.ToString("C", new CultureInfo("en-US"))},
                {_localizer[StoredProcedureColumnsHelper.RevenueTotalsCol], item =>  item.RevenueTotals.ToString("C", new CultureInfo("en-US"))},
                {_localizer[StoredProcedureColumnsHelper.Payer_Name], item => item.PayerName },
                {_localizer[StoredProcedureColumnsHelper.AccountNumberCol], item => item.PayerName },
                {_localizer[StoredProcedureColumnsHelper.PatientLastCommaFirst], item => item.PatientLastCommaFirst }
            };
            //if (!_jobCronManager.IsProductionEnvironment)
            //{
            //    exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            //}
            return exportMapper;
        }
        #endregion

        #region Date Lag services
        public async Task<List<ClaimStatusDateLagResponse>> GetClaimStatusDateLagAsync(ClaimStatusDateLagQuery query, int clientId = 0, string connStr = null)
        {
            // If the client ID is not provided, use the current user's client ID.
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            // Initialize a list to store claim status date lag responses.
            List<ClaimStatusDateLagResponse> claimStatusDateLagResponse = new();

            // Create a SqlConnection to the database using the connection string from configuration.
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                // Open the database connection asynchronously.
                await using (conn)
                {
                    await conn.OpenAsync();

                    // Create a SqlCommand to execute the stored procedure.
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimStatusDateLag, conn, clientId: clientId, delimitedLineItemStatusIds: query.CommaDelimitedLineItemStatusIds, clientInsuranceIds: query.ClientInsuranceIds, clientAuthTypeIds: query.AuthTypeIds, clientProcedureCodes: query.ProcedureCodes, clientExceptionReasonCategoryIds: query.ExceptionReasonCategoryIds, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, dateOfServiceFrom: query.DateOfServiceFrom, dateOfServiceTo: query.DateOfServiceTo, claimBilledFrom: query.ClaimBilledFrom, claimBilledTo: query.ClaimBilledTo, receivedFrom: query.ReceivedFrom, receivedTo: query.ReceivedTo, transactionDateFrom: query.TransactionDateFrom, transactionDateTo: query.TransactionDateTo, patientId: query.PatientId, claimStatusBatchId: query.ClaimStatusBatchId);

                    // Execute the SqlCommand and retrieve the result.
                    var calimStatusDateLag = ExecuteClaimsDateLagSpCommand(cmd);

                    await Task.WhenAll(calimStatusDateLag);
                    claimStatusDateLagResponse = calimStatusDateLag.Result;
                }

                // Return the retrieved claim status date lag data.
                return claimStatusDateLagResponse;
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur during database operations.
                Console.WriteLine(e);
                throw;
            }
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelDateLagReport() //AA-343
        {
            // Create a new dictionary that defines the mapping of Excel report columns to corresponding properties in the ClaimStatusDateLagResponse.
            return new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                // Map the 'ClaimNumber' column to the 'ClaimNumber' property in ClaimStatusDateLagResponse.
                {_localizer[StoredProcedureColumnsHelper.ClaimNumber], item => item.ClaimNumber },
        
                // Map the 'ClaimLevelMd5Hash' column to the 'ClaimLevelMd5Hash' property in ClaimStatusDateLagResponse.
                {_localizer[StoredProcedureColumnsHelper.ClaimLevelMd5Hash], item => item.ClaimLevelMd5Hash },
        
                // Map the 'ServiceToBilledDateLag' column to the 'ServiceToBilledDateLag' property in ClaimStatusDateLagResponse.
                {_localizer[StoredProcedureColumnsHelper.ServiceToBilledDateLag], item => item.ServiceToBilledDateLag },
        
                // Map the 'ServiceToPaymentDateLag' column to the 'ServiceToPaymentDateLag' property in ClaimStatusDateLagResponse.
                {_localizer[StoredProcedureColumnsHelper.ServiceToPaymentDateLag], item => item.ServiceToPaymentDateLag },
        
                // Map the 'BilledToPaymentDateLag' column to the 'BilledToPaymentDateLag' property in ClaimStatusDateLagResponse.
                {_localizer[StoredProcedureColumnsHelper.BilledToPaymentDateLag], item => item.BilledToPaymentDateLag }
            };
        }

        /// <summary>
        /// Execute the SqlCommand and retrieve the results.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private async Task<List<ClaimStatusDateLagResponse>> ExecuteClaimsDateLagSpCommand(SqlCommand cmd)
        {
            try
            {
                // Initialize a list to store claim status date lag responses.
                List<ClaimStatusDateLagResponse> claimStatusDateLagTotals = new List<ClaimStatusDateLagResponse>();

                // Ensure that the database connection is open.
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    // Execute the SqlCommand and retrieve data using a SqlDataReader.
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    // Iterate through the SqlDataReader and create ClaimStatusDateLagResponse objects.
                    while (reader.Read())
                    {
                        var claimStatusDateLagResponse = new ClaimStatusDateLagResponse
                        {
                            ServiceToBilledDateLag = reader[StoredProcedureColumnsHelper.ServiceToBilled] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToBilled],
                            ServiceToPaymentDateLag = reader[StoredProcedureColumnsHelper.ServiceToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToPayment],
                            BilledToPaymentDateLag = reader[StoredProcedureColumnsHelper.BilledToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.BilledToPayment]
                        };

                        claimStatusDateLagTotals.Add(claimStatusDateLagResponse);
                    }

                    // Close the SqlDataReader.
                    reader.Close();
                }

                // Return the list of claim status date lag responses.
                return claimStatusDateLagTotals;
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur during database operations.
                Console.WriteLine(e);
                throw;
            }
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelRevenueTotalsReport() //AA-343
        {
            // Create a new dictionary that defines the mapping of Excel report columns to corresponding properties in the ClaimStatusDateLagResponse.
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                // Map the 'AllowedAmountSum' column to the 'AllowedAmountSum' property in ClaimStatusRevenueTotal.
                {_localizer[StoredProcedureColumnsHelper.AllowedAmountSum], item => item.AllowedAmountSum.ToString("C", new CultureInfo("en-US")) },
        
                // Map the 'NonAllowedAmountSum' column to the 'NonAllowedAmountSum' property in ClaimStatusRevenueTotal.
                {_localizer[StoredProcedureColumnsHelper.NonAllowedAmountSum], item => item.NonAllowedAmountSum.ToString("C", new CultureInfo("en-US")) },
        
                // Map the 'ChargedSum' column to the 'ChargedSum' property in ClaimStatusRevenueTotal.
                {_localizer[StoredProcedureColumnsHelper.ChargedSum], item => item.ChargedSum.ToString("C", new CultureInfo("en-US")) },
        
                // Map the 'PaidAmountSum' column to the 'PaidAmountSum' property in ClaimStatusRevenueTotal.
                {_localizer[StoredProcedureColumnsHelper.PaidAmountSum], item => item.PaidAmountSum.ToString("C", new CultureInfo("en-US")) },
        
                // Map the 'ClaimLineItemStatus' column to the 'ClaimLineItemStatus' property in ClaimStatusRevenueTotal.
                {_localizer[StoredProcedureColumnsHelper.ClaimLineItemStatus], item => item.ClaimLineItemStatus },

                // Map the 'WriteOffAmountSum' column to the 'WriteOffAmountSum' property in ClaimStatusRevenueTotal.
                {_localizer[StoredProcedureColumnsHelper.WriteOffAmountSum], item => item.WriteOffAmountSum.ToString("C", new CultureInfo("en-US")) },

                // Map the 'DateOfServiceFrom' column to the 'DateOfServiceFrom' property in ClaimStatusRevenueTotal.
                {_localizer[StoredProcedureColumnsHelper.DateOfServiceFrom], item => $"Date{item.DateOfServiceFrom}" }
            };

            //if (!_jobCronManager.IsProductionEnvironment)
            //{
            //    exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            //}
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelReverseAnalysisReport() //EN-66
        {
            // Create a new dictionary that defines the mapping of Excel report columns to corresponding properties in the ARAgingData.
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                {_localizer[StoredProcedureColumnsHelper.InsuranceId], item => item.InsuranceId },
                {_localizer[StoredProcedureColumnsHelper.ClaimLineItemStatusId], item => item.ClaimLineItemStatusId },
                {_localizer[StoredProcedureColumnsHelper.LocationId], item => item.LocationId },
                {_localizer[StoredProcedureColumnsHelper.ProviderId], item => item.ProviderId },
                {_localizer[StoredProcedureColumnsHelper.ChargedSum], item => item.ChargedSum.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.DateOfServiceFrom], item => $"Date{item.DateOfServiceFrom}" },
                {_localizer[StoredProcedureColumnsHelper.ClaimBilledOn], item => $"Date{item.ClaimBilledOn}" }
            };

            return exportMapper;
        }
        #endregion
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportDetailsExcel(ExportClaimStatusDetailsQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Exception Reason"], item => item.ExceptionReason },
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["Ins Claim #"], item => item.PayerClaimNumber },
                { _localizer["Ins Lineitem Control #"], item => item.PayerLineItemControlNumber },
                { _localizer["DOS From"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["DOS To"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Quantity"], item => item.Quantity },//EN-172
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Billed Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { _localizer["Lineitem Status"], item => item.ClaimLineItemStatus },
                //{ _localizer["Reported Lineitem Status"], item => item.ClaimLineItemStatusValue },
                { _localizer["Exception Category"], item => item.ExceptionReasonCategory },
                { _localizer["Exception Remark"], item => item.ExceptionRemark },
                { _localizer["Remark Code"], item => item.RemarkCode },
                { _localizer["Remark Description"], item => item.RemarkDescription },
                { _localizer["Reason Code"], item => item.ReasonCode },
                { _localizer["Reason Description"], item => item.ReasonDescription },
                { _localizer["Deductible Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.DeductibleAmount.HasValue ? item.DeductibleAmount.Value.ToString("C", new CultureInfo("en-US")) :  "$0.00" )},
                { _localizer["Copay Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CopayAmount.HasValue ? item.CopayAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Coinsurance Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CoinsuranceAmount.HasValue ? item.CoinsuranceAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Penality Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PenalityAmount.HasValue ? item.PenalityAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Lineitem Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemPaidAmount.HasValue ? item.LineItemPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Allowed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.TotalAllowedAmount.HasValue ? item.TotalAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Non-Allowed Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.NonAllowedAmount.HasValue ? item.NonAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Check #"], item => item.CheckNumber },
                { _localizer["Check Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDateString ?? null) },
                { _localizer["Check Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CheckPaidAmount.HasValue ? item.CheckPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Eligibility Ins"], item => item.EligibilityInsurance },
                { _localizer["Eligibility Policy #"], item => item.EligibilityPolicyNumber },
                { _localizer["Eligibility From Date"], item => item.EligibilityFromDate },
                { _localizer["Eligibility Status"], item => item.EligibilityStatus },
                { _localizer["VerifiedMemberId"], item => item.VerifiedMemberId },
                { _localizer["CobLastVerified"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CobLastVerified?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["LastActiveEligibleDateRange"], item => item.LastActiveEligibleDateRange },
                { _localizer["PrimaryPayer"], item => item.PrimaryPayer },
                { _localizer["PrimaryPolicyNumber"], item => item.PrimaryPolicyNumber },
                { _localizer["PartA_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_EligibilityTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_DeductibleFrom "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_DeductibleTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartA_RemainingDeductible.HasValue ? item.PartA_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer["PartB_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_EligibilityTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_DeductibleFrom "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_DeductibleTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartB_RemainingDeductible.HasValue ? item.PartB_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer["OtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["OtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["OtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.OtCapUsedAmount.HasValue ? item.OtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer["PtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PtCapUsedAmount.HasValue ? item.PtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                 { _localizer["AIT Received Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                { _localizer["AIT Received Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                { _localizer["AIT Transaction Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                { _localizer["AIT Transaction Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                { _localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { request.FlattenedLineItemStatus != "Denied" ? _localizer["Payment Type"] : "", item => request.FlattenedLineItemStatus != "Denied" ? item.PaymentType : "" }, //AA-324
                //{ _localizer["Last History Created On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastHistoryCreatedOn?.ToString("MM/dd/yyyy") ?? null)}, //EN-127
                { _localizer["ClaimStatusBatchId"], item => item.ClaimStatusBatchClaimId },
                { _localizer["LineItemChargeAmount"], item =>_excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemChargeAmount.HasValue ?  item.LineItemChargeAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer[StoredProcedureColumnsHelper.AllowedToPaidPercentage], item => item.BilledAmount > 0 ? (item.AllowedAmount/item.BilledAmount)*100 : 0}
            };
            if (!_jobCronManager.IsProductionEnvironment)
            {
                exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            }
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetFinicalSummaryDetailsExcel(FinicalSummaryExportDetailQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Lineitem Status"], item => item.ClaimLineItemStatus },
                { _localizer["Exception Category"], item => item.ExceptionReasonCategory },
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["Ins Claim #"], item => item.PayerClaimNumber },
                { _localizer["Ins Lineitem Control #"], item => item.PayerLineItemControlNumber },
                { _localizer["DOS From"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["DOS To"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Quantity"], item => item.Quantity },//EN-172
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Billed Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { _localizer["Deductible Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.DeductibleAmount.HasValue ? item.DeductibleAmount.Value.ToString("C", new CultureInfo("en-US")) :  "$0.00" )},
                { _localizer["Copay Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CopayAmount.HasValue ? item.CopayAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Coinsurance Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CoinsuranceAmount.HasValue ? item.CoinsuranceAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Penality Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PenalityAmount.HasValue ? item.PenalityAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Lineitem Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemPaidAmount.HasValue ? item.LineItemPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Allowed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.TotalAllowedAmount.HasValue ? item.TotalAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Non-Allowed Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.NonAllowedAmount.HasValue ? item.NonAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Check #"], item => item.CheckNumber },
                { _localizer["Check Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDateString ?? null) },
                { _localizer["Check Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CheckPaidAmount.HasValue ? item.CheckPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                { _localizer["AIT Received Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                { _localizer["AIT Transaction Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                { _localizer["AIT Transaction Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
               {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                { _localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { request.FlattenedLineItemStatus != "Denied" ? _localizer["Payment Type"] : "", item => request.FlattenedLineItemStatus != "Denied" ? item.PaymentType : "" }, //AA-324
                { _localizer["ClaimStatusBatchId"], item => item.ClaimStatusBatchClaimId }
            };
            if (!_jobCronManager.IsProductionEnvironment)
            {
                exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            }
            return exportMapper;
        }
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportInProcessExcel()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => item.DateOfBirth },
                {_localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["DOS From"], item => item.DateOfServiceFromString },
                { _localizer["DOS To"], item => item.DateOfServiceToString },
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Billed Amt"],item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US")))},
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                { _localizer["AIT Received Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { _localizer["Payment Type"], item => item.PaymentType } //AA-324
			};

            if (!_jobCronManager.IsProductionEnvironment)
            {
                exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            }
            return exportMapper;
        }


        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportProcedureCodeSummaryExcel()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Row Label"], item => item.RowLabels },
                { _localizer["Cpt Code Count"], item => item.CptCodeCount },
                { _localizer["Sum of Allowed Amt"], item => item.AllowedAmountSum },
                {_localizer["Average of Allowed Amt2"], item => item.AvgAllowedAmount },
            };

            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportPaymentSummaryExcel()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Row Label"], item => item.RowLabels },
                { _localizer["Count of Payer Name"], item => item.PayerNameCount },
                { _localizer["Sum of Billed Amt"], item => item.BilledAmountSum },
                {_localizer["Sum of Deductible Amt"], item => item.DeductibleAmtSum },
                { _localizer["Sum of Copay Amt"], item => item.CopayAmtSum },
                { _localizer["Sum of Coinsurance Amt"], item => item.CoInsuranceAmtSum },
                { _localizer["Sum of Penality Amt"], item => item.PenalityAmtSum },
                {_localizer["Sum of Lineitem Paid Amt"], item => item.LineitemPaidAmtSum },
                { _localizer["Sum of Allowed Amt"], item => item.AllowedAmountSum },
                { _localizer["Average of Allowed Amt2"], item => item.AvgAllowedAmount },
                { _localizer["Average of Lineitem Paid Amt2"], item => item.AvgLineitemPaidAmt },
            };

            return exportMapper;
        }

        public List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineExportDashboardReportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails)
        {
            return new List<Dictionary<string, Func<ExportQueryResponse, object>>>()
            {
                excelReport.ToDictionary(summaryKey => summaryKey.Key, summary => (Func<ExportQueryResponse, object>)(exp => summary.Value((ExportQueryResponse)exp))),
                excelReportDetails.ToDictionary(detailKey => detailKey.Key, detail => (Func<ExportQueryResponse, object>)(exp => detail.Value((ExportQueryResponse)exp)))
            };
        }

        public List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineSummaryExportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails)
        {
            return new List<Dictionary<string, Func<ExportQueryResponse, object>>>()
            {
                excelReport.ToDictionary(summaryKey => summaryKey.Key, summary => (Func<ExportQueryResponse, object>)(exp => summary.Value((ExportQueryResponse)exp))),
                excelReportDetails.ToDictionary(detailKey => detailKey.Key, detail => (Func<ExportQueryResponse, object>)(exp => detail.Value((ExportQueryResponse)exp)))
                 //summaryReportDetails.ToDictionary(detailKey => detailKey.Key, detail => (Func<ExportQueryResponse, object>)(exp => detail.Value((ExportQueryResponse)exp)))
            };
        }

        public List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineExportDashboardReportModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails, Dictionary<string, Func<ExportQueryResponse, object>> procedureReport)
        {
            return new List<Dictionary<string, Func<ExportQueryResponse, object>>>()
            {
                excelReport.ToDictionary(summaryKey => summaryKey.Key, summary => (Func<ExportQueryResponse, object>)(exp => summary.Value((ExportQueryResponse)exp))),
                excelReportDetails.ToDictionary(detailKey => detailKey.Key, detail => (Func<ExportQueryResponse, object>)(exp => detail.Value((ExportQueryResponse)exp))),
                 procedureReport.ToDictionary(detailKey => detailKey.Key, detail => (Func<ExportQueryResponse, object>)(exp => detail.Value((ExportQueryResponse)exp)))
            };
        }

        public async Task<List<ExportQueryResponse>> GetFilteredReportAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null)
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }

            if (string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }
            if (string.IsNullOrEmpty(connStr))
                connStr = _configuration.GetConnectionString("DefaultConnection");

            List<ExportQueryResponse> response = [];

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new(connStr);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    int? claimStatusType = (int?)filters.ClaimStatusType;

                    //if (string.IsNullOrWhiteSpace(filters.CommaDelimitedLineItemStatusIds) && (!claimStatusType.HasValue || claimStatusType.Value == 0))
                    //{
                    //    filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName(filters.FlattenedLineItemStatus);
                    //}
                    //if (filters.FlattenedLineItemStatus != "In-Process")
                    //{
                    //    filters.FlattenedLineItemStatus = null;
                    //}
                    string dashboardType = string.Empty;
                    if (!string.IsNullOrEmpty(filters.DashboardType)) { dashboardType = filters.DashboardType; }

                    //create the sp to get cash projection for last 7 days by passing the required parameters to the sp
                    ///StoreProcedureTitle.spGetClaimStatusReport
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spDynamicExportDashboardQuery, conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId, claimStatusType: claimStatusType, claimStatusTypeStatus: filters.ClaimStatusTypeValue, dashboardType: dashboardType);

                    //execute the sp
                    //var claimStatusDashboardDetailsResponse = ExecuteFilteredReportSpCommand(cmd);
                    List<ExportQueryResponse> claimStatusDashboardDetailsResponse = await ExecuteClaimDynamicExportSpCommand(cmd);

                    response = claimStatusDashboardDetailsResponse;

                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task<List<ClaimStatusDashboardDetailsResponse>> ExecuteFilteredReportSpCommand(SqlCommand cmd) //EN-66
        {
            try
            {
                List<ClaimStatusDashboardDetailsResponse> claimStatusDashboardDetailsResponses = new List<ClaimStatusDashboardDetailsResponse>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var dataItem = new ClaimStatusDashboardDetailsResponse
                        {
                            ExceptionReason = reader[StoredProcedureColumnsHelper.ExceptionReason] as string,
                            PatientFirstName = reader[StoredProcedureColumnsHelper.PatientFirstName] as string,
                            PatientLastName = reader[StoredProcedureColumnsHelper.PatientLastName] as string,
                            DateOfBirthString = HasColumn(rows, StoredProcedureColumnsHelper.PatientDOB) ?
                            (reader[StoredProcedureColumnsHelper.PatientDOB] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.PatientDOB]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            PolicyNumber = reader[StoredProcedureColumnsHelper.PolicyNumber] as string,
                            ServiceType = reader[StoredProcedureColumnsHelper.ServiceType] as string,
                            PayerName = reader[StoredProcedureColumnsHelper.PayerName] as string,
                            OfficeClaimNumber = reader[StoredProcedureColumnsHelper.OfficeClaimNumber] as string,
                            PayerClaimNumber = reader[StoredProcedureColumnsHelper.PayerClaimNumber] as string,
                            PayerLineItemControlNumber = reader[StoredProcedureColumnsHelper.PayerLineItemControlNumber] as string,
                            ProcedureCode = reader[StoredProcedureColumnsHelper.ProcedureCode] as string,
                            DateOfServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.DateOfServiceFrom) ?
                                        (reader[StoredProcedureColumnsHelper.DateOfServiceFrom] == DBNull.Value ?
                                            default(DateTime?) : ((DateTime?)reader[StoredProcedureColumnsHelper.DateOfServiceFrom]).Value) : default(DateTime?),
                            DateOfServiceToString = HasColumn(rows, StoredProcedureColumnsHelper.DateOfServiceTo) ?
                            (reader[StoredProcedureColumnsHelper.DateOfServiceTo] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.DateOfServiceTo]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            ClaimLineItemStatus = reader[StoredProcedureColumnsHelper.ClaimLineItemStatus] as string,
                            ClaimLineItemStatusId = reader.IsDBNull(StoredProcedureColumnsHelper.ClaimLineItemStatusId)
                                                        ? null : (ClaimLineItemStatusEnum)reader.GetInt32(StoredProcedureColumnsHelper.ClaimLineItemStatusId),
                            ClaimLineItemStatusValue = reader[StoredProcedureColumnsHelper.ClaimLineItemStatusValue] as string,
                            ExceptionRemark = reader[StoredProcedureColumnsHelper.ExceptionRemark] as string,
                            ReasonCode = reader[StoredProcedureColumnsHelper.ReasonCode] as string,
                            ClaimBilledOnString = HasColumn(rows, StoredProcedureColumnsHelper.ClaimBilledOn) ?
                            (reader[StoredProcedureColumnsHelper.PatientDOB] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.PatientDOB]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            BilledAmount = reader[StoredProcedureColumnsHelper.BilledAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.BilledAmount],
                            LineItemPaidAmount = reader[StoredProcedureColumnsHelper.LineItemPaidAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.LineItemPaidAmount],
                            TotalAllowedAmount = reader[StoredProcedureColumnsHelper.TotalAllowedAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.TotalAllowedAmount],
                            NonAllowedAmount = reader[StoredProcedureColumnsHelper.NonAllowedAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.NonAllowedAmount],
                            CheckPaidAmount = reader[StoredProcedureColumnsHelper.CheckPaidAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CheckPaidAmount],
                            CheckDateString = HasColumn(rows, StoredProcedureColumnsHelper.CheckDate) ?
                            (reader[StoredProcedureColumnsHelper.CheckDate] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.CheckDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            CheckNumber = reader[StoredProcedureColumnsHelper.CheckNumber] as string,
                            ReasonDescription = reader[StoredProcedureColumnsHelper.ReasonDescription] as string,
                            RemarkCode = reader[StoredProcedureColumnsHelper.RemarkCode] as string,
                            RemarkDescription = reader[StoredProcedureColumnsHelper.RemarkDescription] as string,
                            CoinsuranceAmount = reader[StoredProcedureColumnsHelper.CoinsuranceAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CoinsuranceAmount],
                            CopayAmount = reader[StoredProcedureColumnsHelper.CopayAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CopayAmount],
                            DeductibleAmount = reader[StoredProcedureColumnsHelper.DeductibleAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.DeductibleAmount],
                            CobAmount = reader[StoredProcedureColumnsHelper.CobAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CobAmount],
                            PenalityAmount = reader[StoredProcedureColumnsHelper.PenalityAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.PenalityAmount],
                            EligibilityStatus = reader[StoredProcedureColumnsHelper.EligibilityStatus] as string,
                            EligibilityInsurance = reader[StoredProcedureColumnsHelper.EligibilityInsurance] as string,
                            EligibilityPolicyNumber = reader[StoredProcedureColumnsHelper.EligibilityPolicyNumber] as string,
                            EligibilityFromDateString = HasColumn(rows, StoredProcedureColumnsHelper.EligibilityFromDate) ?
                            (reader[StoredProcedureColumnsHelper.CheckDate] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.CheckDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            VerifiedMemberId = reader[StoredProcedureColumnsHelper.VerifiedMemberId] as string,
                            CobLastVerifiedString = HasColumn(rows, StoredProcedureColumnsHelper.CobLastVerified) ?
                            (reader[StoredProcedureColumnsHelper.CobLastVerified] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.CobLastVerified]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            LastActiveEligibleDateRange = reader[StoredProcedureColumnsHelper.LastActiveEligibleDateRange] as string,
                            PrimaryPayer = reader[StoredProcedureColumnsHelper.PrimaryPayer] as string,
                            PrimaryPolicyNumber = reader[StoredProcedureColumnsHelper.PrimaryPolicyNumber] as string,
                            BatchNumber = reader[StoredProcedureColumnsHelper.BatchNumber] as string,
                            AitClaimReceivedDate = HasColumn(rows, StoredProcedureColumnsHelper.AitClaimReceivedDate) &&
                                            !reader.IsDBNull(StoredProcedureColumnsHelper.AitClaimReceivedDate) && DateTime.TryParseExact(reader[StoredProcedureColumnsHelper.AitClaimReceivedDate].ToString(), ClaimFiltersHelpers._dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) ? parsedDate.ToString(ClaimFiltersHelpers._dateFormat) : default(string),
                            AitClaimReceivedTime = HasColumn(rows, StoredProcedureColumnsHelper.AitClaimReceivedTime) ?
                                            (reader[StoredProcedureColumnsHelper.AitClaimReceivedTime] == DBNull.Value ?
                                            default(string) : reader[StoredProcedureColumnsHelper.AitClaimReceivedTime].ToString()) : default(string),

                            TransactionDate = HasColumn(rows, StoredProcedureColumnsHelper.TransactionDate) ?
                                                    (reader[StoredProcedureColumnsHelper.TransactionDate] == DBNull.Value ?
                                                    default(string) : reader[StoredProcedureColumnsHelper.TransactionDate].ToString()) : default(string),
                            TransactionTime = HasColumn(rows, StoredProcedureColumnsHelper.TransactionTime) ?
                                                    (reader[StoredProcedureColumnsHelper.TransactionTime] == DBNull.Value ?
                                                    default(string) : reader[StoredProcedureColumnsHelper.TransactionTime].ToString()) : default(string),
                            ClientProviderName = reader[StoredProcedureColumnsHelper.ClientProviderName] as string,
                            PaymentType = reader[StoredProcedureColumnsHelper.PaymentType] as string
                        };
                        claimStatusDashboardDetailsResponses.Add(dataItem);
                    }
                    reader.Close();
                }
                return claimStatusDashboardDetailsResponses;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelFilteredClaimsReport() //EN-66
        {
            // Create a new dictionary that defines the mapping of Excel report columns to corresponding properties in the ClaimStatusDateLagResponse.
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                {_localizer[StoredProcedureColumnsHelper.ExceptionReason], item => item.ExceptionReason },
                {_localizer[StoredProcedureColumnsHelper.PatientFirstName], item => item.PatientFirstName },
                {_localizer[StoredProcedureColumnsHelper.PatientLastName], item => item.PatientLastName },
                {_localizer[StoredProcedureColumnsHelper.DateOfBirth], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                {_localizer[StoredProcedureColumnsHelper.PolicyNumber], item => item.PolicyNumber },
                {_localizer[StoredProcedureColumnsHelper.ServiceType], item => item.ServiceType },
                {_localizer[StoredProcedureColumnsHelper.PayerName], item => item.PayerName },
                {_localizer[StoredProcedureColumnsHelper.OfficeClaimNumber], item => item.OfficeClaimNumber },
                {_localizer[StoredProcedureColumnsHelper.PayerClaimNumber], item => item.PayerClaimNumber },
                {_localizer[StoredProcedureColumnsHelper.PayerLineItemControlNumber], item => item.PayerLineItemControlNumber },
                {_localizer[StoredProcedureColumnsHelper.ProcedureCode], item => item.ProcedureCode },
                {_localizer[StoredProcedureColumnsHelper.DateOfServiceFrom], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                {_localizer[StoredProcedureColumnsHelper.DateOfServiceTo], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                {_localizer[StoredProcedureColumnsHelper.ClaimLineItemStatus], item => item.ClaimLineItemStatus },
                {_localizer[StoredProcedureColumnsHelper.ClaimLineItemStatusId], item => item.ClaimLineItemStatusId },
                {_localizer[StoredProcedureColumnsHelper.ClaimLineItemStatusValue], item => item.ClaimLineItemStatusValue },
                {_localizer[StoredProcedureColumnsHelper.ExceptionRemark], item => item.ExceptionRemark },
                {_localizer[StoredProcedureColumnsHelper.ReasonCode], item => item.ReasonCode },
                {_localizer[StoredProcedureColumnsHelper.ClaimBilledOn], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                {_localizer[StoredProcedureColumnsHelper.BilledAmount], item => item.BilledAmount.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.LineItemPaidAmount], item => item.LineItemPaidAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.TotalAllowedAmount], item => item.TotalAllowedAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.NonAllowedAmount], item => item.NonAllowedAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.CheckPaidAmount], item => item.CheckPaidAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.CheckDate], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDate?.ToString("MM/dd/yyyy") ?? null) },
                {_localizer[StoredProcedureColumnsHelper.CheckNumber], item => item.CheckNumber },
                {_localizer[StoredProcedureColumnsHelper.ReasonDescription], item => item.ReasonDescription },
                {_localizer[StoredProcedureColumnsHelper.RemarkCode], item => item.RemarkCode },
                {_localizer[StoredProcedureColumnsHelper.RemarkDescription], item => item.RemarkDescription },
                {_localizer[StoredProcedureColumnsHelper.CoinsuranceAmount], item => item.CoinsuranceAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.CopayAmount], item => item.CopayAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.DeductibleAmount], item => item.DeductibleAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.CobAmount], item => item.CobAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.PenalityAmount], item => item.PenalityAmount?.ToString("C", new CultureInfo("en-US")) },
                {_localizer[StoredProcedureColumnsHelper.EligibilityStatus], item => item.EligibilityStatus },
                {_localizer[StoredProcedureColumnsHelper.EligibilityInsurance], item => item.EligibilityInsurance },
                {_localizer[StoredProcedureColumnsHelper.EligibilityPolicyNumber], item => item.EligibilityPolicyNumber },
                {_localizer[StoredProcedureColumnsHelper.EligibilityFromDate], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.EligibilityFromDate?.ToString("MM/dd/yyyy") ?? null) },
                {_localizer[StoredProcedureColumnsHelper.VerifiedMemberId], item => item.VerifiedMemberId },
                {_localizer[StoredProcedureColumnsHelper.CobLastVerified], item => item.CobLastVerifiedString },
                {_localizer[StoredProcedureColumnsHelper.LastActiveEligibleDateRange], item => item.LastActiveEligibleDateRange },
                {_localizer[StoredProcedureColumnsHelper.PrimaryPayer], item => item.PrimaryPayer },
                {_localizer[StoredProcedureColumnsHelper.PrimaryPolicyNumber], item => item.PrimaryPolicyNumber },
                {_localizer[StoredProcedureColumnsHelper.BatchNumber], item => item.BatchNumber },
                {_localizer[StoredProcedureColumnsHelper.AitClaimReceivedDate], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.AitClaimReceivedTime], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                {_localizer[StoredProcedureColumnsHelper.TransactionDate], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.TransactionTime], item =>_excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                {_localizer[StoredProcedureColumnsHelper.ClientProviderName], item => item.ClientProviderName },
            };
            //if (!_jobCronManager.IsProductionEnvironment)
            //{
            //    exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            //}
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelDenialReport()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer[StoredProcedureColumnsHelper.Exception_Reason], item => item.ExceptionReason },
                { _localizer[StoredProcedureColumnsHelper.Last_Name], item => item.PatientLastName },
                { _localizer[StoredProcedureColumnsHelper.First_Name], item => item.PatientFirstName },
                { _localizer[StoredProcedureColumnsHelper.DOB], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.Provider], item => item.ClientProviderName },
                { _localizer[StoredProcedureColumnsHelper.Policy_Number], item => item.PolicyNumber },
                { _localizer[StoredProcedureColumnsHelper.Service_Type], item => item.ServiceType },
                { _localizer[StoredProcedureColumnsHelper.Payer_Name], item => item.PayerName },
                { _localizer[StoredProcedureColumnsHelper.OfficeClaimHash], item => item.OfficeClaimNumber },
                { _localizer[StoredProcedureColumnsHelper.InsClaimHash], item => item.PayerClaimNumber },
                { _localizer[StoredProcedureColumnsHelper.PayerLineItemControlNumber], item => item.PayerLineItemControlNumber },
                { _localizer[StoredProcedureColumnsHelper.DOS_From], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.DOS_TO], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.CPT_Code], item => item.ProcedureCode },
                { _localizer[StoredProcedureColumnsHelper.Billed_On], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.Allowed_Amt], item => item.TotalAllowedAmount?.ToString("C", new CultureInfo("en-US")) },
                { _localizer[StoredProcedureColumnsHelper.Billed_Amt], item => item.BilledAmount.ToString("C", new CultureInfo("en-US"))},
                { _localizer[StoredProcedureColumnsHelper.Lineitem_Status], item => item.ClaimLineItemStatus },
                //{ _localizer["Reported Lineitem Status"], item => item.ClaimLineItemStatusValue },
                { _localizer[StoredProcedureColumnsHelper.ExceptionReasonCategory], item => item.ExceptionReasonCategory },
                { _localizer[StoredProcedureColumnsHelper.Exception_Remark], item => item.ExceptionRemark },
                { _localizer[StoredProcedureColumnsHelper.Remark_Code], item => item.RemarkCode },
                { _localizer[StoredProcedureColumnsHelper.Remark_Description], item => item.RemarkDescription },
                { _localizer[StoredProcedureColumnsHelper.Reason_Code], item => item.ReasonCode },
                { _localizer[StoredProcedureColumnsHelper.Reason_Description], item => item.ReasonDescription },
                { _localizer[StoredProcedureColumnsHelper.Deductible_Amt], item => item.DeductibleAmount?.ToString("C", new CultureInfo("en-US")) },
                { _localizer[StoredProcedureColumnsHelper.Copay_Amt], item => item.CopayAmount?.ToString("C", new CultureInfo("en-US")) },
                { _localizer[StoredProcedureColumnsHelper.Coinsurance_Amt], item => item.CoinsuranceAmount?.ToString("C", new CultureInfo("en-US")) },
                { _localizer[StoredProcedureColumnsHelper.Penality_Amt], item => item.PenalityAmount?.ToString("C", new CultureInfo("en-US"))},
                { _localizer[StoredProcedureColumnsHelper.Lineitem_Paid_Amt], item => item.LineItemPaidAmount?.ToString("C", new CultureInfo("en-US"))},
                { _localizer[StoredProcedureColumnsHelper.CheckHash], item => item.CheckNumber },
                { _localizer[StoredProcedureColumnsHelper.Check_Date], item => item.CheckDateString },
                { _localizer[StoredProcedureColumnsHelper.CheckPaidAmt], item => item.CheckPaidAmount?.ToString("C", new CultureInfo("en-US")) },
                { _localizer[StoredProcedureColumnsHelper.Eligibility_Ins], item => item.EligibilityInsurance },
                { _localizer[StoredProcedureColumnsHelper.EligibilityPolicyHash], item => item.EligibilityPolicyNumber ?? string.Empty },
                { _localizer[StoredProcedureColumnsHelper.Eligibility_From_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.EligibilityFromDate?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.Eligibility_Status], item => item.EligibilityStatus },
                { _localizer[StoredProcedureColumnsHelper.VerifiedMemberId], item => item.VerifiedMemberId },
                { _localizer[StoredProcedureColumnsHelper.CobLastVerified], item => item.CobLastVerifiedString },
                { _localizer[StoredProcedureColumnsHelper.LastActiveEligibleDateRange], item => item.LastActiveEligibleDateRange },
                { _localizer[StoredProcedureColumnsHelper.PrimaryPayer], item => item.PrimaryPayer },
                { _localizer[StoredProcedureColumnsHelper.PrimaryPolicyNumber], item => item.PrimaryPolicyNumber },
                { _localizer[StoredProcedureColumnsHelper.PartA_EligibilityFrom], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartA_EligibilityTo], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartA_DeductibleFrom], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartA_DeductibleTo], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartA_RemainingDeductible], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartA_RemainingDeductible.HasValue ? item.PartA_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer[StoredProcedureColumnsHelper.PartB_EligibilityFrom], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartB_EligibilityTo], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartB_DeductibleFrom], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartB_DeductibleTo], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PartB_RemainingDeductible], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartB_RemainingDeductible.HasValue ? item.PartB_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer[StoredProcedureColumnsHelper.OtCapYearFrom], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.OtCapYearTo], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.OtCapUsedAmount], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.OtCapUsedAmount.HasValue ? item.OtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer[StoredProcedureColumnsHelper.PtCapYearFrom], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PtCapYearTo], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer[StoredProcedureColumnsHelper.PtCapUsedAmount], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PtCapUsedAmount.HasValue ? item.PtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer[StoredProcedureColumnsHelper.AIT_Batch_Hash], item => item.BatchNumber },
                { _localizer[StoredProcedureColumnsHelper.AIT_Received_Date], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                { _localizer[StoredProcedureColumnsHelper.AIT_Received_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                { _localizer[StoredProcedureColumnsHelper.AIT_Transaction_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                { _localizer[StoredProcedureColumnsHelper.AIT_Transaction_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                { _localizer[StoredProcedureColumnsHelper.Client_Location_Name], item => item.ClientLocationName },
                { _localizer[StoredProcedureColumnsHelper.Client_Location_Npi], item => item.ClientLocationNpi },
                { _localizer[StoredProcedureColumnsHelper.Quantity], item => item.Quantity },
                //{ _localizer[StoredProcedureColumnsHelper.Last_History_Created_On], item => item.LastHistoryCreatedOn != null ?  item.LastHistoryCreatedOn?.ToString("MM/dd/yyyy")  :  string.Empty },
                { _localizer[StoredProcedureColumnsHelper.ClaimStatusBatchClaimId], item => item.ClaimStatusBatchClaimId },
                { _localizer[StoredProcedureColumnsHelper.ClientLocationId], item => item.ClientLocationId },
                { _localizer[StoredProcedureColumnsHelper.ClientInsuranceId], item => item.ClientInsuranceId },
            };
            if (!_jobCronManager.IsProductionEnvironment)
            {
                exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            }
            return exportMapper;
        }

        #region not in use, may use later
        public static Dictionary<string, Func<ClaimStatusDashboardDetailsResponse, object>> GetExcelReport()
        {
            var type = typeof(ClaimStatusDashboardDetailsResponse);
            var properties = type.GetProperties();
            var excelReport = new Dictionary<string, Func<ClaimStatusDashboardDetailsResponse, object>>();

            foreach (var property in properties)
            {
                var excelAttribute = property.GetCustomAttribute<ExcelCustomAttribute>();
                if (excelAttribute != null)
                {
                    var propertyName = excelAttribute.PropertyName;
                    excelReport.Add(propertyName, item => property.GetValue(item));
                }
            }

            return excelReport;
        }
        #endregion

        #region Financial summary

        public async Task<ClaimSummary> GetFinancialSummaryDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            ClaimSummary response = new ClaimSummary();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    //SqlCommand cmd = CreateFinancialSummarySpCommand(StoreProcedureTitle.spGetFinancialSummaryTotals, conn, clientId, query);

                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetFinancialSummaryTotals, conn, clientId: clientId, delimitedLineItemStatusIds: query.CommaDelimitedLineItemStatusIds, clientInsuranceIds: query.ClientInsuranceIds, clientAuthTypeIds: query.AuthTypeIds, clientProcedureCodes: query.ProcedureCodes, clientExceptionReasonCategoryIds: query.ExceptionReasonCategoryIds, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, dateOfServiceFrom: query.DateOfServiceFrom, dateOfServiceTo: query.DateOfServiceTo, claimBilledFrom: query.ClaimBilledFrom, claimBilledTo: query.ClaimBilledTo, receivedFrom: query.ReceivedFrom, receivedTo: query.ReceivedTo, transactionDateFrom: query.TransactionDateFrom, transactionDateTo: query.TransactionDateTo, patientId: query.PatientId, claimStatusBatchId: query.ClaimStatusBatchId, flattenedLineItemStatus: query.FlattenedLineItemStatus);

                    //execute the sp
                    var financialSummaryTask = await ExecuteFinancialSymmarySp(cmd);

                    //wait for the task to complete and then map the result into response
                    //await Task.WhenAll(financialSummaryTask);
                    response = financialSummaryTask;
                }
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private SqlCommand CreateFinancialSymmarySpCommand(string spName, SqlConnection conn, int clientId, IClaimStatusDashboardStandardQuery filters)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, clientId);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, filters.CommaDelimitedLineItemStatusIds ?? String.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ProcedureCodes ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedFrom, filters.ReceivedFrom);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedTo, filters.ReceivedTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, filters.DateOfServiceFrom);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, filters.DateOfServiceTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateFrom, filters.TransactionDateFrom ?? null);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateTo, filters.TransactionDateTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, filters.ClaimBilledFrom ?? null);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, filters.ClaimBilledTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, filters.ClientLocationIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, filters.ClientProviderIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.PatientId, filters.PatientId == 0 ? null : filters.PatientId);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimStatusBatchId, filters.ClaimStatusBatchId);

                // Return the configured SqlCommand.
                return cmd;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<ClaimSummary> ExecuteFinancialSymmarySp(SqlCommand cmd) //AA-331
        {
            try
            {
                ClaimSummary financialSummary = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        financialSummary.ChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedTotals);
                        financialSummary.ChargedVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity);
                        financialSummary.PaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidTotals);
                        financialSummary.PaymentVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.PaidVisits);
                        financialSummary.ContractualTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ContractualTotals);
                        financialSummary.ContractualVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ContractualVisits);
                        financialSummary.WriteOffAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.WriteOffTotals);
                        financialSummary.WriteOffVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.WriteOffVisits);
                        financialSummary.ARBegining = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ArBeginning);
                        financialSummary.AREnding = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ArEnding);
                        financialSummary.AREndingVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ArEndingVisits);
                        financialSummary.ARBeginingVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ArBeginningVisits);
                    }
                    reader.Close();
                }
                return financialSummary;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Claims summary

        public async Task<ClaimDetailsSummary> GetClaimsSummaryDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            ClaimDetailsSummary response = new ClaimDetailsSummary();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimSummaryTotals, conn, clientId: clientId, delimitedLineItemStatusIds: query.CommaDelimitedLineItemStatusIds, clientInsuranceIds: query.ClientInsuranceIds, clientAuthTypeIds: query.AuthTypeIds, clientProcedureCodes: query.ProcedureCodes, clientExceptionReasonCategoryIds: query.ExceptionReasonCategoryIds, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, dateOfServiceFrom: query.DateOfServiceFrom, dateOfServiceTo: query.DateOfServiceTo, claimBilledFrom: query.ClaimBilledFrom, claimBilledTo: query.ClaimBilledTo, receivedFrom: query.ReceivedFrom, receivedTo: query.ReceivedTo, transactionDateFrom: query.TransactionDateFrom, transactionDateTo: query.TransactionDateTo, patientId: query.PatientId, claimStatusBatchId: query.ClaimStatusBatchId, flattenedLineItemStatus: query.FlattenedLineItemStatus);

                    //execute the sp
                    var claimsSummaryTask = ExecuteClaimsSymmarySp(cmd);

                    //wait for the task to complete and then map the result into response
                    await claimsSummaryTask;
                    response = claimsSummaryTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ClaimDetailsSummary> ExecuteClaimsSymmarySp(SqlCommand cmd) //EN-339
        {
            try
            {
                ClaimDetailsSummary dataItem = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        dataItem = new ClaimDetailsSummary()
                        {
                            ChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedTotals),
                            ChargedVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.TotalVisits),
                            PaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaymentTotals),
                            PaymentVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.PaymentVisits),
                            DenialTotals = GetDecimalValue(reader, rows, ReportConstants.DenialTotals),
                            DenialVisits = GetIntValue(reader, rows, ReportConstants.DenialVisits),
                            InProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.InProcessTotals),
                            InProcessVisits = GetIntValue(reader, rows, ReportConstants.InProcessVisits),
                            OpenTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedTotals),
                            OpenVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedVisits),
                            LastMonthPaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthPaymentTotals),
                            LastMonthDenialTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthDenialTotals),
                            LastMonthInProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthInProcessTotals),
                            LastMonthNotAdjudicatedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthNotAdjudicatedTotals),
                            LastMonthChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthChargedTotals),
                            CurrentMonthPaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthPaymentTotals),
                            CurrentMonthDenialTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthDenialTotals),
                            CurrentMonthInProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthInProcessTotals),
                            CurrentMonthNotAdjudicatedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthNotAdjudicatedTotals),
                            CurrentMonthChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthChargedTotals)
                        };
                    }
                    reader.Close();
                }
                return dataItem;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region AverageDaysToPayByPayer
        public async Task<List<AverageDaysByPayer>> GetAverageDaysToPayByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<AverageDaysByPayer> response = new List<AverageDaysByPayer>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetAvgDaysToPayByPayerData, conn, clientId: clientId, delimitedLineItemStatusIds: query.CommaDelimitedLineItemStatusIds, clientInsuranceIds: query.ClientInsuranceIds, clientAuthTypeIds: query.AuthTypeIds, clientProcedureCodes: query.ProcedureCodes, clientExceptionReasonCategoryIds: query.ExceptionReasonCategoryIds, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, dateOfServiceFrom: query.DateOfServiceFrom, dateOfServiceTo: query.DateOfServiceTo, claimBilledFrom: query.ClaimBilledFrom, claimBilledTo: query.ClaimBilledTo, receivedFrom: query.ReceivedFrom, receivedTo: query.ReceivedTo, transactionDateFrom: query.TransactionDateFrom, transactionDateTo: query.TransactionDateTo, patientId: query.PatientId, claimStatusBatchId: query.ClaimStatusBatchId, flattenedLineItemStatus: query.FlattenedLineItemStatus);

                    //execute the sp
                    var averageDaysToPayByPayerTask = await ExecuteAverageDaysToPayByPayerSp(cmd);

                    response = averageDaysToPayByPayerTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<AverageDaysByPayer>> ExecuteAverageDaysToPayByPayerSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<AverageDaysByPayer> averageDaysByPayers = new List<AverageDaysByPayer>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var daysByPayer = new AverageDaysByPayer()
                        {
                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfService = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            PayerId = HasColumn(rows, StoredProcedureColumnsHelper.PayerId) ? reader[StoredProcedureColumnsHelper.PayerId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.PayerId]
                                            : 0,
                            PayerName = HasColumn(rows, StoredProcedureColumnsHelper.PayerName) ? reader[StoredProcedureColumnsHelper.PayerName] as string
                                            : default(string),
                            DaysToPayByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.BilledToPayment) && reader[StoredProcedureColumnsHelper.BilledToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.BilledToPayment],
                            DaysToPayByServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToPayment) && reader[StoredProcedureColumnsHelper.ServiceToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToPayment],
                            DaysServiceToBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToBilled) && reader[StoredProcedureColumnsHelper.ServiceToBilled] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToBilled]
                        };
                        averageDaysByPayers.Add(daysByPayer);
                    }
                    reader.Close();
                }
                return averageDaysByPayers;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Charges By Payer
        public async Task<List<ChargesByPayer>> GetChargesByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ChargesByPayer> response = new List<ChargesByPayer>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp to get cash projection for last 7 days by passing the required parameters to the sp
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spChargesByPayer, conn, clientId, query);

                    //execute the sp
                    var chargesByPayerTask = await ExecuteChargesByPayerSp(cmd);

                    //wait for the task to complete and then map the result into response
                    response = chargesByPayerTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private SqlCommand CreateCommonClaimsQuery(string spName, SqlConnection conn, int clientId, IClaimStatusDashboardStandardQuery filters)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, clientId);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, filters.CommaDelimitedLineItemStatusIds ?? String.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ProcedureCodes ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedFrom, filters.ReceivedFrom);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedTo, filters.ReceivedTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, filters.DateOfServiceFrom);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, filters.DateOfServiceTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateFrom, filters.TransactionDateFrom ?? null);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateTo, filters.TransactionDateTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, filters.ClaimBilledFrom ?? null);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, filters.ClaimBilledTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, filters.ClientLocationIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, filters.ClientProviderIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.PatientId, filters.PatientId == 0 ? null : filters.PatientId);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimStatusBatchId, filters.ClaimStatusBatchId);

                // Return the configured SqlCommand.
                return cmd;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<ChargesByPayer>> ExecuteChargesByPayerSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<ChargesByPayer> chargesByPayers = new List<ChargesByPayer>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ChargesByPayer()
                        {
                            PayerName = HasColumn(rows, StoredProcedureColumnsHelper.PayerName) ? reader[StoredProcedureColumnsHelper.PayerName] as string
                                            : default(string),
                            PayerId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,
                            ChargesTotal = HasColumn(rows, StoredProcedureColumnsHelper.ChargedTotals) ? reader[StoredProcedureColumnsHelper.ChargedTotals] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedTotals]
                                            : 0.00m,
                            ClaimCount = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? reader[StoredProcedureColumnsHelper.Quantity] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.Quantity]
                                            : 0,

                        };
                        chargesByPayers.Add(charges);
                    }
                    reader.Close();
                }
                return chargesByPayers;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Payments Monthly Data
        public async Task<List<MonthlyClaimSummary>> GetPaymentsMonthlyDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<MonthlyClaimSummary> response = new List<MonthlyClaimSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spPaymentsMonthly, conn, clientId, query);

                    //execute the sp
                    var paymentsMonthlyTask = ExecutePaymentsMonthlySp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(paymentsMonthlyTask);
                    response = paymentsMonthlyTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private SqlCommand CreateMonthlySummarySpCommand(string spName, SqlConnection conn, int clientId, IClaimStatusDashboardStandardQuery filters)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, clientId);
                if (!string.IsNullOrEmpty(filters.CommaDelimitedLineItemStatusIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, filters.CommaDelimitedLineItemStatusIds ?? String.Empty);
                if (!string.IsNullOrEmpty(filters.ClientInsuranceIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
                if (!string.IsNullOrEmpty(filters.AuthTypeIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds ?? string.Empty);
                if (!string.IsNullOrEmpty(filters.ProcedureCodes)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ProcedureCodes ?? string.Empty);
                if (!string.IsNullOrEmpty(filters.ExceptionReasonCategoryIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds ?? string.Empty);
                if (filters.ReceivedFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedFrom, filters.ReceivedFrom);
                if (filters.ReceivedTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedTo, filters.ReceivedTo);
                if (filters.DateOfServiceFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, filters.DateOfServiceFrom);
                if (filters.DateOfServiceTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, filters.DateOfServiceTo);
                if (filters.TransactionDateFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateFrom, filters.TransactionDateFrom ?? null);
                if (filters.TransactionDateTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateTo, filters.TransactionDateTo);
                if (filters.ClaimBilledFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, filters.ClaimBilledFrom ?? null);
                if (filters.ClaimBilledTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, filters.ClaimBilledTo);
                if (!string.IsNullOrEmpty(filters.ClientLocationIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, filters.ClientLocationIds ?? string.Empty);
                if (!string.IsNullOrEmpty(filters.ClientProviderIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, filters.ClientProviderIds ?? string.Empty);
                if (filters.PatientId > 0) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.PatientId, filters.PatientId == 0 ? null : filters.PatientId);
                if (filters.ClaimStatusBatchId > 0) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimStatusBatchId, filters.ClaimStatusBatchId);

                // Return the configured SqlCommand.
                return cmd;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<MonthlyClaimSummary>> ExecutePaymentsMonthlySp(SqlCommand cmd) //AA-133
        {
            try
            {
                List<MonthlyClaimSummary> monthlyClaimSummary = new List<MonthlyClaimSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var claimData = new MonthlyClaimSummary()
                        {
                            ClaimLevelMD5Hash = HasColumn(rows, StoredProcedureColumnsHelper.ClaimLevelMd5Hash) ? reader[StoredProcedureColumnsHelper.ClaimLevelMd5Hash] as string : default(string),
                            PaymentTotals = HasColumn(rows, StoredProcedureColumnsHelper.PaymentTotals) ? reader[StoredProcedureColumnsHelper.PaymentTotals] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.PaymentTotals]
                                            : 0.00m,
                            ClaimCount = HasColumn(rows, StoredProcedureColumnsHelper.ClaimCount) ? reader[StoredProcedureColumnsHelper.ClaimCount] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClaimCount]
                                            : 0,
                            BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                            (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                            (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        monthlyClaimSummary.Add(claimData);
                    }
                    reader.Close();
                }
                return monthlyClaimSummary;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Monthly Denials Summary
        public async Task<List<MonthlyClaimSummary>> GetMonthlyDenialSummaryAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<MonthlyClaimSummary> response = new List<MonthlyClaimSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetMonthlyDenialsTask, conn, clientId, query);
                    var parameters = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);

                    //execute the sp
                    var paymentsMonthlyTask = ExecuteDenialsMonthlySummarySp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(paymentsMonthlyTask);
                    response = paymentsMonthlyTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<MonthlyClaimSummary>> ExecuteDenialsMonthlySummarySp(SqlCommand cmd) //AA-133
        {
            try
            {
                List<MonthlyClaimSummary> monthlyClaimSummary = new List<MonthlyClaimSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var claimData = new MonthlyClaimSummary()
                        {
                            ExceptionReasonCategoryId = HasColumn(rows, StoredProcedureColumnsHelper.ExceptionReasonCategoryId) ? reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId]
                                            : 0,
                            ClaimLevelMD5Hash = HasColumn(rows, StoredProcedureColumnsHelper.ClaimLevelMd5Hash) ? reader[StoredProcedureColumnsHelper.ClaimLevelMd5Hash] as string : default(string),
                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            DenialTotals = HasColumn(rows, StoredProcedureColumnsHelper.DenialTotals) ? reader[StoredProcedureColumnsHelper.DenialTotals] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.DenialTotals]
                                            : 0.00m,

                            ClaimLineItemStatusId = HasColumn(rows, StoredProcedureColumnsHelper.ClaimLineItemStatusId) ? reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId]
                                            : 0,

                            BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                            (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                            (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity),

                        };
                        monthlyClaimSummary.Add(claimData);
                    }
                    reader.Close();
                }
                return monthlyClaimSummary;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion



        #region AverageDaysToPayByProvider
        public async Task<List<AverageDaysByProvider>> GetAverageDaysToPayByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<AverageDaysByProvider> response = new List<AverageDaysByProvider>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetAvgDaysToPayByProvider, conn, clientId: clientId, delimitedLineItemStatusIds: query.CommaDelimitedLineItemStatusIds, clientInsuranceIds: query.ClientInsuranceIds, clientAuthTypeIds: query.AuthTypeIds, clientProcedureCodes: query.ProcedureCodes, clientExceptionReasonCategoryIds: query.ExceptionReasonCategoryIds, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, dateOfServiceFrom: query.DateOfServiceFrom, dateOfServiceTo: query.DateOfServiceTo, claimBilledFrom: query.ClaimBilledFrom, claimBilledTo: query.ClaimBilledTo, receivedFrom: query.ReceivedFrom, receivedTo: query.ReceivedTo, transactionDateFrom: query.TransactionDateFrom, transactionDateTo: query.TransactionDateTo, patientId: query.PatientId, claimStatusBatchId: query.ClaimStatusBatchId, flattenedLineItemStatus: query.FlattenedLineItemStatus);

                    //execute the sp
                    var averageDaysToPayByProviderTask = ExecuteAverageDaysToPayByProviderSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(averageDaysToPayByProviderTask);
                    response = averageDaysToPayByProviderTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<AverageDaysByProvider>> ExecuteAverageDaysToPayByProviderSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<AverageDaysByProvider> averageDaysByPayers = new List<AverageDaysByProvider>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var daysByPayer = new AverageDaysByProvider()
                        {
                            BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                            (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfService = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                            (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            ProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ProviderId) ? reader[StoredProcedureColumnsHelper.ProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ProviderId]
                                            : 0,
                            ProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ProviderLastCommaFirst) ? reader[StoredProcedureColumnsHelper.ProviderLastCommaFirst] as string
                                            : default(string),
                            DaysToPayByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.BilledToPayment) && reader[StoredProcedureColumnsHelper.BilledToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.BilledToPayment],
                            DaysToPayByServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToPayment) && reader[StoredProcedureColumnsHelper.ServiceToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToPayment],
                            DaysToServiceByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToBilled) && reader[StoredProcedureColumnsHelper.ServiceToBilled] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToBilled]
                        };
                        averageDaysByPayers.Add(daysByPayer);
                    }
                    reader.Close();
                }
                return averageDaysByPayers;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Charges By Payer
        public async Task<List<ChargesTotalsByProvider>> GetChargesByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ChargesTotalsByProvider> response = new List<ChargesTotalsByProvider>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spChargesByProvider, conn, clientId, query);

                    //execute the sp
                    var chargesByProviderTask = await ExecuteChargesByProviderSp(cmd);

                    response = chargesByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private async Task<List<ChargesTotalsByProvider>> ExecuteChargesByProviderSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<ChargesTotalsByProvider> chargesByProviders = new List<ChargesTotalsByProvider>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ChargesTotalsByProvider()
                        {
                            ProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ProviderLastCommaFirst) ? reader[StoredProcedureColumnsHelper.ProviderLastCommaFirst] as string
                                            : default(string),
                            ProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ProviderId) ? reader[StoredProcedureColumnsHelper.ProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ProviderId]
                                            : 0,
                            ChargesTotal = HasColumn(rows, StoredProcedureColumnsHelper.ChargedTotals) ? reader[StoredProcedureColumnsHelper.ChargedTotals] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedTotals]
                                            : 0.00m,
                            ClaimCount = HasColumn(rows, StoredProcedureColumnsHelper.ClaimCount) ? reader[StoredProcedureColumnsHelper.ClaimCount] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClaimCount]
                                            : 0,
                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ServiceDate = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        chargesByProviders.Add(charges);
                    }
                    reader.Close();
                }
                return chargesByProviders;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region ClaimsInProcessDateWise

        public async Task<List<ClaimProcessSummary>> GetClaimInProcessDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ClaimProcessSummary> response = new List<ClaimProcessSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetClaimInProcessDateWise, conn, clientId, query);

                    //execute the sp
                    var claimsInProcessTask = ExecuteClaimsInProcessSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(claimsInProcessTask);
                    response = claimsInProcessTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private async Task<List<ClaimProcessSummary>> ExecuteClaimsInProcessSp(SqlCommand cmd)
        {
            try
            {
                List<ClaimProcessSummary> claimsInProcess = new List<ClaimProcessSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ClaimProcessSummary()
                        {
                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),
                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,
                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,
                            BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                            (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                            (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                                            (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                                            (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        claimsInProcess.Add(charges);
                    }
                    reader.Close();
                }
                return claimsInProcess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region ClaimStatusTotalsDateWise


        public async Task<ClaimStatusTotalSummary> GetClaimStatusTotalsDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            ClaimStatusTotalSummary response = new ClaimStatusTotalSummary();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetClaimStatusTotals, conn, clientId, query);

                    //execute the sp
                    var claimsStatusTotalsTask = await ExecuteClaimsStatusTotalsTaskSp(cmd);

                    response = claimsStatusTotalsTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private async Task<ClaimStatusTotalSummary> ExecuteClaimsStatusTotalsTaskSp(SqlCommand cmd) //AA-137
        {
            try
            {
                ClaimStatusTotalSummary claimsStatusTotals = new ClaimStatusTotalSummary();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        claimsStatusTotals.PaymentVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.PaidVisits);
                        claimsStatusTotals.PaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidTotals);
                        claimsStatusTotals.DenialTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.DenialTotals);
                        claimsStatusTotals.DenialVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.DenialVisits);
                        claimsStatusTotals.WriteOffAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.WriteOffTotals);
                        claimsStatusTotals.WriteOffVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.WriteOffVisits);
                        claimsStatusTotals.OpenTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedTotals);
                        claimsStatusTotals.OpenVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedVisits);
                        claimsStatusTotals.ContractualTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ContractualTotals);
                        claimsStatusTotals.ContractualVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ContractualVisits);
                        claimsStatusTotals.InProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.InProcessTotals);
                        claimsStatusTotals.InProcessVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.InProcessVisits);
                    }
                    reader.Close();
                }
                return claimsStatusTotals;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region AvgAllowedAmtDateWise

        public async Task<List<AvgAllowedAmountSummary>> GetAvgAllowedAmtDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<AvgAllowedAmountSummary> response = new List<AvgAllowedAmountSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetAvgAllowedAmtDateWise, conn, clientId, query);

                    //execute the sp
                    var avgAllowedAmtTask = ExecuteAvgAllowedAmtSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(avgAllowedAmtTask);
                    response = avgAllowedAmtTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<AvgAllowedAmountSummary>> ExecuteAvgAllowedAmtSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<AvgAllowedAmountSummary> avgAllowedAmt = new List<AvgAllowedAmountSummary>();
                var parameters = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);

                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new AvgAllowedAmountSummary()
                        {
                            ClientInsuranceName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceName),
                            ClientInsuranceId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceId),
                            AllowedAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.AllowedAmount),
                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)
                        };
                        avgAllowedAmt.Add(charges);
                    }
                    reader.Close();
                }
                return avgAllowedAmt;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region DenialsByInsuranceDateWise

        public async Task<List<DenialClaimSummary>> GetDenialsByInsuranceDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialClaimSummary> response = new List<DenialClaimSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetDenialsByInsuranceDateWise, conn, clientId, query);

                    //execute the sp
                    var avgAllowedAmtTask = await ExecuteDenialsByInsuranceSp(cmd);

                    //wait for the task to complete and then map the result into response
                    response = avgAllowedAmtTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<DenialClaimSummary>> ExecuteDenialsByInsuranceSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<DenialClaimSummary> avgAllowedAmt = new List<DenialClaimSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new DenialClaimSummary();
                        try
                        {

                            charges.ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                                 : default(string);
                            charges.ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                                ? 0
                                                : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                                : 0;
                            charges.ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                                ? 0.00m
                                                : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                                : 0.00m;
                            charges.BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                                default(string) : (reader[StoreProcedureTitle.BilledOnDate].GetType().Name == "String" ? reader[StoreProcedureTitle.BilledOnDate].ToString() : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat))) : default(string);

                            charges.DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                                default(string) : (reader[StoreProcedureTitle.DateOfService].GetType().Name == "String" ? reader[StoreProcedureTitle.DateOfService].ToString() : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat))) : default(string);

                            charges.ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                                                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                                                default(string) : (reader[StoreProcedureTitle.ReceivedDate].GetType().Name == "String" ? reader[StoreProcedureTitle.ReceivedDate].ToString() : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat))) : default(string);

                            charges.DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                                                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                                                default(string) : (reader[StoreProcedureTitle.DateOfTransaction].GetType().Name == "String" ? reader[StoreProcedureTitle.DateOfTransaction].ToString() : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat))) : default(string);

                            charges.Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.DenialVisits);

                        }
                        catch (Exception ex)
                        {

                        }
                        avgAllowedAmt.Add(charges);
                    }
                    reader.Close();
                }
                return avgAllowedAmt;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region ReimbursementByLocation

        public async Task<List<ReimbursementByLocationSummary>> GetReimbursementByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)  //EN-229
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ReimbursementByLocationSummary> response = new List<ReimbursementByLocationSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetReimbursementByLocation, conn, clientId, query);

                    //execute the sp
                    var reimbursementByLocationTask = await ExecuteReimbursementByLocationSp(cmd);

                    response = reimbursementByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ReimbursementByLocationSummary>> ExecuteReimbursementByLocationSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ReimbursementByLocationSummary> reimbursementByLocation = new List<ReimbursementByLocationSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ReimbursementByLocationSummary()
                        {

                            ClientLocationName = HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationName) ? reader[StoredProcedureColumnsHelper.ClientLocationName] as string
                                                    : default(string),
                            ClientLocationId = HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationId) ? reader[StoredProcedureColumnsHelper.ClientLocationId] == System.DBNull.Value
                                                     ? 0
                                                     : (int)reader[StoredProcedureColumnsHelper.ClientLocationId]
                                                     : 0,
                            //ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                            //                         ? 0.00m
                            //                         : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                            //                         : 0.00m,
                            AllowedAmountSum = HasColumn(rows, StoredProcedureColumnsHelper.AllowedAmount) ? reader[StoredProcedureColumnsHelper.AllowedAmount] == System.DBNull.Value
                                                     ? 0.00m
                                                     : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmount]
                                                     : 0.00m,

                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),


                            Quantity = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? reader[StoredProcedureColumnsHelper.Quantity] == System.DBNull.Value
                                                     ? 0
                                                     : (int)reader[StoredProcedureColumnsHelper.Quantity]
                                                     : 0,

                        };
                        reimbursementByLocation.Add(charges);
                    }
                    reader.Close();
                }
                return reimbursementByLocation;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region ReimbursementByProvider

        public async Task<List<ReimbursementByProviderSummary>> GetReimbursementByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)  //EN-229
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ReimbursementByProviderSummary> response = new List<ReimbursementByProviderSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetReimbursementByProvider, conn, clientId, query);

                    //execute the sp
                    var reimbursementByProviderTask = ExecuteReimbursementByProviderSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(reimbursementByProviderTask);
                    response = reimbursementByProviderTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ReimbursementByProviderSummary>> ExecuteReimbursementByProviderSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ReimbursementByProviderSummary> reimbursementByProvider = new List<ReimbursementByProviderSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ReimbursementByProviderSummary()
                        {

                            ClientProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderName) ? reader[StoredProcedureColumnsHelper.ClientProviderName] as string
                                                    : default(string),
                            ClientProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderId) ? reader[StoredProcedureColumnsHelper.ClientProviderId] == System.DBNull.Value
                                                     ? 0
                                                     : (int)reader[StoredProcedureColumnsHelper.ClientProviderId]
                                                     : 0,
                            AllowedAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.AllowedAmount),
                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)

                        };
                        reimbursementByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return reimbursementByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region ProcedureTotalsByProvider
        public async Task<List<ProcedureTotalsByProvider>> GetProcedureTotalsByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProcedureTotalsByProvider> response = new List<ProcedureTotalsByProvider>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProcedureTotalsByProvider, conn, clientId, query);

                    //execute the sp
                    var proceduresByProviderTask = await ExecuteProcedureTotalsByProviderSp(cmd);

                    response = proceduresByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProcedureTotalsByProvider>> ExecuteProcedureTotalsByProviderSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ProcedureTotalsByProvider> proceduresByProvider = new List<ProcedureTotalsByProvider>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProcedureTotalsByProvider()
                        {
                            ProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderId) ? reader[StoredProcedureColumnsHelper.ClientProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientProviderId]
                                            : 0,

                            ProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderName) ? reader[StoredProcedureColumnsHelper.ClientProviderName] as string
                                            : default(string),

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity),

                            ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ProviderProcedureTotal>> GetProviderProcedureTotalAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProviderProcedureTotal> response = new List<ProviderProcedureTotal>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProviderProcedureTotal, conn, clientId, query);

                    //execute the sp
                    var proceduresByProviderTask = await ExecuteProviderProcedureTotalSp(cmd);

                    response = proceduresByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProviderProcedureTotal>> ExecuteProviderProcedureTotalSp(SqlCommand cmd)
        {
            try
            {
                List<ProviderProcedureTotal> proceduresByProvider = new List<ProviderProcedureTotal>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProviderProcedureTotal()
                        {
                            ProcedureCode = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ProcedureCode),

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity),

                            ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region InsuranceTotalsByProvider
        public async Task<List<InsuranceTotalsByProviderSummary>> GetInsuranceTotalsByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<InsuranceTotalsByProviderSummary> response = new List<InsuranceTotalsByProviderSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetInsuranceTotalsByProvider, conn, clientId, query);

                    //execute the sp
                    var proceduresByProviderTask = await ExecuteInsuranceTotalsByProviderSp(cmd);

                    response = proceduresByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<InsuranceTotalsByProviderSummary>> ExecuteInsuranceTotalsByProviderSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<InsuranceTotalsByProviderSummary> proceduresByProvider = new List<InsuranceTotalsByProviderSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new InsuranceTotalsByProviderSummary()
                        {
                            ClientProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderId) ? reader[StoredProcedureColumnsHelper.ClientProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientProviderId]
                                            : 0,

                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,

                            ClientProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderName) ? reader[StoredProcedureColumnsHelper.ClientProviderName] as string
                                            : default(string),

                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),

                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region DenialReasonsByProvider
        public async Task<List<DenialReasonsTotalsByProvider>> GetDenialReasonsByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialReasonsTotalsByProvider> response = new List<DenialReasonsTotalsByProvider>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetDenialReasonsByProvider, conn, clientId, query);

                    //execute the sp
                    var denialsByProviderTask = await ExecuteDenialReasonsByProviderSp(cmd);

                    response = denialsByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<DenialReasonsTotalsByProvider>> ExecuteDenialReasonsByProviderSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<DenialReasonsTotalsByProvider> denialsByProvider = new List<DenialReasonsTotalsByProvider>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new DenialReasonsTotalsByProvider()
                        {
                            ProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderId) ? reader[StoredProcedureColumnsHelper.ClientProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientProviderId]
                                            : 0,

                            ProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderName) ? reader[StoredProcedureColumnsHelper.ClientProviderName] as string
                                            : default(string),

                            ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)

                        };
                        denialsByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return denialsByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ProviderDenialReasonTotal>> GetProviderDenialReasonTotalAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProviderDenialReasonTotal> response = new List<ProviderDenialReasonTotal>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProviderDenialReasonTotal, conn, clientId, query);

                    //execute the sp
                    var denialsByProviderTask = await ExecuteProviderDenialReasonTotalSp(cmd);

                    response = denialsByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProviderDenialReasonTotal>> ExecuteProviderDenialReasonTotalSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ProviderDenialReasonTotal> denialsByProvider = new List<ProviderDenialReasonTotal>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProviderDenialReasonTotal()
                        {
                            ExceptionReasonCategoryId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ExceptionReasonCategoryId),
                            ExceptionReasonCategory = GetEnumValue<ClaimStatusExceptionReasonCategoryEnum>(reader, rows, StoredProcedureColumnsHelper.ExceptionReasonCategoryId).GetDescription(),
                            ChargedSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedSum),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity),
                            ProviderId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientProviderId),
                            ProviderName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientProviderName)
                        };

                        denialsByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return denialsByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region ProcedureReimbursement
        public async Task<List<ProcedureReimbursementByProvider>> GetProcedureReimbursementByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProcedureReimbursementByProvider> response = new List<ProcedureReimbursementByProvider>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProcedureReimbursementByProvider, conn, clientId, query);

                    //execute the sp
                    var proceduresByProviderTask = await ExecuteProcedureReimbursementByProviderSp(cmd);
                    response = proceduresByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProcedureReimbursementByProvider>> ExecuteProcedureReimbursementByProviderSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ProcedureReimbursementByProvider> proceduresByProvider = new List<ProcedureReimbursementByProvider>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProcedureReimbursementByProvider()
                        {
                            ClientProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderId) ? reader[StoredProcedureColumnsHelper.ClientProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientProviderId]
                                            : 0,

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                            ClientProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderName) ? reader[StoredProcedureColumnsHelper.ClientProviderName] as string
                                            : default(string),

                            AllowedAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.AllowedAmount),
                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)


                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region PayerReimbursement
        public async Task<List<PayerReimbursementByProviderSummary>> GetPayerReimbursementByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<PayerReimbursementByProviderSummary> response = new List<PayerReimbursementByProviderSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetPayerReimbursementByProvider, conn, clientId, query);

                    //execute the sp
                    var payerReimbursementByProviderTask = await ExecutePayerReimbursementByProviderSp(cmd);

                    response = payerReimbursementByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<PayerReimbursementByProviderSummary>> ExecutePayerReimbursementByProviderSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<PayerReimbursementByProviderSummary> proceduresByProvider = new List<PayerReimbursementByProviderSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new PayerReimbursementByProviderSummary()
                        {
                            ClientProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderId) ? reader[StoredProcedureColumnsHelper.ClientProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientProviderId]
                                            : 0,

                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,

                            ClientProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderName) ? reader[StoredProcedureColumnsHelper.ClientProviderName] as string
                                            : default(string),

                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),

                            AllowedAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.AllowedAmount),
                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Denials By Procedure
        public async Task<List<DenialsByProcedureSummary>> GetDenialByProcedureAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialsByProcedureSummary> response = new List<DenialsByProcedureSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetDenialsByProcedure, conn, clientId, query);
                    //execute the sp
                    var denialsByProcedureTask = await ExecuteDenialByProcedureSp(cmd);

                    //var filterData = denialsByProcedureTask.Where(z => z.ProcedureCode == "99214").GroupBy(z => z.ProcedureCode).Select(g => new { ProcedureCode = g.Key, ChargedTotalsSum = g.Sum(z => z.ChargedTotals) }).ToList();

                    response = denialsByProcedureTask;
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<DenialsByProcedureSummary>> ExecuteDenialByProcedureSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<DenialsByProcedureSummary> denialsByProcedure = new List<DenialsByProcedureSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new DenialsByProcedureSummary()
                        {
                            ExceptionReasonCategoryId = HasColumn(rows, StoredProcedureColumnsHelper.ExceptionReasonCategoryId) ? reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId]
                                            : 0,

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)
                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        denialsByProcedure.Add(charges);
                    }
                    reader.Close();
                }
                return denialsByProcedure;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Denials By Procedure
        public async Task<List<DenialReasonsByInsuranceSummary>> GetDenialByInsuranceAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialReasonsByInsuranceSummary> response = new List<DenialReasonsByInsuranceSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetDenialsByInsurance, conn, clientId, query);

                    //execute the sp
                    var denialsByInsuranceTask = await ExecuteDenialByInsuranceSp(cmd);

                    response = denialsByInsuranceTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<DenialReasonsByInsuranceSummary>> ExecuteDenialByInsuranceSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<DenialReasonsByInsuranceSummary> denialsByInsurance = new List<DenialReasonsByInsuranceSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new DenialReasonsByInsuranceSummary()
                        {
                            ExceptionReasonCategoryId = HasColumn(rows, StoredProcedureColumnsHelper.ExceptionReasonCategoryId) ? reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId]
                                            : 0,

                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,

                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),

                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.DenialVisits)

                        };
                        denialsByInsurance.Add(charges);
                    }
                    reader.Close();
                }
                return denialsByInsurance;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region Provider totals grouped by payers
        public async Task<List<PayerProviderTotals>> GetProviderTotalsByPayerQuery(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<PayerProviderTotals> response = new List<PayerProviderTotals>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProviderTotalsbyPayer, conn, clientId, query);

                    //execute the sp
                    var providerTotalsByPayerTask = await ExecuteProviderTotalsbyPayerSp(cmd);

                    response = providerTotalsByPayerTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<PayerProviderTotals>> ExecuteProviderTotalsbyPayerSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<PayerProviderTotals> providerTotalsByPayer = new List<PayerProviderTotals>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new PayerProviderTotals()
                        {
                            ProviderId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientProviderId),
                            ProviderName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientProviderName),
                            Charges = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedSum),
                            Visits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)
                        };
                        providerTotalsByPayer.Add(charges);
                    }
                    reader.Close();
                }
                return providerTotalsByPayer;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Payment totals by payers
        public async Task<List<PaymentSummary>> GetPaymentsTotalsByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<PaymentSummary> response = new List<PaymentSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetPaymentTotalsbyPayer, conn, clientId, query);

                    //execute the sp
                    var paymentTotalsByPayerTask = await ExecutePaymentTotalsbyPayerSp(cmd);

                    response = paymentTotalsByPayerTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<PaymentSummary>> ExecutePaymentTotalsbyPayerSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<PaymentSummary> paymentsByInsurances = new List<PaymentSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new PaymentSummary()
                        {
                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,

                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),

                            PaidAmountSum = HasColumn(rows, StoredProcedureColumnsHelper.PaidAmountSum) ? reader[StoredProcedureColumnsHelper.PaidAmountSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.PaidAmountSum]
                                            : 0.00m,

                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)

                        };
                        paymentsByInsurances.Add(charges);
                    }
                    reader.Close();
                }
                return paymentsByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Denial totals by payers
        public async Task<List<DenialTotalsByInsuranceSummary>> GetDenialTotalsByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialTotalsByInsuranceSummary> response = new List<DenialTotalsByInsuranceSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetDenialTotalsbyPayer, conn, clientId, query);

                    //execute the sp
                    var denialTotalsByPayerTask = ExecuteDenialTotalsbyPayerSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(denialTotalsByPayerTask);
                    response = denialTotalsByPayerTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<DenialTotalsByInsuranceSummary>> ExecuteDenialTotalsbyPayerSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<DenialTotalsByInsuranceSummary> denialByInsurances = new List<DenialTotalsByInsuranceSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new DenialTotalsByInsuranceSummary()
                        {
                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,

                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),

                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                            (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                            (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                                            (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                                            (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.DenialVisits)

                        };
                        denialByInsurances.Add(charges);
                    }
                    reader.Close();
                }
                return denialByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Get initial claims data
        public async Task<ClaimDetailsSummary> GetInitialClaimSummaryDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            ClaimDetailsSummary response = new ClaimDetailsSummary();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetInitialClaimsSummaryTotals, conn, clientId, query);

                    //execute the sp
                    var initialClaimSummaryData = ExecuteClaimsSymmarySp(cmd);

                    //wait for the task to complete and then map the result into response
                    await initialClaimSummaryData;
                    response = initialClaimSummaryData.Result;
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<DenialClaimSummary>> GetInitialDenialsByInsuranceAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialClaimSummary> response = new List<DenialClaimSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetInitialDenialsByInsurance, conn, clientId, query);

                    //execute the sp
                    var avgAllowedAmtTask = ExecuteDenialsByInsuranceSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(avgAllowedAmtTask);
                    response = avgAllowedAmtTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<DenialClaimSummary>> GetInitialInProcessClaimsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialClaimSummary> response = new List<DenialClaimSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetInitialInProcessClaims, conn, clientId, query);

                    //execute the sp
                    var avgAllowedAmtTask = ExecuteDenialsByInsuranceSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(avgAllowedAmtTask);
                    response = avgAllowedAmtTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        public async Task<List<ExportQueryResponse>> GetFilteredInProcessDetailsReportAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null)
        {
            if (clientId == 0)
            {
                clientId = _currentUserService.ClientId;
            }
            if (string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }
            List<ExportQueryResponse> response = [];

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new(connStr);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    if (string.IsNullOrWhiteSpace(filters.CommaDelimitedLineItemStatusIds))
                    {
                        filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName(filters.FlattenedLineItemStatus);
                    }
                    if (filters.DashboardType != ExportStoredProcedureColumnHelper.DashBoardType) { filters.DashboardType = string.Empty; }

                    //create the sp 
                    //StoreProcedureTitle.spGetClaimInProcessReport
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spDynamicExportDashboardQuery, conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId, dashboardType: filters.DashboardType);

                    var parameterList = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);

                    //execute the sp
                    //var claimStatusDashboardInProcessDetailsResponse = ExecuteFilteredInProcessReportSpCommand(cmd);
                    var claimExportDetail = await ExecuteClaimDynamicExportSpCommand(cmd);

                    //wait for the task to complete and then map the result into response
                    var finalResult = claimExportDetail;

                    response = ExportCommonMethod.SelectAndFilter(claimExportDetail,
                                       item => new ExportQueryResponse
                                       {
                                           PatientFirstName = item.PatientFirstName,
                                           PatientLastName = item.PatientLastName,
                                           PolicyNumber = item.PolicyNumber,
                                           ServiceType = item.ServiceType,
                                           PayerName = item.PayerName,
                                           OfficeClaimNumber = item.OfficeClaimNumber,
                                           ProcedureCode = item.ProcedureCode,
                                           DateOfServiceFromString = item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? "",
                                           DateOfServiceToString = item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? "",
                                           BatchNumber = item.BatchNumber,
                                           AitClaimReceivedDate = item.AitClaimReceivedDate,
                                           AitClaimReceivedTime = item.AitClaimReceivedTime,
                                           ClientProviderName = item.ClientProviderName,
                                           PaymentType = item.PaymentType,
                                           ClientLocationName = item.ClientLocationName,
                                           ClientLocationNpi = item.ClientLocationNpi,
                                           DateOfBirth = item.DateOfBirth,
                                           ClaimBilledOnString = DateTime.TryParse(item.ClaimBilledOnString, out DateTime claimBilledOn) ? claimBilledOn.ToString("MM/dd/yyyy") : "",
                                           BilledAmount = item.BilledAmount,
                                           ClaimLevelMd5Hash = item.ClaimLevelMd5Hash,
                                       }).ToList();
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task<List<ClaimStatusDashboardInProcessDetailsResponse>> ExecuteFilteredInProcessReportSpCommand(SqlCommand cmd) //EN-66
        {
            try
            {
                List<ClaimStatusDashboardInProcessDetailsResponse> claimStatusDashboardInProcessDetailsResponse = new List<ClaimStatusDashboardInProcessDetailsResponse>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var dataItem = new ClaimStatusDashboardInProcessDetailsResponse
                        {
                            PatientFirstName = reader[StoredProcedureColumnsHelper.PatientFirstName] as string,
                            PatientLastName = reader[StoredProcedureColumnsHelper.PatientLastName] as string,
                            PolicyNumber = reader[StoredProcedureColumnsHelper.PolicyNumber] as string,
                            ServiceType = reader[StoredProcedureColumnsHelper.ServiceType] as string,
                            PayerName = reader[StoredProcedureColumnsHelper.PayerName] as string,
                            OfficeClaimNumber = reader[StoredProcedureColumnsHelper.OfficeClaimNumber] as string,
                            ProcedureCode = reader[StoredProcedureColumnsHelper.ProcedureCode] as string,
                            DateOfServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.DateOfServiceFrom) ?
                            (reader[StoredProcedureColumnsHelper.DateOfServiceFrom] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.DateOfServiceFrom]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            DateOfServiceTo = HasColumn(rows, StoredProcedureColumnsHelper.DateOfServiceTo) ?
                            (reader[StoredProcedureColumnsHelper.DateOfServiceTo] == DBNull.Value ?
                            default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.DateOfServiceTo]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            BatchNumber = reader[StoredProcedureColumnsHelper.BatchNumber] as string,
                            AitClaimReceivedDate = HasColumn(rows, StoredProcedureColumnsHelper.AitClaimReceivedDate) &&
                                    !reader.IsDBNull(StoredProcedureColumnsHelper.AitClaimReceivedDate)
                                        ? DateTime.Parse(reader[StoredProcedureColumnsHelper.AitClaimReceivedDate].ToString()).ToString(ClaimFiltersHelpers._dateFormat)
                                        : default(string),
                            AitClaimReceivedTime = HasColumn(rows, StoredProcedureColumnsHelper.AitClaimReceivedTime) ?
                                            (reader[StoredProcedureColumnsHelper.AitClaimReceivedTime] == DBNull.Value ?
                                            default(string) : reader[StoredProcedureColumnsHelper.AitClaimReceivedTime].ToString()) : default(string),
                            ClientProviderName = reader[StoredProcedureColumnsHelper.ClientProviderName] as string,
                            PaymentType = reader[StoredProcedureColumnsHelper.PaymentType] as string,
                            ClientLocationName = reader[StoredProcedureColumnsHelper.ClientLocationName] as string,
                            ClientLocationNpi = reader[StoredProcedureColumnsHelper.ClientLocationNpi] as string
                        };
                        claimStatusDashboardInProcessDetailsResponse.Add(dataItem);
                    }
                    reader.Close();
                }
                return claimStatusDashboardInProcessDetailsResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region EN-231
        /// <summary>
        /// Retrieves claims data by calling the stored procedure for procedure summary.
        /// </summary>
        /// <param name="connectionString">The connection string to the database. If not provided, the default connection string from _tenantInfo is used.</param>
        /// <returns>A list of GetClaimByProcedureSummaryResponse objects representing the retrieved claims data.</returns>
        public async Task<List<GetClaimByProcedureSummaryResponse>> GetLastFourYearClaims(string connectionString = null)
        {
            List<GetClaimByProcedureSummaryResponse> response = new List<GetClaimByProcedureSummaryResponse>();

            if (string.IsNullOrEmpty(connectionString))
                connectionString = _tenantInfo.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand command = CreateClaimsByProcedureSummarySpCommand(StoreProcedureTitle.spGetLastFourYearClaims, connection);

                    var executeCommandTask = ExecuteGetClaimsByProcedureSummarySpCommand(command);

                    await Task.WhenAll(executeCommandTask);
                    response = executeCommandTask.Result;

                }
                catch (Exception ex)
                {
                    // Log and rethrow the exception
                    Console.WriteLine($"An error occurred while retrieving claims data: {ex}");
                    throw;
                }
            }
            return response;
        }

        #region Procedure totals by location
        public async Task<List<ProcedureTotalsbyLocationsSummary>> GetProcedureTotalsByLocationsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProcedureTotalsbyLocationsSummary> response = new List<ProcedureTotalsbyLocationsSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProcedureTotalsByLocation, conn, clientId, query);

                    //execute the sp
                    var procedureTotalsByLocationTask = await ExecuteProcedureTotalsbyLocationsSp(cmd);

                    response = procedureTotalsByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProcedureTotalsbyLocationsSummary>> ExecuteProcedureTotalsbyLocationsSp(SqlCommand cmd) //EN-312
        {
            try
            {
                List<ProcedureTotalsbyLocationsSummary> denialByInsurances = new List<ProcedureTotalsbyLocationsSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProcedureTotalsbyLocationsSummary()
                        {
                            ClientLocationId = HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationId) ? reader[StoredProcedureColumnsHelper.ClientLocationId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientLocationId]
                                            : 0,

                            ClientLocationName = HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationName) ? reader[StoredProcedureColumnsHelper.ClientLocationName] as string
                                            : default(string),

                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity),

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                        };
                        denialByInsurances.Add(charges);
                    }
                    reader.Close();
                }
                return denialByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Claim status dashboard data
        public async Task<ClaimStatusDashboardData> GetClaimStatusDashbaordDataAsync(IClaimStatusDashboardQueryBase filters, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            ClaimStatusDashboardData response = new ClaimStatusDashboardData();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    string spName = filters.HasProcedureDashboard.Value ? StoreProcedureTitle.spGetClaimStatusVisitsForProcedureDashboard : StoreProcedureTitle.spGetClaimStatusVisits;
                    //create the sp to get cash projection for last 7 days by passing the required parameters to the sp
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(spName, conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId, flattenedLineItemStatus: filters.FlattenedLineItemStatus);

                    var parameters = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);

                    //execute the sp
                    var dashboardData = await ExecuteClaimStatusVisitsSp(cmd);

                    //wait for the task to complete and then map the result into response
                    response = dashboardData;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ClaimStatusDashboardData> ExecuteClaimStatusVisitsSp(SqlCommand cmd) //EN-312
        {
            try
            {
                ClaimStatusDashboardData ClaimStatusDashboardData = new ClaimStatusDashboardData();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        ClaimStatusDashboardData = new ClaimStatusDashboardData()
                        {
                            ChargedVisit = GetIntValue(reader, rows, ReportConstants.Quantity),
                            ChargedTotals = GetDecimalValue(reader, rows, ReportConstants.BilledAmount),
                            AllowedAmountSum = GetDecimalValue(reader, rows, ReportConstants.AllowedAmountSum),
                            PaidAmountSum = GetDecimalValue(reader, rows, ReportConstants.PaidAmountSum),
                            AdjudicatedVisits = GetIntValue(reader, rows, ReportConstants.AdjudicatedVisits),
                            AdjudicatedTotals = GetDecimalValue(reader, rows, ReportConstants.AdjudicatedTotals),
                            AllowedVisits = GetIntValue(reader, rows, ReportConstants.AllowedVisits),
                            AllowedTotals = GetDecimalValue(reader, rows, ReportConstants.AllowedTotals),
                            PaidVisits = GetIntValue(reader, rows, ReportConstants.PaidVisits),
                            PaidTotals = GetDecimalValue(reader, rows, ReportConstants.PaidTotals),
                            ContractualTotals = GetDecimalValue(reader, rows, ReportConstants.ContractualTotals),
                            ContractualVisits = GetIntValue(reader, rows, ReportConstants.ContractualVisits),
                            DenialVisits = GetIntValue(reader, rows, ReportConstants.DenialVisits),
                            DenialTotals = GetDecimalValue(reader, rows, ReportConstants.DenialTotals),
                            WriteOffVisits = GetIntValue(reader, rows, ReportConstants.WriteOffVisits),
                            WriteOffTotals = GetDecimalValue(reader, rows, ReportConstants.WriteOffTotals),
                            SecPsTotals = GetDecimalValue(reader, rows, ReportConstants.SecPs),
                            OpenVisits = GetIntValue(reader, rows, ReportConstants.OpenVisits),
                            OpenTotals = GetDecimalValue(reader, rows, ReportConstants.TotalOpen),
                            InProcessVisits = GetIntValue(reader, rows, ReportConstants.InProcessVisits),
                            InProcessTotals = GetDecimalValue(reader, rows, ReportConstants.TotalInProcess),
                            TotalProcessed = GetDecimalValue(reader, rows, ReportConstants.TotalProcessed),
                        };
                    }
                    reader.Close();
                }
                return ClaimStatusDashboardData;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private int GetIntValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) && reader[columnName] != DBNull.Value
                ? (int)reader[columnName]
                : 0;
        }

        private decimal GetDecimalValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) && reader[columnName] != DBNull.Value
                ? (decimal)reader[columnName]
                : 0.00m;
        }

        private string GetStringValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ? reader[columnName] as string
                     : default(string);
        }

        private string GetDateStringValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ?
                    (reader[columnName] == DBNull.Value ?
                    default(string) : ((DateTime?)reader[columnName]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string);
        }
        private DateTime? GetDateTimeValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ?
                    (reader[columnName] == DBNull.Value ?
                    default : ((DateTime?)reader[columnName]).Value) : default;
        }

        public string GetTimeStringValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ?
                                            (reader[columnName] == DBNull.Value ?
                                            default(string) : reader[columnName].ToString()) : default(string);
        }
        private T GetEnumValue<T>(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            if (HasColumn(rows, columnName) && reader[columnName] != DBNull.Value)
            {
                object value = reader[columnName];
                if (typeof(T).IsEnum)
                {
                    // If T is an enum type, parse the value to the enum type
                    return (T)Enum.Parse(typeof(T), value.ToString());
                }
                else
                {
                    // Otherwise, directly cast to the specified type
                    return (T)value;
                }
            }
            else
            {
                return default(T);
            }
        }

        private bool GetBooleanValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ?
                                            (reader[columnName] == DBNull.Value ?
                                            false : reader.GetBoolean(reader.GetOrdinal(StoredProcedureColumnsHelper.FailureReported))) : false;
        }

        #endregion

        #region Insurance totals by location
        public async Task<List<InsuranceTotalsByLocationSummary>> GetInsuranceTotalsByLocationsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null) //EN-312
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<InsuranceTotalsByLocationSummary> response = new List<InsuranceTotalsByLocationSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetInsuranceTotalsByLocation, conn, clientId, query);

                    //execute the sp
                    var insuranceTotalsByLocationTask = await ExecuteInsuranceTotalsByLocationsSp(cmd);

                    response = insuranceTotalsByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<InsuranceTotalsByLocationSummary>> ExecuteInsuranceTotalsByLocationsSp(SqlCommand cmd) //EN-312
        {
            try
            {
                List<InsuranceTotalsByLocationSummary> denialByInsurances = new List<InsuranceTotalsByLocationSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new InsuranceTotalsByLocationSummary()
                        {
                            ClientLocationId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationId),
                            ClientLocationName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationName),
                            ChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedSum),
                            //BilledOnDate = GetDateStringValue(reader, rows, StoreProcedureTitle.BilledOnDate),
                            //DateOfServiceFrom = GetDateStringValue(reader, rows, StoreProcedureTitle.DateOfService),
                            //ReceivedFrom = GetDateStringValue(reader, rows, StoreProcedureTitle.ReceivedDate),
                            //DateOfTransactionFrom = GetDateStringValue(reader, rows, StoreProcedureTitle.DateOfTransaction),
                            ClientInsuranceId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceId),
                            ClientInsuranceName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceName),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)
                        };
                        denialByInsurances.Add(charges);
                    }
                    reader.Close();
                }
                return denialByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region Insurance totals by location
        public async Task<List<DenialReasonsByLocationsSummary>> GetDenialReasonssByLocationsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null) //EN-312
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialReasonsByLocationsSummary> response = new List<DenialReasonsByLocationsSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetDenialReasonsByLocation, conn, clientId, query);

                    //execute the sp
                    var insuranceTotalsByLocationTask = await ExecuteDenialReasonsByLocationsSp(cmd);

                    response = insuranceTotalsByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<DenialReasonsByLocationsSummary>> ExecuteDenialReasonsByLocationsSp(SqlCommand cmd) //EN-312
        {
            try
            {
                List<DenialReasonsByLocationsSummary> denialByInsurances = new List<DenialReasonsByLocationsSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new DenialReasonsByLocationsSummary()
                        {
                            ClientLocationId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationId),
                            ClientLocationName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationName),
                            ChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedSum),
                            //BilledOnDate = GetDateStringValue(reader, rows, StoreProcedureTitle.BilledOnDate),
                            //DateOfServiceFrom = GetDateStringValue(reader, rows, StoreProcedureTitle.DateOfService),
                            //ReceivedFrom = GetDateStringValue(reader, rows, StoreProcedureTitle.ReceivedDate),
                            //DateOfTransactionFrom = GetDateStringValue(reader, rows, StoreProcedureTitle.DateOfTransaction),
                            ExceptionReasonCategoryId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ExceptionReasonCategoryId),
                        };
                        denialByInsurances.Add(charges);
                    }
                    reader.Close();
                }
                return denialByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Insurance totals by location
        public async Task<List<ProcedureReimbursementByLocation>> GetProcedureReimbursementByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null) //EN-312
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProcedureReimbursementByLocation> response = new List<ProcedureReimbursementByLocation>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp to get cash projection for last  by passiing the required parameters to the sp
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProcedureReimbursementByLocation, conn, clientId, query);

                    //execute the sp to get the cash projection data for last 
                    var insuranceTotalsByLocationTask = await ExecuteProcedureReimbursementByLocationsSp(cmd);

                    response = insuranceTotalsByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProcedureReimbursementByLocation>> ExecuteProcedureReimbursementByLocationsSp(SqlCommand cmd) //EN-312
        {
            try
            {
                List<ProcedureReimbursementByLocation> denialByInsurances = new List<ProcedureReimbursementByLocation>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProcedureReimbursementByLocation()
                        {
                            ClientLocationId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationId),
                            ClientLocationName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationName),
                            AllowedAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.AllowedAmount),
                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity),
                            ProcedureCode = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ProcedureCode),
                        };
                        denialByInsurances.Add(charges);
                    }
                    reader.Close();
                }
                return denialByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region
        public async Task<List<PayerReimbursementSummary>> GetPayerReimbursementByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null) //EN-312
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<PayerReimbursementSummary> response = new List<PayerReimbursementSummary>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp to get cash projection for last  by passiing the required parameters to the sp
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetPayerReimbursementByLocation, conn, clientId, query);

                    //execute the sp to get the cash projection data for last 
                    var insuranceTotalsByLocationTask = await ExecutePayerReimbursementByLocationsSp(cmd);

                    response = insuranceTotalsByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<PayerReimbursementSummary>> ExecutePayerReimbursementByLocationsSp(SqlCommand cmd) //EN-312
        {
            try
            {
                List<PayerReimbursementSummary> denialByInsurances = new List<PayerReimbursementSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new PayerReimbursementSummary()
                        {
                            ClientLocationId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationId),
                            ClientLocationName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationName),
                            AllowedAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.AllowedAmount),
                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity),
                            ClientInsuranceId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceId),
                            ClientInsuranceName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceName)
                        };
                        denialByInsurances.Add(charges);
                    }
                    reader.Close();
                }
                return denialByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region AverageDaysToPayByLocation
        public async Task<List<AverageDaysByLocation>> GetAverageDaysToPayByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<AverageDaysByLocation> response = new List<AverageDaysByLocation>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 

                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetAvgDaysToPayByLocation, conn, clientId: clientId, delimitedLineItemStatusIds: query.CommaDelimitedLineItemStatusIds, clientInsuranceIds: query.ClientInsuranceIds, clientAuthTypeIds: query.AuthTypeIds, clientProcedureCodes: query.ProcedureCodes, clientExceptionReasonCategoryIds: query.ExceptionReasonCategoryIds, clientProviderIds: query.ClientProviderIds, clientLocationIds: query.ClientLocationIds, dateOfServiceFrom: query.DateOfServiceFrom, dateOfServiceTo: query.DateOfServiceTo, claimBilledFrom: query.ClaimBilledFrom, claimBilledTo: query.ClaimBilledTo, receivedFrom: query.ReceivedFrom, receivedTo: query.ReceivedTo, transactionDateFrom: query.TransactionDateFrom, transactionDateTo: query.TransactionDateTo, patientId: query.PatientId, claimStatusBatchId: query.ClaimStatusBatchId, providerLevelId: null, specialtyId: null, filterForDays: 0, filterBy: "", flattenedLineItemStatus: query.FlattenedLineItemStatus);

                    //execute the sp
                    var averageDaysToPayByLocationTask = ExecuteAverageDaysToPayByLocationSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(averageDaysToPayByLocationTask);
                    response = averageDaysToPayByLocationTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<AverageDaysByLocation>> ExecuteAverageDaysToPayByLocationSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<AverageDaysByLocation> averageDaysByLocation = new List<AverageDaysByLocation>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var result = new AverageDaysByLocation()
                        {
                            BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                            (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfService = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                            (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            LocationId = HasColumn(rows, StoredProcedureColumnsHelper.LocationId) ? reader[StoredProcedureColumnsHelper.LocationId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.LocationId]
                                            : 0,
                            LocationName = HasColumn(rows, StoredProcedureColumnsHelper.LocationName) ? reader[StoredProcedureColumnsHelper.LocationName] as string
                                            : default(string),
                            DaysToPayByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.BilledToPayment) && reader[StoredProcedureColumnsHelper.BilledToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.BilledToPayment],
                            DaysToPayByServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToPayment) && reader[StoredProcedureColumnsHelper.ServiceToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToPayment],
                            DaysToServiceByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToBilled) && reader[StoredProcedureColumnsHelper.ServiceToBilled] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToBilled]
                        };
                        averageDaysByLocation.Add(result);
                    }
                    reader.Close();
                }
                return averageDaysByLocation;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Charges By Location
        public async Task<List<ChargesTotalsByLocation>> GetChargesByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ChargesTotalsByLocation> response = new List<ChargesTotalsByLocation>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetChargesByLocation, conn, clientId, query);
                    var parameters = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);

                    //execute the sp
                    var chargesByLocationTask = await ExecuteChargesByLocationSp(cmd);

                    response = chargesByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ChargesTotalsByLocation>> ExecuteChargesByLocationSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<ChargesTotalsByLocation> chargesByLocations = new List<ChargesTotalsByLocation>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;

                        var charges = new ChargesTotalsByLocation()
                        {
                            LocationId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationId),
                            LocationName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientLocationName),
                            ChargesTotal = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedTotals),
                            ClaimCount = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClaimCount)
                        };
                        chargesByLocations.Add(charges);
                    }
                    reader.Close();
                }
                return chargesByLocations;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion
        private SqlCommand CreateClaimsByProcedureSummarySpCommand(string storedProcedureName, SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand(storedProcedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                return command;
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception
                Console.WriteLine($"An error occurred while creating stored procedure command: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Executes the stored procedure command to retrieve claims data and maps the results to a list of GetClaimByProcedureSummaryResponse objects.
        /// </summary>
        /// <param name="command">The SqlCommand object representing the stored procedure command to execute.</param>
        /// <returns>A list of GetClaimByProcedureSummaryResponse objects containing the retrieved claims data.</returns>
        private async Task<List<GetClaimByProcedureSummaryResponse>> ExecuteGetClaimsByProcedureSummarySpCommand(SqlCommand command)
        {
            try
            {
                List<GetClaimByProcedureSummaryResponse> response = new List<GetClaimByProcedureSummaryResponse>();
                using (command)
                {
                    if (command.Connection.State != ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var dataItem = new GetClaimByProcedureSummaryResponse();
                        dataItem.ClientId = reader["ClientId"] != DBNull.Value ? (int)reader["ClientId"] : default(int?);
                        dataItem.ClaimLineItemStatusId = reader["ClaimLineItemStatusId"] as int?;
                        dataItem.ClaimStatusExceptionReasonCategoryId = reader["ClaimStatusExceptionReasonCategoryId"] == DBNull.Value
                                                                                                        ? null
                                                                                                        : reader["ClaimStatusExceptionReasonCategoryId"] as string;
                        dataItem.ClientCptCodeId = reader["ClientCodeId"] != DBNull.Value ? (int)reader["ClientCodeId"] : default(int?);
                        dataItem.Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : default(int);
                        dataItem.ChargedSum = reader["ChargedSum"] != DBNull.Value ? (decimal)reader["ChargedSum"] : default(decimal);
                        dataItem.PaidAmountSum = reader["PaidAmountSum"] != DBNull.Value ? (decimal)reader["PaidAmountSum"] : default(decimal);
                        dataItem.AllowedAmountSum = reader["AllowedAmountSum"] != DBNull.Value ? (decimal)reader["AllowedAmountSum"] : default(decimal);
                        dataItem.NonAllowedAmountSum = reader["NonAllowedAmountSum"] != DBNull.Value ? (decimal)reader["NonAllowedAmountSum"] : default(decimal);
                        dataItem.ClientLocationId = reader["ClientLocationId"] != DBNull.Value ? (int)reader["ClientLocationId"] : default(int?);
                        dataItem.ClientProviderId = reader["ClientProviderId"] != DBNull.Value ? (int)reader["ClientProviderId"] : default(int?);
                        dataItem.WriteOffAmountSum = reader["WriteOffAmountSum"] != DBNull.Value ? (decimal)reader["WriteOffAmountSum"] : default(decimal);
                        dataItem.BatchProcessDate = reader["BilledOnDate"] != DBNull.Value ? (DateTime)reader["BilledOnDate"] : default(DateTime);
                        dataItem.DateOfServiceFrom = reader["DateOfServiceFrom"] != DBNull.Value ? (DateTime)reader["DateOfServiceFrom"] : default(DateTime);
                        dataItem.DateOfServiceTo = reader["DateOfServiceTo"] != DBNull.Value ? (DateTime)reader["DateOfServiceTo"] : default(DateTime);
                        dataItem.TransactionDate = reader["TransactionDate"] != DBNull.Value ? (DateTime)reader["TransactionDate"] : default(DateTime);
                        dataItem.ClaimReceivedDate = reader["AitClaimReceivedDate"] != DBNull.Value ? (DateTime)reader["AitClaimReceivedDate"] : default(DateTime);
                        dataItem.ClientInsuranceId = (int)(reader["ClientInsuranceId"] != DBNull.Value ? (int)reader["ClientInsuranceId"] : default(int?));
                        dataItem.BilledOnDate = reader["BilledOnDate"] != DBNull.Value ? (DateTime)reader["BilledOnDate"] : default(DateTime);
                        response.Add(dataItem);
                    }
                    reader.Close();
                }
                return response;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while executing stored procedure command: {ex}");
                // Rethrow the exception or handle it as appropriate
                throw;
            }
        }

        public async Task<List<GetClaimStatusTotalReportResponse>> GetClaimStatusTotalReportAsync(string tenantIdentifier)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tenantIdentifier))
                {
                    _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
                }

                var claimStatusTotalReportData = await this._unitOfWork.Repository<ClaimStatusTotalResult>().Entities
                                                                    .Specify(new ClaimStatusTotalReportSpecification(this._currentUserService.ClientId))
                                                     .Select(t => new GetClaimStatusTotalReportResponse()
                                                     {
                                                         ClientId = t.ClientId,
                                                         ClientCptCodeId = t.ClientCptCodeId,
                                                         DateOfServiceFrom = t.DateOfServiceFrom,
                                                         DateOfServiceTo = t.DateOfServiceTo,
                                                         ClaimLineItemStatusId = t.ClaimLineItemStatusId,
                                                         ClaimStatusExceptionReasonCategoryId = t.ClaimStatusExceptionReasonCategoryId,
                                                         WriteOffAmountSum = t.WriteOffAmountSum,
                                                         AllowedAmountSum = t.AllowedAmountSum,
                                                         BatchProcessDate = t.BatchProcessDate,
                                                         NonAllowedAmountSum = t.NonAllowedAmountSum,
                                                         ChargedSum = t.ChargedSum,
                                                         TransactionDate = t.TransactionDate,
                                                         ClientLocationId = t.ClientLocationId,
                                                         ClientProviderId = t.ClientProviderId,
                                                         ClaimReceivedDate = t.ClaimReceivedDate,
                                                         PaidAmountSum = t.PaidAmountSum,
                                                         Quantity = t.Quantity,
                                                         ProviderName = t.ClientProvider.Person.FirstName + " " + t.ClientProvider.Person.LastName,
                                                         IsDeleted = t.IsDeleted,
                                                         BilledOnDate = t.BilledOnDate,
                                                         ClientInsuranceId = t.ClientInsuranceId
                                                     }).ToListAsync();

                return claimStatusTotalReportData;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Table components for the procedure level dashbaord | EN-334

        private SqlCommand CreateAverageDaysToPayByPayerSpCommand(string spName, SqlConnection conn, int clientId, IClaimStatusDashboardStandardQuery filters)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, clientId);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, filters.CommaDelimitedLineItemStatusIds ?? String.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ProcedureCodes ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedFrom, filters.ReceivedFrom);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedTo, filters.ReceivedTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, filters.DateOfServiceFrom);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, filters.DateOfServiceTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateFrom, filters.TransactionDateFrom ?? null);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateTo, filters.TransactionDateTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, filters.ClaimBilledFrom ?? null);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, filters.ClaimBilledTo);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, filters.ClientLocationIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, filters.ClientProviderIds ?? string.Empty);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.PatientId, filters.PatientId == 0 ? null : filters.PatientId);
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimStatusBatchId, filters.ClaimStatusBatchId);

                // Return the configured SqlCommand.
                return cmd;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region InsuranceTotalsByProcedureCode EN-334
        public async Task<List<InsuranceTotalsByProcedureCode>> GetInsuranceTotalsByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<InsuranceTotalsByProcedureCode> response = new List<InsuranceTotalsByProcedureCode>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetInsuranceTotalsByProcedureCode, conn, clientId, query);

                    //execute the sp
                    var proceduresByProviderTask = await ExecuteInsuranceTotalsByProcedureCodeSp(cmd);

                    response = proceduresByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<InsuranceTotalsByProcedureCode>> ExecuteInsuranceTotalsByProcedureCodeSp(SqlCommand cmd) //EN-334
        {
            try
            {
                List<InsuranceTotalsByProcedureCode> proceduresByProvider = new List<InsuranceTotalsByProcedureCode>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new InsuranceTotalsByProcedureCode()
                        {

                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),

                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)

                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region ProviderTotalsByProcedure
        public async Task<List<ProviderTotalsByProcedure>> GetProviderTotalsByProcedureAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProviderTotalsByProcedure> response = new List<ProviderTotalsByProcedure>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProviderTotalsByProcedure, conn, clientId, query);

                    //execute the sp 
                    var proceduresByProviderTask = await ExecuteProviderTotalsByProcedureSp(cmd);

                    response = proceduresByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProviderTotalsByProcedure>> ExecuteProviderTotalsByProcedureSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ProviderTotalsByProcedure> proceduresByProvider = new List<ProviderTotalsByProcedure>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProviderTotalsByProcedure()
                        {
                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                            ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ProviderTotals>> GetProviderTotalsByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProviderTotals> response = new List<ProviderTotals>();

            string connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProviderTotalsByProcedureCode, conn, clientId, query);

                    //execute the sp 
                    var proceduresByProviderTask = await ExecuteProviderTotalsByProcedureCodeSp(cmd);

                    response = proceduresByProviderTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProviderTotals>> ExecuteProviderTotalsByProcedureCodeSp(SqlCommand cmd)
        {
            try
            {
                List<ProviderTotals> proceduresByProvider = new List<ProviderTotals>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProviderTotals()
                        {
                            ProviderId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientProviderId),
                            ProviderName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientProviderName),

                            ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DenialReasonByProcedureCode EN-334

        public async Task<List<DenialsByProcedureSummary>> GetDenialReasonsByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<DenialsByProcedureSummary> response = new List<DenialsByProcedureSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetDenialReasonsByProcedureCode, conn, clientId, query);

                    var denialReasonsByProcedureCode = await ExecuteDenialReasonsByProcedureCodeSp(cmd);

                    response = denialReasonsByProcedureCode;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<DenialsByProcedureSummary>> ExecuteDenialReasonsByProcedureCodeSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<DenialsByProcedureSummary> proceduresByProvider = new List<DenialsByProcedureSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new DenialsByProcedureSummary()
                        {

                            ExceptionReasonCategoryId = HasColumn(rows, StoredProcedureColumnsHelper.ExceptionReasonCategoryId) ? reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId]
                                            : 0,

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,

                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region PayerReimbursement By ProcedureCode EN-334
        public async Task<List<PayerReimbursementByProcedureCode>> GetPayerReimbursementByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<PayerReimbursementByProcedureCode> response = new List<PayerReimbursementByProcedureCode>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetPayerReimbursementByProcedureCode, conn, clientId, query);

                    //execute the sp
                    var payerReimbursementByProcedureCodeTask = await ExecutePayerReimbursementByProcedureCodeSp(cmd);

                    response = payerReimbursementByProcedureCodeTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<PayerReimbursementByProcedureCode>> ExecutePayerReimbursementByProcedureCodeSp(SqlCommand cmd) //EN-334
        {
            try
            {
                List<PayerReimbursementByProcedureCode> proceduresByProvider = new List<PayerReimbursementByProcedureCode>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new PayerReimbursementByProcedureCode()
                        {

                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),

                            AllowedAmountSum = HasColumn(rows, StoredProcedureColumnsHelper.AllowedAmount) ? reader[StoredProcedureColumnsHelper.AllowedAmount] == System.DBNull.Value
                                                     ? 0.00m
                                                     : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmount]
                                                     : 0.00m,

                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),


                            Quantity = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? reader[StoredProcedureColumnsHelper.Quantity] == System.DBNull.Value
                                                     ? 0
                                                     : (int)reader[StoredProcedureColumnsHelper.Quantity]
                                                     : 0,


                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region ProviderReimbursementByProcedureCode EN-334
        public async Task<List<ProviderReimbursementByProcedureCodeSummary>> GetProviderReimbursementByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProviderReimbursementByProcedureCodeSummary> response = new List<ProviderReimbursementByProcedureCodeSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetProcedureReimbursementByProvider, conn, clientId, query);

                    //execute the sp
                    var providerByProcedureTask = await ExecuteProviderReimbursementByProcedureCodeSp(cmd);

                    response = providerByProcedureTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProviderReimbursementByProcedureCodeSummary>> ExecuteProviderReimbursementByProcedureCodeSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ProviderReimbursementByProcedureCodeSummary> proceduresByProvider = new List<ProviderReimbursementByProcedureCodeSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProviderReimbursementByProcedureCodeSummary()
                        {
                            ClientProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderId) ? reader[StoredProcedureColumnsHelper.ClientProviderId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientProviderId]
                                            : 0,

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),

                            ClientProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ClientProviderName) ? reader[StoredProcedureColumnsHelper.ClientProviderName] as string
                                            : default(string),

                            AllowedAmountSum = HasColumn(rows, StoredProcedureColumnsHelper.AllowedAmount) ? reader[StoredProcedureColumnsHelper.AllowedAmount] == System.DBNull.Value
                                                     ? 0.00m
                                                     : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmount]
                                                     : 0.00m,

                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),


                            Quantity = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? reader[StoredProcedureColumnsHelper.Quantity] == System.DBNull.Value
                                                     ? 0
                                                     : (int)reader[StoredProcedureColumnsHelper.Quantity]
                                                     : 0,



                        };
                        proceduresByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return proceduresByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Charges By ProcedureCode EN-334
        public async Task<List<ChargesTotalsByProcedureCode>> GetChargesByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ChargesTotalsByProcedureCode> response = new List<ChargesTotalsByProcedureCode>();
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spChargesByProcedureCode, conn, clientId, query);

                    //execute the sp 
                    var chargesByProcedureCodeTask = await ExecuteChargesByProcedureCodeSp(cmd);

                    response = chargesByProcedureCodeTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private async Task<List<ChargesTotalsByProcedureCode>> ExecuteChargesByProcedureCodeSp(SqlCommand cmd) //EN-334
        {
            try
            {
                List<ChargesTotalsByProcedureCode> chargesByProviders = new List<ChargesTotalsByProcedureCode>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ChargesTotalsByProcedureCode()
                        {
                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),
                            ChargesTotal = HasColumn(rows, StoredProcedureColumnsHelper.ChargedTotals) ? reader[StoredProcedureColumnsHelper.ChargedTotals] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedTotals]
                                            : 0.00m,
                            ClaimCount = HasColumn(rows, StoredProcedureColumnsHelper.ClaimCount) ? reader[StoredProcedureColumnsHelper.ClaimCount] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClaimCount]
                                            : 0,
                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ServiceDate = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        chargesByProviders.Add(charges);
                    }
                    reader.Close();
                }
                return chargesByProviders;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region ReimbursementByProcedureCOde EENE-334

        public async Task<List<ProviderReimbursementByProcedureCodeSummary>> GetReimbursementByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)  //EN-334
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<ProviderReimbursementByProcedureCodeSummary> response = new List<ProviderReimbursementByProcedureCodeSummary>();
            //if (clientId == 0)
            //{
            //    clientId = _currentUserService.ClientId;
            //}
            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetReimbursementByProcedureCode, conn, clientId, query);
                    var parameterList = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);

                    //execute the sp
                    var reimbursementByProcedureTask = await ExecuteReimbursementByProcedureCodeSp(cmd);

                    response = reimbursementByProcedureTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ProviderReimbursementByProcedureCodeSummary>> ExecuteReimbursementByProcedureCodeSp(SqlCommand cmd) //EN-229
        {
            try
            {
                List<ProviderReimbursementByProcedureCodeSummary> reimbursementByProvider = new List<ProviderReimbursementByProcedureCodeSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ProviderReimbursementByProcedureCodeSummary()
                        {

                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                                    : default(string),
                            AllowedAmountSum = HasColumn(rows, StoredProcedureColumnsHelper.AllowedAmount) ? reader[StoredProcedureColumnsHelper.AllowedAmount] == System.DBNull.Value
                                                     ? 0.00m
                                                     : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmount]
                                                     : 0.00m,

                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),


                            Quantity = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? reader[StoredProcedureColumnsHelper.Quantity] == System.DBNull.Value
                                                     ? 0
                                                     : (int)reader[StoredProcedureColumnsHelper.Quantity]
                                                     : 0,


                        };
                        reimbursementByProvider.Add(charges);
                    }
                    reader.Close();
                }
                return reimbursementByProvider;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region AverageDaysToPayByProcedureCode
        public async Task<List<AverageDaysByProcedureCode>> GetAverageDaysToPayByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<AverageDaysByProcedureCode> response = new List<AverageDaysByProcedureCode>();

            if (string.IsNullOrEmpty(connStr))
                connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = CreateAverageDaysToPayByPayerSpCommand(StoreProcedureTitle.spGetAvgDaysToPayByProcedureCode, conn, clientId, query);

                    var averageDaysToPayByLocationTask = await ExecuteAverageDaysToPayByProcedureCodeSp(cmd);

                    response = averageDaysToPayByLocationTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<AverageDaysByProcedureCode>> ExecuteAverageDaysToPayByProcedureCodeSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<AverageDaysByProcedureCode> averageDaysByProcedureCode = new List<AverageDaysByProcedureCode>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var result = new AverageDaysByProcedureCode()
                        {
                            BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                                            (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            DateOfService = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                                            (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                                            default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string
                                            : default(string),
                            DaysToPayByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.BilledToPayment) && reader[StoredProcedureColumnsHelper.BilledToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.BilledToPayment],
                            DaysToPayByServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToPayment) && reader[StoredProcedureColumnsHelper.ServiceToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToPayment],
                            DaysToServiceByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToBilled) && reader[StoredProcedureColumnsHelper.ServiceToBilled] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToBilled]
                        };
                        averageDaysByProcedureCode.Add(result);
                    }
                    reader.Close();
                }
                return averageDaysByProcedureCode;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion


        #endregion

        #region Export SQL Quries handler | EN-235

        private async Task<List<DynamicExportQueryResponse>> ExecuteDashboardDynamicExportSpCommand(SqlCommand cmd)
        {
            try
            {
                // Initialize a list to store claim status date lag responses.
                List<DynamicExportQueryResponse> dynamicExportDetails = new();

                // Ensure that the database connection is open.
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var columnNames = GetColumnNamesFromDataRow(rows);

                        var dynamicExportQueryResponse = ExportCommonMethod.GetDynamicExportQueryResponse(reader);
                        if (dynamicExportQueryResponse is not null)
                        {
                            dynamicExportDetails.Add(dynamicExportQueryResponse);
                        }
                    }

                    // Close the SqlDataReader.
                    reader.Close();
                }

                // Return the list of claim status date lag responses.
                return dynamicExportDetails;
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur during database operations.
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<List<ExportQueryResponse>> ExecuteClaimDynamicExportSpCommand(SqlCommand cmd)
        {
            try
            {
                // Initialize a list to store claim status date lag responses.
                List<ExportQueryResponse> claimStatusDateLagTotals = new();

                // Ensure that the database connection is open.
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    var parameterList = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);


                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var columnNames = GetColumnNamesFromDataRow(rows);
                        var claimStatusDashboardDetailsResponse = new ExportQueryResponse();//StoredProcedureColumnsHelper

                        try
                        {

                            #region Read SQL Data fom reader.

                            claimStatusDashboardDetailsResponse.ExceptionReasonCategory = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusExceptionReasonCategory_Id) ? reader[ExportStoredProcedureColumnHelper.claimStatusExceptionReasonCategory_Id] == DBNull.Value ? string.Empty : ((ClaimStatusExceptionReasonCategoryEnum?)((int?)reader[ExportStoredProcedureColumnHelper.claimStatusExceptionReasonCategory_Id])).ToString() : string.Empty;

                            claimStatusDashboardDetailsResponse.ClaimStatusTypeId = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimLineItemStatus_ClaimStatusTypeId) ? reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatus_ClaimStatusTypeId] == DBNull.Value ? null : ((ClaimStatusTypeEnum?)((int)reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatus_ClaimStatusTypeId])).ToString() : null;

                            claimStatusDashboardDetailsResponse.ClaimStatusTransactionId = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimStatusBatchClaim_ClaimStatusTransactionId) ? reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatus_ClaimStatusTypeId] == DBNull.Value ? null : ((int?)reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatus_ClaimStatusTypeId]) : null;

                            //claimStatusDashboardDetailsResponse.ExceptionReasonCategory = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusExceptionReasonCategory_Code) ? reader[ExportStoredProcedureColumnHelper.claimStatusExceptionReasonCategory_Code] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.claimStatusExceptionReasonCategory_Code].ToString() : null;

                            claimStatusDashboardDetailsResponse.Quantity = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusBatchClaims_Quantity) ? reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_Quantity] == DBNull.Value ? 0 : (int)reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_Quantity] : 0;

                            claimStatusDashboardDetailsResponse.ExceptionReason = HasColumn(rows, ExportStoredProcedureColumnHelper.ExceptionReason) ? reader[ExportStoredProcedureColumnHelper.ExceptionReason] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ExceptionReason].ToString() : null;

                            claimStatusDashboardDetailsResponse.PatientLastName = HasColumn(rows, ExportStoredProcedureColumnHelper.PatientLastName) ? reader[ExportStoredProcedureColumnHelper.PatientLastName] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PatientLastName].ToString() : null;

                            claimStatusDashboardDetailsResponse.PatientFirstName = HasColumn(rows, ExportStoredProcedureColumnHelper.PatientFirstName) ? reader[ExportStoredProcedureColumnHelper.PatientFirstName] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PatientFirstName].ToString() : null;

                            claimStatusDashboardDetailsResponse.DateOfBirth = HasColumn(rows, ExportStoredProcedureColumnHelper.PatientDOB) ? reader[ExportStoredProcedureColumnHelper.PatientDOB] == DBNull.Value ? null : (DateTime?)reader[ExportStoredProcedureColumnHelper.PatientDOB] : null;

                            claimStatusDashboardDetailsResponse.DateOfBirthString = HasColumn(rows, ExportStoredProcedureColumnHelper.PatientDOB) ? reader[ExportStoredProcedureColumnHelper.PatientDOB] == DBNull.Value ? null : ((DateTime?)reader[ExportStoredProcedureColumnHelper.PatientDOB]).Value.ToString("MM/dd/yyyy") : null;

                            claimStatusDashboardDetailsResponse.PolicyNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusBatchClaims_PolicyNumber) ? reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_PolicyNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_PolicyNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.ServiceType = HasColumn(rows, ExportStoredProcedureColumnHelper.ServiceType) ? reader[ExportStoredProcedureColumnHelper.ServiceType] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ServiceType].ToString() : null;

                            claimStatusDashboardDetailsResponse.PayerName = HasColumn(rows, ExportStoredProcedureColumnHelper.PayerName) ? reader[ExportStoredProcedureColumnHelper.PayerName] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PayerName].ToString() : null;

                            claimStatusDashboardDetailsResponse.OfficeClaimNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.OfficeClaimNumber) ? reader[ExportStoredProcedureColumnHelper.OfficeClaimNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.OfficeClaimNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.PayerClaimNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.PayerClaimNumber) ? reader[ExportStoredProcedureColumnHelper.PayerClaimNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PayerClaimNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.PayerLineItemControlNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.PayerLineItemControlNumber) ? reader[ExportStoredProcedureColumnHelper.PayerLineItemControlNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PayerLineItemControlNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.ProcedureCode = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ProcedureCode) ? reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ProcedureCode] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ProcedureCode].ToString() : null;

                            claimStatusDashboardDetailsResponse.DateOfServiceFrom = HasColumn(rows, ExportStoredProcedureColumnHelper.DateOfServiceFrom) ? reader[ExportStoredProcedureColumnHelper.DateOfServiceFrom] == DBNull.Value ? null : (DateTime?)reader[ExportStoredProcedureColumnHelper.DateOfServiceFrom] : null;

                            claimStatusDashboardDetailsResponse.DateOfServiceFromString = HasColumn(rows, ExportStoredProcedureColumnHelper.DateOfServiceFrom) ? reader[ExportStoredProcedureColumnHelper.DateOfServiceFrom] == DBNull.Value ? null : ((DateTime?)reader[ExportStoredProcedureColumnHelper.DateOfServiceFrom]).Value.ToString("MM/dd/yyyy") : null;

                            claimStatusDashboardDetailsResponse.DateOfServiceTo = HasColumn(rows, ExportStoredProcedureColumnHelper.DateOfServiceTo) ? reader[ExportStoredProcedureColumnHelper.DateOfServiceTo] == DBNull.Value ? null : (DateTime?)reader[ExportStoredProcedureColumnHelper.DateOfServiceTo] : null;

                            claimStatusDashboardDetailsResponse.DateOfServiceToString = HasColumn(rows, ExportStoredProcedureColumnHelper.DateOfServiceTo) ? reader[ExportStoredProcedureColumnHelper.DateOfServiceTo] == DBNull.Value ? null : ((DateTime?)reader[ExportStoredProcedureColumnHelper.DateOfServiceTo]).Value.ToString("MM/dd/yyyy") : null;

                            claimStatusDashboardDetailsResponse.ClaimLineItemStatus = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimLineItemStatus) ? reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatus] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatus].ToString() : null;

                            claimStatusDashboardDetailsResponse.ClaimLineItemStatusId = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimLineItemStatusId) ? (reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatusId] == DBNull.Value ? null : (ClaimLineItemStatusEnum?)((int?)reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatusId])) ?? null : null;

                            claimStatusDashboardDetailsResponse.ClaimLineItemStatusValue = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimLineItemStatusValue) ? reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatusValue] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ClaimLineItemStatusValue].ToString() : null;

                            claimStatusDashboardDetailsResponse.ExceptionRemark = HasColumn(rows, ExportStoredProcedureColumnHelper.ExceptionRemark) ? reader[ExportStoredProcedureColumnHelper.ExceptionRemark] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ExceptionRemark].ToString() : null;

                            claimStatusDashboardDetailsResponse.ReasonCode = HasColumn(rows, ExportStoredProcedureColumnHelper.ReasonCode) ? reader[ExportStoredProcedureColumnHelper.ReasonCode] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ReasonCode].ToString() : null;

                            claimStatusDashboardDetailsResponse.ClaimBilledOn = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimBilledOn) ? reader[ExportStoredProcedureColumnHelper.ClaimBilledOn] == DBNull.Value ? null : (DateTime?)reader[ExportStoredProcedureColumnHelper.ClaimBilledOn] : null;

                            claimStatusDashboardDetailsResponse.ClaimBilledOnString = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimBilledOn) ? reader[ExportStoredProcedureColumnHelper.ClaimBilledOn] == DBNull.Value ? null : ((DateTime?)reader[ExportStoredProcedureColumnHelper.ClaimBilledOn]).Value.ToString("MM/dd/yyyy") : null;

                            claimStatusDashboardDetailsResponse.BilledAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.BilledAmount) ? (decimal)(reader[ExportStoredProcedureColumnHelper.BilledAmount] == DBNull.Value ? 0.0m : (decimal?)reader[ExportStoredProcedureColumnHelper.BilledAmount]) : 0.0m;

                            claimStatusDashboardDetailsResponse.LineItemPaidAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.LineItemPaidAmount) ? reader[ExportStoredProcedureColumnHelper.LineItemPaidAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.LineItemPaidAmount] : null;

                            claimStatusDashboardDetailsResponse.TotalAllowedAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.TotalAllowedAmount) ? reader[ExportStoredProcedureColumnHelper.TotalAllowedAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.TotalAllowedAmount] : null;

                            claimStatusDashboardDetailsResponse.NonAllowedAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.NonAllowedAmount) ? reader[ExportStoredProcedureColumnHelper.NonAllowedAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.NonAllowedAmount] : null;

                            claimStatusDashboardDetailsResponse.CheckPaidAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.CheckPaidAmount) ? reader[ExportStoredProcedureColumnHelper.CheckPaidAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.CheckPaidAmount] : null;

                            claimStatusDashboardDetailsResponse.CheckDateString = HasColumn(rows, ExportStoredProcedureColumnHelper.CheckDate) ? reader[ExportStoredProcedureColumnHelper.CheckDate] == DBNull.Value ? null : ((DateTime?)reader[ExportStoredProcedureColumnHelper.CheckDate])?.Date.ToString("MM/dd/yyyy") : null;

                            claimStatusDashboardDetailsResponse.CheckDate = HasColumn(rows, ExportStoredProcedureColumnHelper.CheckDate) ? reader[ExportStoredProcedureColumnHelper.CheckDate] == DBNull.Value ? null : (DateTime?)reader[ExportStoredProcedureColumnHelper.CheckDate] : null;

                            claimStatusDashboardDetailsResponse.CheckNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.CheckNumber) ? reader[ExportStoredProcedureColumnHelper.CheckNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.CheckNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.ReasonDescription = HasColumn(rows, ExportStoredProcedureColumnHelper.ReasonDescription) ? reader[ExportStoredProcedureColumnHelper.ReasonDescription] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ReasonDescription].ToString() : null;

                            claimStatusDashboardDetailsResponse.RemarkCode = HasColumn(rows, ExportStoredProcedureColumnHelper.RemarkCode) ? reader[ExportStoredProcedureColumnHelper.RemarkCode] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.RemarkCode].ToString() : null;

                            claimStatusDashboardDetailsResponse.RemarkDescription = HasColumn(rows, ExportStoredProcedureColumnHelper.RemarkDescription) ? reader[ExportStoredProcedureColumnHelper.RemarkDescription] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.RemarkDescription].ToString() : null;

                            claimStatusDashboardDetailsResponse.CoinsuranceAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.CoinsuranceAmount) ? reader[ExportStoredProcedureColumnHelper.CoinsuranceAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.CoinsuranceAmount] : null;

                            claimStatusDashboardDetailsResponse.CopayAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.CopayAmount) ? reader[ExportStoredProcedureColumnHelper.CopayAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.CopayAmount] : null;

                            claimStatusDashboardDetailsResponse.DeductibleAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.DeductibleAmount) ? reader[ExportStoredProcedureColumnHelper.DeductibleAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.DeductibleAmount] : null;

                            claimStatusDashboardDetailsResponse.CobAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.CobAmount) ? reader[ExportStoredProcedureColumnHelper.CobAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.CobAmount] : null;

                            claimStatusDashboardDetailsResponse.PenalityAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.PenalityAmount) ? reader[ExportStoredProcedureColumnHelper.PenalityAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.PenalityAmount] : null;

                            claimStatusDashboardDetailsResponse.EligibilityStatus = HasColumn(rows, ExportStoredProcedureColumnHelper.EligibilityStatus) ? reader[ExportStoredProcedureColumnHelper.EligibilityStatus] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.EligibilityStatus].ToString() : null;

                            claimStatusDashboardDetailsResponse.EligibilityInsurance = HasColumn(rows, ExportStoredProcedureColumnHelper.EligibilityInsurance) ? reader[ExportStoredProcedureColumnHelper.EligibilityInsurance] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.EligibilityInsurance].ToString() : null;

                            claimStatusDashboardDetailsResponse.EligibilityPolicyNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.EligibilityPolicyNumber) ? reader[ExportStoredProcedureColumnHelper.EligibilityPolicyNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.EligibilityPolicyNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.EligibilityFromDate = HasColumn(rows, ExportStoredProcedureColumnHelper.EligibilityFromDate) ? reader[ExportStoredProcedureColumnHelper.EligibilityFromDate] == DBNull.Value ? null : (DateTime?)reader[ExportStoredProcedureColumnHelper.EligibilityFromDate] : null;

                            claimStatusDashboardDetailsResponse.EligibilityFromDateString = HasColumn(rows, ExportStoredProcedureColumnHelper.EligibilityFromDate) ? reader[ExportStoredProcedureColumnHelper.EligibilityFromDate] == DBNull.Value ? null : ((DateTime?)reader[ExportStoredProcedureColumnHelper.EligibilityFromDate]).Value.ToString("MM/dd/yyyy") : null;

                            claimStatusDashboardDetailsResponse.VerifiedMemberId = HasColumn(rows, ExportStoredProcedureColumnHelper.VerifiedMemberId) ? reader[ExportStoredProcedureColumnHelper.VerifiedMemberId] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.VerifiedMemberId].ToString() : null;

                            claimStatusDashboardDetailsResponse.CobLastVerified = HasColumn(rows, ExportStoredProcedureColumnHelper.CobLastVerified) ? reader[ExportStoredProcedureColumnHelper.CobLastVerified] == DBNull.Value ? null : (DateTime?)reader[ExportStoredProcedureColumnHelper.CobLastVerified] : null;

                            claimStatusDashboardDetailsResponse.CobLastVerifiedString = HasColumn(rows, ExportStoredProcedureColumnHelper.CobLastVerified) ? reader[ExportStoredProcedureColumnHelper.CobLastVerified] == DBNull.Value ? null : (((DateTime?)reader[ExportStoredProcedureColumnHelper.CobLastVerified])?.Date.ToString("MM/dd/yyyy")) : null;

                            claimStatusDashboardDetailsResponse.LastActiveEligibleDateRange = HasColumn(rows, ExportStoredProcedureColumnHelper.LastActiveEligibleDateRange) ? reader[ExportStoredProcedureColumnHelper.LastActiveEligibleDateRange] == DBNull.Value ? string.Empty : reader[ExportStoredProcedureColumnHelper.LastActiveEligibleDateRange] as string : string.Empty;

                            claimStatusDashboardDetailsResponse.PrimaryPayer = HasColumn(rows, ExportStoredProcedureColumnHelper.PrimaryPayer) ? reader[ExportStoredProcedureColumnHelper.PrimaryPayer] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PrimaryPayer].ToString() : null;

                            claimStatusDashboardDetailsResponse.PrimaryPolicyNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.PrimaryPolicyNumber) ? reader[ExportStoredProcedureColumnHelper.PrimaryPolicyNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PrimaryPolicyNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.BatchNumber = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusBatches_BatchNumber) ? reader[ExportStoredProcedureColumnHelper.claimStatusBatches_BatchNumber] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.claimStatusBatches_BatchNumber].ToString() : null;

                            claimStatusDashboardDetailsResponse.AitClaimReceivedDate = HasColumn(rows, ExportStoredProcedureColumnHelper.AitClaimReceivedDate) ? reader[ExportStoredProcedureColumnHelper.AitClaimReceivedDate] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.AitClaimReceivedDate].ToString() : null;

                            claimStatusDashboardDetailsResponse.AitClaimReceivedTime = HasColumn(rows, ExportStoredProcedureColumnHelper.AitClaimReceivedTime) ? reader[ExportStoredProcedureColumnHelper.AitClaimReceivedTime] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.AitClaimReceivedTime].ToString() : null;

                            claimStatusDashboardDetailsResponse.TransactionDate = HasColumn(rows, ExportStoredProcedureColumnHelper.AitClaimReceivedDate) ? (reader[ExportStoredProcedureColumnHelper.AitClaimReceivedDate] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.AitClaimReceivedDate].ToString()) : null;

                            claimStatusDashboardDetailsResponse.TransactionTime = HasColumn(rows, ExportStoredProcedureColumnHelper.AitClaimReceivedTime) ? (reader[ExportStoredProcedureColumnHelper.AitClaimReceivedTime] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.AitClaimReceivedTime].ToString()) : null;

                            claimStatusDashboardDetailsResponse.ClientProviderName = HasColumn(rows, ExportStoredProcedureColumnHelper.ClientProviderName) ? reader[ExportStoredProcedureColumnHelper.ClientProviderName] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ClientProviderName].ToString() : null;

                            claimStatusDashboardDetailsResponse.ClientLocationName = HasColumn(rows, ExportStoredProcedureColumnHelper.ClientLocation_Name) ? reader[ExportStoredProcedureColumnHelper.ClientLocation_Name] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ClientLocation_Name].ToString() : null;

                            claimStatusDashboardDetailsResponse.ClientLocationNpi = HasColumn(rows, ExportStoredProcedureColumnHelper.ClientLocation_NPI) ? reader[ExportStoredProcedureColumnHelper.ClientLocation_NPI] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ClientLocation_NPI].ToString() : null;

                            claimStatusDashboardDetailsResponse.PaymentType = HasColumn(rows, ExportStoredProcedureColumnHelper.PaymentType) ? reader[ExportStoredProcedureColumnHelper.PaymentType] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.PaymentType].ToString() : null;

                            claimStatusDashboardDetailsResponse.ClaimStatusBatchClaimId = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimStatusBatchClaimId) ? reader[ExportStoredProcedureColumnHelper.ClaimStatusBatchClaimId] == DBNull.Value ? null : (int?)reader[ExportStoredProcedureColumnHelper.ClaimStatusBatchClaimId] : null;

                            claimStatusDashboardDetailsResponse.ClaimLevelMd5Hash = HasColumn(rows, ExportStoredProcedureColumnHelper.ClaimStatusBatchClaim_ClaimLevelMd5Hash) ? reader[ExportStoredProcedureColumnHelper.ClaimStatusBatchClaim_ClaimLevelMd5Hash] == DBNull.Value ? null : reader[ExportStoredProcedureColumnHelper.ClaimStatusBatchClaim_ClaimLevelMd5Hash].ToString() : null;

                            claimStatusDashboardDetailsResponse.LineItemChargeAmount = HasColumn(rows, ExportStoredProcedureColumnHelper.LineItemChargeAmount) ? reader[ExportStoredProcedureColumnHelper.LineItemChargeAmount] == DBNull.Value ? null : (decimal?)reader[ExportStoredProcedureColumnHelper.LineItemChargeAmount] : null;


                            claimStatusDashboardDetailsResponse.UpdatedClaimLineItemStatusId = HasColumn(rows, ExportStoredProcedureColumnHelper.UpdatedClaimLineItemStatusId) ? reader[ExportStoredProcedureColumnHelper.UpdatedClaimLineItemStatusId] == DBNull.Value ? null : ((ClaimLineItemStatusEnum?)((int?)reader[ExportStoredProcedureColumnHelper.UpdatedClaimLineItemStatusId])) : null;

                            claimStatusDashboardDetailsResponse.PreviousClaimLineItemStatusId = HasColumn(rows, ExportStoredProcedureColumnHelper.PreviousClaimLineItemStatusId) ? reader[ExportStoredProcedureColumnHelper.PreviousClaimLineItemStatusId] == DBNull.Value ? null : ((ClaimLineItemStatusEnum?)((int?)reader[ExportStoredProcedureColumnHelper.PreviousClaimLineItemStatusId])) : null;
                            
                            // For LastCheckedDate
                            claimStatusDashboardDetailsResponse.LastCheckedDate = HasColumn(rows, ExportStoredProcedureColumnHelper.LastCheckedDate)
                                ? reader[ExportStoredProcedureColumnHelper.LastCheckedDate] == DBNull.Value
                                    ? null
                                    : DateTime.TryParse(reader[ExportStoredProcedureColumnHelper.LastCheckedDate].ToString(), out var lastCheckedDate)
                                        ? lastCheckedDate.ToString("MM/dd/yyyy")
                                        : null
                                : null;

                            // For LastCheckedTime
                            claimStatusDashboardDetailsResponse.LastCheckedTime = HasColumn(rows, ExportStoredProcedureColumnHelper.LastCheckedTime)
                                ? reader[ExportStoredProcedureColumnHelper.LastCheckedTime] == DBNull.Value
                                    ? null
                                    : DateTime.TryParse(reader[ExportStoredProcedureColumnHelper.LastCheckedTime].ToString(), out var lastCheckedTime)
                                        ? lastCheckedTime.ToString("h:mm:ss")
                                        : null
                                : null;

                            //EN_769
                            claimStatusDashboardDetailsResponse.ClientInsuranceId = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ClientInsuranceId) ?
                                reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ClientInsuranceId] == DBNull.Value ? 0 :
                                (((int)reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ClientInsuranceId])) : 0;

                            //EN_769
                            claimStatusDashboardDetailsResponse.ClientLocationId = HasColumn(rows, ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ClientLocationId) ?
                                reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ClientLocationId] == DBNull.Value ? 0 :
                                (((int?)reader[ExportStoredProcedureColumnHelper.claimStatusBatchClaims_ClientLocationId])) : 0; 
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            throw;
                        }

                        claimStatusDateLagTotals.Add(claimStatusDashboardDetailsResponse);
                    }

                    //// Close the SqlDataReader.
                    //reader.Close();
                    // Close the SqlDataReader.
                    await reader.CloseAsync();
                }

                // Return the list of claim status date lag responses.
                return claimStatusDateLagTotals;
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur during database operations.
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<DynamicExportQueryResponse>> GetDynamicClaimExportDashboardQueryAsync(ExportDynamicClaimDashboardQuery filters, string connStr = "", int clientId = 0)
        {

            if (clientId == 0)
            {
                filters.ClientId = _currentUserService.ClientId;
            }
            else
            {
                filters.ClientId = clientId;
            }

            if (string.IsNullOrEmpty(connStr) || string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }
            List<DynamicExportQueryResponse> response = new();

            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new(connStr);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    if (string.IsNullOrWhiteSpace(filters.DelimitedLineItemStatusIds))
                    {
                        filters.DelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName(filters.FlattenedLineItemStatus);
                    }
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spDynamicExportDashboardQuery, conn, filters.ClientId, filters.DelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.ClientAuthTypeIds, filters.ClientProcedureCodes, filters.ClientExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId);

                    var claimStatusDashboardInProcessDetailsResponse = ExecuteDashboardDynamicExportSpCommand(cmd);

                    await claimStatusDashboardInProcessDetailsResponse;
                    response = claimStatusDashboardInProcessDetailsResponse.Result ?? new();

                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #endregion

        #region Get in process claims for the given tenant
        public async Task<List<ExportQueryResponse>> GetInProcessClaimsReportAsync(TenantDto tenant)
        {
            var connStr = tenant.ConnectionString;
            var response = new List<ExportQueryResponse>();
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = new(StoreProcedureTitle.spGetClaimInProcessMasterReport, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    List<ExportQueryResponse> inProcessClaims = await ExecuteInProcessClaimsSpCommand(cmd);

                    response = inProcessClaims;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ExportQueryResponse>> ExecuteInProcessClaimsSpCommand(SqlCommand cmd) //AA-137
        {
            try
            {
                List<ExportQueryResponse> inProcessClaimSummary = new List<ExportQueryResponse>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var result = new ExportQueryResponse()
                        {
                            ClientId = GetIntValue(reader, rows, StoreProcedureParamsHelper.ClientId),
                            ClientName = GetStringValue(reader, rows, CorporateDashboardConstants.ClientName),
                            ClientInsuranceId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.PayerId),
                            ClientInsuranceName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.PayerName),
                            ClaimCount = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClaimCount),
                            FailureReported = GetBooleanValue(reader, rows, StoredProcedureColumnsHelper.FailureReported),
                            IsExpiryWarningReported = GetBooleanValue(reader, rows, StoredProcedureColumnsHelper.ExpiryWarningReported),
                            //ClaimLevelMd5Hash = GetStringValue(reader,rows,StoredProcedureColumnsHelper.ClaimLevelMd5Hash)
                        };
                        inProcessClaimSummary.Add(result);
                    }
                    reader.Close();
                }
                return inProcessClaimSummary;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportInProcessReportExcel()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => item.DateOfBirth },
                { _localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["DOS From"], item => item.DateOfServiceFrom },
                { _localizer["DOS To"], item => item.DateOfServiceTo },
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => item.ClaimBilledOn },
                { _localizer["Billed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))) },
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => item.AitClaimReceivedDate },
                { _localizer["AIT Received Time"], item => item.AitClaimReceivedTime },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { _localizer["Payment Type"], item => item.PaymentType }
            };
            //if (!_jobCronManager.IsProductionEnvironment)
            //{
            //    exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            //}
            return exportMapper;
        }


        #endregion

        public void UpdateClaimStatusExceptionReasonCategoryForTenant(TenantDto tenant)
        {
            var connStr = tenant.ConnectionString;
            using SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();
                SqlCommand cmd = CreateStoredProcedureCommand(StoreProcedureTitle.spUpdateClaimStatusExceptionReasonCategory, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private SqlCommand CreateStoredProcedureCommand(string spName, SqlConnection conn)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn)
                {
                    // Set the command type to stored procedure.
                    CommandType = CommandType.StoredProcedure
                };

                // Add parameters to the SqlCommand based on the provided filters.
                // cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, clientId);

                // Return the configured SqlCommand.
                return cmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region
        public async Task<List<PayerTotalsByProvider>> GetPayerTotalsQuery(GetPayerTotalsQuery query)
        {
            //get client id from the current user service 
            int clientId = _currentUserService.ClientId;
            List<PayerTotalsByProvider> response = new List<PayerTotalsByProvider>();

            string connStr = _tenantInfo.ConnectionString;
            //initialize sql connection that will be used to execute the stored procedure
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = CreateCommonClaimsQuery(StoreProcedureTitle.spGetTotalsByPayer, conn, clientId, query);

                    //execute the sp
                    var providerTotalsByPayerTask = await ExecuteTotalsByPayerSp(cmd);

                    response = providerTotalsByPayerTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Dictionary<string, Func<ExportQueryResponse, object>> MapInProcessDetailsInSheet()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => item.DateOfBirth },
                {_localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["DOS From"], item => item.DateOfServiceFromString },
                { _localizer["DOS To"], item => item.DateOfServiceToString },
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => item.ClaimBilledOnString},
                { _localizer["Billed Amt"], item => string.Concat(CustomReportHelper.Currency, item.BilledAmount.ToString("C", new CultureInfo("en-US")))},
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => item.AitClaimReceivedDate },
                { _localizer["AIT Received Time"], item => item.AitClaimReceivedTime },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { _localizer["Payment Type"], item => item.PaymentType } //AA-324
			};

            if (!_jobCronManager.IsProductionEnvironment)
            {
                exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            }
            return exportMapper;
        }
        private async Task<List<PayerTotalsByProvider>> ExecuteTotalsByPayerSp(SqlCommand cmd)
        {
            try
            {
                List<PayerTotalsByProvider> providerTotalsByPayer = new List<PayerTotalsByProvider>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new PayerTotalsByProvider()
                        {

                            PayerId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceId),
                            PayerName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceName),
                            Charges = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedSum),
                            Visits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)
                        };
                        providerTotalsByPayer.Add(charges);
                    }
                    reader.Close();
                }
                return providerTotalsByPayer;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #endregion


        #region Avg Days To Pay Report.

        public async Task<List<ExportQueryResponse>> GetAverageDaysToPayReportAsync(ExportAvgDayToPayReportDetailsQuery filters, string connStr = "", int clientId = 0, bool hasAvgDayToPayByProvider = false)
        {
            var response = new List<ExportQueryResponse>();
            if (clientId == 0)
            {
                filters.ClientId = _currentUserService.ClientId;
            }
            else
            {
                filters.ClientId = clientId;
            }

            if (string.IsNullOrEmpty(connStr) || string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }

            SqlConnection conn = new(connStr);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    string spName = hasAvgDayToPayByProvider ? StoreProcedureTitle.spGetAverageDaysToPayByProvider : StoreProcedureTitle.spGetAvgDaysToPay;

                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(spName, conn, filters.ClientId, filters.DelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.ClientAuthTypeIds, filters.ClientProcedureCodes, filters.ClientExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId, flattenedLineItemStatus: filters.FlattenedLineItemStatus);
                    var report = await ExecuteAvgDaysToPayExportSpCommand(cmd);
                    response = report;
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ExportQueryResponse>> ExecuteAvgDaysToPayExportSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ExportQueryResponse> claimStatusDateLagTotals = new();

                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    var parameterList = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);


                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var columnNames = GetColumnNamesFromDataRow(rows);
                        var queryResponse = new ExportQueryResponse();//StoredProcedureColumnsHelper

                        try
                        {

                            #region Read SQL Data fom reader.

                            queryResponse.DateOfServiceFromString = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.DateOfServiceFrom);
                            queryResponse.ClaimBilledOnString = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.ClaimBilledOn);
                            queryResponse.CheckDateString = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.CheckDate);
                            queryResponse.AvgDaysToPay = GetIntValue(reader, rows, ExportStoredProcedureColumnHelper.AvgDaysToPay);
                            queryResponse.AvgDaysToBill = GetIntValue(reader, rows, ExportStoredProcedureColumnHelper.AvgDaysToBill);
                            queryResponse.AvgDaysfromDOStoPay = GetIntValue(reader, rows, ExportStoredProcedureColumnHelper.AvgDaysfromDOStoPay);
                            queryResponse.LastName = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.LastName);
                            queryResponse.FirstName = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.FirstName);
                            queryResponse.DateOfBirthString = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.DOB);
                            queryResponse.PolicyNumber = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.PolicyNumber);
                            queryResponse.PayerName = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.Payer_Name);
                            queryResponse.OfficeClaimNumber = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.Office_ClaimNumber);
                            queryResponse.CptCode = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.CptCode);
                            queryResponse.ProviderId = GetIntValue(reader, rows, ExportStoredProcedureColumnHelper.ProviderId);
                            queryResponse.ProviderNPI = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.ProviderNPI);
                            queryResponse.ProviderName = GetStringValue(reader, rows, ExportStoredProcedureColumnHelper.ProviderName);

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            throw;
                        }

                        claimStatusDateLagTotals.Add(queryResponse);
                    }

                    await reader.CloseAsync();
                }

                // Return the list of claim status date lag responses.
                return claimStatusDateLagTotals;
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur during database operations.
                Console.WriteLine(e);
                throw;
            }
        }


        public Dictionary<string, Func<ExportQueryResponse, object>> GetAvgDaysToPayExportDetailsExcel(ExportAvgDayToPayReportDetailsQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["DateOfServiceFrom"], item => item.DateOfServiceFromString },
                { _localizer["ClaimBilledOn"], item => item.ClaimBilledOnString },
                { _localizer["CheckDate"], item => item.CheckDateString },
                { _localizer["Avg Days to Pay"], item => item.AvgDaysToPay },
                { _localizer["Avg Days to Bill"], item => item.AvgDaysToBill },
                { _localizer["Total Days From DOS to Pay"], item => item.AvgDaysfromDOStoPay },
                { _localizer["Last Name"], item => item.LastName },
                { _localizer["First Name"], item => item.FirstName },
                { _localizer["DOB"], item => item.DateOfBirthString },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["CPT Code"], item => item.CptCode }
            };
            return exportMapper;
        }
        public Dictionary<string, Func<ExportQueryResponse, object>> GetAvgDaysToPayByProviderExportDetailsExcel(ExportAvgDayToPayReportDetailsQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["DateOfServiceFrom"], item => item.DateOfServiceFromString },
                { _localizer["ClaimBilledOn"], item => item.ClaimBilledOnString },
                { _localizer["CheckDate"], item => item.CheckDateString },
                { _localizer["Avg Days to Pay"], item => item.AvgDaysToPay },
                { _localizer["Avg Days to Bill"], item => item.AvgDaysToBill },
                { _localizer["Last Name"], item => item.LastName },
                { _localizer["First Name"], item => item.FirstName },
                { _localizer["DOB"], item => item.DateOfBirthString },
                { _localizer["Provider Name"], item => item.ProviderName },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["NPI"], item => item.ProviderNPI },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["CPT Code"], item => item.CptCode }
            };
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportAvgDayToPaySummaryExcel()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Payer"], item => item.PayerName },
                { _localizer["From DOS"], item => item.AverageDaysToBill },
                { _localizer["From Billed Date"], item => item.AverageDaysToPay },
                { _localizer["From DOS to Pay"], item => item.AverageDaysFromDosTOPay }
            };
            return exportMapper;
        }
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportAvgDayToPayByProviderSummaryExcel()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Provider"], item => item.ProviderName },
                { _localizer["From DOS"], item => item.AverageDaysToBill },
                { _localizer["From Billed Date"], item => item.AverageDaysToPay },
            };
            return exportMapper;
        }

        #endregion

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportFinicalSummaryExcel()
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Row Labels"], item => item.RowLabels },
                { _localizer["Count of Lineitem Status"], item => item.LineItemStatusCount },
                { _localizer["Sum of Billed Amt"], item => item.BilledAmountSum },
                {_localizer["Sum of Lineitem Paid Amt"], item => item.LineitemPaidAmtSum }
            };
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetPaymentAndProcedureCodeExportDetailsExcel(ExportCustomPaymentAndProcedureCodeQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Exception Reason"], item => item.ExceptionReason },
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["Ins Claim #"], item => item.PayerClaimNumber },
                { _localizer["Ins Lineitem Control #"], item => item.PayerLineItemControlNumber },
                { _localizer["DOS From"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["DOS To"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Quantity"], item => item.Quantity },//EN-172
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Billed Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { _localizer["Lineitem Status"], item => item.ClaimLineItemStatus },
                //{ _localizer["Reported Lineitem Status"], item => item.ClaimLineItemStatusValue },
                { _localizer["Exception Category"], item => item.ExceptionReasonCategory },
                { _localizer["Exception Remark"], item => item.ExceptionRemark },
                { _localizer["Remark Code"], item => item.RemarkCode },
                { _localizer["Remark Description"], item => item.RemarkDescription },
                { _localizer["Reason Code"], item => item.ReasonCode },
                { _localizer["Reason Description"], item => item.ReasonDescription },
                { _localizer["Deductible Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.DeductibleAmount.HasValue ? item.DeductibleAmount.Value.ToString("C", new CultureInfo("en-US")) :  "$0.00" )},
                { _localizer["Copay Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CopayAmount.HasValue ? item.CopayAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Coinsurance Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CoinsuranceAmount.HasValue ? item.CoinsuranceAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Penality Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PenalityAmount.HasValue ? item.PenalityAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Lineitem Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemPaidAmount.HasValue ? item.LineItemPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Allowed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.TotalAllowedAmount.HasValue ? item.TotalAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Non-Allowed Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.NonAllowedAmount.HasValue ? item.NonAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Check #"], item => item.CheckNumber },
                { _localizer["Check Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDateString ?? null) },
                { _localizer["Check Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CheckPaidAmount.HasValue ? item.CheckPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Eligibility Ins"], item => item.EligibilityInsurance },
                { _localizer["Eligibility Policy #"], item => item.EligibilityPolicyNumber },
                { _localizer["Eligibility From Date"], item => item.EligibilityFromDate },
                { _localizer["Eligibility Status"], item => item.EligibilityStatus },
                { _localizer["VerifiedMemberId"], item => item.VerifiedMemberId },
                { _localizer["CobLastVerified"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CobLastVerified?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["LastActiveEligibleDateRange"], item => item.LastActiveEligibleDateRange },
                { _localizer["PrimaryPayer"], item => item.PrimaryPayer },
                { _localizer["PrimaryPolicyNumber"], item => item.PrimaryPolicyNumber },
                { _localizer["PartA_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_EligibilityTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_DeductibleFrom "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_DeductibleTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartA_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartA_RemainingDeductible.HasValue ? item.PartA_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer["PartB_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_EligibilityTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_DeductibleFrom "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_DeductibleTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PartB_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartB_RemainingDeductible.HasValue ? item.PartB_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer["OtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["OtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["OtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.OtCapUsedAmount.HasValue ? item.OtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")},
                { _localizer["PtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["PtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PtCapUsedAmount.HasValue ? item.PtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => item.AitClaimReceivedDate },
                { _localizer["AIT Received Time"], item => item.AitClaimReceivedTime },
                { _localizer["AIT Transaction Date"], item => item.TransactionDate },
                { _localizer["AIT Transaction Time"], item => item.TransactionTime },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                { _localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { request.FlattenedLineItemStatus != "Denied" ? _localizer["Payment Type"] : "", item => request.FlattenedLineItemStatus != "Denied" ? item.PaymentType : "" }, //AA-324
                //{ _localizer["Last History Created On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastHistoryCreatedOn?.ToString("MM/dd/yyyy") ?? null)}, //EN-127
                { _localizer["ClaimStatusBatchId"], item => item.ClaimStatusBatchClaimId },
                { _localizer["LineItemChargeAmount"], item =>_excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemChargeAmount.HasValue ?  item.LineItemChargeAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")}
            };
            if (!_jobCronManager.IsProductionEnvironment)
            {
                exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            }
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportClaimStatusExcel(ExportClaimStatusDetailsQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Lineitem Status"], item => item.ClaimLineItemStatus },
                { _localizer["Exception Category"], item => item.ClaimStatusExceptionReasonCategory },
                { _localizer["Exception Reason"], item => item.ExceptionReason },
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["Ins Claim #"], item => item.PayerClaimNumber },
                { _localizer["Ins Lineitem Control #"], item => item.PayerLineItemControlNumber },
                { _localizer["DOS From"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["DOS To"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Quantity"], item => item.Quantity },//EN-172
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Billed Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { _localizer["Deductible Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.DeductibleAmount.HasValue ? item.DeductibleAmount.Value.ToString("C", new CultureInfo("en-US")) :  "$0.00" )},
                { _localizer["Copay Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CopayAmount.HasValue ? item.CopayAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Coinsurance Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CoinsuranceAmount.HasValue ? item.CoinsuranceAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Penality Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PenalityAmount.HasValue ? item.PenalityAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Lineitem Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemPaidAmount.HasValue ? item.LineItemPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Allowed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.TotalAllowedAmount.HasValue ? item.TotalAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Non-Allowed Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.NonAllowedAmount.HasValue ? item.NonAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Check #"], item => item.CheckNumber },
                { _localizer["Check Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDateString ?? null) },
                { _localizer["Check Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CheckPaidAmount.HasValue ? item.CheckPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                { _localizer["AIT Received Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                { _localizer["AIT Transaction Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                { _localizer["AIT Transaction Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                { _localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { request.FlattenedLineItemStatus != "Denied" ? _localizer["Payment Type"] : "", item => request.FlattenedLineItemStatus != "Denied" ? item.PaymentType : "" }, //AA-324
                { _localizer["ClaimStatusBatchId"], item => item.ClaimStatusBatchClaimId },
                { _localizer["LineItemChargeAmount"], item =>_excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemChargeAmount.HasValue ?  item.LineItemChargeAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")}

            };
            if (!_jobCronManager.IsProductionEnvironment)
            {
                exportMapper.Add(_localizer["ClaimLevelMd5Hash"], item => item.ClaimLevelMd5Hash);
            }
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportClaimStatusReportExcel(IClaimStatusDashboardStandardQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Exception Reason"], item => item.ExceptionReason },
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["Ins Claim #"], item => item.PayerClaimNumber },
                { _localizer["Ins Lineitem Control #"], item => item.PayerLineItemControlNumber },
                { _localizer["DOS From"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["DOS To"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Quantity"], item => item.Quantity },
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Billed Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { _localizer["Lineitem Status"], item => item.ClaimLineItemStatus },
                { _localizer["Exception Category"], item => item.ClaimStatusExceptionReasonCategory },
                { _localizer["Exception Remark"], item => item.ExceptionRemark },
                { _localizer["Remark Code"], item => item.RemarkCode },
                { _localizer["Remark Description"], item => item.RemarkDescription },
                { _localizer["Reason Code"], item => item.ReasonCode },
                { _localizer["Reason Description"], item => item.ReasonDescription },
                { _localizer["Deductible Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.DeductibleAmount.HasValue ? item.DeductibleAmount.Value.ToString("C", new CultureInfo("en-US")) :  "$0.00" )},
                { _localizer["Copay Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CopayAmount.HasValue ? item.CopayAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Coinsurance Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CoinsuranceAmount.HasValue ? item.CoinsuranceAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Penality Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PenalityAmount.HasValue ? item.PenalityAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Lineitem Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemPaidAmount.HasValue ? item.LineItemPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Allowed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.TotalAllowedAmount.HasValue ? item.TotalAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Non-Allowed Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.NonAllowedAmount.HasValue ? item.NonAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Check #"], item => item.CheckNumber },
                { _localizer["Check Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDateString ?? null) },
                { _localizer["Check Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CheckPaidAmount.HasValue ? item.CheckPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Eligibility Ins"], item =>  item.EligibilityInsurance },
                { _localizer["Eligibility Policy #"], item => item.EligibilityPolicyNumber },
                { _localizer["Eligibility From Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.EligibilityFromDateString ?? null)},
                { _localizer["Eligibility Status"], item => item.EligibilityStatus },
                { _localizer["VerifiedMemberId"], item => item.VerifiedMemberId },
                { _localizer["CobLastVerified"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CobLastVerifiedString ?? null) },
                { _localizer["LastActiveEligibleDateRange"], item => item.LastActiveEligibleDateRange },
                { _localizer["PrimaryPayer"], item => item.PrimaryPayer },
                { _localizer["PrimaryPolicyNumber"], item => item.PrimaryPolicyNumber },
                { _localizer["PartA_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityFrom.ToString() ?? null) },
                { _localizer["PartA_EligibilityTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityTo.ToString() ?? null) },
                { _localizer["PartA_DeductibleFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleFrom.ToString() ?? null) },
                { _localizer["PartA_DeductibleTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleTo.ToString() ?? null) },
                { _localizer["PartA_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartA_RemainingDeductible.HasValue ? item.PartA_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) :  string.Empty ) },
                { _localizer["PartB_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityFrom.ToString() ?? null) },
                { _localizer["PartB_EligibilityTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityTo.ToString() ?? null) },
                { _localizer["PartB_DeductibleFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleFrom.ToString() ?? null) },
                { _localizer["PartB_DeductibleTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleTo.ToString() ?? null) },
                { _localizer["PartB_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartB_RemainingDeductible.HasValue ? item.PartB_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) :  String.Empty ) },
                { _localizer["OtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearFrom.ToString() ?? null) },
                { _localizer["OtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearTo.ToString() ?? null) },
                { _localizer["OtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.OtCapUsedAmount.HasValue ? item.OtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) :  String.Empty ) },
                { _localizer["PtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearFrom.ToString() ?? null) },
                { _localizer["PtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearTo.ToString() ?? null) },
                { _localizer["PtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PtCapUsedAmount.HasValue ? item.PtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) :  String.Empty ) },
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                { _localizer["AIT Received Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                { _localizer["AIT Transaction Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                { _localizer["AIT Transaction Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                { _localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { request.FlattenedLineItemStatus != "Denied" ? _localizer["Payment Type"] : "", item => request.FlattenedLineItemStatus != "Denied" ? item.PaymentType : "" }, //AA-324
                { _localizer["ClaimStatusBatchId"], item => item.ClaimStatusBatchClaimId },
                { _localizer["LineItemChargeAmount"], item =>_excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemChargeAmount.HasValue ?  item.LineItemChargeAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")}
            };
            return exportMapper;
        }

        public Dictionary<string, Func<ExportQueryResponse, object>> GetExportInitialClaimStatusReportExcel(IInitialClaimStatusDashboardDetailsQuery request)
        {
            var exportMapper = new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Exception Reason"], item => item.ExceptionReason },
                { _localizer["Last Name"], item => item.PatientLastName },
                { _localizer["First Name"], item => item.PatientFirstName },
                { _localizer["DOB"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Policy Number"], item => item.PolicyNumber },
                { _localizer["Service Type"], item => item.ServiceType },
                { _localizer["Payer Name"], item => item.PayerName },
                { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                { _localizer["Ins Claim #"], item => item.PayerClaimNumber },
                { _localizer["Ins Lineitem Control #"], item => item.PayerLineItemControlNumber },
                { _localizer["DOS From"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["DOS To"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Quantity"], item => item.Quantity },
                { _localizer["CPT Code"], item => item.ProcedureCode },
                { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn?.ToString("MM/dd/yyyy") ?? null) },
                { _localizer["Billed Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US"))  ?? "$0.00")},
                { _localizer["Lineitem Status"], item => item.ClaimLineItemStatus },
                { _localizer["Exception Category"], item => item.ClaimStatusExceptionReasonCategory },
                { _localizer["Exception Remark"], item => item.ExceptionRemark },
                { _localizer["Remark Code"], item => item.RemarkCode },
                { _localizer["Remark Description"], item => item.RemarkDescription },
                { _localizer["Reason Code"], item => item.ReasonCode },
                { _localizer["Reason Description"], item => item.ReasonDescription },
                { _localizer["Deductible Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.DeductibleAmount.HasValue ? item.DeductibleAmount.Value.ToString("C", new CultureInfo("en-US")) :  "$0.00" )},
                { _localizer["Copay Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CopayAmount.HasValue ? item.CopayAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Coinsurance Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CoinsuranceAmount.HasValue ? item.CoinsuranceAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Penality Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PenalityAmount.HasValue ? item.PenalityAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Lineitem Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemPaidAmount.HasValue ? item.LineItemPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Allowed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.TotalAllowedAmount.HasValue ? item.TotalAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Non-Allowed Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.NonAllowedAmount.HasValue ? item.NonAllowedAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Check #"], item => item.CheckNumber },
                { _localizer["Check Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDateString ?? null) },
                { _localizer["Check Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CheckPaidAmount.HasValue ? item.CheckPaidAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00") },
                { _localizer["Eligibility Ins"], item =>  item.EligibilityInsurance },
                { _localizer["Eligibility Policy #"], item => item.EligibilityPolicyNumber },
                { _localizer["Eligibility From Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.EligibilityFromDateString ?? null)},
                { _localizer["Eligibility Status"], item => item.EligibilityStatus },
                { _localizer["VerifiedMemberId"], item => item.VerifiedMemberId },
                { _localizer["CobLastVerified"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CobLastVerifiedString ?? null) },
                { _localizer["LastActiveEligibleDateRange"], item => item.LastActiveEligibleDateRange },
                { _localizer["PrimaryPayer"], item => item.PrimaryPayer },
                { _localizer["PrimaryPolicyNumber"], item => item.PrimaryPolicyNumber },
                { _localizer["PartA_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityFrom.ToString() ?? null) },
                { _localizer["PartA_EligibilityTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityTo.ToString() ?? null) },
                { _localizer["PartA_DeductibleFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleFrom.ToString() ?? null) },
                { _localizer["PartA_DeductibleTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleTo.ToString() ?? null) },
                { _localizer["PartA_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartA_RemainingDeductible.HasValue ? item.PartA_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) :  string.Empty ) },
                { _localizer["PartB_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityFrom.ToString() ?? null) },
                { _localizer["PartB_EligibilityTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityTo.ToString() ?? null) },
                { _localizer["PartB_DeductibleFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleFrom.ToString() ?? null) },
                { _localizer["PartB_DeductibleTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleTo.ToString() ?? null) },
                { _localizer["PartB_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartB_RemainingDeductible.HasValue ? item.PartB_RemainingDeductible.Value.ToString("C", new CultureInfo("en-US")) :  String.Empty ) },
                { _localizer["OtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearFrom.ToString() ?? null) },
                { _localizer["OtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearTo.ToString() ?? null) },
                { _localizer["OtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.OtCapUsedAmount.HasValue ? item.OtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) :  String.Empty ) },
                { _localizer["PtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearFrom.ToString() ?? null) },
                { _localizer["PtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearTo.ToString() ?? null) },
                { _localizer["PtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PtCapUsedAmount.HasValue ? item.PtCapUsedAmount.Value.ToString("C", new CultureInfo("en-US")) :  String.Empty ) },
                { _localizer["AIT Batch #"], item => item.BatchNumber },
                { _localizer["AIT Received Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                { _localizer["AIT Received Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                { _localizer["AIT Transaction Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                { _localizer["AIT Transaction Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
               {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                { _localizer["Provider"], item => item.ClientProviderName },
                { _localizer["Client Location Name"], item => item.ClientLocationName },
                { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                { request.FlattenedLineItemStatus != "Denied" ? _localizer["Payment Type"] : "", item => request.FlattenedLineItemStatus != "Denied" ? item.PaymentType : "" }, //AA-324
                { _localizer["ClaimStatusBatchId"], item => item.ClaimStatusBatchClaimId },
                { _localizer["LineItemChargeAmount"], item =>_excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemChargeAmount.HasValue ?  item.LineItemChargeAmount.Value.ToString("C", new CultureInfo("en-US")) : "$0.00")}
            };
            return exportMapper;
        }


        public async Task<GetClientByIdResponse> GetByClientIdAsync(int clientId)
        {
            var client = new GetClientByIdResponse();

            try
            {
                using (var connection = new SqlConnection(_tenantInfo.ConnectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    await using (var cmd = new SqlCommand("spGetClientDetailsById", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClientId", clientId));

                        await using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            if (await reader.ReadAsync().ConfigureAwait(false))
                            {
                                client.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                                client.Name = reader.GetString(reader.GetOrdinal("Name"));
                                client.ClientCode = reader.IsDBNull(reader.GetOrdinal("ClientCode")) ? null : reader.GetString(reader.GetOrdinal("ClientCode"));
                                client.TaxId = reader.IsDBNull(reader.GetOrdinal("TaxId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("TaxId"));
                                client.PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("PhoneNumber"));
                                client.FaxNumber = reader.IsDBNull(reader.GetOrdinal("FaxNumber")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("FaxNumber"));
                                client.ClientKpiId = reader.IsDBNull(reader.GetOrdinal("ClientKpiId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ClientKpiId"));
                                client.SourceSystemId = reader.IsDBNull(reader.GetOrdinal("SourceSystemId")) ? (SourceSystemEnum?)null : (SourceSystemEnum)reader.GetInt32(reader.GetOrdinal("SourceSystemId"));
                                client.NpiNumber = reader.IsDBNull(reader.GetOrdinal("NpiNumber")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("NpiNumber"));
                                client.AddressId = reader.IsDBNull(reader.GetOrdinal("AddressId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AddressId"));
                                client.ClientQuestionnaireId = reader.IsDBNull(reader.GetOrdinal("ClientQuestionnaireId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ClientQuestionnaireId"));
                                client.IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) && reader.GetBoolean(reader.GetOrdinal("IsActive"));
                                client.InitialAnalysisEndOn = reader.IsDBNull(reader.GetOrdinal("InitialAnalysisEndOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("InitialAnalysisEndOn"));
                                client.AutoLogMinutes = reader.IsDBNull(reader.GetOrdinal("AutoLogMinutes")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("AutoLogMinutes"));

                                // Handle JSON columns
                                var clientSpecialtiesJson = reader.IsDBNull(reader.GetOrdinal("ClientSpecialties")) ? null : reader.GetString(reader.GetOrdinal("ClientSpecialties"));
                                var clientHolidaysJson = reader.IsDBNull(reader.GetOrdinal("ClientHolidays")) ? null : reader.GetString(reader.GetOrdinal("ClientHolidays"));
                                var clientDaysOfOperationJson = reader.IsDBNull(reader.GetOrdinal("ClientDaysOfOperation")) ? null : reader.GetString(reader.GetOrdinal("ClientDaysOfOperation"));
                                var employeeClientsJson = reader.IsDBNull(reader.GetOrdinal("EmployeeClients")) ? null : reader.GetString(reader.GetOrdinal("EmployeeClients"));
                                var clientApplicationFeaturesJson = reader.IsDBNull(reader.GetOrdinal("ClientApplicationFeatures")) ? null : reader.GetString(reader.GetOrdinal("ClientApplicationFeatures"));
                                var clientApiIntegrationKeysJson = reader.IsDBNull(reader.GetOrdinal("ClientApiIntegrationKeys")) ? null : reader.GetString(reader.GetOrdinal("ClientApiIntegrationKeys"));
                                var clientAuthTypesJson = reader.IsDBNull(reader.GetOrdinal("ClientAuthTypes")) ? null : reader.GetString(reader.GetOrdinal("ClientAuthTypes"));
                                var clientLocationsJson = reader.IsDBNull(reader.GetOrdinal("ClientLocations")) ? null : reader.GetString(reader.GetOrdinal("ClientLocations"));
                                var clientInsurancesJson = reader.IsDBNull(reader.GetOrdinal("ClientInsurances")) ? null : reader.GetString(reader.GetOrdinal("ClientInsurances"));
                                var clientKpiJson = reader.IsDBNull(reader.GetOrdinal("ClientKpi")) ? null : reader.GetString(reader.GetOrdinal("ClientKpi"));
                                // Deserialize JSON to collections
                                client.ClientSpecialties = JsonSerializer.Deserialize<List<ClientSpecialityDto>>(clientSpecialtiesJson ?? "[]");
                                client.ClientHolidays = JsonSerializer.Deserialize<List<ClientHoliday>>(clientHolidaysJson ?? "[]");
                                client.ClientDaysOfOperation = JsonSerializer.Deserialize<List<ClientDayOfOperation>>(clientDaysOfOperationJson ?? "[]");
                                client.EmployeeClients = JsonSerializer.Deserialize<List<EmployeeClientViewModel>>(employeeClientsJson ?? "[]");
                                client.ClientApplicationFeatures = JsonSerializer.Deserialize<List<ClientApplicationFeatureDto>>(clientApplicationFeaturesJson ?? "[]");
                                client.ClientApiIntegrationKeys = JsonSerializer.Deserialize<List<ClientApiIntegrationKeyDto>>(clientApiIntegrationKeysJson ?? "[]");
                                client.ClientAuthTypes = JsonSerializer.Deserialize<List<ClientAuthTypeDto>>(clientAuthTypesJson ?? "[]");
                                client.ClientLocations = JsonSerializer.Deserialize<List<ClientLocationDto>>(clientLocationsJson ?? "[]");
                                client.ClientInsurances = JsonSerializer.Deserialize<List<ClientInsuranceDto>>(clientInsurancesJson ?? "[]");
                                client.ClientKpi = string.IsNullOrEmpty(clientKpiJson) ? null : JsonSerializer.Deserialize<ClientKpiDto>(clientKpiJson);

                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
            }

            return client;
        }

    }
}