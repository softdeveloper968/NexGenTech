using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications;
using System.Linq;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByPatientId
{
    public class GetAuthorizationByPatientIdQuery : IRequest<PaginatedResult<GetAllPagedAuthorizationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PatientId { get; set; }
    }

    public class GetAuthorizationByPatientIdQueryHandler : IRequestHandler<GetAuthorizationByPatientIdQuery, PaginatedResult<GetAllPagedAuthorizationsResponse>>
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;

        public GetAuthorizationByPatientIdQueryHandler(IAuthorizationRepository authorizationRepository, IMapper mapper)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> Handle(GetAuthorizationByPatientIdQuery query, CancellationToken cancellationToken)
        {            
            return await _authorizationRepository.GetByPatientIdWithProperName(query);
        }
    }
}