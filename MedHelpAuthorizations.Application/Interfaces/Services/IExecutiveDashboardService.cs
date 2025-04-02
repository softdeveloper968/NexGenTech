using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.Executive;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IExecutiveDashboardService : IService
    {
        /// <summary>
        /// to get current month charges for the selected client for the executive dashboard component
        /// </summary>
        /// <param name="quey"></param>
        /// <returns></returns>
        Task<List<ExecutiveSummary>> GetExecutiveCurrentMonthDataAsync(IExecutiveDashboardQueryBase quey);

        /// <summary>
        /// To get the current month denials data for the selected client for the executive dashbaord component
        /// </summary>
        /// <param name="quey"></param>
        /// <returns></returns>
        Task<List<ExecutiveSummary>> GetCurrentMonthDenialTotalsAsync(IExecutiveDashboardQueryBase query);

        /// <summary>
        /// To get the current month payments data for the selected client for the executive dashbaord
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ExecutiveSummary>> GetCurrentMonthPaymentTotalsAsync(IExecutiveDashboardQueryBase query);
        
        /// <summary>
        /// To get the current month payments data for the selected client for the executive dashbaord
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<CurrentAROverPercentageNintyDaysLocation>> GetCurrentAROverPercentageNintyDaysLocationAsync(IExecutiveDashboardQueryBase query);

        /// <summary>
        /// Get Current AR Over Percentage Ninty Days Payer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<CurrentAROverPercentageNintyDaysPayer>> GetCurrentAROverPercentageNintyDaysPayerAsync(GetCurrentAROverNintyDaysPayerByLocationQuery query);
        
        /// <summary>
        /// Get Clean claim rate
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ExecutiveClaimRate>> GetCleanClaimRateAsync(GetExecutiveCleanClaimRateQuery query);

        /// <summary>
        /// Get denial claim rate
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ExecutiveClaimRate>> GetDenialClaimRateAsync(GetExecutiveDenialRateQuery query);

        /// <summary>
        /// Get monthly days in AR
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<MonthlyDaysInAR> GetMonthlyDaysInARAsync(GetExecutiveMonthlyDaysInARQuery query);

        /// <summary>
        /// Get current Month employee work summary
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<MonthlyEmployeeSummary>> GetCurrentMonthEmployeeWorkAsync(IExecutiveDashboardQueryBase query);
    }
}
