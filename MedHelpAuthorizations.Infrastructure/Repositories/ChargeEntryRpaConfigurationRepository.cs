using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ChargeEntryRpaConfigurationRepository : RepositoryAsync<ChargeEntryRpaConfiguration, int>, IChargeEntryRpaConfigurationRepository
    {
        private readonly ApplicationContext _dbContext;
        public ChargeEntryRpaConfigurationRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ChargeEntryRpaConfiguration> ChargeEntryRpaConfigurationes => _dbContext.ChargeEntryRpaConfigurations;

        public async Task DeleteAsync(ChargeEntryRpaConfiguration clientInsuranceRpaConfiguration)
        {
            _dbContext.ChargeEntryRpaConfigurations.Remove(clientInsuranceRpaConfiguration);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChargeEntryRpaConfiguration> GetByIdAsync(int clientInsuranceRpaConfigurationId)
        {
            return await ChargeEntryRpaConfigurationes
                .Include(c => c.AuthType)
                .Include(c => c.RpaType)
                .Where(p => p.Id == clientInsuranceRpaConfigurationId)
                .FirstOrDefaultAsync();
        }
        public async Task<ChargeEntryRpaConfiguration> GetByCriteriaAsync(int clientId, int authTypeId, RpaTypeEnum rpaTypeId, TransactionTypeEnum transactionTypeId = TransactionTypeEnum.ChargeEntry)
        {
            return await ChargeEntryRpaConfigurationes
                .Include(c => c.AuthType)
                .Include(c => c.RpaType)
                .Where(c => c.ClientId == clientId
                    && c.RpaTypeId == rpaTypeId
                    && c.TransactionTypeId == transactionTypeId
                    && c.AuthTypeId == authTypeId
                    && !c.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChargeEntryRpaConfiguration>> GetListAsync()
        {
            return await ChargeEntryRpaConfigurationes
                .Include(c => c.AuthType)
                .Include(c => c.RpaType)
                .ToListAsync();
        }

        public async Task<int> InsertAsync(ChargeEntryRpaConfiguration clientInsuranceRpaConfiguration)
        {
            await _dbContext.ChargeEntryRpaConfigurations.AddAsync(clientInsuranceRpaConfiguration);
            return clientInsuranceRpaConfiguration.Id;
        }

        public async Task UpdateAsync(ChargeEntryRpaConfiguration clientInsuranceRpaConfiguration)
        {
            _dbContext.ChargeEntryRpaConfigurations.Update(clientInsuranceRpaConfiguration);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ChargeEntryRpaConfiguration>> GetByRpaTypeIdAsync(RpaTypeEnum rpaTypeId, TransactionTypeEnum transactionTypeId)
        {
            return await ChargeEntryRpaConfigurationes
                .Include(c => c.AuthType)
                .Include(c => c.RpaType)
                .Where(c => c.RpaTypeId == rpaTypeId
                    && c.TransactionTypeId == transactionTypeId
                    && !c.IsDeleted)
                .ToListAsync();
        }
    }
}
