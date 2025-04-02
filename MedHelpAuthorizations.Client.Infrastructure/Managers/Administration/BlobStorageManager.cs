using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Constants.BlobStorage;
using MedHelpAuthorizations.Shared.Requests;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class BlobStorageManager : IBlobStorageManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public BlobStorageManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<string> UploadFileAsync(UploadRequest fileData)
        {
            try
            {
                string url = BlobStorageEndpoints.UploadReportsToBlobAndSendMailAsync(fileData);

                // Perform the POST request
                var response = await _tenantHttpClient.PostAsJsonAsync(url, fileData);
                // Ensure the response is successful
                response.EnsureSuccessStatusCode(); 

                // If the response has content, read it as a string (assuming it's a string response).
                return await response.Content.ReadAsStringAsync();
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }

        public async Task<IResult<BlobStorageFileDownloadResponse>> DownloadFileFromBlobStaorageURLAsync(string downloadURL)
        {
            try
            {
                string url = BlobStorageEndpoints.DownloadFromBlobStorageURL(downloadURL);
                var response = await _tenantHttpClient.GetAsync(url);
                return await response.ToResult<BlobStorageFileDownloadResponse>();
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }
    }
}
