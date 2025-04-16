using System;
using System.Globalization;
using System.Net.Http;

namespace MedHelpAuthorizations.Infrastructure.Integrations.HttpClients
{
    public class SelfPayInternalClient : ISelfPayInternalClient
    {
        public HttpClient Client { get; private set; }

        public SelfPayInternalClient(HttpClient httpClient ,string environmentStatus = "Beta",
            string apiKey = "BBC6A080F9EB34CE35FBAC3B7DBED786B6573943EA839B3710D9CE4F1D252C36299582270CCB593E089CC1A3F15C17D7C4B928FFDD1B19A4E496975BB76DB75F")
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

            // Add API Key to request headers
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            }
            else
            {
                throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));
            }

            string environmentType = GetEnvironmentType(environmentStatus);

            // Set BaseAddress based on the environment type
            httpClient.BaseAddress = environmentType switch
            {
                "Production" => new Uri("https://sp-eligibility.azurewebsites.net/"),
                "Beta" => new Uri("https://sp-eligibility.azurewebsites.net/"),
                "Local" => new Uri("https://localhost:6001/"),
                _ => throw new InvalidOperationException($"Unsupported environment type: {environmentType}")
            };

            // Set default request headers
            httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName ?? "en");

            Client = httpClient;
        }

        private string GetEnvironmentType(string environmentStatus)
        {
            if (!string.IsNullOrWhiteSpace(environmentStatus))
            {
                return environmentStatus;
            }

            throw new InvalidOperationException("Environment type could not be determined.");
        }
    }
}
