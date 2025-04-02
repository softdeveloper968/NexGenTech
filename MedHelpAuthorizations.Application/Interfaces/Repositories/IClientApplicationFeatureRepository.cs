using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientApplicationFeatureRepository
    {
        //Task<List<UserClient>> GetClientsForUser(string userId);
        Task<List<ApplicationFeature>> GetClientApplicationFeatures(int clientId);
        Task DeleteApplicationFeaturesForClient(int clientId);
    }
}