using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById
{
    public class GetAuthorizationByIdQuery : IRequest<Result<GetAuthorizationByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetAuthorizationByIdQueryHandler : IRequestHandler<GetAuthorizationByIdQuery, Result<GetAuthorizationByIdResponse>>
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;

        public GetAuthorizationByIdQueryHandler(IAuthorizationRepository authorizationRepository, IMapper mapper)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
        }

        public async Task<Result<GetAuthorizationByIdResponse>> Handle(GetAuthorizationByIdQuery query, CancellationToken cancellationToken)
        {            
            return await Result<GetAuthorizationByIdResponse>.SuccessAsync(await _authorizationRepository.GetByIdWithProperName(query));
        }
    }
}