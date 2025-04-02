using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetClientsByCriteria;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
	public class ClientManager : IClientManager
	{
		private readonly ITenantHttpClient _tenantHttpClient;
		//[Inject] private HttpClient httpClient { get; set; }
		public ClientManager(ITenantHttpClient tenantHttpClient)
		{
			_tenantHttpClient = tenantHttpClient;
		}

		public async Task<IResult<int>> DeleteAsync(int id)
		{
			var response = await _tenantHttpClient.DeleteAsync($"{ClientsEndpoint.Delete}/{id}");
			return await response.ToResult<int>();
		}

		public async Task<string> ExportToExcelAsync()
		{
			var response = await _tenantHttpClient.GetAsync(ClientsEndpoint.Export);
			var data = await response.Content.ReadAsStringAsync();
			return data;
		}

		public async Task<IResult<string>> GetClientImageAsync(int id)
		{
			var response = await _tenantHttpClient.GetAsync(ClientsEndpoint.GetClientImage(id));
			return await response.ToResult<string>();
		}

		public async Task<PaginatedResult<GetAllPagedClientsResponse>> GetClientsAsync(GetAllPagedClientsRequest request)
		{
			try
			{
				var response = await _tenantHttpClient.GetAsync(ClientsEndpoint.GetAllPaged(request.PageNumber, request.PageSize));
				return await response.ToPaginatedResult<GetAllPagedClientsResponse>();
			}
			catch (System.Exception ex)
			{

				throw ex;
			}
			
		}

		public async Task<IResult<int>> SaveAsync(AddEditClientCommand request)
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(ClientsEndpoint.Save, request);
			return await response.ToResult<int>();
		}

		public async Task<PaginatedResult<GetClientsByCriteriaResponse>> GetByCriteriaAsync(GetClientsByCriteriaQuery query)
		{
			var response = await _tenantHttpClient.GetAsync(ClientsEndpoint.GetByCriteriaPaged(query));
			return await response.ToPaginatedResult<GetClientsByCriteriaResponse>();
		}

		public async Task<IResult<GetClientByIdResponse>> GetClientByIdAsync(int id = 0)
		{
			var response = await _tenantHttpClient.GetAsync(ClientsEndpoint.GetById(id));
			return await response.ToResult<GetClientByIdResponse>();
		}

		public async Task<IResult<List<MedHelpAuthorizations.Domain.Entities.Client>>> GetAllClient()
		{
			var manager = await _tenantHttpClient.GetAsync(ClientsEndpoint.GetAllClients());
			return await manager.ToResult<List<MedHelpAuthorizations.Domain.Entities.Client>>();
		}

        //public async Task<IResult<List<TenantResponse>>> GetTenants() //AA-206
        //{
        //	var response = await httpClient.GetAsync(UserEndpoints.GetUserTenants());
        //	return await response.ToResult<List<TenantResponse>>();
        //}
    }
}
