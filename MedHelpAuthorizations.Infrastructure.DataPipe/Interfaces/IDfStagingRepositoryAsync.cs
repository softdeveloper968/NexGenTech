using System.Linq.Expressions;using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
namespace MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces{
	public interface IDfStagingRepositoryAsync<T> where T : class, IDfStagingAuditableEntity
	{
		IQueryable<T> Entities { get; }		Task<T> GetByIdAsync(int id);

		//Task<List<T>> GetAllUnprocessedAsync();

		Task<List<T>> GetUnprocessedByTenantClientAsync(string tenantClientString);		Task UpdateProcessedSuccessfullyAsync(T entity);

		Task UpdateErroredAsync(T entity, string errorMessage);
		Task DeleteAsync(T entity);

		void ExecuteUpdate(Expression<Func<T, bool>> filterExpression, Action<T> updateAction);

		void ExecuteDelete(Expression<Func<T, bool>> filterExpression);

		Task<int> Commit(CancellationToken cancellationToken);

		//void ExecuteDeleteAll();

	}}