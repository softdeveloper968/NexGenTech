using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Common.Exceptions;
using MedHelpAuthorizations.Application.Common.Persistence;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;

namespace MedHelpAuthorizations.Infrastructure.Services.MultiTenancy
{
    //internal class TenantService2 : ITenantService2
    //{
    //    private readonly IMultiTenantStore<AitTenantInfo> _tenantStore;
    //    private readonly IConnectionStringSecurer _csSecurer;
    //    private readonly IDatabaseInitializer _dbInitializer;
    //    private readonly IStringLocalizer<CurrentTenantService> _localizer;
    //    private readonly DatabaseSettings _dbSettings;

    //    public TenantService2(
    //        IMultiTenantStore<AitTenantInfo> tenantStore,
    //        IConnectionStringSecurer csSecurer,
    //        IDatabaseInitializer dbInitializer,
    //        IStringLocalizer<CurrentTenantService> localizer,
    //        IOptions<DatabaseSettings> dbSettings)
    //    {
    //        _tenantStore = tenantStore;
    //        _csSecurer = csSecurer;
    //        _dbInitializer = dbInitializer;
    //        _localizer = localizer;
    //        _dbSettings = dbSettings.Value;
    //    }

    //    public async Task<List<TenantDto>> GetAllAsync()
    //    {
    //        var tenants = (await _tenantStore.GetAllAsync()).Adapt<List<TenantDto>>();
    //        tenants.ForEach(t => t.ConnectionString = _csSecurer.MakeSecure(t.ConnectionString));
    //        return tenants;
    //    }

    //    public async Task<bool> ExistsWithIdAsync(string id) =>
    //        await _tenantStore.TryGetAsync(id) is not null;

    //    public async Task<bool> ExistsWithNameAsync(string name) =>
    //        (await _tenantStore.GetAllAsync()).Any(t => t.Name == name);

    //    public async Task<TenantDto> GetByIdAsync(string id) =>
    //        (await GetTenantInfoAsync(id))
    //            .Adapt<TenantDto>();

    //    public async Task<string> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken)
    //    {
    //        //if (request.ConnectionString?.Trim() == _dbSettings.ConnectionString?.Trim()) request.ConnectionString = string.Empty;

    //        //var tenant = new AitTenantInfo(request.Id, request.Name, request.ConnectionString, request.AdminEmail, request.Issuer);
    //        //await _tenantStore.TryAddAsync(tenant);

    //        //// TODO: run this in a hangfire job? will then have to send mail when it's ready or not
    //        //try
    //        //{
    //        //    await _dbInitializer.InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
    //        //}
    //        //catch
    //        //{
    //        //    await _tenantStore.TryRemoveAsync(request.Id);
    //        //    throw;
    //        //}

    //        //return tenant.Id;
    //        return null;
    //    }

    //    public async Task<string> ActivateAsync(string id)
    //    {
    //        var tenant = await GetTenantInfoAsync(id);

    //        if (tenant.IsActive)
    //        {
    //            throw new ConflictException("Tenant is already Activated.");
    //        }

    //        tenant.Activate();

    //        await _tenantStore.TryUpdateAsync(tenant);

    //        return $"Tenant {id} is now Activated.";
    //    }

    //    public async Task<string> DeactivateAsync(string id)
    //    {
    //        var tenant = await GetTenantInfoAsync(id);

    //        if (!tenant.IsActive)
    //        {
    //            throw new ConflictException("Tenant is already Deactivated.");
    //        }

    //        tenant.Deactivate();

    //        await _tenantStore.TryUpdateAsync(tenant);

    //        return $"Tenant {id} is now Deactivated.";
    //    }

    //    public async Task<string> UpdateSubscription(string id, DateTime extendedExpiryDate)
    //    {
    //        var tenant = await GetTenantInfoAsync(id);

    //        tenant.SetValidity(extendedExpiryDate);

    //        await _tenantStore.TryUpdateAsync(tenant);

    //        return $"Tenant {id}'s Subscription Upgraded. Now Valid till {tenant.ValidUpto}.";
    //    }

    //    private async Task<AitTenantInfo> GetTenantInfoAsync(string id) =>
    //        await _tenantStore.TryGetAsync(id)
    //            ?? throw new NotFoundException(string.Format(_localizer["entity.notfound"], typeof(AitTenantInfo).Name, id));

    //}
}
