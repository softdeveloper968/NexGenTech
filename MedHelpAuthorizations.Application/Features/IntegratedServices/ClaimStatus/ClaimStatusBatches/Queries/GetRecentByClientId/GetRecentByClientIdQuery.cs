using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetCleanupBatches
{
    public class GetRecentClaimStatusBatchesByClientIdQuery : IRequest<Result<List<GetRecentClaimStatusBatchesByClientIdResponse>>>
    {

    }

    public class GetRecentByClientIdQueryHandler : IRequestHandler<GetRecentClaimStatusBatchesByClientIdQuery, Result<List<GetRecentClaimStatusBatchesByClientIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public GetRecentByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetRecentClaimStatusBatchesByClientIdResponse>>> Handle(GetRecentClaimStatusBatchesByClientIdQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchList = await _unitOfWork.Repository<ClaimStatusBatch>()
                .Entities
                .Specify(new ClaimStatusBatchRecentByClientIdSpecification(_currentUserService.ClientId))
                //.Select(x => _mapper.Map<GetRecentClaimStatusBatchesByClientIdResponse>(x))
                .Select(x => new GetRecentClaimStatusBatchesByClientIdResponse()
                {
                    Id = x.Id,
                    AbortedOnUtc = x.AbortedOnUtc,
                    AssignedClientRpaConfigurationId = x.AssignedClientRpaConfigurationId,
                    AssignedDateTimeUtc = x.AssignedDateTimeUtc,
                    AssignedToHostName = x.AssignedToHostName,
                    AssignedToIpAddress = x.AssignedToIpAddress,
                    AssignedToRpaCode = x.AssignedToRpaCode,
                    AssignedToRpaLocalProcessIds = x.AssignedToRpaLocalProcessIds,
                    AuthTypeId = x.AuthTypeId ?? 0,
                    AuthTypeName = x.AuthType != null ? x.AuthType.Name : string.Empty,
                    //BatchAssignmentCount = x.b
                    BatchNumber = x.BatchNumber,
                    ClientCode = x.Client.ClientCode,
                    ClientId = x.ClientId,
                    ClientInsuranceLookupName = x.ClientInsurance != null ? x.ClientInsurance.Name : string.Empty,
                    ClientLocationId = x.InputDocument != null ? x.InputDocument.ClientLocationId : null,
                    CreatedOn = x.CreatedOn,
                    LastModifiedOn = x.LastModifiedOn,
                })
                .ToListAsync();                

            return await Result<List<GetRecentClaimStatusBatchesByClientIdResponse>>.SuccessAsync(claimStatusBatchList);
        }
    }
}
