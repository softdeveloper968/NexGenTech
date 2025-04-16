using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.ChargesDashboard
{
    public interface IChargesDashboardManager : IManager
    {
        /// <summary>
        /// Get cash projection by day for the current user
        /// </summary>
        /// <returns></returns>
        Task<IResult<IEnumerable<GetCashProjectionByDayResponse>>> GetCashProjectionByDay(GetCashProjectionByDayQuery query);

        /// <summary>
        /// Get cash value for revenue for day for the current client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<IEnumerable<GetCashValueForRevenueByDayResponse>>> GetCashValueForRevenueByDay(GetCashValueForRevenueByDayQuery query);

        /// <summary>
        /// Export cash value for revenue for day for the current client
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<string> ExportCashValueForRevenueByDay(ExportCashValueForRevenueQuery query);

        /// <summary>
        /// Export cash projection report for the charges dashboard
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<string> ExportCashProjectionByDay(ExportCashProjectionByDayQuery query); //AA-343
    }
}
