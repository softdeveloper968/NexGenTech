using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientProviderLocationRepository : IClientProviderLocationRepository
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IRepositoryAsync<ClientProviderLocation, int> _repository;
        public ClientProviderLocationRepository(
            IUnitOfWork<int> unitOfWork,
            IRepositoryAsync<ClientProviderLocation, int> repository
            )    
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public IQueryable<ClientProviderLocation> ClientProviderLocations => _repository.Entities;

        public async Task<List<ClientProviderLocation>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<ClientProviderLocation> GetByIdAsync(int clientProviderLocationId)
        {
            return await _repository.Entities.Where(p => p.Id == clientProviderLocationId).FirstOrDefaultAsync();
        }

        public async Task<int> InsertAsync(ClientProviderLocation clientProviderLocation)
        {
            await _repository.AddAsync(clientProviderLocation);
            return clientProviderLocation.Id;
        }

        public async Task UpdateAsync(ClientProviderLocation clientProviderLocation)
        {
            await _repository.UpdateAsync(clientProviderLocation);
        }

        public async Task DeleteAsync(ClientProviderLocation clientProviderLocation)
        {
            await _repository.DeleteAsync(clientProviderLocation);
        }

        /// <summary>
        /// get all the provider-location mappings from ClientProviderLocations table by provider Id
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public async Task<List<ClientProviderLocation>> GetProviderLocationMappingsByProviderId(int providerId)
        {
            return await _repository.Entities
                .Where(x => x.ClientProviderId == providerId)
                .ToListAsync();
        }
        
        public async Task<List<ClientLocation>> GetClientLocationsByProviderId(int providerId)
        {
            return await _repository.Entities
                .Include(x => x.ClientLocation)
                .Where(x => x.ClientProviderId == providerId)
                .Select(x => x.ClientLocation)
                .ToListAsync();
        }

        /// <summary>
        /// get all the provider-location mappings from ClientProviderLocations table by location Id
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<List<ClientProviderLocation>> GetProviderLocationMappingsByLocationId(int locationId)
        {
            return await _repository.Entities
                .Where(x => x.ClientLocationId == locationId)
                .ToListAsync();
        }

        /// <summary>
        /// Delete mappings from ClientProviderLocations table
        /// </summary>
        /// <param name="providerLocationList"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProviderLocationMappings(List<ClientProviderLocation> providerLocationList, CancellationToken cancellationToken)
        {
            try
            {
                if (providerLocationList.Any())
                {
                    foreach (var providerLocation in providerLocationList)
                    {
                        await _repository.DeleteAsync(providerLocation);
                    }
                    //_dbContext.Set<ClientProviderLocation>().RemoveRange(providerLocationList);
                    //await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
