using MedHelpAuthorizations.Shared.Constants.BlobStorage;
using MedHelpAuthorizations.Shared.Requests;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IBlobStorageManager : IManager
    {
        Task<string> UploadFileAsync(UploadRequest fileData);

        Task<IResult<BlobStorageFileDownloadResponse>> DownloadFileFromBlobStaorageURLAsync(string downloadURL);
    }
}
