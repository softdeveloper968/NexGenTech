
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces
{
	public interface IDfStagingUnitOfWork : IDisposable
	{
		IDfStagingRepositoryAsync<T> Repository<T>() where T : DfStagingAuditableEntity;

		Task<int> Commit(CancellationToken cancellationToken);

		Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

		Task Rollback();
	}
}
