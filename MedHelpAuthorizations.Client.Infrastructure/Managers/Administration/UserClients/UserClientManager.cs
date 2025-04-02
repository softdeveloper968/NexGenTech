using MedHelpAuthorizations.Application.Features.UserClients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.UserClients.Queries.GetByUserId;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration.UserClients
{
	public class UserClientManager : IUserClientManager
	{
        private readonly ITenantHttpClient _tenantHttpClient;

        public UserClientManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }


        public async Task<PaginatedResult<GetByUserIdQueryResponse>> GetClientsByUserAsync(GetByUserIdQuery request)
		{
			var response = await _tenantHttpClient.GetAsync(UserClientEndpoints.GetClientsByUserIdPaged(request.PageNumber, request.PageSize, request.UserId));
			return await response.ToPaginatedResult<GetByUserIdQueryResponse>();
		}

		public async Task<IResult<string>> SaveAsync(AddEditUserClientCommand request)
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(UserClientEndpoints.Save, request);
			return await response.ToResult<string>();
		}
	}
}
