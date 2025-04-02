using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetRpaConfigurationsWithLocation;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientInsuranceRpaConfigurationRepository
    {
        IQueryable<ClientInsuranceRpaConfiguration> ClientInsuranceRpaConfigurations { get; }

        Task<List<ClientInsuranceRpaConfiguration>> GetByClientIdAsync(int clientId);
        
        Task<List<ClientInsuranceRpaConfiguration>> GetListAsync();

        Task<ClientInsuranceRpaConfiguration> GetByIdAsync(int clientInsuranceRpaConfigurationId);

        Task<ClientInsuranceRpaConfiguration> GetByCriteriaAsync(int clientId, int rpaInsuranceId, TransactionTypeEnum transactionTypeId, int? authTypeId, int? clientLocationId);

        Task<List<ClientInsuranceRpaConfiguration>> GetByUsernameAndUrlAsync(string username, string url, TransactionTypeEnum transactionTypeId);

        Task<List<ClientInsuranceRpaConfiguration>> GetByRpaInsuranceIdAsync(int rpaInsuranceId, TransactionTypeEnum transactopnTypeId);

        Task<int> InsertAsync(ClientInsuranceRpaConfiguration clientInsuranceRpaConfiguration);

        Task UpdateAsync(ClientInsuranceRpaConfiguration clientInsuranceRpaConfiguration);

        Task DeleteAsync(ClientInsuranceRpaConfiguration clientInsuranceRpaConfiguration);

        Task<List<ClientInsuranceRpaConfiguration>> GetFailedClientInsuranceRpaConfigurationsAsync();

        Task<int> GetFailedClientInsuranceRpaConfigurationsCountAsync();

        void ExecuteUpdate(Expression<Func<ClientInsuranceRpaConfiguration, bool>> filterExpression, Action<ClientInsuranceRpaConfiguration> updateAction); //AA-228

        Task<List<ClientInsuranceRpaConfiguration>> GetExpiryWarningOrFailedClientInsuranceRpaConfigByClientIdAsync(int ClientId); //AA-250
        Task<List<GetRpaConfigurationsWithLocationResponse>> GetRpaConfigurationsWithLocationAsync(int clientId); //En-409

	}
}
