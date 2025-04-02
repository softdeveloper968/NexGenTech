using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class AboutDialogManager : IAboutDialogManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public AboutDialogManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<ReleaseArtifactInfo>> GetPublishInformation()
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(AboutEndPoint.Get);
                return await response.ToResult<ReleaseArtifactInfo>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
