using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetById;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class AuthTypeManager : IAuthTypeManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public AuthTypeManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{AuthTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(AuthTypesEndpoints.Export);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<PaginatedResult<GetAllPagedAuthTypesResponse>> GetAuthTypesPaginatedAsync(GetAllPagedAuthTypesRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(AuthTypesEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedAuthTypesResponse>();
        }

        public async Task<IResult<List<GetAllPagedAuthTypesResponse>>> GetAuthTypesAsync() //AA-23
        {
            var response = await _tenantHttpClient.GetAsync(AuthTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllPagedAuthTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditAuthTypeCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(AuthTypesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<GetAuthTypeByIdResponse>> GetAuthTypesByIdAsync(GetAuthTypeByIdQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(AuthTypesEndpoints.GetById(request.Id));
            return await response.ToResult<GetAuthTypeByIdResponse>();
        }
    }
}