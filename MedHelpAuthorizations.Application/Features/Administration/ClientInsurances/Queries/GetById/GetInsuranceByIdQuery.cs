using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetById;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetById
{
    public class GetInsuranceByIdQuery : IRequest<Result<GetInsuranceByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetInsuranceByIdQueryHandler : IRequestHandler<GetInsuranceByIdQuery, Result<GetInsuranceByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetInsuranceByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetInsuranceByIdResponse>> Handle(GetInsuranceByIdQuery query, CancellationToken cancellationToken)
        {
            var authorization = await _unitOfWork.Repository<ClientInsurance>().GetByIdAsync(query.Id);
            var mappedInsurance = _mapper.Map<GetInsuranceByIdResponse>(authorization);
            return await Result<GetInsuranceByIdResponse>.SuccessAsync(mappedInsurance);
        }
    }
}