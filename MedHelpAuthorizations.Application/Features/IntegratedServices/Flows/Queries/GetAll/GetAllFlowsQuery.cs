using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Flows.Queries.GetAll
{
    public class GetAllFlowsQuery : IRequest<Result<List<GetAllFlowsQueryResponse>>>
    {
        public GetAllFlowsQuery()
        {
        }
    }

    public class GetAllFlowsQueryHandler : IRequestHandler<GetAllFlowsQuery, Result<List<GetAllFlowsQueryResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllFlowsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllFlowsQueryResponse>>> Handle(GetAllFlowsQuery request, CancellationToken cancellationToken)
        {
            var flowList = await _unitOfWork.Repository<Flow>().GetAllAsync();
            var mappedFlows = _mapper.Map<List<GetAllFlowsQueryResponse>>(flowList);

            return Result<List<GetAllFlowsQueryResponse>>.Success(mappedFlows);
        }
    }
}
