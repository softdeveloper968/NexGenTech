using MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientApiKeyManager : IClientApiKeyManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public ClientApiKeyManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
            
        public async Task<PaginatedResult<ApiKeyViewModel>> GetAllClientApiKeyPagedAsync(GetAllPagedClientApiKeyRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientApiKeyEndPoints.GetAllPaged(request.PageNumber, request.PageSize, request.ClientId));
            return await response.ToPaginatedResult<ApiKeyViewModel>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientApiKeyEndPoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientApiKeyCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientApiKeyEndPoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<ApiKeyViewModel>> GetByCriteria(GetAllPagedClientApiKeyRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientApiKeyEndPoints.GetAllPagedByClientId(request));
            return await response.ToPaginatedResult<ApiKeyViewModel>();
        }
    }
}
