using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface ICorporateDashboardService : IService
    {
        /// <summary>
        /// Get current month charges for all the clients
        /// </summary>
        /// <param name="query"></param>
        /// <returns>List of type MonthlyClientSummary</returns>
        Task<List<MonthlyClientSummary>> GetCurrentMonthChargesTotalsAsync(ICorporateDashboardQueryBase query);

        /// <summary>
        /// Get current month payment totals for all the clients
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<MonthlyClientSummary>> GetCurrentMonthPaymentTotalsAsync(ICorporateDashboardQueryBase query);

        /// <summary>
        /// Get current Month denial totals for all the clients
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<MonthlyClientSummary>> GetCurrentMonthDenialTotalsAsync(ICorporateDashboardQueryBase query);

        /// <summary>
        /// Get current Month employee work summary
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<MonthlyEmployeeSummary>> GetCurrentMonthEmployeeWorkAsync(ICorporateDashboardQueryBase query);

        /// <summary>
        /// Get Current AR Over Percentage 90 Days
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<CurrentAROverPercentageNintyDaysClient>> GetCurrentAROverPercentageNintyDaysClientAsync(ICorporateDashboardQueryBase query);

        /// <summary>
        /// Get Current AR Over Percentage Ninty Days Payer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<CurrentAROverPercentageNintyDaysPayer>> GetCurrentAROverPercentageNintyDaysPayerAsync(GetCurrentAROverPercentageNintyDaysPayerQuery query);

        /// <summary>
        /// Get monthly days in AR
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<MonthlyDaysInAR> GetMonthlyDaysInARAsync(GetMonthlyDaysInARQuery query);

        /// <summary>
        /// Get Clean claim rate
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ClaimRate>> GetCleanClaimRateAsync(GetCleanClaimRateQuery query);
        /// <summary>
        /// Get denial claim rate
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ClaimRate>> GetDenialClaimRateAsync(GetDenialRateQuery query);

        /// <summary>
        /// To get stat totals data for the selected client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ClaimStatusDashboardData> GetClientStatTotalsDataAsync(GetClientStatTotalsDataQuery query);

        /// <summary>
        /// To get client financial summary data for the selected client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ClaimSummary> GetClientFinancialSummaryDataAsync(GetClientFinancialSummayDataQuery query);

        /// <summary>
        /// Get denial reasons by insurance data for the selected tenant
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ClaimSummary>> GetDenialReasonsByInsuranceDataAsync(GetClientDenialReasonsByInsuranceDataQuery query);

        /// <summary>
        /// Get claims summary data for the selected tenant
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ClaimSummary> GetClientClaimSummaryDataAsync(GetClientClaimSummaryDataQuery query);
        
        /// <summary>
        /// To get client average days by payer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<AverageDaysByPayer>> GetClientAverageDasyByPayerDataAsync(GetClientAverageDaysToPayByPayerQuery query);

        /// <summary>
        /// To get client charges by payer data for the selected client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ChargesByPayer>> GetClientChargesByPayerDataAsync(GetClientChargesByPayersQuery query);

        /// <summary>
        /// To Get Clilent claim status data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ClaimSummary> GetClientClaimStatusDataAsync(GetClientClaimStatusDataQuery query);

        /// <summary>
        /// Get client claims in process data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ClaimSummary>> GetClientClaimsInProcessDataAsync(GetClientClaimsInProcessQuery query);

        /// <summary>
        /// Get avg allowed amt totals data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ClaimSummary>> GetClientAvgAllowedAmtTotalsAsync(GetClientAvgAllowedAmtTotalsQuery query);
    }
}
