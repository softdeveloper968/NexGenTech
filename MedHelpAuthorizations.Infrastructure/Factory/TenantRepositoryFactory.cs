using AutoMapper;
using Finbuckle.MultiTenant;
using LazyCache;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Persistence.Initialization;
using MedHelpAuthorizations.Infrastructure.Repositories;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Factory
{
    public class TenantRepositoryFactory : ITenantRepositoryFactory
    {
        private readonly IMapper _mapper;
        private readonly IMultiTenantStore<AitTenantInfo> _tenantStore;
        private readonly ICurrentUserService _currentUserService;
        private readonly AdminDbContext _identityContext;
        private readonly IAppCache _cache;

        public TenantRepositoryFactory(IMultiTenantStore<AitTenantInfo> tenantStore, ICurrentUserService currentUserService, AdminDbContext identityContext, IMapper mapper, IAppCache cache)
        {
            _tenantStore = tenantStore;
            _currentUserService = currentUserService;
            _identityContext = identityContext;
            _cache = cache;
            _mapper = mapper;
        }

        private ITenantInfo tenantInfo
        {
            get; set;
        }

        private ApplicationContext GetApplicationContext(string tenantIdentifier)
        {

            tenantInfo = _tenantStore.TryGetByIdentifierAsync(tenantIdentifier).GetAwaiter().GetResult();

            if (tenantInfo == null)
            {
                throw new Exception("Tenant not found in Tenant Store");
            }

            DatabaseSettingsDesignTimeFactoryOptions dbSettings = new DatabaseSettingsDesignTimeFactoryOptions()
            {
                Value = new DatabaseSettings()
                {
                    ConnectionString = tenantInfo.ConnectionString,
                    DBProvider = "mssql"
                }
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(tenantInfo.ConnectionString, options => options.EnableRetryOnFailure());

            ApplicationContext dbContext = new ApplicationContext(tenantInfo, optionsBuilder.Options, _currentUserService, dbSettings);

            return dbContext;
        }

        public T Get<T>(string tenantIdentifier)
        {

            Type type = typeof(T);

            T retVal = default(T);

            switch (type.Name)
            {
                //case nameof(IUnitOfWork<int>):
                //    retVal = (T)(object)GetUnitOfWork(tenantIdentifier);
                //    break;
                case nameof(IUserClientRepository):
                    retVal = (T)(object)GetUserClientRepository(tenantIdentifier);
                    break;
                case nameof(IUserAlertRepository):
                    retVal = (T)(object)GetUserAlertRepository(tenantIdentifier);
                    break;
                case nameof(IClientRepository):
                    retVal = (T)(object)GetClientRepository(tenantIdentifier);
                    break;
                case nameof(IEmployeeRepository):
                    retVal = (T)(object)GetEmployeeRepository(tenantIdentifier);
                    break;
				case nameof(IPersonRepository):
					retVal = (T)(object)GetPersonRepository(tenantIdentifier);
					break;
                case nameof(IAddressRepository):
                    retVal = (T)(object)GetAddressRepository(tenantIdentifier);
                    break;
                case nameof(IEmployeeClientRepository):
                    retVal = (T)(object)GetEmployeeClientRepository(tenantIdentifier);
                    break;
                case nameof(IClaimStatusBatchRepository):
                    retVal = (T)(object)GetClaimStatusBatchRepository(tenantIdentifier);
                    break;
                case nameof(IClientInsuranceRpaConfigurationRepository):
                    retVal = (T)(object)GetClientInsuranceRpaConfigurationRepository(tenantIdentifier);
                    break;
                case nameof(IClaimStatusBatchClaimsRepository):
                    retVal = (T)(object)GetClaimStatusBatchClaimsRepository(tenantIdentifier);
                    break;
                case nameof(IClientLocationRepository):
                    retVal = (T)(object)GetClientLocationRepository(tenantIdentifier);
                    break;
                case nameof(IChargeEntryRpaConfigurationRepository):
                    retVal = (T)(object)GetChargeEntryRpaConfigurationRepository(tenantIdentifier);
                    break;
                case nameof(IClaimStatusTransactionRepository):
                    retVal = (T)(object)GetClaimStatusTransactionRepository(tenantIdentifier);
                    break;
                case nameof(IClaimStatusTransactionLineItemStatusChangeRepository):
                    retVal = (T)(object)GetClaimStatusTransactionLineItemStatusChangeRepository(tenantIdentifier);
                    break;
                case nameof(IClientInsuranceAverageCollectionPercentageRepository):
                    retVal = (T)(object)GetClientInsuranceAverageCollectionPercentageRepository(tenantIdentifier); //EN-91
                    break;
                case nameof(ISystemDefaultReportFilterRepository):
                    retVal = (T)(object)GetSystemDefaultReportFilterRepository(tenantIdentifier);//EN-108
                    break;
                case nameof(IClientCptCodeRepository):
                    retVal = (T)(object)GetClientCptCodeRepository(tenantIdentifier); //EN-214
                    break;
                case nameof(IClaimLineItemStatusRepository):
                    retVal = (T)(object)GetClaimLineItemStatusRepository(tenantIdentifier); //EN-214
                    break;

                case nameof(IClaimStatusExceptionReasonCategoryMapRepository):
                    retVal = (T)(object)GetClaimStatusExceptionReasonCategoryMapRepository(tenantIdentifier); //EN-214
                    break;
                case nameof(IClaimStatusTransactionHistoryRepository):
                    retVal = (T)(object)GetClaimStatusTransactionHistoryRepository(tenantIdentifier); //EN-214
                    break;


                case nameof(IX12ClaimCategoryCodeLineItemStatusRepository):
                    retVal = (T)(object)GetX12ClaimCategoryCodeLineItemStatusRepository(tenantIdentifier);
                    break;
                case nameof(IX12ClaimCodeLineItemStatusRepository):
                    retVal = (T)(object)GetX12ClaimCodeLineItemStatusRepository(tenantIdentifier);
                    break;
                case nameof(IClaimStatusBatchHistoryRepository):
                    retVal = (T)(object)GetClaimStatusBatchHistoryRepository(tenantIdentifier); //EN-438
                    break;
                case nameof(IClientUserNotificationRepository):
                    retVal = (T)(object)GetClientUserNotificationRepository(tenantIdentifier); //EN-438
                    break;
                default:
                    break;
            }

            return retVal;
        }
        public T Get<T>(int tenantId)
        {
            var aitTenantInfo = _tenantStore.TryGetAsync(tenantId.ToString()).Result;
            return Get<T>(aitTenantInfo.Identifier);
        }
        public Task<T> GetAsync<T>(string tenantIdentifier)
        {
            return Task.Run(() => { return Get<T>(tenantIdentifier); });
        }
        public IUnitOfWork<TId> GetUnitOfWork<TId>(string tenantIdentifier)
        {
            var applicationDbContext = GetApplicationContext(tenantIdentifier);
            var unitOfWork = new UnitOfWork<TId>(applicationDbContext, _currentUserService, _cache);
            return unitOfWork;
        }
        public IUnitOfWork<TId> GetUnitOfWork<TId>(int tenantId)
        {
            var aitTenantInfo = _tenantStore.TryGetAsync(tenantId.ToString()).Result;
            return GetUnitOfWork<TId>(aitTenantInfo.Identifier);
        }
        public IUserClientRepository GetUserClientRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);

            return new UserClientRepositoryAsync(dbContext, tenantInfo);
        }
        public IUserAlertRepository GetUserAlertRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);

            return new UserAlertRepositoryAysnc(dbContext);
        }

        public IClientRepository GetClientRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClientRepositoryAsync(dbContext);
        }

        public IClientCptCodeRepository GetClientCptCodeRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClientCptCodeRepository(dbContext);
        }

        public IClaimLineItemStatusRepository GetClaimLineItemStatusRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClaimLineItemStatusRepository(dbContext);
        }

        public IClaimStatusExceptionReasonCategoryMapRepository GetClaimStatusExceptionReasonCategoryMapRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClaimStatusExceptionReasonCategoryMapRepository(dbContext);
        }

        public IClaimStatusTransactionHistoryRepository GetClaimStatusTransactionHistoryRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClaimStatusTransactionHistoryRepository(dbContext);
        }

        public IX12ClaimCategoryCodeLineItemStatusRepository GetX12ClaimCategoryCodeLineItemStatusRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new X12ClaimCategoryCodeLineItemStatusRepository(dbContext);
        }

        public IX12ClaimCodeLineItemStatusRepository GetX12ClaimCodeLineItemStatusRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new X12ClaimCodeLineItemStatusRepository(dbContext);
        }

        //AA-233
        public IEmployeeRepository GetEmployeeRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new EmployeeRepository(dbContext);
        }

        public IEmployeeClientRepository GetEmployeeClientRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new EmployeeClientRepository(dbContext);
        }

        //AA-228
        public IClaimStatusBatchRepository GetClaimStatusBatchRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClaimStatusBatchRepository(dbContext, tenantInfo);
        }

        //AA-228
        public IClientInsuranceRpaConfigurationRepository GetClientInsuranceRpaConfigurationRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClientInsuranceRpaConfigurationRepository(dbContext);
        }

        //AA-228
        public IClaimStatusBatchClaimsRepository GetClaimStatusBatchClaimsRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClaimStatusBatchClaimsRepository(dbContext, _mapper, tenantInfo);
        }

        //AA-228
        public IClientLocationRepository GetClientLocationRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClientLocationRepositoryAsync(dbContext);
        }

        //AA-228
        public IChargeEntryRpaConfigurationRepository GetChargeEntryRpaConfigurationRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ChargeEntryRpaConfigurationRepository(dbContext);
        }

        //AA-228
        public IClaimStatusTransactionRepository GetClaimStatusTransactionRepository(string tenantIdentifier)
        {

            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClaimStatusTransactionRepository(dbContext, tenantInfo);
        }

        //AA-228
        public IClaimStatusTransactionLineItemStatusChangeRepository GetClaimStatusTransactionLineItemStatusChangeRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClaimStatusTransactionLineItemStatusChangeRepository(dbContext);
        }

        public IClientInsuranceAverageCollectionPercentageRepository GetClientInsuranceAverageCollectionPercentageRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClientInsuranceAverageCollectionPercentageRepository(dbContext);
        }
        //EN-111
        public ISystemDefaultReportFilterRepository GetSystemDefaultReportFilterRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new SystemDefaultReportFilterRepository(dbContext);
        }

        public IClaimStatusBatchHistoryRepository GetClaimStatusBatchHistoryRepository(string tenantIdentifier)
        {
            var repo = GetUnitOfWork<int>(tenantIdentifier).Repository<ClaimStatusBatchHistory>();
            return new ClaimStatusBatchHistoryRepository(repo);
        }

		public IPersonRepository GetPersonRepository(string tenantIdentifier)
		{
			var dbContext = GetApplicationContext(tenantIdentifier);
			return new PersonRepositoryAsync(dbContext);
        }

        public IAddressRepository GetAddressRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            var repo = GetUnitOfWork<int>(tenantIdentifier).Repository<Address>();
            return new AddressRepositoryAsync(dbContext, repo, _mapper);
        }

        public IClientUserNotificationRepository GetClientUserNotificationRepository(string tenantIdentifier)
        {
            var dbContext = GetApplicationContext(tenantIdentifier);
            return new ClientUserNotificationRepository(dbContext);
        }

    }
}
