using MedHelpAuthorizations.Domain.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IGenericPersonLinkedRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : PersonLinkedEntity
    {
        Task<IEnumerable<T>> FindByLastCommaFirstAsync(string lastName, string firstName);
    }
}
    
