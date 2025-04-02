using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IUserClientRepository : IRepositoryAsync<UserClient, int>, ITenantRepository
    {
        IQueryable<UserClient> UserClients { get; }
        Task<bool> AddClientsForUserAsync(string userId, ICollection<int> clientId);

        Task<List<Domain.Entities.Client>> GetClientsForUser(string userId, bool isActiveOnly = false);

        Task<bool> UpdateClientsForUserAsync(string userId, ICollection<int> clientIds);

        Task<bool> VerifyUserAllowedForClient(string userId, string clientname);

        Task<List<string>> GetUsersForClientAsync(int clientId);

        List<string> GetUsersForClient(int clientId);
    }
}