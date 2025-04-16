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

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetRecent
{
    public class GetRecentChargeEntryBatchesByClientIdQuery : IRequest<Result<List<GetRecentChargeEntryBatchesByClientIdResponse>>>
    {

    }

    public class GetRecentByClientIdQueryHandler : IRequestHandler<GetRecentChargeEntryBatchesByClientIdQuery, Result<List<GetRecentChargeEntryBatchesByClientIdResponse>>>
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

        public async Task<Result<List<GetRecentChargeEntryBatchesByClientIdResponse>>> Handle(GetRecentChargeEntryBatchesByClientIdQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchList = await _unitOfWork.Repository<ChargeEntryBatch>()
                .Entities
                .Specify(new ChargeEntryBatchRecentByClientIdSpecification(_currentUserService.ClientId))
                .Select(x => _mapper.Map<GetRecentChargeEntryBatchesByClientIdResponse>(x))
                .ToListAsync();                

            return await Result<List<GetRecentChargeEntryBatchesByClientIdResponse>>.SuccessAsync(claimStatusBatchList);
        }
    }
}
