using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetBySearchString
{
    public class GetClientInsurancesBySearchStringQuery : IRequest<Result<List<GetClientInsurancesBySearchStringResponse>>>
    {
        public string SearchString { get; set; }
        
        public GetClientInsurancesBySearchStringQuery(string searchString)
        {
            SearchString = searchString;
        }
    }

    public class GetClientInsurancesSearchStringQueryHandler : IRequestHandler<GetClientInsurancesBySearchStringQuery, Result<List<GetClientInsurancesBySearchStringResponse>>>
    {
        private readonly IClientInsuranceRepository _clientInsuranceRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetClientInsurancesSearchStringQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetClientInsurancesBySearchStringResponse>>> Handle(GetClientInsurancesBySearchStringQuery request, CancellationToken cancellationToken)
        {
            var insurancesSearchResult = await _unitOfWork.Repository<ClientInsurance>().Entities
                .Specify(new ClientInsuranceBySearchStringSpecification(request, _clientId))
                .ToListAsync(cancellationToken);
            
            var mappedinsurances = _mapper.Map<List<GetClientInsurancesBySearchStringResponse>>(insurancesSearchResult);

            return await Result<List<GetClientInsurancesBySearchStringResponse>>.SuccessAsync(mappedinsurances);
        }
    }
}