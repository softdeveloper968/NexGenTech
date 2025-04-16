using System;
using System.Collections.Generic;using System.Linq;using MedHelpAuthorizations.Domain.Common.Contracts;
using System.Linq.Expressions;using System.Threading;
using Finbuckle.MultiTenant;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories{    public interface IRepositoryAsync<T, in TId> where T : class, IEntity<TId>    {        ITenantInfo TenantInfo { get; }        IQueryable<T> Entities { get; }		Task<T> GetByIdAsync(TId id);
		Task<List<T>> GetAllAsync();
		Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
		Task<T> AddAsync(T entity);
		void AddRange(IEnumerable<T> entities);

		void RemoveRange(IEnumerable<T> entities);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);

		void ExecuteUpdate(Expression<Func<T, bool>> filterExpression, Action<T> updateAction);

		void ExecuteDelete(Expression<Func<T, bool>> filterExpression);

		Task<int> Commit(CancellationToken cancellationToken);

		void ExecuteDeleteAll();

    }}