using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientRepositoryAsync : RepositoryAsync<Domain.Entities.Client, int>, IClientRepository
    {

        private readonly ApplicationContext _dbContext;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        //private readonly UserManager<ApplicationUser> _userManager;

        public ClientRepositoryAsync(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ClientRepositoryAsync(ApplicationContext dbContext, IClaimStatusQueryService claimStatusQueryService) : this(dbContext)
        {
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Domain.Entities.Client> GetClientByIdIncludes(int clientId)
        {
            return await _dbContext.Clients
               .Include(x => x.ClientAuthTypes)
                    .ThenInclude(y => y.AuthType)
                .Where(x => x.Id == clientId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Client>> GetAllClientsIncludes()
        {
            return await _dbContext.Clients
               .Include(x => x.ClientAuthTypes)
                    .ThenInclude(y => y.AuthType)
                    .ToListAsync();
        }
        public async Task VerifyUserAllowedForClient(string userId, string clientname)
        {
            //todo:  NEed to get the logic back once data came
            //return Task.FromResult(true);
            await _dbContext.UserClients
                .Include(x => x.Client)
                .Where(x => x.UserId == userId)
                .AnyAsync(x => x.Client.Name == clientname);
        }

        public async Task<Domain.Entities.Client> GetByClientCode(string clientCode)
        {
            return await _dbContext.Clients
                .Include(x => x.ClientApplicationFeatures)
                .Include(x => x.ClientApiIntegrationKeys)
                .Where(x => x.ClientCode == clientCode)
                .SingleOrDefaultAsync();
        }

        public async Task<Domain.Entities.Client> GetClientForUser(string userId)
        {
            return await _dbContext.UserClients
               .Include(x => x.Client)
               .Where(x => x.UserId == userId)
               .Select(x => x.Client)
               .FirstOrDefaultAsync();
        }

        public async Task<List<Domain.Entities.Client>> GetAllClientsForUser(string userId)
        {
            return await _dbContext.UserClients
               .Include(x => x.Client)
               .Where(x => x.UserId == userId)
               .Select(x => x.Client)
               .ToListAsync();
        }

        public async Task<Domain.Entities.Client> GetById(int clientId)
        {
            return await _dbContext.Clients
                .Include(c => c.ClientApiIntegrationKeys)
                .Include(c => c.ClientApplicationFeatures)
                .Include(c => c.ClientAuthTypes)
                .Include(c => c.ClientInsurances)
                .Include(c => c.ClientLocations)
                .Include(c => c.ClientKpi)
                .Include(c => c.ClientSpecialties)
                .Include(c => c.ClientDaysOfOperation)
                .Include(c => c.ClientHolidays)
                //.Include("EmployeeClients.Employee.Person")
                //.Include(c => c.EmployeeClients)
                //    .ThenInclude(ec => ec.Employee)
                //        .ThenInclude(p => p.Person)

                //.Include("EmployeeClients.AssignedClientEmployeeRoles.EmployeeRole")
                //.Include(c => c.EmployeeClients)
                //    .ThenInclude(ea => ea.AssignedClientEmployeeRoles)
                //        .ThenInclude(d => d.EmployeeRole)

                //.Include("EmployeeClients.EmployeeClientAlphaSplits")
                //.Include(c => c.EmployeeClients)
                //    .ThenInclude(ck => ck.EmployeeClientAlphaSplits)

                //.Include("EmployeeClients.EmployeeClientLocations")
                //.Include(c => c.EmployeeClients)
                //    .ThenInclude(ec => ec.EmployeeClientLocations)

                //.Include("EmployeeClients.EmployeeClientInsurances")
                //.Include(c => c.EmployeeClients)
                //    .ThenInclude(ec => ec.EmployeeClientInsurances)
                .FirstOrDefaultAsync(x => x.Id == clientId);
        }

        public async Task<Domain.Entities.Client> GetClientDatById(int clientId)
        {
            return await _dbContext.Clients
                .Include(c => c.ClientApplicationFeatures)
                .Include(c => c.ClientAuthTypes)
                .Include(c => c.ClientSpecialties)
                .Include(c => c.ClientDaysOfOperation)
                .Include(c => c.ClientHolidays)
                .FirstOrDefaultAsync(x => x.Id == clientId);
        }


        public async Task<IEnumerable<Domain.Entities.Client>> GetAllCLientsByEmployee()
        {
            var clients = await _dbContext.Clients
                //.AsNoTracking()
                .Include(z => z.EmployeeClients)
                    .ThenInclude(x => x.Employee)
                //.Include(a => a.EmployeeClients)                    
                //.Include(d=>d.EmployeeClients)
                //    .ThenInclude(f=>f.Employee)
                       //.ThenInclude(g=>g.Person)
                .ToListAsync();

            //foreach (var client in clients) //AA-206
            //{
            //    var user = await _userManager.FindByIdAsync(employeeClient.Employee.UserId); //AA-206

            //}

            return clients;
        }

        public async Task<List<Domain.Entities.Client>> GetAllClients()
        {
            return await _dbContext.Clients
                             .ToListAsync();
        }

        public async Task<List<Domain.Entities.Client>> GetAllClientsByEligibilityCriteria()
        {
            try
            {
                return await _dbContext.Clients
                            .Include(x => x.EmployeeClients)
                                .ThenInclude(y => y.Employee)
                            .Include(x => x.EmployeeClients)
                                .ThenInclude(y => y.AssignedClientEmployeeRoles)
                                .ThenInclude(z => z.EmployeeRole)
                            .Specify(new ClientByEligibilitySpecificationCriteria())
                            .ToListAsync();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Domain.Entities.Client>> GetAllDataPipeFeatureClients()
		{
				return await _dbContext.Clients							
							.Specify(new ClientsByDataPipeSpecificationCriteria())
							.ToListAsync();			
		}

		public async Task<IEnumerable<BasicClientReponse>> GetAllBasicClients()
        {
            return await _dbContext.Clients.Select(x => new BasicClientReponse()
            {
                ClientId = x.Id,
                ClientName = x.Name,
            }).ToListAsync();
        }

		/// <summary>
		/// Retrieves selected information of a client by its ID, including client details, 
		/// API integration keys, and application features. This method fetches only the 
		/// necessary data to optimize performance.
		/// </summary>
		/// <param name="clientId">The unique identifier of the client to retrieve.</param>
		/// <returns>
		/// A task that represents the asynchronous operation. The task result contains 
		/// the client entity with selected information or null if no client is found with the given ID.
		/// </returns>
		public async Task<Domain.Entities.Client> GetSelectedClientInfoById(int clientId)
		{
			return await _dbContext.Clients
				.Where(c => c.Id == clientId)
				.Select(c => new Domain.Entities.Client
				{
					Id = c.Id,
					Name = c.Name,
					ClientCode = c.ClientCode,
					// Select only necessary navigation properties
					ClientApiIntegrationKeys = c.ClientApiIntegrationKeys.Select(apiKey => new ClientApiIntegrationKey
					{
						ApiIntegrationId = apiKey.ApiIntegrationId,
						ApiKey = apiKey.ApiKey
					}).ToList(),
					ClientApplicationFeatures = c.ClientApplicationFeatures.Select(feature => new ClientApplicationFeature
					{
						ApplicationFeatureId = feature.ApplicationFeatureId
					}).ToList(),
                    AutoLogMinutes = c.AutoLogMinutes,
				})
				.FirstOrDefaultAsync();
		}


        public async Task<List<Domain.Entities.Client>> GetAllActiveClients()
        {
            return await _dbContext.Clients.Where(x => x.IsActive)
                             .ToListAsync();
        }

        /// <summary>
        /// Get the Client Data through SP
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<GetClientByIdResponse> GetClientById(int clientId)
        {
            var clientData = await _claimStatusQueryService.GetByClientIdAsync(clientId);

            return clientData;
        }
    }
}