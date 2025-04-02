using MedHelpAuthorizations.Domain.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IGenericPersonRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : PersonLinkedEntity
    {
        Task<IEnumerable<T>> FindByLastCommaFirst(string lastName, string firstName);
    }
}
    
