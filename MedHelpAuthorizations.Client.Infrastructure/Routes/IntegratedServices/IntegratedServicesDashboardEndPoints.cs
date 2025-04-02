using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using MedHelpAuthorizations.Shared.Enums;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public static class IntegratedServicesDashboardEndPoints
    {
        public static string GetByCriteria(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), int? patientId = null, bool? hasProcedureDashboard = false) //May Add Later - DateTime? billedFrom, DateTime? billedTo, DateTime? reviewedFrom, DateTime? reviewedTo)
        {
            var url = $"api/v1/tenant/IntegratedServicesDashboard?createdOnFrom={createdOnFrom}&createdOnTo={createdOnTo}&dateOfServiceFrom={dateOfServiceFrom}&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}&claimBilledTo={claimBilledTo}&clientInsuranceIds={clientInsuranceIds}&authTypeIds={authTypeIds}&claimStatusBatchId={claimStatusBatchId}&exceptionReasonCategoryIds={exceptionReasonCategoryIds}&procedureCodes={procedureCodes}&locationIds={locationIds}&providerIds={providerIds}&patientId={patientId}";
            
            if (hasProcedureDashboard.HasValue && hasProcedureDashboard.Value)
            {
                url += $"&hasProcedureDashboard={hasProcedureDashboard}";
            }
            return url;
        }

        public static string GetClaimStatusDetailsByCriteria(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string flattenedLineItemStatus, int clientInsuranceIds, int authTypeIds, int claimStatusBatchId, ClaimStatusExceptionReasonCategoryEnum? exceptionReasonCategoryIds = null, string procedureCodes = null)
        {
            return $"api/v1/tenant/IntegratedServicesDashboard/GetClaimStatusDetails?createdOnFrom={createdOnFrom}&createdOnTo={createdOnTo}&dateOfServiceFrom={dateOfServiceFrom}&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}&claimBilledTo={claimBilledTo}&flattenedLineItemStatus={flattenedLineItemStatus}&clientInsuranceIds={clientInsuranceIds}&authTypeIds={authTypeIds}&claimStatusBatchId={claimStatusBatchId}&exceptionReasonCategoryIds={exceptionReasonCategoryIds}&procedureCodes={procedureCodes}";
        }
        public static string GetClaimStatusDenialsDetailsByCriteria(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string flattenedDenialReason, int clientInsuranceIds, int authTypeIds, int claimStatusBatchId, ClaimStatusExceptionReasonCategoryEnum? exceptionReasonCategoryIds = null, string procedureCodes = null)
        {
            return $"api/v1/tenant/IntegratedServicesDashboard/GetClaimStatusDenialsDetailData?createdOnFrom={createdOnFrom}&createdOnTo={createdOnTo}&dateOfServiceFrom={dateOfServiceFrom}&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}&claimBilledTo={claimBilledTo}&flattenedDenialReason={flattenedDenialReason}&clientInsuranceIds={clientInsuranceIds}&authTypeIds={authTypeIds}&claimStatusBatchId={claimStatusBatchId}&exceptionReasonCategoryIds={exceptionReasonCategoryIds}&procedureCodes={procedureCodes}";
        }
        public static string GetInitialClaimStatusByCriteria(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), string procedureCodes = default(string), string exceptionReasonCategoryIds = default(string), DateGroupingTypeEnum? dateGroupingType = null) //May Add Later - DateTime? billedFrom, DateTime? billedTo, DateTime? reviewedFrom, DateTime? reviewedTo)
        {
            return $"api/v1/tenant/IntegratedServicesDashboard/initialStatus?createdOnFrom={createdOnFrom}&createdOnTo={createdOnTo}&dateOfServiceFrom={dateOfServiceFrom}&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}&claimBilledTo={claimBilledTo}&clientInsuranceIds={clientInsuranceIds}&authTypeIds={authTypeIds}&procedureCodes={procedureCodes}&exceptionReasonCategoryIds={exceptionReasonCategoryIds}&dateGroupingType={dateGroupingType}";
        }
        public static string GetClaimStatusTrendsByCriteria(DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), string procedureCodes = default(string), string exceptionReasonCategoryIds = default(string)) //May Add Later - DateTime? billedFrom, DateTime? billedTo, DateTime? reviewedFrom, DateTime? reviewedTo)
        {
            //return $"api/v1/tenant/IntegratedServicesDashboard/claimStatusTrends?dateOfServiceFrom={dateOfServiceFrom}&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}&claimBilledTo={claimBilledTo}&clientInsuranceIds={clientInsuranceIds}&authTypeIds={authTypeIds}&procedureCodes={procedureCodes}&exceptionReasonCategoryIds={exceptionReasonCategoryIds}";
            return $"api/v1/tenant/IntegratedServicesDashboard/claimStatusTrends";
        }
        public static string GetClaimStatusProductivityDataByCriteria(DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), string procedureCodes = default(string), string exceptionReasonCategoryIds = default(string), string locationIds = default(string), string providerIds = default(string))
        //May Add Later - DateTime? billedFrom, DateTime? billedTo, DateTime? reviewedFrom, DateTime? reviewedTo)
        {
            return $"api/v1/tenant/IntegratedServicesDashboard/claimStatusProductivityData?dateOfServiceFrom={dateOfServiceFrom}&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}&claimBilledTo={claimBilledTo}&clientInsuranceIds={clientInsuranceIds}&authTypeIds={authTypeIds}&procedureCodes={procedureCodes}&exceptionReasonCategoryIds={exceptionReasonCategoryIds}&locationIds={locationIds}&providerIds={providerIds}";
        }

        public static string GetDateLagDataByCriteria = "api/v1/tenant/IntegratedServicesDashboard/GetDateLag";

        public static string GetRevenueTotalsByCriteria(DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, string clientInsuranceIds = default(string), string authTypeIds = default(string), int claimStatusBatchId = 0, string exceptionReasonCategoryIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), int? patientId = null) //May Add Later - DateTime? billedFrom, DateTime? billedTo, DateTime? reviewedFrom, DateTime? reviewedTo)
        {
            return $"api/v1/tenant/IntegratedServicesDashboard/GetRevenueTotals?createdOnFrom={createdOnFrom}&createdOnTo={createdOnTo}&dateOfServiceFrom={dateOfServiceFrom}&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}&claimBilledTo={claimBilledTo}&clientInsuranceIds={clientInsuranceIds}&authTypeIds={authTypeIds}&claimStatusBatchId={claimStatusBatchId}&exceptionReasonCategoryIds={exceptionReasonCategoryIds}&procedureCodes={procedureCodes}&locationIds={locationIds}&providerIds={providerIds}&patientId={patientId}";
        }

        //EN-125
        public static string GetFinancialSummaryByCriteria = "api/v1/tenant/IntegratedServicesDashboard/FinancialSummary";
        public static string GetClaimsSummaryByCriteria = "api/v1/tenant/IntegratedServicesDashboard/ClaimsSummary";
        public static string AvgDaysByPayerByCriteria = "api/v1/tenant/IntegratedServicesDashboard/AvgDaysByPayer";
        public static string ChargesByPayer = "api/v1/tenant/IntegratedServicesDashboard/ChargesByPayer";
        public static string PaymentsMonthly = "api/v1/tenant/IntegratedServicesDashboard/PaymentsMonthly";

        public static string ClaimInProcessDateWise = "api/v1/tenant/IntegratedServicesDashboard/ClaimInProcessDateWise";
        public static string ClaimStatusTotalsDateWise = "api/v1/tenant/IntegratedServicesDashboard/ClaimStatusTotalsDateWise";
        public static string AvgAllowedAmtDateWise = "api/v1/tenant/IntegratedServicesDashboard/AvgAllowedAmtDateWise";

        public static string ReimbursementByLocation = "api/v1/tenant/IntegratedServicesDashboard/ReimbursementByLocation"; //EN-229
        public static string ReimbursementByProvider = "api/v1/tenant/IntegratedServicesDashboard/ReimbursementByProvider"; //EN-229

        public static string DenialsByInsuranceDateWise = "api/v1/tenant/IntegratedServicesDashboard/DenialsByInsuranceDateWise";

        //EN-66
        //End-point to get the date lag data exported
        public static string ExportDateLagResponse = "api/v1/tenant/IntegratedServicesDashboard/ExportDateLag";

        //EN-66
        //End-point to get the revenue analysis data exported
        public static string ExportRevenueAnalysisResponse = "api/v1/tenant/IntegratedServicesDashboard/ExportRevenueAnalysis";

        //EN-174
        public static string DenialMonthlyForComparison = "api/v1/tenant/IntegratedServicesDashboard/DenialMonthlyForComparison";

        //EN-295
        public static string GetInitialClaimSummary = "api/v1/tenant/IntegratedServicesDashboard/GetInitialClaimSummary";
        public static string GetInitialDenialsByInsurance = "api/v1/tenant/IntegratedServicesDashboard/GetInitialDenialsByInsurance";
        public static string GetInitialInProcessClaims = "api/v1/tenant/IntegratedServicesDashboard/GetInitialInProcessClaims";
    }
}
