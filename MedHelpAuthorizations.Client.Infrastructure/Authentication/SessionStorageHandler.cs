using Blazored.SessionStorage;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Authentication
{
	public class SessionStorageHandler : DelegatingHandler
	{
		private readonly ISessionStorageService _sessionStorageService;

		public SessionStorageHandler(ISessionStorageService sessionStorageService)
		{
			_sessionStorageService = sessionStorageService;
		}

		/// <summary>
		/// Invoke when Api trigger, To Set the last activity time in session storage
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			// Set the last activity time in session storage asynchronously
			await SetLastActivityTimeAsync();

			// Call the base SendAsync method and return its result
			return await base.SendAsync(request, cancellationToken);
		}

		/// <summary>
		/// Store the last activity time in session storage
		/// </summary>
		/// <returns></returns>
		private async Task SetLastActivityTimeAsync()
		{
			// Set the last activity time in session storage
			await _sessionStorageService.SetItemAsync("LastActivityTime", DateTime.UtcNow.ToLocalTime().ToString());
		}
	}
}
