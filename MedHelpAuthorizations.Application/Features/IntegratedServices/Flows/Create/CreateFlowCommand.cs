using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Flows.Create
{
    public class CreateFlowCommand : IRequest<Result<int>>
    {
        public string FlowName { get; set; }

        public CreateFlowCommand()
        {

        }
    }

    public class CreateFlowCommandHandler : IRequestHandler<CreateFlowCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private IUnitOfWork<int> _unitOfWork { get; set; }

        public CreateFlowCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateFlowCommand request, CancellationToken cancellationToken)
        {
            var flow = _mapper.Map<Flow>(request);
            await _unitOfWork.Repository<Flow>().AddAsync(flow);
            await _unitOfWork.Commit(cancellationToken); 

            return await Result<int>.SuccessAsync(flow.Id);
        }
    }
}
