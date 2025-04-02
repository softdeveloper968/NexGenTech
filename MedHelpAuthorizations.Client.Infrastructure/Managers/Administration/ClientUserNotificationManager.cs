using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Commands;
using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetById;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientUserNotificationManager : IClientUserNotificationManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientUserNotificationManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientUserNotificationCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientUserNotificationEndPoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllClientUserNotificationResponse>>> GetRecentClientsAsync(int maxResults)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ClientUserNotificationEndPoints.GetRecent(maxResults));
                return await response.ToResult<List<GetAllClientUserNotificationResponse>>();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<IResult<GetAllClientUserNotificationResponse>> GetNotificationByFileNameAsync(string fileName)
        {
            var response = await _tenantHttpClient.GetAsync(ClientUserNotificationEndPoints.GetByFileName(fileName));
            return await response.ToResult<GetAllClientUserNotificationResponse>();
        }
    }
}
