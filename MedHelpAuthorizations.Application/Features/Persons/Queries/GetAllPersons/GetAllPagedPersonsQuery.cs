using MedHelpAuthorizations.Application.Interfaces.Repositories;
using AutoMapper;
using System.Threading;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Specifications;

namespace MedHelpAuthorizations.Application.Features.Persons.Queries.GetAllPersons
{
    public class GetAllPagedPersonsQuery : IRequest<PaginatedResult<PersonDto>>
    {
        public GetAllPagedPersonsQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
    }
    public class GetAllPagedPersonsQueryHandler : IRequestHandler<GetAllPagedPersonsQuery, PaginatedResult<PersonDto>>
    {
        //private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        
        public GetAllPagedPersonsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<PersonDto>> Handle(GetAllPagedPersonsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Person, PersonDto>> expression = e => _mapper.Map<PersonDto>(e);

                var persons = await _unitOfWork.Repository<Person>().Entities
                   .Include(x => x.Address)
                   //TODO: Add searchString query filter Specification 
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
