using MedHelpAuthorizations.Application.Features.IntegratedServices.ExceptionReason.Queries;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClaimStatusTransactionRepository : IRepositoryAsync<ClaimStatusTransaction, int>
    {

		IQueryable<ClaimStatusTransaction> ClaimStatusTransactions { get; }

        Task<List<ClaimStatusTransaction>> GetListAsync();

        //Need to hide base member because need Include lazy loading of other Entities
        new Task<ClaimStatusTransaction> GetByIdAsync(int id);

        Task<int> InsertAsync(ClaimStatusTransaction claimStatusTransaction);

        Task<ClaimStatusTransaction> GetLastTransactionByBatchIdAsync(int claimStatusBatchId);
    }
}
