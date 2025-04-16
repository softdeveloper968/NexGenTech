using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;
using MedHelpAuthorizations.Application.Multitenancy;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class TenantClientApiKeyService : ITenantClientApiKeyService
    {
        private readonly ITenantCryptographyService _tenantCryptographyService;
        private string? _tenantIdentifier { get; set; }
        private int? _clientId { get; set; }
        public string? TenantIdentifier { get => _tenantIdentifier; }
        public int? ClientId { get => _clientId; }

        //public TenantClientApiKeyService(bool isDesignTime = false)
        //{
        //    if(isDesignTime)
        //    {
        //        _clientId = 1;
        //        _tenantIdentifier = "initial";
        //    }
        //}
        public TenantClientApiKeyService(ITenantCryptographyService tenantCryptographyService)
        {
            _tenantCryptographyService = tenantCryptographyService;
        }

        public void SetFromTenantClientString(string tenantClientString)
        {
            var decryptedTenantClient = _tenantCryptographyService.Decrypt(tenantClientString);

            _tenantIdentifier = decryptedTenantClient.Item1;
            _clientId = decryptedTenantClient.Item2;
        }
    }
}
