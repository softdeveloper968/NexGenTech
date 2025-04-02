using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Executive
{
    public interface IExecutiveDashboardManager : IManager
    {
        /// <summary>
        /// Retrieves the current month charges asynchronously.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>The result containing a collection of monthly location summary.</returns>
        Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthChargesAsync(GetExecutiveSummaryDataQuery query); //EN-463
    }
}
