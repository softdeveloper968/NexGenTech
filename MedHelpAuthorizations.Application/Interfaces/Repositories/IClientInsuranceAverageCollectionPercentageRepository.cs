using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository for managing operations related to <see cref="ClientInsuranceAverageCollectionPercentage"/>.
    /// </summary>
    public interface IClientInsuranceAverageCollectionPercentageRepository : IRepositoryAsync<ClientInsuranceAverageCollectionPercentage, int>
    {
        /// <summary>
        /// Gets a queryable collection of client insurance average collection percentages.
        /// </summary>
        IQueryable<ClientInsuranceAverageCollectionPercentage> ClientInsuranceAverageCollectionPercentages { get; }

        /// <summary>
        /// Retrieves a <see cref="ClientInsuranceAverageCollectionPercentage"/> based on the specified quarter, year, and client insurance identifier.
        /// </summary>
        /// <param name="quarter">The quarter for which the data is requested.</param>
        /// <param name="year">The year for which the data is requested.</param>
        /// <param name="clientInsuranceId">The unique identifier of the client insurance.</param>
        /// <returns>The matching <see cref="ClientInsuranceAverageCollectionPercentage"/> or null if not found.</returns>
        Task<ClientInsuranceAverageCollectionPercentage> GetDataByQuarterAndClientInsurance(int Quarter, string Year, int ClientInsuranceId);
    }
}
