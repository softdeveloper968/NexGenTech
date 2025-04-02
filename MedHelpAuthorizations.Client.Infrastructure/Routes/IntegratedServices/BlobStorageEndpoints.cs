using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public static class BlobStorageEndpoints
    {
        public static int apiVersion = 1;
        public static string DownloadFromBlobStorageURL(string blobName)
        {
            return $"/api/v1/tenant/blobstorage/download?blobUrl={blobName}";
        }

        public static string UploadReportsToBlobAndSendMailAsync(UploadRequest fileData)
        {
            return $"/api/v1/tenant/blobstorage/uploadReportsToBlob";
        }
    }
}
