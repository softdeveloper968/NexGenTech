using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Domain.IdentityEntities;
using MedHelpAuthorizations.Infrastructure.Persistence.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services.Admin
{
    public class TenantMediatorService : ITenantMediatorService
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public TenantMediatorService(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }
        public async Task InitializeTenant(Tenant tenant, CancellationToken cancellationToken)
        {
            await _databaseInitializer.InitializeApplicationDbForTenantAsync(new Shared.MultiTenancy.AitTenantInfo(tenant), cancellationToken);
        }
    }
}
