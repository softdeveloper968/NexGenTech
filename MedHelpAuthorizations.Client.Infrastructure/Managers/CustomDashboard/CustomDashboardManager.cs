using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.CustomDashboard
{
    public class CustomDashboardManager : ICustomDashboardManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public CustomDashboardManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        /// <summary>
        /// Gets dashboard items from the API.
        /// </summary>
        /// <returns>An <see cref="IResult"/> containing a collection of <see cref="DashboardItem"/>.</returns>
        public async Task<IResult<IEnumerable<DashboardItem>>> GetDashboardItems()
        {
            // Send an HTTP GET request to retrieve dashboard items
            var response = await _tenantHttpClient.GetAsync(Routes.CustomDashboardEndPoints.GetDashboardItems);

            // Convert the response to a result containing a collection of DashboardItem
            var data = await response.ToResult<IEnumerable<DashboardItem>>();

            // Return the result
            return data;
        }

        /// <summary>
        /// Gets user-specific dashboard items from the API.
        /// </summary>
        /// <returns>An <see cref="IResult"/> containing a collection of <see cref="UserDashboardItem"/>.</returns>
        public async Task<IResult<IEnumerable<UserDashboardItem>>> GetUserDashboardItems()
        {
            // Send an HTTP GET request to retrieve user-specific dashboard items
            var response = await _tenantHttpClient.GetAsync(Routes.CustomDashboardEndPoints.GetUserDashboardItems);

            // Convert the response to a result containing a collection of UserDashboardItem
            var data = await response.ToResult<IEnumerable<UserDashboardItem>>();

            // Return the result
            return data;
        }

        /// <summary>
        /// Saves user dashboard configurations to the API.
        /// </summary>
        /// <param name="UserDashboardItems">The list of user dashboard items to save.</param>
        /// <returns>An <see cref="IResult"/> indicating the success of the operation.</returns>
        public async Task<IResult> SaveDashboardConfigurations(List<UserDashboardItem> UserDashboardItems)
        {
            // Send an HTTP POST request to save user dashboard configurations
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.CustomDashboardEndPoints.SaveUserDashboardConfig, UserDashboardItems);

            // Convert the response to a result indicating the success of the operation
            var data = await response.ToResult();

            // Return the result
            return data;
        }
    }
}
