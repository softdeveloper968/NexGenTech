using LazyCache;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces;
using MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context;
using System.Collections;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Repositories
{
	public class DfStagingUnitOfWork : IDfStagingUnitOfWork
	{
		private readonly DfStagingDbContext _dbContext;
		private Hashtable _repositories;
		private readonly IAppCache _cache;
		private bool disposed;

		public DfStagingUnitOfWork(DfStagingDbContext dbContext, IAppCache cache)
		{
			_dbContext = dbContext;
			_cache = cache;
		}

		public IDfStagingRepositoryAsync<TEntity> Repository<TEntity>() where TEntity : DfStagingAuditableEntity
		{
			if (_repositories == null)
				_repositories = new Hashtable();
			var type = typeof(TEntity).Name;

			if (!_repositories.ContainsKey(type))
			{
				var repositoryType = typeof(DfStagingRepositoryAsync<>);
				var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

				_repositories.Add(type, repositoryInstance);
			}

			return (IDfStagingRepositoryAsync<TEntity>)_repositories[type];
		}

		public async Task<int> Commit(CancellationToken cancellationToken)
		{
			return await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
		{
			var result = await _dbContext.SaveChangesAsync(cancellationToken);
			foreach (var cacheKey in cacheKeys)
			{
				_cache.Remove(cacheKey);
			}
			return result;
		}

		public Task Rollback()
		{
			_dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					//dispose managed resources
					_dbContext.Dispose();
				}
			}
			//dispose unmanaged resources
			disposed = true;
		}
	}
}
