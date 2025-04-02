using MedHelpAuthorizations.Domain.Entities;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientLocationRepository
    {
        Task<ClientLocation> FindByNameAsync(string name);
        Task<ClientLocation> FindByEligibilityLocationIdAsync(int EligibilityLocationId); //AA-228
    }
}
