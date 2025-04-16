using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Logs.Create
{
    public class CreateFlowLogEntryCommand : IRequest<Result<int>>
    {
        public int ClientId { get; set; }
        public int FlowId { get; set; }
        public string StepName { get; set; } = string.Empty;
        public bool IsSuccessFul { get; set; } = false;
        public string Message { get; set; } = string.Empty;

        public CreateFlowLogEntryCommand()
        {

        }
    }

    public class CreateFlowLogEntryCommandHandler : IRequestHandler<CreateFlowLogEntryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private IUnitOfWork<int> _unitOfWork { get; set; }

        public CreateFlowLogEntryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateFlowLogEntryCommand request, CancellationToken cancellationToken)
        {
            var dbFlowLogEntry = _mapper.Map<FlowLogEntry>(request);
            await _unitOfWork.Repository<FlowLogEntry>().AddAsync(dbFlowLogEntry);
            await _unitOfWork.Commit(cancellationToken); 

            return await Result<int>.SuccessAsync(dbFlowLogEntry.Id);
        }
    }
}
