using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Create
{
    public class CreateClaimStatusBatchCommand : IRequest<Result<int>>
    {
        public int ClientId { get; set; }
        public int RpaInsuranceId { get; set; }
        public int AuthTypeId { get; set; }
        public int InputDocumentId { get; set; }
        public ICollection<ClaimStatusBatchClaim> ClaimStatusBatchClaims { get; set; }

        public CreateClaimStatusBatchCommand()
        {

        }       
    }

    public class CreateClaimStatusBatchCommandHandler : IRequestHandler<CreateClaimStatusBatchCommand, Result<int>>
    {
        private readonly ICurrentUserService _currentUserService;

        private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
        private readonly IMapper _mapper; private IUnitOfWork<int> _unitOfWork { get; set; }
        private int _clientId => _currentUserService.ClientId;

        public CreateClaimStatusBatchCommandHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _claimStatusBatchRepository = claimStatusBatchRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateClaimStatusBatchCommand request, CancellationToken cancellationToken)
        {
            ClaimStatusBatchHistory claimStatusBatchHistory;

            var claimStatusBatch = _mapper.Map<ClaimStatusBatch>(request);
            claimStatusBatch.ClientId = _clientId;
            await _claimStatusBatchRepository.InsertAsync(claimStatusBatch);

            claimStatusBatchHistory = _mapper.Map<ClaimStatusBatchHistory>(claimStatusBatch);
            claimStatusBatchHistory.DbOperationId = DbOperationEnum.Insert;
            claimStatusBatch.ClaimStatusBatchHistories.Add(claimStatusBatchHistory);

            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(claimStatusBatch.Id);
        }
    }
}
