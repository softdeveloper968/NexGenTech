using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.CardholderViewModels;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetAllPaged
{
    public class GetAllCardholdersQuery : IRequest<PaginatedResult<CardholderViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public GetAllCardholdersQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllCardholdersQueryHandler : IRequestHandler<GetAllCardholdersQuery, PaginatedResult<CardholderViewModel>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        private int _clientId => _currentUserService.ClientId;

        public GetAllCardholdersQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CardholderViewModel>> Handle(GetAllCardholdersQuery request, CancellationToken cancellationToken)
        {
            //Expression<Func<Cardholder, CardholderViewModels>> expression = e => new CardholderViewModels
            //{
            //    Id = e.Id,
            //    FirstName = e.Person.FirstName,
            //    MiddleName = e.Person.MiddleName,
            //    LastName = e.Person.LastName,
            //    DateOfBirth = e.Person.DateOfBirth,
            //    AddressId = e.Person.AddressId,
            //    Address = e.Person.Address,
            //    Email = e.Person.Email,
            //    GenderIdentityId = e.Person.GenderIdentityId,
            //    //HomePhoneNumber = e.Person.HomePhoneNumber,
            //    //MobilePhoneNumber = e.Person.MobilePhoneNumber
            //    ClientId = _clientId,
            //};
            Expression<Func<Cardholder, CardholderViewModel>> expression = e => _mapper.Map<CardholderViewModel>(e);
            var patientFilterSpec = new CardholderFilterSpecification(request.SearchString, _clientId);
            var data = await _unitOfWork.Repository<Cardholder>().Entities
                .Specify(patientFilterSpec)
               
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }
    }
}
