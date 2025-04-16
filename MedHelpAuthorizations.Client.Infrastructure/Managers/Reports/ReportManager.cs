using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports
{
    public class ReportManager : IReportManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ReportManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<string> ExportExpiringAuthorizationsToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportExpiringAuthorizations);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetExpiringAuthorizationsAsync(GetPagedExpiringAuthorizationsQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.GetExpiringAuthorizations(request));
            return await response.ToPaginatedResult<GetAllPagedAuthorizationsResponse>();
        }

        public async Task<string> ExportClaimStatusDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
            DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerids = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportClaimStatusDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo
                , transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId
                , exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, providerids, commaDelimitedLineItemStatusIds, hasGroupByKeySelector, claimStatusType, claimStatusTypeValue, dashboardType, fileName: fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> ExportClaimStatusDenialDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom,
            DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds, string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), int? denialClaimStatusType = null, string dashboardType = null, bool hasGroupByKeySelector = false, bool hasProcedureDashboard = false, string fileName = default(string))
        {
            try
            {
                var url = Routes.ReportEndpoints.ExportClaimStatusDenialDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, providerIds, denialClaimStatusType, dashboardType, hasGroupByKeySelector, hasProcedureDashboard, fileName);

                var response = await _tenantHttpClient.GetAsync(url);
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> ExportClaimStatusInProcessDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
            DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? claimBilledFrom, DateTime? claimBilledTo,
            string clientInsuranceIds, string authTypeIds, int claimStatusBatchId, int? patientId = 0, string procedureCodes = default(string), string ClaimLineItemStatusId = null,
            string locationIds = default(string), string providerIds = default(string), DateTime? transactionDateFrom = null, DateTime? transactionDateTo = null, string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportClaimStatusInProcessDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, claimBilledFrom,
                claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, procedureCodes, patientId, ClaimLineItemStatusId, locationIds, providerIds, transactionDateFrom, transactionDateTo, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> ExportInitialClaimStatusDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
            DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo,
            DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds,
            string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string),
            string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string), string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportInitialClaimStatusDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, transactionDateFrom,
                transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> ExportInitialClaimStatusDenialDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
            DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo,
            DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds,
            string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string),
            string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string), string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportInitialClaimStatusDenialDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, transactionDateFrom,
                transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> ExportInitialClaimStatusInProcessDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
            DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? claimBilledFrom, DateTime? claimBilledTo,
            string clientInsuranceIds, string authTypeIds, int claimStatusBatchId, int? patientId = 0, string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string), string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportInitialClaimStatusInProcessDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, claimBilledFrom,
                claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, procedureCodes, patientId, locationIds, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        public async Task<string> ExportARAgingDetailsToExcelAsync(ARAgingReportExportDetailsQuery query)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ReportEndpoints.ExportARAgingReportDetails, query);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> ExportARAgingSummaryToExcelAsync(ARAgingReportExportSummaryQuery query)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ReportEndpoints.ExportARAgingReportSummary, query);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ExportFilteredReportAsync(ExportFilteredReportQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ReportEndpoints.GetFilteredReport, query);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<string> ExportAvgDayToPayReportDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerids = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, bool hasAvgDayToPayByProvider = false, string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportAvgDayToPayReportDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, providerids, commaDelimitedLineItemStatusIds, hasGroupByKeySelector, claimStatusType, claimStatusTypeValue, dashboardType, hasAvgDayToPayByProvider, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> FinicalSummaryExportsDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
            DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerids = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.FinicalSummaryExportDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo
                , transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId
                , exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, providerids, commaDelimitedLineItemStatusIds, hasGroupByKeySelector, claimStatusType, claimStatusTypeValue, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }


        public async Task<string> ExportPaymentClaimStatusDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
        DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerids = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportPaymentClaimStatusDetails(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo
                , transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId
                , exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, providerids, commaDelimitedLineItemStatusIds, hasGroupByKeySelector, claimStatusType, claimStatusTypeValue, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public  async Task<string> ExportClaimStatusReportExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = null, string authTypeIds = null, int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = null, string procedureCodes = null, string locationIds = null, string providerIds = null, string commaDelimitedLineItemStatusIds = null, bool hasGroupByKeySelector = false, ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = null, string dashboardType = null, string fileName = null)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportClaimStatusReport(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo
                , transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId
                , exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, providerIds, commaDelimitedLineItemStatusIds, hasGroupByKeySelector, claimStatusType, claimStatusTypeValue, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> ExportClaimStatusDenialReportExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportClaimStatusDenialReport(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo
                , transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId
                , exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, providerIds, commaDelimitedLineItemStatusIds, hasGroupByKeySelector, claimStatusType, claimStatusTypeValue, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> ExportInitialClaimStatusDenialsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
             DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo,
             DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds,
             string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string),
             string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string),string fileName = default(string))
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportInitialClaimStatusDenials(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, transactionDateFrom,
                transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, exceptionReasonCategoryIds, procedureCodes, patientId, locationIds, dashboardType, fileName));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

    }
}