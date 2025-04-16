using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories.Admin
{
    public interface IAdminUnitOfWork : IDisposable
    {
        IAdminRepository<T, TId> Repository<T, TId>() where T : AuditableEntity<TId>;

        Task<int> Commit(CancellationToken cancellationToken);

        Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        Task Rollback();
    }
}
