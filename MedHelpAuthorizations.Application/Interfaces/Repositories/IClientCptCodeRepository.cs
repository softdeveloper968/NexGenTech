using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientCptCodeRepository  : IRepositoryAsync<ClientCptCode, int>
	{
        Task<ClientCptCode> GetByClientId(int id, string code); //EN-155
	}
}
