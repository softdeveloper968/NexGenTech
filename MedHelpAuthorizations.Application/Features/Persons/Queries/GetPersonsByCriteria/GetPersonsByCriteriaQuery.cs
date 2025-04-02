using MedHelpAuthorizations.Application.Interfaces.Repositories;
using AutoMapper;
using System.Threading;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MedHelpAuthorizations.Application.Extensions;
using System.Linq;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsByCriteria
{
    public class GetPersonsByCriteriaQuery : IRequest<PaginatedResult<PersonDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AddressStreetLine1 { get; set; }
        public string City { get; set; }
        public StateEnum? StateId  { get; set; }
        public string PostalCode { get; set; }
        public long? PhoneNumber { get; set; }
        public string Email { get; set; }        
    }

    public class GetPersonsByCriteriaQueryHandler : IRequestHandler<GetPersonsByCriteriaQuery, PaginatedResult<PersonDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetPersonsByCriteriaQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<PersonDto>> Handle(GetPersonsByCriteriaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Person, PersonDto>> expression = e => _mapper.Map<PersonDto>(e);

                var persons = await _unitOfWork.Repository<Person>().Entities
                                       .Include(x => x.Address)
                                       .Specify(new PersonByCriteriaSpecification(request, _clientId))
                                       .Specify(new GenericByClientIdSpecification<Person>(_clientId))
                                       .Select(expression)
                                       .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return persons;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }
    }
}
