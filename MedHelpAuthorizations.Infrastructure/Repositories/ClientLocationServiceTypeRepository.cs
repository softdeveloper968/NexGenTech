using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientLocationServiceTypeRepository : IClientLocationServiceTypeRepository
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IRepositoryAsync<ClientLocationTypeOfService, int> _repository;
        public ClientLocationServiceTypeRepository(
            IUnitOfWork<int> unitOfWork,
            IRepositoryAsync<ClientLocationTypeOfService, int> repository
            )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public IQueryable<ClientLocationTypeOfService> ClientProviderLocations => _repository.Entities;

        public async Task<List<ClientLocationTypeOfService>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<ClientLocationTypeOfService> GetByIdAsync(int clientProviderLocationId)
        {
            return await _repository.Entities.Where(p => p.Id == clientProviderLocationId).FirstOrDefaultAsync();
        }

        public async Task<int> InsertAsync(ClientLocationTypeOfService ClientLocationServiceType)
        {
            await _repository.AddAsync(ClientLocationServiceType);
            return ClientLocationServiceType.Id;
        }

        public async Task UpdateAsync(ClientLocationTypeOfService ClientLocationServiceType)
        {
            await _repository.UpdateAsync(ClientLocationServiceType);
        }

        public async Task DeleteAsync(ClientLocationTypeOfService ClientLocationServiceType)
        {
            await _repository.DeleteAsync(ClientLocationServiceType);
        }

        public async Task<List<ClientLocationTypeOfService>> GetClientLocationsServicetypeByLocationId(int clientId, int locationId)
        {
            return await _repository.Entities
                    .Include(x => x.ClientLocation)
                    .Include(a => a.TypeOfService)
                    .Where(x => x.ClientId == clientId && x.ClientLocationId == locationId)
                    .ToListAsync();
        }

        public async Task<List<ClientLocationTypeOfService>> GetClientLocationsServicetypeByClientId(int clientId)
        {
            return await _repository.Entities
                .Where(x => x.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<List<ClientLocationTypeOfService>> GetClientLocationServiceTypeByLocationId(int clientId, int locationId, List<int> serviceTypeIds)
        {
            return await _repository.Entities
                    .Include(x => x.ClientLocation)
                    .Include(a => a.TypeOfService)
                    .Where(x => x.ClientId == clientId && x.ClientLocationId == locationId && serviceTypeIds.Contains((int)x.TypeOfServiceId))
                    .ToListAsync();
        }
    }
}
