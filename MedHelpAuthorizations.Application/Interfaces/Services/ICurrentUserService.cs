using MedHelpAuthorizations.Application.Interfaces.Common;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
        string TenantIdentifier { get; }
        int ClientId { get; }
    }
}