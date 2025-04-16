
using MedHelpAuthorizations.Domain.Contracts;
using System.Threading;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
	public interface IDataPipeRepository<T, in TId> : IRepositoryAsync<T, TId> where T : class, IDataPipe<TId>
	{
		Task<T> GetByDfExternalIdAsync(string dfExternalId);
	}
}
