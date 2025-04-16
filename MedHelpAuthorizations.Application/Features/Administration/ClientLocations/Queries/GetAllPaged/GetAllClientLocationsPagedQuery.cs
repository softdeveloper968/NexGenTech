using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged
{
    public class GetAllClientLocationsPagedQuery : IRequest<PaginatedResult<GetAllClientLocationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetAllClientLocationsPagedQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllClientLocationsPagedQueryHandler : IRequestHandler<GetAllClientLocationsPagedQuery, PaginatedResult<GetAllClientLocationsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;
        IAddressRepository _addressRepository;

        public GetAllClientLocationsPagedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IAddressRepository addressRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _addressRepository = addressRepository;
        }

        public async Task<PaginatedResult<GetAllClientLocationsResponse>> Handle(GetAllClientLocationsPagedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientLocation, GetAllClientLocationsResponse>> expression = e => new GetAllClientLocationsResponse
            {
                Id = e.Id,
                Name = e.Name,
                Address = e.Address,
                AddressId = e.AddressId,
                ClientId = e.ClientId,
                OfficeFaxNumber = e.OfficeFaxNumber,
                OfficePhoneNumber = e.OfficePhoneNumber,
                Npi = e.Npi,
                EligibilityLocationId = e.EligibilityLocationId,
            };
            var clientLocationCriteriaSpec = new ClientLocationsByClientIdSpecification(_clientId);

            var data = await _unitOfWork.Repository<ClientLocation>().Entities
                .Include(l => l.Address)
               .Specify(clientLocationCriteriaSpec)
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}
