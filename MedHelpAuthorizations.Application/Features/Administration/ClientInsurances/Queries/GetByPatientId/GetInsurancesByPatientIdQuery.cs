using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByPatientId
{
    public class GetInsurancesByPatientIdQuery : IRequest<PaginatedResult<GetAllPagedInsurancesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PatientId { get; set; }
    }

    public class GetInsurancesByPatientIdQueryHandler : IRequestHandler<GetInsurancesByPatientIdQuery, PaginatedResult<GetAllPagedInsurancesResponse>>
    {
        private readonly IClientInsuranceRepository _clientInsuranceRepository;
        private readonly IMapper _mapper;

        public GetInsurancesByPatientIdQueryHandler(IClientInsuranceRepository clientInsuranceRepository, IMapper mapper)
        {
            _clientInsuranceRepository = clientInsuranceRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllPagedInsurancesResponse>> Handle(GetInsurancesByPatientIdQuery query, CancellationToken cancellationToken)
        {
            //return await _clientInsuranceRepository.GetByPatientId(query);
            throw new System.NotImplementedException();
        }
    }
}