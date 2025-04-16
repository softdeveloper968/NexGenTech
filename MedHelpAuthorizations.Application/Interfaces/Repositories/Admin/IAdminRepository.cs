using MedHelpAuthorizations.Domain.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories.Admin
{
    public interface IAdminRepository<T, in TId> : IRepositoryAsync<T, TId> where T : class, IEntity<TId>
    {
    }
}
