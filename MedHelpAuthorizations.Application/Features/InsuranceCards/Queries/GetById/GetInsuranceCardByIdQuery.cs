using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetById
{
    public class GetInsuranceCardByIdQuery : IRequest<Result<GetInsuranceCardByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetInsuranceCardByIdQueryHandler : IRequestHandler<GetInsuranceCardByIdQuery, Result<GetInsuranceCardByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetInsuranceCardByIdQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService _currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = _currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<GetInsuranceCardByIdResponse>> Handle(GetInsuranceCardByIdQuery query, CancellationToken cancellationToken)
        {
            var insuranceCard = await _unitOfWork.Repository<InsuranceCard>().GetByIdAsync(query.Id);
            var mappedInsuranceCard = _mapper.Map<GetInsuranceCardByIdResponse>(insuranceCard);
            return await Result<GetInsuranceCardByIdResponse>.SuccessAsync(mappedInsuranceCard);
        }
    }
}
