using MedHelpAuthorizations.Application.Features.MonthClose.Queries;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IMonthCloseQueryService : IService
    {
        /// <summary>
        /// Asynchronously retrieves the month cash collection data based on the provided query parameters.
        /// The data is fetched from the database by executing the stored procedure for monthly cash collection details.
        /// </summary>
        /// <param name="query">The query parameters to filter the data, including client, location, provider, insurance, and CPT code IDs.</param>
        /// <returns>A task representing the asynchronous operation, with a result of an enumerable collection of <see cref="MonthlyCashCollectionData"/> objects.</returns>
        Task<IEnumerable<MonthlyCashCollectionData>> GetMonthCashCollectionDataAsync(IMonthCloseDashboardQuery query);

        /// <summary>
        /// Asynchronously retrieves the monthly accounts receivable (AR) data based on the provided query parameters.
        /// The data is fetched from the database by executing the stored procedure for monthly AR details.
        /// </summary>
        /// <param name="query">The query parameters to filter the data, including client, location, provider, insurance, and CPT code IDs.</param>
        /// <returns>A task representing the asynchronous operation, with a result of an enumerable collection of <see cref="MonthlyARData"/> objects.</returns>
        Task<IEnumerable<MonthlyARData>> GetMonthlyARDataAsync(IMonthCloseDashboardQuery query);

        /// <summary>
        /// Asynchronously retrieves the monthly denial data based on the provided query parameters.
        /// The data is fetched from the database by executing the stored procedure for monthly denial details.
        /// </summary>
        /// <param name="query">The query parameters to filter the data, including client, location, provider, insurance, and CPT code IDs.</param>
        /// <returns>A task representing the asynchronous operation, with a result of an enumerable collection of <see cref="MonthlyDenialData"/> objects.</returns>

        Task<IEnumerable<MonthlyDenialData>> GetMonthlyDenialDataAsync(IMonthCloseDashboardQuery query);

        /// <summary>
        /// Asynchronously retrieves the monthly receivables data based on the provided query parameters.
        /// The data is fetched from the database by executing the stored procedure for monthly receivables details.
        /// </summary>
        /// <param name="query">The query parameters to filter the data, including client, location, provider, insurance, and CPT code IDs.</param>
        /// <returns>A task representing the asynchronous operation, with a result of an enumerable collection of <see cref="MonthlyReceivablesData"/> objects.</returns>
        Task<IEnumerable<MonthlyReceivablesData>> GetMonthlyReceivablesDataAsync(IMonthCloseDashboardQuery query);
    }

}