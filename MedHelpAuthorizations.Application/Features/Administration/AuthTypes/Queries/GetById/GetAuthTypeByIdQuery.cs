using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetById
{
    public class GetAuthTypeByIdQuery : IRequest<Result<GetAuthTypeByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetAuthTypeByIdQueryHandler : IRequestHandler<GetAuthTypeByIdQuery, Result<GetAuthTypeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAuthTypeByIdResponse>> Handle(GetAuthTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var authorization = await _unitOfWork.Repository<AuthType>().GetByIdAsync(query.Id);
            var mappedAuthType = _mapper.Map<GetAuthTypeByIdResponse>(authorization);
            return await Result<GetAuthTypeByIdResponse>.SuccessAsync(mappedAuthType);
        }
    }
}