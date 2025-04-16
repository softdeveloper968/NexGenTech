using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientAuthTypesRepository
    {
        //Task<List<UserClient>> GetClientsForUser(string userId);
        Task<List<AuthType>> GetClientAuthTypes(int clientId);
        Task DeleteAuthTypesForClient(int clientId);
        //Task DeleteUnassignedClientAuthTypes();
    }
}