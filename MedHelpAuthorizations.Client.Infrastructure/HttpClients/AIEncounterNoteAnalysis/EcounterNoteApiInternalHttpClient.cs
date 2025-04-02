using MedHelpAuthorizations.Client.Infrastructure.Authentication;
using MedHelpAuthorizations.Shared.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.HttpClients.AIEncounterNoteAnalysis
{
    public class EcounterNoteApiInternalHttpClient : IEcounterNoteApiInternalHttpClient
    {
        public HttpClient Client { get; private set; }
        [Inject] 
        private ClientInfoStateProvider _clientInfoStateProvider { get; set; }

        private readonly ISnackbar _snackBar;

        public EcounterNoteApiInternalHttpClient(HttpClient httpClient, ClientInfoStateProvider clientInfoStateProvider, ISnackbar snackbar /*string apiKey = "64DC397C90060CB2CD50FAFF33BBE4F13D52F8C1FA4317D3D17997CF49ADA5B288B2958ADB429FF15AD735DBE558F7C5C1CAD29E382D599AD19155FF2A2E9135"*/)
        {
            try
            {
                _clientInfoStateProvider = clientInfoStateProvider;
                _snackBar = snackbar;
                var apiKeys = _clientInfoStateProvider?.ClientInfo?.ApiIntegrationKeys;

                if (apiKeys == null)
                {
                    _snackBar.Add("Error: ApiIntegrationKey is null.", Severity.Error);
                    return;
                }

                var encounterApiKey = apiKeys.FirstOrDefault(x => x.ApiIntegrationName == ApiIntegrationEnum.AIEncounterNoteAnalytics.ToString());
                if (encounterApiKey == null || string.IsNullOrEmpty(encounterApiKey.ApiKey))
                {
                    _snackBar.Add("Error: Encounter API key is missing or invalid", Severity.Error);
                    return;
                }
                
                httpClient.DefaultRequestHeaders.Add("x-api-key", encounterApiKey.ApiKey);

                string environmentType = _clientInfoStateProvider.ClientInfo?.EnvironmentType;

                if (string.IsNullOrEmpty(environmentType))
                {
                    _snackBar.Add("Error: EnvironmentType cannot be null or empty.", Severity.Error);
                    return;
                }

                switch (environmentType)
                {
                    case "Production":
                        httpClient.BaseAddress = new Uri("https://ait-encounter-note-services-fpdhhth3c8eycncr.centralus-01.azurewebsites.net/");
                        break;

                    case "Beta":
                        httpClient.BaseAddress = new Uri("https://localhost:6001/");
                        break;

                    case "Local":
                        httpClient.BaseAddress = new Uri("https://localhost:6001/");
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported environment type: {environmentType}");
                }


                //httpClient.BaseAddress = new Uri("https://localhost:6001/");
                //httpClient.BaseAddress = new Uri("https://sp-eligibility.azurewebsites.net/");
                httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
                httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                Client = httpClient;

            }
            catch (Exception ex)
            {
                return;
            }
        }

        public Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) =>
        Client.GetAsync(requestUri);

        public Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) =>
            Client.PostAsync(requestUri, content);

        public Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) =>
            Client.PutAsync(requestUri, content);

        public Task<HttpResponseMessage> PatchAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) =>
            Client.PatchAsync(requestUri, content);

        public Task<HttpResponseMessage> DeleteAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) =>
            Client.DeleteAsync(requestUri);

        public Task<HttpResponseMessage> PostAsJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value) =>
            Client.PostAsJsonAsync<TValue>(requestUri, value);

        public Task<HttpResponseMessage> PutAsJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value) =>
            Client.PutAsJsonAsync<TValue>(requestUri, value);

        public Task<TValue?> GetFromJsonAsync<TValue>([StringSyntax("Uri")] string requestUri) =>
            Client.GetFromJsonAsync<TValue>(requestUri);
    }
}
