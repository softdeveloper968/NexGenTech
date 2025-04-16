using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.HttpClients
{
	public class TenantHttpClient : ITenantHttpClient
	{
		private HttpClient _httpClient;
		public HttpClient Client
		{
			get
			{
				return _httpClient;
			}
			private set
			{
				_httpClient = value;
			}
		}
		public NavigationManager NavigationManager { get; }

		public TenantHttpClient(HttpClient httpClient, NavigationManager navigationManager)
		{
			NavigationManager = navigationManager;
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(NavigationManager.BaseUri);
			_httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
		}
		private string GetTenantClientString()
		{
			StringValues tenantString;
			var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
			if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("t", out tenantString))
			{
				return tenantString.ToString();
			}
			else
			{
				throw new Exception("No tenant String Found");
			}

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
