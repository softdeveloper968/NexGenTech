using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByPatientId
{
    public class GetInsuranceCardsByPatientIdQuery : IRequest<Result<List<GetInsuranceCardsByPatientIdResponse>>>
    {
        public int PatientId { get; set; }

        public GetInsuranceCardsByPatientIdQuery(int patientId)
        {
            PatientId = patientId;
        }

        public GetInsuranceCardsByPatientIdQuery()
        {
        }
    }

    public class GetInsuranceCardByPatientIdQueryHandler : IRequestHandler<GetInsuranceCardsByPatientIdQuery, Result<List<GetInsuranceCardsByPatientIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public GetInsuranceCardByPatientIdQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<List<GetInsuranceCardsByPatientIdResponse>>> Handle(GetInsuranceCardsByPatientIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<InsuranceCard, GetInsuranceCardsByPatientIdResponse>> expression = e => _mapper.Map<GetInsuranceCardsByPatientIdResponse>(e);
            var patientInsuranceCardFilterSpec = new InsuranceCardsByPatientIdFilterSpecification(query.PatientId, _clientId);
            
            var patientInsuranceCardsResult = await _unitOfWork.Repository<InsuranceCard>().Entities
                .Specify(patientInsuranceCardFilterSpec)
                .Select(expression)
                .ToListAsync(cancellationToken);

            return await Result<List<GetInsuranceCardsByPatientIdResponse>>.SuccessAsync(patientInsuranceCardsResult);
        }
    }
}
