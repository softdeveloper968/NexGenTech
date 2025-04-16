using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById
{
    public class GetClientCptCodeByIdQuery : IRequest<Result<GetClientCptCodeByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetClientCptCodeByIdQueryHandler : IRequestHandler<GetClientCptCodeByIdQuery, Result<GetClientCptCodeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientCptCodeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetClientCptCodeByIdResponse>> Handle(GetClientCptCodeByIdQuery query, CancellationToken cancellationToken)
        {
            var clientCptCode = await _unitOfWork.Repository<ClientCptCode>().GetByIdAsync(query.Id);
            var mappedClientCptCode = _mapper.Map<GetClientCptCodeByIdResponse>(clientCptCode);
            return await Result<GetClientCptCodeByIdResponse>.SuccessAsync(mappedClientCptCode);
        }
    }
}