using MedHelpAuthorizations.Application.Interfaces.Common;

namespace MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy
{
    public interface ITenantClientApiKeyService : IService
    {
        string? TenantIdentifier { get; }
        int? ClientId { get; }

        public void SetFromTenantClientString(string tenantClientString);
    }
}