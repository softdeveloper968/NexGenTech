using LazyCache;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using System.Collections;

namespace MedHelpAuthorizations.Infrastructure.Identity.Repositories
{
    public class AdminUnitOfWork : IAdminUnitOfWork
    {
        private readonly AdminDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private Hashtable _repositories;
        private readonly IAppCache _cache;
        private bool disposed;
        public AdminUnitOfWork(AdminDbContext dbContext, ICurrentUserService currentUserService, IAppCache cache)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _cache = cache;
        }
        public IAdminRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : AuditableEntity<TId>
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(AdminRepositoryAsync<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TId)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IAdminRepository<TEntity, TId>)_repositories[type];
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
