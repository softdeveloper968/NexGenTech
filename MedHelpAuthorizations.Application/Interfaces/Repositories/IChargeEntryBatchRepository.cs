using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IChargeEntryBatchRepository
    {
        IQueryable<ChargeEntryBatch> ChargeEntryBatches { get; }

        Task<List<ChargeEntryBatch>> GetListAsync();

        Task<ChargeEntryBatch> GetByIdAsync(int claimStatusBatchId);

        Task<List<ChargeEntryBatch>> GetCompletedByRpaTypeAsync(RpaTypeEnum rpaType);

        Task<List<ChargeEntryBatch>> GetAllCompletedAsync(RpaTypeEnum rpaType);

        Task<List<ChargeEntryBatch>> GetNotCompletedByRpaTypeAsync(RpaTypeEnum rpaType);

        Task<List<ChargeEntryBatch>> GetAllNotCompletedAsync(RpaTypeEnum rpaType);

        Task<List<ChargeEntryBatch>> GetAllUnprocessedAsync();

        Task<int> InsertAsync(ChargeEntryBatch claimStatusBatch);

        Task UpdateAsync(ChargeEntryBatch claimStatusBatch);

        Task DeleteAsync(ChargeEntryBatch claimStatusBatch);
    }
}
