using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class TenantClientApiKeyServiceFake : ITenantClientApiKeyService
    {
        private string? _tenantIdentifier { get; set; }
        private int? _clientId { get; set; }
        public string? TenantIdentifier { get => _tenantIdentifier; }
        public int? ClientId { get => _clientId; }

        public TenantClientApiKeyServiceFake(int clientId, string tenantIdentifier)
        {            
            _clientId = 1;
            _tenantIdentifier = "initial";            
        }

        public void SetFromTenantClientString(string tenantClientString)
        {
            _clientId = 1;
            _tenantIdentifier = "initial";
        }
    }
}
