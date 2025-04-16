using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientLocationSpecialityRepository
    {
        Task<List<ClientLocationSpeciality>> GetLocationSpecialityMappingsByLocationId(int locationId);
        Task<bool> DeleteSpecialityLocationMappings(List<ClientLocationSpeciality> specialityLocationList, CancellationToken cancellationToken);
        Task<List<ClientLocationSpeciality>> GetClientLocationsSpecialityByLocationId(int clientId, int locationId);

	}
}
