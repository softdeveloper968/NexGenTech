using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientLocationServiceTypeRepository
    {
        IQueryable<ClientLocationTypeOfService> ClientProviderLocations { get; }
        Task<List<ClientLocationTypeOfService>> GetListAsync();

        Task<ClientLocationTypeOfService> GetByIdAsync(int clientProviderLocationId);

        Task<int> InsertAsync(ClientLocationTypeOfService ClientLocationServiceType);

        Task UpdateAsync(ClientLocationTypeOfService ClientLocationServiceType);

        Task DeleteAsync(ClientLocationTypeOfService ClientLocationServiceType);

        Task<List<ClientLocationTypeOfService>> GetClientLocationsServicetypeByClientId(int clientId);
        Task<List<ClientLocationTypeOfService>> GetClientLocationsServicetypeByLocationId(int clientId, int locationId);
        Task<List<ClientLocationTypeOfService>> GetClientLocationServiceTypeByLocationId(int clientId, int locationId, List<int> serviceTypeIds);

    }
}
