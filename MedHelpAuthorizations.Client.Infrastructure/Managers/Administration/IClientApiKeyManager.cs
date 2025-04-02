using MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientApiKeyManager : IManager
    {
        Task<PaginatedResult<ApiKeyViewModel>> GetAllClientApiKeyPagedAsync(GetAllPagedClientApiKeyRequest request);
        Task<IResult<int>> SaveAsync(AddEditClientApiKeyCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<PaginatedResult<ApiKeyViewModel>> GetByCriteria(GetAllPagedClientApiKeyRequest request);
    }
}
