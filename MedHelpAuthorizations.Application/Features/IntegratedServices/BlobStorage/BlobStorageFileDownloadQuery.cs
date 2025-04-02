using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Constants.BlobStorage;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.BlobStorage
{
    public class BlobStorageFileDownloadQuery(string blobUrl) : IRequest<Result<BlobStorageFileDownloadResponse>>
    {
        public string URL { get; set; } = blobUrl;
    }

    public class BlobStorageFileDownloadQueryHandler : IRequestHandler<BlobStorageFileDownloadQuery, Result<BlobStorageFileDownloadResponse>>
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringLocalizer<BlobStorageFileDownloadQueryHandler> _localizer;

        public BlobStorageFileDownloadQueryHandler(IBlobStorageService blobStorageService, IStringLocalizer<BlobStorageFileDownloadQueryHandler> localizer)
        {
            _blobStorageService = blobStorageService;
            _localizer = localizer;
        }

        public async Task<Result<BlobStorageFileDownloadResponse>> Handle(BlobStorageFileDownloadQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _blobStorageService.DownloadBlobAsByteArrayAsync(query.URL);
                return await Result<BlobStorageFileDownloadResponse>.SuccessAsync(response);
            }
            catch (Exception e)
            {
                return await Result<BlobStorageFileDownloadResponse>.FailAsync(e.Message);
            }

        }
    }
}
