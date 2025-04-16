using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientProviderLocationRepository
    {
        IQueryable<ClientProviderLocation> ClientProviderLocations { get; }
        Task<List<ClientProviderLocation>> GetListAsync();

        Task<ClientProviderLocation> GetByIdAsync(int clientProviderLocationId);

        Task<int> InsertAsync(ClientProviderLocation clientProviderLocation);

        Task UpdateAsync(ClientProviderLocation clientProviderLocation);

        Task DeleteAsync(ClientProviderLocation clientProviderLocation);
        Task<List<ClientProviderLocation>> GetProviderLocationMappingsByProviderId(int providerId);

        Task<List<ClientLocation>> GetClientLocationsByProviderId(int providerId);
        Task<List<ClientProviderLocation>> GetProviderLocationMappingsByLocationId(int locationId);
        Task<bool> DeleteProviderLocationMappings(List<ClientProviderLocation> providerLocationList, CancellationToken cancellationToken);
    }
}
