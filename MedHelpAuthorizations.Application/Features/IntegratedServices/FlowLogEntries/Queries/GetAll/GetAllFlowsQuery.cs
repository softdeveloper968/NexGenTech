using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.FlowLogEntries.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.FlowLogEntries.Queries.GetAll
{
    public class GetAllFlowLogEntriesQuery : IRequest<Result<List<GetAllFlowLogEntryQueryResponse>>>
    {
        public GetAllFlowLogEntriesQuery()
        {
        }
    }

    public class GetAllFlowLogEntriesQueryHandler : IRequestHandler<GetAllFlowLogEntriesQuery, Result<List<GetAllFlowLogEntryQueryResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllFlowLogEntriesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllFlowLogEntryQueryResponse>>> Handle(GetAllFlowLogEntriesQuery request, CancellationToken cancellationToken)
        {
            var flowLogEntryList = await _unitOfWork.Repository<FlowLogEntry>().GetAllAsync();
            var mappedFlows = _mapper.Map<List<GetAllFlowLogEntryQueryResponse>>(flowLogEntryList);

            return await Result<List<GetAllFlowLogEntryQueryResponse>>.SuccessAsync(mappedFlows);
        }
    }
}
