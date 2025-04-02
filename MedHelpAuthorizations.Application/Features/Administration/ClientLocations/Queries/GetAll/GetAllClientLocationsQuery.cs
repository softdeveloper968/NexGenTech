using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAll
{
    public class GetAllClientLocationsQuery : IRequest<Result<List<GetAllClientLocationsResponse>>>
    {
        
    }

    public class GetAllClientLocationsQueryHandler : IRequestHandler<GetAllClientLocationsQuery, Result<List<GetAllClientLocationsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;
        IAddressRepository _addressRepository;

        public GetAllClientLocationsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IAddressRepository addressRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _addressRepository = addressRepository;
        }

        public async Task<Result<List<GetAllClientLocationsResponse>>> Handle(GetAllClientLocationsQuery request, CancellationToken cancellationToken)
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
               .ToListAsync();

            return await Result<List<GetAllClientLocationsResponse>>.SuccessAsync(data);
        }
    }
}
