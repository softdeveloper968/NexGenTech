using MedHelpAuthorizations.Client.Infrastructure.Authentication;
using Microsoft.AspNetCore.Components;
using System;
using System.Globalization;
using System.Net.Http;

namespace MedHelpAuthorizations.Client.Infrastructure.HttpClients
{
    public class SelfPayEligibilityHttpClient : ISelfPayEligibilityHttpClient
    {
        public HttpClient Client { get; private set; }

        [Inject] private ClientInfoStateProvider _clientInfoStateProvider { get; set; }

        public SelfPayEligibilityHttpClient(HttpClient httpClient, ClientInfoStateProvider clientInfoStateProvider)
        {
            //_localStorageService = localStorageService;

            //var apiKeys = GetApiIntegrationKeys(_localStorageService).GetAwaiter().GetResult();
            //var speApiKey = apiKeys.FirstOrDefault(x => x.ApiIntegrationId == MedHelpAuthorizations.Shared.Enums.ApiIntegrationEnum.SelfPayEligibility);

            //if (speApiKey != null)
            //    httpClient.DefaultRequestHeaders.Add("x-api-key", "tE564fdMEu8hjXjZeLX4xM7mkUHhtWPlIk2XNWKdYsBTGXIKIHJAS4FvMzlPY1KGCQt7J5edfXlib3wkQvI3osULuj8T4rwFj0XZI5czRUrq6UFpxVCAl21fKXZM2B2F");

            _clientInfoStateProvider = clientInfoStateProvider;

            string environmentType = _clientInfoStateProvider?.ClientInfo?.EnvironmentType;

            if (environmentType != null)
            {
                switch (environmentType)
                {
                    case "Production":
                        httpClient.BaseAddress = new Uri("https://sp-eligibility.azurewebsites.net/");
                        break;

                    case "Beta":
                        httpClient.BaseAddress = new Uri("https://sp-eligibility.azurewebsites.net/");
                        break;

                    case "Local":
                        httpClient.BaseAddress = new Uri("https://localhost:6001/");
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported environment type: {environmentType}");
                }
            }
            else
            {
                httpClient.BaseAddress = new Uri("https://sp-eligibility.azurewebsites.net/");
            }
            //httpClient.BaseAddress = new Uri("https://localhost:6001/");
            httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
            Client = httpClient;
        }
    }
}
