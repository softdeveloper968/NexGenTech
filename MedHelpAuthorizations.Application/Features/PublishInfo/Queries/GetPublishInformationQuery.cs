using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.PublishInfo.Queries
{
    public class GetPublishInformationQuery : IRequest<Result<ReleaseArtifactInfo>>
    {
    }

    public class GetPublishInformationQueryHandler : IRequestHandler<GetPublishInformationQuery, Result<ReleaseArtifactInfo>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        public readonly IPublishInformationManager  _publishInformationManager;

        public GetPublishInformationQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IPublishInformationManager publishInformationManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publishInformationManager = publishInformationManager;
        }
        public async Task<Result<ReleaseArtifactInfo>> Handle(GetPublishInformationQuery query, CancellationToken cancellationToken)
        {
            var publishInfo = _publishInformationManager.GetPublishInfo();
            if (publishInfo != null) 
            {
                return await Result<ReleaseArtifactInfo>.SuccessAsync(publishInfo);
            }

            return await Result<ReleaseArtifactInfo>.FailAsync("Publish Information Not Found");
        }
    }
}
