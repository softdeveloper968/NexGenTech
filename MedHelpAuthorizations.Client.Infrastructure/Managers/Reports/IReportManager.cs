using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using System;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Application.Features.Reports.CurrentSummary;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports
{
    public interface IReportManager : IManager
    {
        Task<string> ExportExpiringAuthorizationsToExcelAsync();

        Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetExpiringAuthorizationsAsync(GetPagedExpiringAuthorizationsQuery request);

        Task<string> ExportClaimStatusDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string));

		Task<string> ExportClaimStatusDenialDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom,
			DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds, string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), int? denialClaimStatusType = null, string dashboardType = null,bool hasGroupByKeySelector=false, bool hasProcedureDashboard = false, string fileName = default(string));

        Task<string> ExportClaimStatusInProcessDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
            DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? claimBilledFrom, DateTime? claimBilledTo,
            string clientInsuranceIds, string authTypeIds, int claimStatusBatchId, int? patientId = 0, string procedureCodes = default(string), string ClaimLineItemStatusId = null,
            string locationIds = default(string), string providerIds = default(string), DateTime? transactionDateFrom = null, DateTime? transactionDateTo = null, string fileName = default(string));

        Task<string> ExportInitialClaimStatusDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
             DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo,
             DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds,
             string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string),
             string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string), string fileName = default(string));

        Task<string> ExportInitialClaimStatusDenialDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
             DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo,
             DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds,
             string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string),
             string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string), string fileName = default(string));

        Task<string> ExportInitialClaimStatusInProcessDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
               DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? claimBilledFrom, DateTime? claimBilledTo,
               string clientInsuranceIds, string authTypeIds, int claimStatusBatchId, int? patientId = 0, string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string), string fileName = default(string));

        Task<string> ExportARAgingDetailsToExcelAsync(ARAgingReportExportDetailsQuery query);
        Task<string> ExportARAgingSummaryToExcelAsync(ARAgingReportExportSummaryQuery query);

        /// <summary>
        /// Manager to call the ExportFilteredReportQuery to get report filtered according to the coomponent
        /// </summary>
        /// <param name="query"></param>
        /// <returns>base64 string</returns>
        Task<string> ExportFilteredReportAsync(ExportFilteredReportQuery query); //EN-66

        Task<string> ExportAvgDayToPayReportDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerids = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, bool hasAvgDayToPayByProvider = false, string fileName = default(string));//EN-374

		Task<string> FinicalSummaryExportsDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
			DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerids = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string));
		Task<string> ExportPaymentClaimStatusDetailsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
		DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerids = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string));

        Task<string> ExportClaimStatusReportExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo,  string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string)); //EN-584

        Task<string> ExportClaimStatusDenialReportExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo,     string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, int? patientId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), string commaDelimitedLineItemStatusIds = default(string), bool hasGroupByKeySelector = default(bool), ClaimStatusTypeEnum? claimStatusType = null, string claimStatusTypeValue = default(string), string dashboardType = null, string fileName = default(string)); //EN-584

        Task<string> ExportInitialClaimStatusDenialsToExcelAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
             DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo,
             DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds,
             string authTypeIds, int claimStatusBatchId, int? patientId = 0, string exceptionReasonCategoryIds = default(string),
             string procedureCodes = default(string), string locationIds = default(string), string dashboardType = default(string), string fileName = default(string)); //EN-584
    }
}