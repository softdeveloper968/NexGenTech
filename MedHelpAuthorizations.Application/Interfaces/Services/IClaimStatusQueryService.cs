using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClaimStatus.Queries.GetEmployeeClaimStatusByEmployeeID;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.Dashboards.GetClaimsData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetClaimStatusTotalReort;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Application.Features.Reports.DailyClaimReports;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Models.Exports;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IClaimStatusQueryService : IService
    {
        Task<ClaimStatusDashboardResponse> GetClaimsStatusTotalsAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null);

        Task<List<ExportQueryResponse>> GetClaimsStatusDetailsAsync(IClaimStatusDashboardDetailsQuery filters, int clientId = 0, string conn = null);

        Task<List<ExportQueryResponse>> GetDenialDetailsAsync(IClaimStatusDashboardDetailsQuery filters, int clientId = 0, string connStr = "");

        Task<ClaimStatusDashboardResponse> GetInitialClaimsStatusTotalsAsync(IClaimStatusDashboardInitialQuery filters);

        Task<List<ExportQueryResponse>> GetInProcessDetailsAsync(IClaimStatusDashboardDetailsQuery filters, int clientId = 0, string conn = null);

        Task<List<ExportQueryResponse>> GetInitialClaimStatusDetailsAsync(IInitialClaimStatusDashboardDetailsQuery filters, int clientId, string connStrings);

        Task<List<ExportQueryResponse>> GetInitialClaimStatusDenialDetailsAsync(IInitialClaimStatusDashboardDetailsQuery filters, int clientId, string connStrings);

        Task<List<ExportQueryResponse>> GetInitialInProcessDetailsAsync(IInitialClaimStatusDashboardDetailsQuery filters, int clientId, string connStr);

        Task<ClaimStatusTrendsResponse> GetInitialClaimsStatusTrendsAsync(IClaimStatusDashboardInitialQuery filters);
        //Task<PaginatedResult<ClaimWorkstationDetailsResponse>> GetClaimsWorkstationDetailsAsync(IClaimWorkstationDetailQuery filters, int pageNumber, int pageSize);
        Task<PaginatedResult<ClaimWorkstationDetailsResponse>> GetClaimsWorkstationDetailsAsync(IClaimWorkstationDetailQuery filters, int pageNumber, int pageSize, ClaimWorkstationSearchOptions? ClaimWorkstationSearchOptions);

        Task<ClaimStatusDashboardResponse> GetDailyClaimsStatusTotalsAsync(DailyClaimReportDetailsQuery filters, string connStr = null);

        DailyClaimStatusReportResponse GetDailyClaimStatusReportResponse(DateTime day, List<ClaimStatusTotal> claimStatusUploadedTotals, List<ClaimStatusTotal> claimStatusTransactionTotals, List<ClaimStatusTotal> claimStatusInProcessTotals, List<ClaimStatusTotal> claimStatusDenialReasonTotals, bool hasLastPage = false, string claimDateTitleForLastDate = "");

        //AA-77 : Provider productivity dashboard
        //Task<ClaimStatusProductivityDataResponse> GetClaimsStatusProductivityDataAsync(ClaimStatusProductivityDataQuery filters);

        //AA-142
        Task<List<ProviderComparisonResponse>> GetProviderComparisonDataAsync(ProviderComparisonQuery query, int clientId = 0);

        //AA-163
        Task<List<EmployeesClaimStatusResponseModel>> GetEmployeeClaimStatusDataAsync(int clientId = 0, string connStr = null);

        Task<List<ClaimStatusDashboardInProcessDetailsResponse>> GetUncheckedClaimsDetailsAsync(int clientId, string tenantIdentifier = null);

        Task<List<ClaimStatusDaysWaitLapsedDetailResponse>> GetDaysWaitLapsedByClientIdAsync(int clientId, string tenantIdentifier = null);

        /// <summary>
        /// This method retrieves claim status date lag data from the database.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
		Task<List<ClaimStatusDateLagResponse>> GetClaimStatusDateLagAsync(ClaimStatusDateLagQuery filters, int clientId = 0, string connStr = null);

        Task<GetClientFeeSschedulelEntryDataByClaimsModel> GetClientFeeScheduleEntry(string procedureCode,
            int clientInsuranceId,
            DateTime dateOfService,
            ProviderLevelEnum? providerLevelId = null,
            SpecialtyEnum? specialtyId = null,
            string connStr = null); //AA-231

        Task<List<ExportQueryResponse>> GetCashProjectionByDayAsync(GetCashProjectionByDayQuery query); //Charges Dashboard AA-326 //Updated AA-343

        Task<List<GetCashValueForRevenueByDayResponse>> GetCashValueForRevenueByDayAsync(GetCashValueForRevenueByDayQuery filterBy, int clientId = 0, string connStr = null); //Charges Dashboard AA-326

        Task<List<ExportQueryResponse>> GetClaimStatusRevenueTotalsAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null); //AA-330

        Task<List<CashValueForRevenueDetails>> GetCashValueForRevenueDetails(GetCashValueForRevenueByDayQuery filterBy,int clientId, string connStr = null); //AA-331

        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelCashValueForRevenue(string FilterBy = "ServiceDate"); //AA-331

        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelCashValueForRevenueDetails(string FilterBy = "ServiceDate"); //AA-331

        List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineTwoExportReportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails); //AA-331

        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelCashProjection(string FilterBy = "ServiceDate"); //AA-343

        /// <summary>
        /// Provides a mapping of Excel report columns to corresponding properties in the ClaimStatusDateLagResponse.
        /// </summary>
        /// <returns>mapping expression</returns>
        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelDateLagReport(); //EN-66
        /// <summary>
        /// Provides a mapping of Excel report columns to corresponding properties in the ClaimStatusRevenueTotals.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelRevenueTotalsReport(); //EN-66//EN-308

        /// <summary>
        /// Provides a mapping of Excel report columns to corresponding properties in the ARAgingData.
        /// </summary>
        /// <returns>maping for ARAgingData excel report</returns>
        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelReverseAnalysisReport(); //EN-66

        /// <summary>
        /// Get filtered report data for chart export
        /// </summary>
        /// <param name="filters">Criteria to be applied to the query</param>
        /// <param name="clientId">Client Id for which data to be fetched</param>
        /// <param name="connStr">Connection string of the db from which data to be fetched</param>
        /// <returns>List items filterd according to the criteria</returns>
        Task<List<ExportQueryResponse>> GetFilteredReportAsync(IClaimStatusDashboardQueryBase filters, int clientId = 0, string connStr = null); //EN-66

        /// <summary>
        /// Provides a mapping of Excel report columns to corresponding properties in the ClaimStatusDashboardDetailsResponse. 
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelFilteredClaimsReport(); //EN-66

        Task<int> UnassignClaimStatusBatchAsync(int batchId, string tenantIdentifier = null);

        Dictionary<string, Func<ExportQueryResponse, object>> GetExportDetailsExcel(ExportClaimStatusDetailsQuery request);
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportClaimStatusExcel(ExportClaimStatusDetailsQuery request);
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportInProcessExcel();
        List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineExportDashboardReportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails);

        List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineExportDashboardReportModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails, Dictionary<string, Func<ExportQueryResponse, object>> procedureReport);
        /// <summary>
        /// To produce mapping for the Denial Report export
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelDenialReport(); //EN-105

        /// <summary>
        /// To get financial summary data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ClaimSummary> GetFinancialSummaryDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-133

        /// <summary>
        /// To get financial summary data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ClaimDetailsSummary> GetClaimsSummaryDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-133

        /// <summary>
        /// To get Average Days to pay by Payer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<AverageDaysByPayer>> GetAverageDaysToPayByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-137

        /// <summary>
        /// Get charges by Payer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ChargesByPayer>> GetChargesByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-138

        /// <summary>
        /// Get charges by Payer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<MonthlyClaimSummary>> GetPaymentsMonthlyDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-138

        /// <summary>
        /// Get charges by Payer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<MonthlyClaimSummary>> GetMonthlyDenialSummaryAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-174

        /// <summary>
        /// To get Average Days to pay by Provider
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<AverageDaysByProvider>> GetAverageDaysToPayByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null);

        /// <summary>
        /// Get charges by Provider
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ChargesTotalsByProvider>> GetChargesByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-138

        /// <summary>
        /// Get ClaimsInProcess by Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ClaimProcessSummary>> GetClaimInProcessDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-219

        /// <summary>
        /// Get Claims Total by Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ClaimStatusTotalSummary> GetClaimStatusTotalsDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-219

        /// <summary>
        /// Get Average Allowed Amount by Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<AvgAllowedAmountSummary>> GetAvgAllowedAmtDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-219

        /// <summary>
        /// Get Denials by insurance by Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<DenialClaimSummary>> GetDenialsByInsuranceDateWiseAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-219

        /// <summary>
        /// Get Average Allowed Amount by Location
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ReimbursementByLocationSummary>> GetReimbursementByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-229

        /// <summary>
        /// Get Average Allowed Amount by Provider
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ReimbursementByProviderSummary>> GetReimbursementByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-229

        /// <summary>
        /// Get Procedure Totals By Provider 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ProcedureTotalsByProvider>> GetProcedureTotalsByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-241

        /// <summary>
        /// Get Insurance Totals By Provider 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<InsuranceTotalsByProviderSummary>> GetInsuranceTotalsByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-250

        /// <summary>
        /// Get Denial Reasons Totals By Provider 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialReasonsTotalsByProvider>> GetDenialReasonsByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-252

        /// <summary>
        /// Get Procedure Reimbursement By Provider 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ProcedureReimbursementByProvider>> GetProcedureReimbursementByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-254

        /// <summary>
        /// Get Payer Reimbursement By Provider 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<PayerReimbursementByProviderSummary>> GetPayerReimbursementByProviderAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-257

        /// <summary>
        /// Get Denial Reasons Totals By Procedure 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialsByProcedureSummary>> GetDenialByProcedureAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-289

        /// <summary>
        /// Get Denial Reasons Totals By Insurance 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialReasonsByInsuranceSummary>> GetDenialByInsuranceAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-289

        /// <summary>
        /// Get totals for providers grouped by payers
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<PayerProviderTotals>> GetProviderTotalsByPayerQuery(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-278

        /// <summary>
        /// Get payements total by Insurance
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<PaymentSummary>> GetPaymentsTotalsByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-278

        /// <summary>
        /// Get denial total by Insurance
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialTotalsByInsuranceSummary>> GetDenialTotalsByPayerAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-278 

        /// <summary>
        /// Get initial claim summary data 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<ClaimDetailsSummary> GetInitialClaimSummaryDataAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-295
        
        /// <summary>
        /// Get initial claim summary data 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialClaimSummary>> GetInitialDenialsByInsuranceAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-295        

        /// <summary>
        /// Get initial inprocess claims
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialClaimSummary>> GetInitialInProcessClaimsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-295

        /// <summary>
        /// Get procedure totals by location
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ProcedureTotalsbyLocationsSummary>> GetProcedureTotalsByLocationsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-312

        /// <summary>
        /// Get insurance totals by location
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<InsuranceTotalsByLocationSummary>> GetInsuranceTotalsByLocationsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-312

        /// <summary>
        /// Get denial totals by location
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialReasonsByLocationsSummary>> GetDenialReasonssByLocationsAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-312

        /// <summary>
        /// Get data for the dashboard tiles
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<ClaimStatusDashboardData> GetClaimStatusDashbaordDataAsync(IClaimStatusDashboardQueryBase query, string connStr = null); //Dashboard tiles data

        /// <summary>
        /// Get Procedure Reimbursement By Location 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ProcedureReimbursementByLocation>> GetProcedureReimbursementByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-312


        /// <summary>
        /// Get Payer Reimbursement By Location 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<PayerReimbursementSummary>> GetPayerReimbursementByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-257

        /// <summary>
        /// To get Average Days to pay by Location
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<AverageDaysByLocation>> GetAverageDaysToPayByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-312


        /// <summary>
        /// Get charges by Location
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ChargesTotalsByLocation>> GetChargesByLocationAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-312
        Task<List<GetClaimByProcedureSummaryResponse>> GetLastFourYearClaims(string connStr = null); // EN-231
        Task<List<GetClaimStatusTotalReportResponse>> GetClaimStatusTotalReportAsync(string tenantIdentifier);//EN-231

        /// <summary>
        /// Get ProviderTotals By Procedure 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ProviderTotalsByProcedure>> GetProviderTotalsByProcedureAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-334

        /// <summary>
        /// Get InsuranceTOtal By Procedure Code
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<InsuranceTotalsByProcedureCode>> GetInsuranceTotalsByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-334

        /// <summary>
        /// Get DenialReasons By ProcedureCode
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<DenialsByProcedureSummary>> GetDenialReasonsByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); // EN-334


        /// <summary>
        /// Get PayerReimbursement By ProcedureCode
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<PayerReimbursementByProcedureCode>> GetPayerReimbursementByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-334

        /// <summary>
        /// Get ProviderReimbursement By ProcedureCode
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ProviderReimbursementByProcedureCodeSummary>> GetProviderReimbursementByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-334

        /// <summary>
        /// Get Charges By ProcedureCode
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ChargesTotalsByProcedureCode>> GetChargesByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-334

        /// <summary>
        /// Get Reimbursement By ProcedureCode
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<ProviderReimbursementByProcedureCodeSummary>> GetReimbursementByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-334

        /// <summary>
        /// GetAverageDaysToPayByProcedureCodeAsync
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        Task<List<AverageDaysByProcedureCode>> GetAverageDaysToPayByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query, string connStr = null); //EN-334
        
        /// <summary>
        /// Get Dynamic Claim Export dashboard details | Export.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="connStr"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<List<DynamicExportQueryResponse>> GetDynamicClaimExportDashboardQueryAsync(ExportDynamicClaimDashboardQuery filters, string connStr = "", int clientId = 0);//EN-235

        /// <summary>
        /// Get in process claims for the tenant
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        Task<List<ExportQueryResponse>> GetInProcessClaimsReportAsync(TenantDto tenant);

        /// <summary>
        /// Get in process claims report excel
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportInProcessReportExcel();

        /// <summary>
        /// Update ClaimStatusExceptionReasonCategoryId in ClaimStatusTransactions table
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        void UpdateClaimStatusExceptionReasonCategoryForTenant(TenantDto tenant); //EN-544

        /// <summary>
        /// get payer totals
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<PayerTotalsByProvider>> GetPayerTotalsQuery(GetPayerTotalsQuery query);
        Dictionary<string, Func<ExportQueryResponse, object>> MapInProcessDetailsInSheet();

        Dictionary<string, Func<ExportQueryResponse, object>> GetAvgDaysToPayExportDetailsExcel(ExportAvgDayToPayReportDetailsQuery request);//EN-374
        Task<List<ExportQueryResponse>> GetAverageDaysToPayReportAsync(ExportAvgDayToPayReportDetailsQuery filters, string connStr = "", int clientId = 0, bool hasAvgDayToPayByProvider = false);//EN-374
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportAvgDayToPaySummaryExcel();//EN-374
        Dictionary<string, Func<ExportQueryResponse, object>> GetAvgDaysToPayByProviderExportDetailsExcel(ExportAvgDayToPayReportDetailsQuery request);//EN-374
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportAvgDayToPayByProviderSummaryExcel();//EN-374

        Task<List<ProviderTotals>> GetProviderTotalsByProcedureCodeAsync(IClaimStatusDashboardStandardQuery query);
        Task<List<ProviderProcedureTotal>> GetProviderProcedureTotalAsync(IClaimStatusDashboardStandardQuery query, string connStr = null);
        Task<List<ProviderDenialReasonTotal>> GetProviderDenialReasonTotalAsync(IClaimStatusDashboardStandardQuery query, string connStr = null);
        Dictionary<string, Func<ExportQueryResponse, object>> GetFinicalSummaryDetailsExcel(FinicalSummaryExportDetailQuery request);
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportFinicalSummaryExcel();

        List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineSummaryExportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelReport, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails);

        Dictionary<string, Func<ExportQueryResponse, object>> GetExportProcedureCodeSummaryExcel();

        Dictionary<string, Func<ExportQueryResponse, object>> GetPaymentAndProcedureCodeExportDetailsExcel(ExportCustomPaymentAndProcedureCodeQuery request);

        Dictionary<string, Func<ExportQueryResponse, object>> GetExportPaymentSummaryExcel();

        /// <summary>
        /// TO get the excel mapping for the claim status report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportClaimStatusReportExcel(IClaimStatusDashboardStandardQuery request); //EN-584
        Dictionary<string, Func<ExportQueryResponse, object>> GetExportInitialClaimStatusReportExcel(IInitialClaimStatusDashboardDetailsQuery request); //EN-584

        /// <summary>
        /// Get Client Data By Id EN-704
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<GetClientByIdResponse> GetByClientIdAsync(int clientId);

    }
}