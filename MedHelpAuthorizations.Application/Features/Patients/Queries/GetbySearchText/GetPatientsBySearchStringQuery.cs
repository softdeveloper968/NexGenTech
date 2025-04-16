using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetBySearchString;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Patients.Queries.GetBySearchString
{
    public class GetPatientsBySearchStringQuery : IRequest<Result<List<GetPatientsBySearchStringResponse>>>
    {
        public string SearchString { get; set; }

        public GetPatientsBySearchStringQuery(string searchString)
        {
            SearchString = searchString;
        }
    }

    public class GetPatientsSearchStringQueryHandler : IRequestHandler<GetPatientsBySearchStringQuery, Result<List<GetPatientsBySearchStringResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetPatientsSearchStringQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetPatientsBySearchStringResponse>>> Handle(GetPatientsBySearchStringQuery request, CancellationToken cancellationToken)
        {
            var PatientsSearchResult = await _unitOfWork.Repository<Patient>().Entities
                .Specify(new PatientSearchFilterSpecification(request.SearchString, _clientId))
                .ToListAsync(cancellationToken);

            var mappedPatients = _mapper.Map<List<GetPatientsBySearchStringResponse>>(PatientsSearchResult);

            return await Result<List<GetPatientsBySearchStringResponse>>.SuccessAsync(mappedPatients);
        }
    }
}
