using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedHelpAuthorizations.Infrastructure.Identity.Repositories
{
    public class AdminRepositoryAsync<T, TId> : IAdminRepository<T, TId> where T : AuditableEntity<TId>
    {
        private readonly AdminDbContext _dbContext;

        public AdminRepositoryAsync(AdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<T> Entities => _dbContext.Set<T>();

        public ITenantInfo TenantInfo { get; set; }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id);
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

        public void AddRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void ExecuteDeleteAll()
        {
            var entitiesToDelete = _dbContext.Set<T>().ToList();

            _dbContext.RemoveRange(entitiesToDelete);
            _dbContext.SaveChanges();
        }
    }
}
