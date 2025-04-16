using MedHelpAuthorizations.Application.Models.Chat;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Communication
{
    public class ChatManager : IChatManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ChatManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ChatEndpoint.GetChatHistory(cId));
            var data = await response.ToResult<IEnumerable<ChatHistoryResponse>>();
            return data;
        }

        public async Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ChatEndpoint.GetAvailableUsers);
            var data = await response.ToResult<IEnumerable<ChatUserResponse>>();
            return data;
        }

        //public async Task<IResult> SaveMessageAsync(ChatHistory chatHistory)
        //{
        //    var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ChatEndpoint.SaveMessage, chatHistory);
        //    var data = await response.ToResult();
        //    return data;
        //}
    }
}