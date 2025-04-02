using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Corporate
{
    public interface ICorporateDashboardManager : IManager
    {
        /// <summary>
        /// Retrieves the current month charges asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of monthly client summaries.</returns>
        Task<IResult<IEnumerable<MonthlyClientSummary>>> GetCurrentMonthChargesAsync(GetCurrentMonthChargesQuery query); //EN-176

        /// <summary>
        /// Retrieves the current month payments asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of monthly client summaries.</returns>
        Task<IResult<IEnumerable<MonthlyClientSummary>>> GetCurrentMonthPaymentsAsync(GetCurrentMonthPaymentsQuery query); //EN-176

        /// <summary>
        /// Retrieves the current month denials asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of monthly client summaries.</returns>
        Task<IResult<IEnumerable<MonthlyClientSummary>>> GetCurrentMonthDenialsAsync(GetCurrentMonthDenialsQuery query); //EN-176

        /// <summary>
        /// Retrieves the current month employee work asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of monthly employee summaries.</returns>
        Task<IResult<IEnumerable<MonthlyEmployeeSummary>>> GetCurrentMonthEmployeeWorkAsync(GetCurrentMonthEmployeeWorkQuery query); //EN-418

        /// <summary>
        /// Retrieves the current accounts receivable over percentage for clients asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of current accounts receivable over percentage for clients.</returns>
        Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysClient>>> GetCurrentAROverPercentageClientAsync(GetCurrentAROverPercentageNintyDaysClientQuery query); //EN-419

        /// <summary>
        /// Retrieves the current accounts receivable over percentage for payers asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of current accounts receivable over percentage for payers.</returns>
        Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysPayer>>> GetCurrentAROverPercentagePayerAsync(GetCurrentAROverPercentageNintyDaysPayerQuery query); //EN-419

        /// <summary>
        /// Retrieves the monthly days in accounts receivable asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing the monthly days in accounts receivable.</returns>
        Task<IResult<MonthlyDaysInAR>> GetMonthlyDaysInARAsync(GetMonthlyDaysInARQuery query); //EN-419

        /// <summary>
        /// Retrieves the clean claim rate asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a list of clean claim rates.</returns>
        Task<IResult<List<ClaimRate>>> GetCleanClaimRateAsync(GetCleanClaimRateQuery query); //EN-419

        /// <summary>
        /// Get denial rate query manager asynchronous
        /// </summary>
        /// <param name="query">query to filter the records from the db</param>
        /// <returns></returns>
        Task<IResult<List<ClaimRate>>> GetDenialClaimRateAsync(GetDenialRateQuery query); //EN-419

        /// <summary>
        /// To get all the stat totals related to the selected client
        /// </summary>
        /// <returns></returns>
        Task<IResult<ClaimStatusDashboardData>> GetClientStatTotalsDataAsync(GetClientStatTotalsDataQuery query); //EN-449

        /// <summary>
        /// To get client financial summary totals related to the selected client
        /// </summary>
        /// <returns></returns>
        Task<IResult<List<FinancialSummaryData>>> GetClientFinancialSummaryDataAsync(GetClientFinancialSummayDataQuery query);

        /// <summary>
        /// to get denial reasons by insurance related to the selected tenant
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetClientDenialReasonsByInsuranceDataAsync(GetClientDenialReasonsByInsuranceDataQuery query);
        
        /// <summary>
        /// To get the client claim summary for the selected client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<ClaimSummary>> GetClientClaimSummaryDataAsync(GetClientClaimSummaryDataQuery query);

        /// <summary>
        /// To get average days to pay data grouped by payer for the selected client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<AverageDaysByPayer>>> GetClientAverageDaysToPayByPayerDataAsync(GetClientAverageDaysToPayByPayerQuery query);

        /// <summary>
        /// To get charges data grouped by payer for the selected client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ChargesByPayer>>> GetClientChargesByPayersDataAsync(GetClientChargesByPayersQuery query);

        /// <summary>
        /// Get client claim status data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<ClaimSummary>> GetClientClaimStatusDataAsync(GetClientClaimStatusDataQuery query);

        /// <summary>
        /// Get client claims in process data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetClientClaimsInProcessDataAsync(GetClientClaimsInProcessQuery query);

        /// <summary>
        /// Get client avg allowed amt totals data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetClientAvgAllowedAmtTotalsAsync(GetClientAvgAllowedAmtTotalsQuery query);
    }
}
