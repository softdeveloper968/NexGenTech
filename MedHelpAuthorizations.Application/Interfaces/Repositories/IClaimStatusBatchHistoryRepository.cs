using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClaimStatusBatchHistoryRepository
    {
        IQueryable<ClaimStatusBatchHistory> ClaimStatusBatchHistories { get; }

        Task<List<ClaimStatusBatchHistory>> GetListAsync();

        Task<ClaimStatusBatchHistory> GetByIdAsync(int id);

        Task<List<ClaimStatusBatchHistory>> GetByClaimStatusBatchIdAsync(int ClaimStatusBatchId);

        Task<int> InsertAsync(ClaimStatusBatchHistory ClaimStatusBatchHistory);

        Task UpdateAsync(ClaimStatusBatchHistory ClaimStatusBatchHistory);

        Task DeleteAsync(ClaimStatusBatchHistory ClaimStatusBatchHistory);
    }
}
