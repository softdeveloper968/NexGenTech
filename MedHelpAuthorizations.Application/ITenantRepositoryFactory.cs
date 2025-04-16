using MedHelpAuthorizations.Application.Interfaces.Repositories;

namespace MedHelpAuthorizations.Application
{
    public interface ITenantRepositoryFactory
    {
        public T Get<T>(string tenantIdentifier);
        public T Get<T>(int tenantId);
        public Task<T> GetAsync<T> (string tenantIdentifier);
        public IUnitOfWork<TId> GetUnitOfWork<TId>(string tenantIdentifier);
        public IUnitOfWork<TId> GetUnitOfWork<TId>(int tenantId);
        public IUserClientRepository GetUserClientRepository(string tenantIdentifier);
        public IUserAlertRepository GetUserAlertRepository(string tenantIdentifier);
        public IClientRepository GetClientRepository(string tenantIdentifier);
        public IAddressRepository GetAddressRepository(string tenantIdentifier);
        public IPersonRepository GetPersonRepository(string tenantIdentifier);
		public IEmployeeRepository GetEmployeeRepository(string tenantIdentifier); //AA-228
        public IClaimStatusBatchRepository GetClaimStatusBatchRepository(string tenantIdentifier); //AA-228
        public IClientInsuranceRpaConfigurationRepository GetClientInsuranceRpaConfigurationRepository(string tenantIdentifier); //AA-228
        public IClaimStatusBatchClaimsRepository GetClaimStatusBatchClaimsRepository(string tenantIdentifier); //AA-228
        public IClientLocationRepository GetClientLocationRepository(string tenantIdentifier); //AA-228
        public IChargeEntryRpaConfigurationRepository GetChargeEntryRpaConfigurationRepository(string tenantIdentifier); //AA-228
        public IClaimStatusTransactionRepository GetClaimStatusTransactionRepository(string tenantIdentifier); //AA-228
        public IClaimStatusTransactionLineItemStatusChangeRepository GetClaimStatusTransactionLineItemStatusChangeRepository(string tenantIdentifier); //AA-228
        public IClientInsuranceAverageCollectionPercentageRepository GetClientInsuranceAverageCollectionPercentageRepository(string tenantIdentifier); //EN-91
        public IClaimStatusBatchHistoryRepository GetClaimStatusBatchHistoryRepository(string tenantIdentifier);
        public IClientUserNotificationRepository GetClientUserNotificationRepository(string tenantIdentifier);
    }
}
