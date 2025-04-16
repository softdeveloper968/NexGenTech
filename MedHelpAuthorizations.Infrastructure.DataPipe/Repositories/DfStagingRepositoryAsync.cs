using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces;
using MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Repositories
{
	public class DfStagingRepositoryAsync<T> : IDfStagingRepositoryAsync<T> where T : class, IDfStagingAuditableEntity
	{
		private readonly DfStagingDbContext _dbContext;
		public IQueryable<T> Entities => _dbContext.Set<T>();

		public DfStagingRepositoryAsync(DfStagingDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public Task DeleteAsync(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			return Task.CompletedTask;
		}

		public async Task<T> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);

		public async Task<List<T>> GetUnprocessedByTenantClientAsync(string tenantClientString)
		{
			return await _dbContext
				.Set<T>()
				.Where(x => x.IsProcessedSuccessfully == null && x.TenantClientString == tenantClientString)
				.ToListAsync();
		}

		public Task UpdateProcessedSuccessfullyAsync(T entity)
		{
			T exist = _dbContext.Set<T>().Find(entity.StgId);
			if (exist != null)
			{
				exist.IsProcessedSuccessfully = true;
			}
			_dbContext.Entry(exist).CurrentValues.SetValues(entity);
			return Task.CompletedTask;
		}

		public Task UpdateErroredAsync(T entity, string errorMessage)
		{
			T exist = _dbContext.Set<T>().Find(entity.StgId);
			if (exist != null)
			{
				exist.IsProcessedSuccessfully = false;
				exist.ErrorMessage = errorMessage;
			}
			_dbContext.Entry(exist).CurrentValues.SetValues(entity);
			return Task.CompletedTask;
		}

		public Task UpdateAsync(T entity)
		{
			T exist = _dbContext.Set<T>().Find(entity.StgId);
			_dbContext.Entry(exist).CurrentValues.SetValues(entity);
			return Task.CompletedTask;
		}

		public void ExecuteUpdate(Expression<Func<T, bool>> filterExpression, Action<T> updateAction)
		{
			var entitiesToUpdate = _dbContext.Set<T>().Where(filterExpression).ToList();

			foreach (var entity in entitiesToUpdate)
			{
				updateAction.Invoke(entity);
				_dbContext.Entry(entity).State = EntityState.Modified;
			}

			_dbContext.SaveChanges();
		}

		public void ExecuteDelete(Expression<Func<T, bool>> filterExpression)
		{
			var entitiesToDelete = _dbContext.Set<T>().Where(filterExpression).ToList();

			_dbContext.RemoveRange(entitiesToDelete);
			_dbContext.SaveChanges();
		}

		public async Task<int> Commit(CancellationToken cancellationToken)
		{
			return await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}