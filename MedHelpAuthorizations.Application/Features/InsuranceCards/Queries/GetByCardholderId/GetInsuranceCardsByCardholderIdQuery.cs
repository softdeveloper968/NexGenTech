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

namespace MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByCardholderId
{
    public class GetInsuranceCardsByCardholderIdQuery : IRequest<Result<List<GetInsuranceCardsByCardholderIdResponse>>>
    {
        public int CardholderId { get; set; }

        public GetInsuranceCardsByCardholderIdQuery(int cardholderId)
        {
            CardholderId = cardholderId;
        }        
    }

    public class GetInsuranceCardByCardholderIdQueryHandler : IRequestHandler<GetInsuranceCardsByCardholderIdQuery, Result<List<GetInsuranceCardsByCardholderIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public GetInsuranceCardByCardholderIdQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<List<GetInsuranceCardsByCardholderIdResponse>>> Handle(GetInsuranceCardsByCardholderIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<InsuranceCard, GetInsuranceCardsByCardholderIdResponse>> expression = e => _mapper.Map<GetInsuranceCardsByCardholderIdResponse>(e);
            var cardholderInsuranceCardFilterSpec = new InsuranceCardsByCardholderIdFilterSpecification(query.CardholderId, _clientId);
            
            var cardholderInsuranceCardsResult = await _unitOfWork.Repository<InsuranceCard>().Entities
                .Specify(cardholderInsuranceCardFilterSpec)
                .Select(expression)
                .ToListAsync(cancellationToken);

            return await Result<List<GetInsuranceCardsByCardholderIdResponse>>.SuccessAsync(cardholderInsuranceCardsResult);
        }
    }
}
