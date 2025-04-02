using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IChargeEntryRpaConfigurationRepository
    {
        IQueryable<ChargeEntryRpaConfiguration> ChargeEntryRpaConfigurationes { get; }

        Task<List<ChargeEntryRpaConfiguration>> GetListAsync();

        Task<ChargeEntryRpaConfiguration> GetByIdAsync(int chargeEntryRpaConfigurationId);

        Task<ChargeEntryRpaConfiguration> GetByCriteriaAsync(int clientId, int authTypeId, RpaTypeEnum rpaTypeId, TransactionTypeEnum transactionTypeId);

        Task<List<ChargeEntryRpaConfiguration>> GetByRpaTypeIdAsync(RpaTypeEnum rpaTypeId, TransactionTypeEnum transactionTypeId);

        Task<int> InsertAsync(ChargeEntryRpaConfiguration chargeEntryRpaConfiguration);

        Task UpdateAsync(ChargeEntryRpaConfiguration chargeEntryRpaConfiguration);

        Task DeleteAsync(ChargeEntryRpaConfiguration chargeEntryRpaConfiguration);
    }
}
