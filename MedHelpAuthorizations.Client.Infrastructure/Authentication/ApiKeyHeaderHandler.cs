using Blazored.LocalStorage;
using MedHelpAuthorizations.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Authentication
{
    public class ApiKeyHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ClientInfoStateProvider _clientInfoStateProvider;

        public ApiKeyHeaderHandler(ILocalStorageService localStorage, ClientInfoStateProvider clientInfoStateProvider)
        {
            _localStorage = localStorage;
            _clientInfoStateProvider = clientInfoStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                //var apiKeysString = await _localStorage.GetItemAsStringAsync("apiKeys");
               // var keys = JsonConvert.DeserializeObject(apiKeysString);
                //var apiKeys = JsonConvert.DeserializeObject<ApiIntegrationKey[]>(JsonConvert.DeserializeObject(apiKeysString).ToString());
                var apiKeys = _clientInfoStateProvider?.ClientInfo?.ApiIntegrationKeys;
                //var apiKeys = await _localStorage.GetItemAsync<List<ApiIntegrationKey>>("apiKeys");
                var speApiKey = apiKeys != null ? apiKeys.FirstOrDefault(x => x.ApiIntegrationName == MedHelpAuthorizations.Shared.Enums.ApiIntegrationEnum.SelfPayEligibility.ToString()) : null;

                if (speApiKey != null)
                    request.Headers.Add("x-api-key", speApiKey.ApiKey); // "tE564fdMEu8hjXjZeLX4xM7mkUHhtWPlIk2XNWKdYsBTGXIKIHJAS4FvMzlPY1KGCQt7J5edfXlib3wkQvI3osULuj8T4rwFj0XZI5czRUrq6UFpxVCAl21fKXZM2B2F");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}