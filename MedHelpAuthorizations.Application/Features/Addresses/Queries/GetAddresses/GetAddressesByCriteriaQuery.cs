using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Addresses.ViewModels;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;

namespace MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAddresses
{
    public class GetAddressesByCriteriaQuery : IRequest<PaginatedResult<GetAddressesViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    public class GetAddressesByCriteriaQueryHandler : IRequestHandler<GetAddressesByCriteriaQuery, PaginatedResult<GetAddressesViewModel>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        
        public GetAddressesByCriteriaQueryHandler(IAddressRepository addressRepository, IMapper mapper, IUnitOfWork<int> unitOfWork)
        {
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAddressesViewModel>> Handle(GetAddressesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Address, GetAddressesViewModel>> expression = e => _mapper.Map<GetAddressesViewModel>(e);
            var addressCriteriaSpec = new AddressByCriteriaSpecification(request, _clientId);

            var data = await _unitOfWork.Repository<Address>().Entities
               .Specify(addressCriteriaSpec)
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}