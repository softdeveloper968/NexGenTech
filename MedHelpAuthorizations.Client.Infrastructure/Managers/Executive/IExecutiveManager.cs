using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.Executive;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Executive
{
    public interface IExecutiveManager : IManager
    {
        /// <summary>
        /// Retrieves the current month charges asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of monthly location summary.</returns>
        Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthChargesAsync(GetExecutiveSummaryDataQuery query); //EN-463

        /// <summary>
        /// Retrieves the current month denials asynchronously.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthDenialsAsync(GetExecutiveCurrentMonthDenialsDataQuery query); //EN-469

        /// <summary>
        /// Retrieves the current month payments asynchronously.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthPaymentsAsync(GetExecutiveCurrentMonthPaymentsQuery query); //EN-470

        /// <summary>
        /// Retrieves the current accounts receivable over percentage for clients asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of current accounts receivable over percentage for clients.</returns>
        Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysLocation>>> GetCurrentAROverPercentageLocationAsync(CurrentAROverPercentageNintyDaysLocationQuery query);

        /// <summary>
        /// Retrieves the current accounts receivable over percentage for payers asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of current accounts receivable over percentage for payers.</returns>
        Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysPayer>>> GetCurrentAROverPercentagePayerAsync(GetCurrentAROverNintyDaysPayerByLocationQuery query);

        /// <summary>
        /// Retrieves the clean claim rate asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a list of clean claim rates.</returns>
        Task<IResult<IEnumerable<ExecutiveClaimRate>>> GetCleanClaimRateAsync(GetExecutiveCleanClaimRateQuery query);

        /// <summary>
        /// Retrieves the clean claim rate asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a list of clean claim rates.</returns>
        Task<IResult<IEnumerable<ExecutiveClaimRate>>> GetDenialClaimRateAsync(GetExecutiveDenialRateQuery query);
        /// <summary>
        /// Retrieves the monthly days in accounts receivable asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing the monthly days in accounts receivable.</returns>
        Task<IResult<MonthlyDaysInAR>> GetMonthlyDaysInARAsync(GetExecutiveMonthlyDaysInARQuery query);

        /// <summary>
        /// Retrieves the current month employee work asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of monthly employee summaries.</returns>
        Task<IResult<IEnumerable<MonthlyEmployeeSummary>>> GetCurrentMonthEmployeeWorkAsync(GetExecutiveCurrentMonthEmployeeWorkQuery query);
    }
}
