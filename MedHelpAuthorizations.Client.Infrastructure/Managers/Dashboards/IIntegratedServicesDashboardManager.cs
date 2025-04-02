using System;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Enums;
using System.Collections.Generic;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Dashboards
{
    ///AA-120: Updations    
    public interface IIntegratedServicesDashboardManager : IManager
    {
        Task<IResult<IClaimStatusDashboardResponse>> GetClaimStatusTotalsByCriteriaAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, 
            DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), 
            string authTypeIds = default(string), int claimStatusBatchId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), 
            string locationIds = default(string), string providerIds = default(string), int? PatientId = null, bool? hasProcedureDashboard = false);

        Task<IResult<IClaimStatusDashboardResponse>> GetInitialClaimStatusTotalsByCriteriaAsync(
            DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
            DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom,
            DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), string procedureCodes = default(string), string exceptionReasonCategoryIds = default(string), DateGroupingTypeEnum? dateGroupingType = null);

        Task<IResult<IClaimStatusTrendsResponse>> GetClaimStatusTrendsByCriteriaAsync(DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
            DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), string procedureCodes = default(string), string exceptionReasonCategoryIds = default(string));

        //AA-77 : Provider Productivity Dashboard
        Task<IResult<IClaimStatusTrendsResponse>> GetClaimStatusProductivityDataByCriteriaAsync(DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string ClientInsuranceIds, string ExceptionReasonCategoryIds, string AuthTypeIds, string ProcedureCodes, string ClientLocationIds, string ClientProviderIds);

        Task<IResult<List<ClaimStatusDateLagResponse>>> GetClaimStatusDateLagDataByCriteriaAsync(ClaimStatusDateLagQuery query); //AA-255 : Dashbaord Redesign

        //AA-330
        /// <summary>
        /// call the endpoint for the revenue analysis chart data
        /// </summary>
        /// <param name="createdOnFrom"></param>
        /// <param name="createdOnTo"></param>
        /// <param name="dateOfServiceFrom"></param>
        /// <param name="dateOfServiceTo"></param>
        /// <param name="transactionDateFrom"></param>
        /// <param name="transactionDateTo"></param>
        /// <param name="claimBilledFrom"></param>
        /// <param name="claimBilledTo"></param>
        /// <param name="clientInsuranceIds"></param>
        /// <param name="authTypeIds"></param>
        /// <param name="claimStatusBatchId"></param>
        /// <param name="exceptionReasonCategoryIds"></param>
        /// <param name="procedureCodes"></param>
        /// <param name="locationIds"></param>
        /// <param name="providerIds"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        Task<IResult<IList<ClaimStatusRevenueTotal>>> GetRevenueTotalsByCriteriaAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
           DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string),
           string authTypeIds = default(string), int claimStatusBatchId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string),
           string locationIds = default(string), string providerIds = default(string), int? patientId = null); //AA-330


        /// <summary>
        /// To get data for financial summary
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<IResult<List<FinancialSummaryData>>> GetFinancialSummaryByCriteriaAsync(GetFinancialSummaryDataQuery criteria); //EN-133
        /// <summary>
        /// Get claims summary data
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<IResult<ClaimSummary>> GetClaimsSummaryByCriteriaAsync(GetClaimsSummaryDataQuery criteria); //EN-135

        /// <summary>
        /// To Get averaged days to pay by payer data
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<IResult<List<AverageDaysByPayer>>> GetAverageDaysByCriteriaAsync(GetAverageDaysToPayByPayerQuery criteria); //EN-137
        
        /// <summary>
        /// To get charges by payer data
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<IResult<List<ChargesByPayer>>> GetChargesByPayerAsync(ChargesByPayerQuery criteria); //EN-137

        /// <summary>
        /// To get monthly payments data
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<IResult<List<MonthlyClaimSummary>>> GetMonthlyPaymentsAsync(PaymentsMonthlyQuery criteria);


        /// <summary>
        /// This service is used to get exported data for the date lag cards
        /// </summary>
        /// <param name="query">This is the date lag query used to get the same data exported that is in the card</param>
        /// <returns></returns>
        Task<string> ExportDateLagReport(ExportClaimStatusDateLagQuery query);

        /// <summary>
        /// This service is used to get exported data for the Revenue Analysis cards
        /// </summary>
        /// <param name="query">This is the revenue analysis query used to get the same data exported that is in the card</param>
        /// <returns></returns>
        Task<string> ExportRevenueAnalysisReport(ExportRevenueAnalysisReportQuery query);

        /// <summary>
        /// To get denial claim for comparison on the basis of exception reason
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<MonthlyClaimSummary>>> GetDenialMonthlyForComparison(DenialMonthlyComparisonQuery query);

        /// <summary>
        /// To get Claims In Process By Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetClaimInProcessDateWise(ClaimInProcessDateWiseQuery query);

        /// <summary>
        /// To get Claim Status Totals By Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<ClaimSummary>> GetClaimStatusTotalsDateWise(ClaimStatusTotalsDateWiseQuery query);

        /// <summary>
        /// To get Average Allowed Amount By Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetAvgAllowedAmtDateWise(AvgAllowedAmtDateWiseQuery query);
        
        /// <summary>
        /// To get Average Allowed Amount By Date
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetDenialsByInsuranceDateWise(DenialsByInsuranceDateWiseQuery query);

        /// <summary>
        /// To get Average Allowed Amount By Location
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetReimbursementByLocation(ReimbursementByLocationQuery query); //EN-229

        /// <summary>
        /// To get Average Allowed Amount By Provider
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetReimbursementByProvider(ReimbursementByProviderQuery query); //EN-229

        /// <summary>
        /// Get initial claim summary data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<ClaimSummary>> GetInitialClaimSummary(GetInitialClaimSummaryDataQuery query); //EN-295

        /// <summary>
        /// GetInitialDenialsByInsurance
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetInitialDenialsByInsurance(GetInitialDenialsByInsuranceQuery query); //EN-295

        /// <summary>
        /// GetInitialDenialsByInsurance
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetInitialInProcessClaims(GetInitialInProcessClaimsQuery query); //EN-295
    }
}