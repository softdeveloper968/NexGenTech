using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IQuarterlyJobService
    {
        /// <summary>
        /// calculates average collection amount PER CLAIM LEVEL! Grouped By ClaimLevelHash Sum Total Charged, and total Paid.. 
        /// then find average collection % per client insurance. This value needs to be stored in a new table (ClientInsuranceAverageCollectionPercentages) 
        /// and referenced when we want to calculate expected cash value for items that are not tied to a fee schedule.
        /// </summary>
        /// <returns></returns>
        Task<bool> CalculateQuarterlyAverageCollection(CancellationToken stoppingToken);

        /// <summary>
        /// Add or update Average Collection Percentages
        /// </summary>
        /// <param name="AverageCollectionPercentages"></param>
        /// <param name="ClientInsuranceAverageCollectionPercentageRepository"></param>
        /// <returns></returns>
        Task AddupdateAverageCollectionPercentages(List<ClientInsuranceAverageCollectionPercentage> AverageCollectionPercentages, IClientInsuranceAverageCollectionPercentageRepository ClientInsuranceAverageCollectionPercentageRepository, CancellationToken cancellationToken);
    }
}
