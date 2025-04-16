using MedHelpAuthorizations.Application.Requests;
using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}