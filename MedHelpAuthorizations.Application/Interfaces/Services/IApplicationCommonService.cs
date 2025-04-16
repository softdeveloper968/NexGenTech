using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IApplicationCommonService
    {
        Task<Result<string[]>> GetApplicationFeatures();
        Task<Result<ApiIntegrationKey[]>> GetApiKeys();
    }
}
