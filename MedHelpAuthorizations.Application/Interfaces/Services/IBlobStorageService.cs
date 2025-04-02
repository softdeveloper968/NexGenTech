using Azure.Storage.Blobs.Models;
using MedHelpAuthorizations.Shared.Constants.BlobStorage;
using MedHelpAuthorizations.Shared.Requests;
using System.IO;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IBlobStorageService
    {
        Task UploadBlobAsync(string blobName, Stream content, string contentType);
        Task<BlobStorageFileDownloadResponse> DownloadBlobAsByteArrayAsync(string blobName);
        Task DeleteBlobAsync(string blobName);
        Task UploadFolderAsync(string folderPath);
        Task<string> UploadToBlobStorageAsync(UploadRequest request);
        Task<BlobDownloadInfo> DownloadBlobAsStreamAsync(string blobName);
        Task<string> GetImageURL(string blobName);
        Task<string> UploadToBlobStorageAndReturnUrlAsync(UploadRequest request);
        Task<string> UploadToBlobStorageAndSendEmailToUserAsync(UploadRequest request); //EN-714
        Task<string> UploadToBlobStorageAsync(UploadRequest request, string clientCode = null); //EN-791
    }

}