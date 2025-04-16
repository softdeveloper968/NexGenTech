using MedHelpAuthorizations.Application.Features.UserAlerts.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetById;
using MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetByUserId;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.UserAlert
{
    public class UserAlertManager : IUserAlertManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public UserAlertManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{Routes.UserAlertEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetUserAlertByUserIdResponse>> GetUserAlertsByUserAsync(string userId, int pageNumber, int pageSize)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.UserAlertEndpoints.GetByUserId(userId, pageNumber, pageSize));
            return await response.ToPaginatedResult<GetUserAlertByUserIdResponse>();
        }

        public async Task<IResult<GetUserAlertsByIdResponse>> GetByIdAsync(GetUserAlertByIdQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.UserAlertEndpoints.GetById(request.Id));
            return await response.ToResult<GetUserAlertsByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditUserAlertCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync<AddEditUserAlertCommand>(Routes.UserAlertEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }

}
