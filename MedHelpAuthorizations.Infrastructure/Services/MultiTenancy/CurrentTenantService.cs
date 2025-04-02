using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using Microsoft.AspNetCore.Http;
using System;

namespace MedHelpAuthorizations.Infrastructure.Services.MultiTenancy
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private HttpContext _httpContext;
        private ITenantInfo _currentTenant;
        private readonly ITenantCryptographyService _tenantCryptographyService;
        private readonly IMultiTenantStore<AitTenantInfo> _multiTenantStore;

        public CurrentTenantService
        (
            IHttpContextAccessor contextAccessor,
            ITenantCryptographyService tenantCryptographyService,
            IMultiTenantStore<AitTenantInfo> multiTenantStore
        )
        {
            _tenantCryptographyService = tenantCryptographyService;
            _multiTenantStore = multiTenantStore;
            _httpContext = contextAccessor.HttpContext;
            if (_httpContext != null)
            {
                if (_httpContext.Request.Query.TryGetValue("t", out var tenantClientString))
                {
                    string tenantId = _tenantCryptographyService.Decrypt(tenantClientString).Item1;

                    var tenantInfo = _multiTenantStore.TryGetAsync(tenantId).Result;

                    if (tenantInfo != null)
                    {
                        SetTenant(tenantInfo);
                    }
                }
                else
                {
                    throw new Exception("Invalid Tenant!");
                }
            }
        }
        private void SetTenant(ITenantInfo tenantInfo)
        {
            _currentTenant = tenantInfo;
            if (_currentTenant == null) throw new Exception("Invalid Tenant!");
        }
        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }
        public string GetDatabaseProvider()
        {
            throw new NotImplementedException();
        //    return _currentTenant. _tenantSettings.DBProvider;
        }
        public string GetTenantIdentifier()
        {
            return _currentTenant.Identifier;
        }
        public string GetTenantName()
        {
            return _currentTenant.Name;
        }
    }
}
