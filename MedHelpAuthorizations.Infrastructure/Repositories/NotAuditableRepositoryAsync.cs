using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    //public class NotAuditableRepositoryAsync<T> : INotAuditableRepositoryAsync<T> where T : NotAuditableEntity
    //{
    //    private readonly ApplicationContext _dbContext;

    //    public NotAuditableRepositoryAsync(ApplicationContext dbContext)
    //    {
    //        _dbContext = dbContext;
    //    }

    //    public IQueryable<T> Entities => _dbContext.Set<T>();

    //    public async Task<T> AddAsync(T entity)
    //    {
    //        await _dbContext.Set<T>().AddAsync(entity);
    //        return entity;
    //    }
        
    //    public async Task<List<T>> GetAllAsync()
    //    {
    //        return await _dbContext
    //            .Set<T>()
    //            .ToListAsync();
    //    }

    //    public async Task<T> GetByIdAsync(int id)
    //    {
    //        return await _dbContext.Set<T>().FindAsync(id);
    //    }
    //}
}