using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.CardholderViewModels;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetByCriteriaPaged
{
    public class GetCardholdersByCriteriaQuery : IRequest<PaginatedResult<CardholderViewModel>>
    {
        public int Id { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public string InsuranceGroupNumber { get; set; }
        public long? PhoneNumber { get; set; }
        public int? SocialSecurityNumber { get; set; }
        public DateTime? BirthDate { get; set; }

        public GetCardholdersByCriteriaQuery()
        {

        }
    }

    public class GetCardholdersByCriteriaQueryHandler : IRequestHandler<GetCardholdersByCriteriaQuery, PaginatedResult<CardholderViewModel>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetCardholdersByCriteriaQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<CardholderViewModel>> Handle(GetCardholdersByCriteriaQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Cardholder, CardholderViewModel>> expression = e => _mapper.Map<CardholderViewModel>(e);
            var cardholderCriteriaSpec = new CardholderCriteriaSpecification(request, _clientId);
            var data = await _unitOfWork.Repository<Cardholder>().Entities
               .Specify(cardholderCriteriaSpec)
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            
            return data;
        }
    }
}
