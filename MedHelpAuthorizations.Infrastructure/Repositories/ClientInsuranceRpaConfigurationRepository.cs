using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetRpaConfigurationsWithLocation;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientInsuranceRpaConfigurationRepository : RepositoryAsync<ClientInsuranceRpaConfiguration, int>, IClientInsuranceRpaConfigurationRepository
    {
        private readonly ApplicationContext _dbContext;
        public ClientInsuranceRpaConfigurationRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ClientInsuranceRpaConfiguration> ClientInsuranceRpaConfigurations => _dbContext.ClientInsuranceRpaConfigurations;

        public new async Task DeleteAsync(ClientInsuranceRpaConfiguration clientInsuranceRpaConfiguration)
        {
            clientInsuranceRpaConfiguration.IsDeleted = true;
            _dbContext.ClientInsuranceRpaConfigurations.Update(clientInsuranceRpaConfiguration);
            await _dbContext.SaveChangesAsync();
        }

        public new async Task<ClientInsuranceRpaConfiguration> GetByIdAsync(int clientInsuranceRpaConfigurationId)
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientLocation)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.AlternateClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                 .Where(p => p.Id == clientInsuranceRpaConfigurationId && !p.IsDeleted)
                .FirstAsync();
        }


        public async Task<ClientInsuranceRpaConfiguration> GetByCriteriaAsync(int clientId, int rpaInsuranceId, TransactionTypeEnum transactionTypeId, int? authTypeId, int? clientLocationId)
        {
            var configs = await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.AlternateClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                 .Include(c => c.ClientLocation)
                .Where(c => c.ClientInsurance.ClientId == clientId
                    && c.ClientInsurance.RpaInsuranceId == rpaInsuranceId
                    && c.TransactionTypeId == transactionTypeId
                    && !c.IsDeleted).ToListAsync();

            if (!configs.Any())
            {
                return null;
            }

            if (configs.Count == 1)
            {
                return configs.FirstOrDefault();
            }

            var authFilteredConfigs = configs.Where(c => c.AuthTypeId == authTypeId).ToList();

            if (authFilteredConfigs.Count == 1)
            {
                return authFilteredConfigs.OrderByDescending(c => c.CreatedOn).FirstOrDefault();
            }

            // If more than one config is found, filter by clientLocationId if provided
            var configsToFilterByLocation = authFilteredConfigs.Any() ? authFilteredConfigs : configs;
            if (clientLocationId.HasValue && clientLocationId.Value != 0)
            {
                return configsToFilterByLocation.Where(c => c.ClientLocationId == clientLocationId).OrderByDescending(c => c.CreatedOn).FirstOrDefault() ?? null;
            }

            return configsToFilterByLocation.OrderByDescending(c => c.CreatedOn).FirstOrDefault();
        }


        public async Task<List<ClientInsuranceRpaConfiguration>> GetByClientIdAsync(int clientId)
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Where(c => c.ClientInsurance.ClientId == clientId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<ClientInsuranceRpaConfiguration>> GetListAsync()
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> InsertAsync(ClientInsuranceRpaConfiguration clientInsuranceRpaConfiguration)
        {
            await _dbContext.ClientInsuranceRpaConfigurations.AddAsync(clientInsuranceRpaConfiguration);
            await _dbContext.SaveChangesAsync();
            return clientInsuranceRpaConfiguration.Id;
        }

        public async Task UpdateAsync(ClientInsuranceRpaConfiguration clientInsuranceRpaConfiguration)
        {
            _dbContext.ClientInsuranceRpaConfigurations.Update(clientInsuranceRpaConfiguration);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ClientInsuranceRpaConfiguration>> GetByRpaInsuranceIdAsync(int rpaInsuranceId, TransactionTypeEnum transactionTypeId)
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.AlternateClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Where(c => c.ClientInsurance.RpaInsuranceId == rpaInsuranceId
                    && c.TransactionTypeId == transactionTypeId
                    && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<ClientInsuranceRpaConfiguration>> GetFailedClientInsuranceRpaConfigurationsAsync()
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                 .ThenInclude(c => c.Client)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                 .Include(c => c.AuthType)
                .Where(x => x.ClientRpaCredentialConfiguration.FailureReported == true || x.ClientRpaCredentialConfiguration.ExpiryWarningReported == true
                 && !x.IsDeleted)
                .ToListAsync();
        }
        public async Task<int> GetFailedClientInsuranceRpaConfigurationsCountAsync()
        {
            return 0;
            //return await _dbContext.ClientInsuranceRpaConfigurations
            //    .Include(c => c.ClientInsurance)
            //    .Include(c => c.ClientInsurance.RpaInsurance)
            //        .ThenInclude(d => d.RpaInsuranceGroup)
            //    .Include(c => c.ClientRpaCredentialConfiguration)
            //        .ThenInclude(d => d.RpaInsuranceGroup)
            //    .Where(x => x.ClientRpaCredentialConfiguration.FailureReported == true || x.ClientRpaCredentialConfiguration.ExpiryWarningReported == true
            //     && !x.IsDeleted)
            //    .CountAsync();
        }

        public async Task<List<ClientInsuranceRpaConfiguration>> GetExpiryWarningOrFailedClientInsuranceRpaConfigByClientIdAsync(int ClientId) //AA-250
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Where(x => (x.ClientRpaCredentialConfiguration.FailureReported == true || x.ClientRpaCredentialConfiguration.ExpiryWarningReported == true)
                 && x.ClientInsurance.ClientId == ClientId
                 //&& x.ClientRpaCredentialConfiguration.IsCredentialInUse == true
                 && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<ClientInsuranceRpaConfiguration>> GetByUsernameAndUrlAsync(string username, string url, TransactionTypeEnum transactionTypeId)
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(c => c.ClientInsurance)
                .Include(c => c.ClientInsurance.RpaInsurance)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Include(c => c.ClientRpaCredentialConfiguration)
                    .ThenInclude(d => d.RpaInsuranceGroup)
                .Where(x => x.ClientRpaCredentialConfiguration.Username == username && x.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl.ToUpper().Trim() == url.ToUpper().Trim() && x.TransactionTypeId == transactionTypeId && !x.IsDeleted)
                .ToListAsync();
        }

        public new async void ExecuteUpdate(Expression<Func<ClientInsuranceRpaConfiguration, bool>> filterExpression, Action<ClientInsuranceRpaConfiguration> updateAction)
        {
            var entitiesToUpdate = _dbContext.Set<ClientInsuranceRpaConfiguration>().Where(filterExpression).ToList();

            foreach (var entity in entitiesToUpdate)
            {
                updateAction.Invoke(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GetRpaConfigurationsWithLocationResponse>> GetRpaConfigurationsWithLocationAsync(int clientId)
        {
            return await _dbContext.ClientInsuranceRpaConfigurations
                .Include(x => x.ClientLocation)
                .Where(c => c.ClientLocation.ClientId == clientId &&
                            c.ClientLocationId != null &&
                            !c.IsDeleted)
                .Select(c => new GetRpaConfigurationsWithLocationResponse
                {
                    ClientLocationId = c.ClientLocationId,
                    ClientLocationName = c.ClientLocation.Name
                })
                .ToListAsync();
        }

    }
}
