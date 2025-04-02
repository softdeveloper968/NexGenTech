using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.HttpClients
{
    public class TenantApiHttpClient : ITenantHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _tenantClientString;
		public HttpClient Client => _httpClient;
		public TenantApiHttpClient(HttpClient httpClient, string tenantClientString)
		{
			_httpClient = httpClient;
			_tenantClientString = tenantClientString;
		}
		private string GetTenantClientString()
		{
			if (string.IsNullOrEmpty(_tenantClientString))
			{
				throw new Exception("Invalid tenantClientString");
			}

			return _tenantClientString;
		}
		private string ApplyTenantClientString(string url)
		{
			var retVal = QueryHelpers.AddQueryString(url, "t", GetTenantClientString());
			return retVal;
		}
		public Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) =>
			Client.GetAsync(ApplyTenantClientString(requestUri));

		public Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) =>
			Client.PostAsync(ApplyTenantClientString(requestUri), content);

		public Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) =>
			Client.PutAsync(ApplyTenantClientString(requestUri), content);

		public Task<HttpResponseMessage> PatchAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) =>
			Client.PatchAsync(ApplyTenantClientString(requestUri), content);

		public Task<HttpResponseMessage> DeleteAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) =>
			Client.DeleteAsync(ApplyTenantClientString(requestUri));

		public Task<HttpResponseMessage> PostAsJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value) =>
			Client.PostAsJsonAsync<TValue>(ApplyTenantClientString(requestUri), value);

		public Task<HttpResponseMessage> PutAsJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value) =>
			Client.PutAsJsonAsync<TValue>(ApplyTenantClientString(requestUri), value);

		public Task<TValue?> GetFromJsonAsync<TValue>([StringSyntax("Uri")] string requestUri) =>
			Client.GetFromJsonAsync<TValue>(ApplyTenantClientString(requestUri));
	}
}
