using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Dashboards
{
    public class IntegratedServicesDashboardManager : IIntegratedServicesDashboardManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public IntegratedServicesDashboardManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<IClaimStatusDashboardResponse>> GetClaimStatusTotalsByCriteriaAsync(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, 
            DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), 
            int claimStatusBatchId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), int? patientId = null, bool? hasProcedureDashboard = false)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetByCriteria(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, exceptionReasonCategoryIds, procedureCodes, locationIds, providerIds, patientId, hasProcedureDashboard));
            var data = await response.ToResult<ClaimStatusDashboardResponse>();
            return data;
        }

        public async Task<IResult<IClaimStatusDashboardResponse>> GetInitialClaimStatusTotalsByCriteriaAsync(DateTime? createdOnFrom, DateTime? createdOnTo,
             DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo,
             DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), string procedureCodes = default(string), string exceptionReasonCategoryIds = default(string), DateGroupingTypeEnum? dateGroupingType = null)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetInitialClaimStatusByCriteria(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo, transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, procedureCodes, exceptionReasonCategoryIds, dateGroupingType));
            var data = await response.ToResult<ClaimStatusDashboardResponse>();
            return data;
        }

        public async Task<IResult<IClaimStatusTrendsResponse>> GetClaimStatusTrendsByCriteriaAsync(DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo,
            DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), string procedureCodes = default(string), string exceptionReasonCategoryIds = default(string))
        {
            string url = Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetClaimStatusTrendsByCriteria(dateOfServiceFrom, dateOfServiceTo, transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, procedureCodes, exceptionReasonCategoryIds);
            var response = await _tenantHttpClient.GetAsync(url);
            var data = await response.ToResult<ClaimStatusTrendsResponse>();
            return data;
        }

        public async Task<IResult<IClaimStatusTrendsResponse>> GetClaimStatusProductivityDataByCriteriaAsync(DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string ClientInsuranceIds, string ExceptionReasonCategoryIds, string AuthTypeIds, string ProcedureCodes, string ClientLocationIds, string ClientProviderIds)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetClaimStatusProductivityDataByCriteria(
                dateOfServiceFrom, dateOfServiceTo, transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, ClientInsuranceIds, ExceptionReasonCategoryIds, AuthTypeIds, ProcedureCodes,
                ClientLocationIds, ClientProviderIds));
            var data = await response.ToResult<ClaimStatusTrendsResponse>();
            return data;
        }

        public async Task<IResult<List<ClaimStatusDateLagResponse>>> GetClaimStatusDateLagDataByCriteriaAsync(ClaimStatusDateLagQuery query) //AA-255
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetDateLagDataByCriteria, query);
            var data = await response.ToResult<List<ClaimStatusDateLagResponse>>();
            return data;
        }

        //AA-330
        public async Task<IResult<IList<ClaimStatusRevenueTotal>>> GetRevenueTotalsByCriteriaAsync(DateTime? createdOnFrom, DateTime? createdOnTo, 
            DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, 
            string clientInsuranceIds = null, string authTypeIds = null, int claimStatusBatchId = 0, string exceptionReasonCategoryIds = null, string procedureCodes = null, 
            string locationIds = null, string providerIds = null, int? patientId = null) //AA-330
        {
            var response = await _tenantHttpClient.GetAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetRevenueTotalsByCriteria(createdOnFrom, createdOnTo, dateOfServiceFrom, dateOfServiceTo,
                           transactionDateFrom, transactionDateTo, claimBilledFrom, claimBilledTo, clientInsuranceIds, authTypeIds, claimStatusBatchId, exceptionReasonCategoryIds, procedureCodes, locationIds,
                           providerIds, patientId));
            var data = await response.ToResult<List<ClaimStatusRevenueTotal>>();
            return data;
        }

        #region EN-125
        public async Task<IResult<List<FinancialSummaryData>>> GetFinancialSummaryByCriteriaAsync(GetFinancialSummaryDataQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetFinancialSummaryByCriteria, criteria);
                var data = await response.ToResult<List<FinancialSummaryData>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<ClaimSummary>> GetClaimsSummaryByCriteriaAsync(GetClaimsSummaryDataQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetClaimsSummaryByCriteria, criteria);
                var data = await response.ToResult<ClaimSummary>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<List<AverageDaysByPayer>>> GetAverageDaysByCriteriaAsync(GetAverageDaysToPayByPayerQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.AvgDaysByPayerByCriteria, criteria);
                var data = await response.ToResult<List<AverageDaysByPayer>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<List<ChargesByPayer>>> GetChargesByPayerAsync(ChargesByPayerQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.ChargesByPayer, criteria);
                var data = await response.ToResult<List<ChargesByPayer>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<MonthlyClaimSummary>>> GetMonthlyPaymentsAsync(PaymentsMonthlyQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.PaymentsMonthly, criteria);
                var data = await response.ToResult<List<MonthlyClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        //EN-66
        public async Task<string> ExportDateLagReport(ExportClaimStatusDateLagQuery query)
        {
            //post type call to the end-point with the query parameters
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.ExportDateLagResponse, query);

            //get the base64 string response and convert it to a task result
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        
        //EN-66
        public async Task<string> ExportRevenueAnalysisReport(ExportRevenueAnalysisReportQuery query)
        {
            //post type call to the end-point with the query parameters
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.ExportRevenueAnalysisResponse, query);

            //get the base64 string response and convert it to a task result
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        
        //EN-174
        public async Task<IResult<List<MonthlyClaimSummary>>> GetDenialMonthlyForComparison(DenialMonthlyComparisonQuery query)
        {
            //post type call to the end-point with the query parameters
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.DenialMonthlyForComparison, query);

            //get the base64 string response and convert it to a task result
            var data = await response.ToResult<List<MonthlyClaimSummary>>();
            return data;
        }

        public async Task<IResult<List<ClaimSummary>>> GetClaimInProcessDateWise(ClaimInProcessDateWiseQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.ClaimInProcessDateWise, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<ClaimSummary>> GetClaimStatusTotalsDateWise(ClaimStatusTotalsDateWiseQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.ClaimStatusTotalsDateWise, query);
                var data = await response.ToResult<ClaimSummary>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetAvgAllowedAmtDateWise(AvgAllowedAmtDateWiseQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.AvgAllowedAmtDateWise, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<IResult<List<ClaimSummary>>> GetReimbursementByLocation(ReimbursementByLocationQuery query) //EN-229
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.ReimbursementByLocation, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetReimbursementByProvider(ReimbursementByProviderQuery query)  //EN-229
{
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.ReimbursementByProvider, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IResult<List<ClaimSummary>>> GetDenialsByInsuranceDateWise(DenialsByInsuranceDateWiseQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.DenialsByInsuranceDateWise, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<ClaimSummary>> GetInitialClaimSummary(GetInitialClaimSummaryDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetInitialClaimSummary, query);
                var data = await response.ToResult<ClaimSummary>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetInitialDenialsByInsurance(GetInitialDenialsByInsuranceQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetInitialDenialsByInsurance, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetInitialInProcessClaims(GetInitialInProcessClaimsQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.IntegratedServices.IntegratedServicesDashboardEndPoints.GetInitialInProcessClaims, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}