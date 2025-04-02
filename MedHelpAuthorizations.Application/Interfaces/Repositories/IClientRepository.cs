using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientRepository : IRepositoryAsync<MedHelpAuthorizations.Domain.Entities.Client, int>, ITenantRepository 
    {
        Task<List<Domain.Entities.Client>> GetAllClientsForUser(string userId);
        public Task<Domain.Entities.Client> GetByClientCode(string clientCode);

        public Task<Domain.Entities.Client> GetById(int clientId);
        Task<Domain.Entities.Client> GetClientForUser(string userId);
        public Task<IEnumerable<Domain.Entities.Client>> GetAllCLientsByEmployee();
        Task<List<Domain.Entities.Client>> GetAllClients();
        Task<List<Domain.Entities.Client>> GetAllClientsByEligibilityCriteria();
        public Task<IEnumerable<BasicClientReponse>> GetAllBasicClients();

        Task<Domain.Entities.Client> GetSelectedClientInfoById(int clientId); //EN-610
        Task<List<Domain.Entities.Client>> GetAllDataPipeFeatureClients();

        Task<List<Domain.Entities.Client>> GetAllActiveClients();
        Task<GetClientByIdResponse> GetClientById(int clientId);

        Task<Domain.Entities.Client> GetClientDatById(int clientId);
   }
}