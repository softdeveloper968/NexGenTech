using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class UserClientRepositoryAsync : RepositoryAsync<UserClient, int>, IUserClientRepository
    {
        private readonly IRepositoryAsync<UserClient, int> _repository;
        private readonly ApplicationContext _dbContext;

        public UserClientRepositoryAsync(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public UserClientRepositoryAsync(ApplicationContext dbContext, ITenantInfo tenantInfo) : base(dbContext, tenantInfo)
        {
            _dbContext = dbContext;
        }

        public IQueryable<UserClient> UserClients => _dbContext.UserClients;

        public async Task<List<Domain.Entities.Client>> GetClientsForUser(string userId, bool isActiveOnly = false)
        {
            var query = _dbContext.UserClients
                .Include(x => x.Client)
                .Where(x => x.UserId == userId)
                .Select(x => x.Client);

            if (isActiveOnly)
            {
                query = query.Where(x => x.IsActive);
            }

            return await query.ToListAsync();
        }

        public async Task<List<string>> GetUsersForClientAsync(int clientId)
        {
            return await _dbContext.UserClients
               .Include(x => x.Client)
               .Where(x => x.ClientId == clientId)
               .Select(x => x.UserId).ToListAsync();
        }

        public List<string> GetUsersForClient(int clientId)
        {
            return _dbContext.UserClients
               .Include(x => x.Client)
               .Where(x => x.ClientId == clientId)
               .Select(x => x.UserId).ToList();
        }
        public async Task<bool> VerifyUserAllowedForClient(string userId, string clientCode)
        {
            //todo:  NEed to get the logic back once data came
            //return Task.FromResult(true);
            return await _dbContext.UserClients
                .Include(x => x.Client)
                .Where(x => x.UserId == userId)
                .AnyAsync(x => x.Client.ClientCode == clientCode);
        }
        public async Task<bool> AddClientsForUserAsync(string userId, ICollection<int> clientId)
        {
            foreach (var client in clientId)
            {
                await _dbContext.UserClients
                    .AddAsync(new UserClient() { UserId = userId, ClientId = client });
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateClientsForUserAsync(string userId, ICollection<int> clientIds)
        {
            var current = await _dbContext.UserClients.Where(x => x.UserId == userId).ToListAsync();

            foreach (var clientid in clientIds)
            {
                var inDb = current.FirstOrDefault(x => x.ClientId == clientid);
                if (inDb == null)
                {
                    _dbContext.UserClients.Add(new UserClient() { UserId = userId, ClientId = clientid });
                }
            }

            _dbContext.UserClients.RemoveRange(current.Where(x => !clientIds.Contains(x.ClientId)));

            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}