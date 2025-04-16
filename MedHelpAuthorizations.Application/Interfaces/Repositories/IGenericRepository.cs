using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : AuditableEntity<int>
    {
        T GetById(int id);
        IReadOnlyList<T> GetPagedResponse(int pageNumber, int pageSize);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
