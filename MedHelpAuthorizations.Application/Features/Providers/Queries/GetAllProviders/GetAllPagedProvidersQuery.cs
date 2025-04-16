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
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders
{
    public class GetAllPagedProvidersQuery : IRequest<PaginatedResult<GetAllProvidersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; } = string.Empty;
        public GetAllPagedProvidersQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllPagedProvidersQueryHandler : IRequestHandler<GetAllPagedProvidersQuery, PaginatedResult<GetAllProvidersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllPagedProvidersQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllProvidersResponse>> Handle(GetAllPagedProvidersQuery request, CancellationToken cancellationToken)
        {
            try
            {

                Expression<Func<ClientProvider, GetAllProvidersResponse>> expression = e => new GetAllProvidersResponse
                {
                    Id = e.Id,
                    FirstName = e.Person.FirstName,
                    MiddleName = e.Person.MiddleName,
                    LastName = e.Person.LastName,
                    PersonId = e.PersonId,
                    ClientId = e.ClientId,
                    Npi = e.Npi,
                    License = e.License,
                    SpecialtyId = e.SpecialtyId,
                    ProviderLevelId = e.ProviderLevelId,
                    Credentials = e.Credentials,
                    FaxNumber = e.Person.FaxNumber,
                    Email = e.Person.Email,
                    OfficePhoneNumber = e.Person.OfficePhoneNumber,
                    Address = e.Person.Address ?? new Address(),
                    DateOfBirth = e.Person.DateOfBirth ?? null,
                    TaxId = e.TaxId,
                    Upin = e.Upin,
                    TaxonomyCode = e.TaxonomyCode,
                    ExternalId = e.ExternalId,
                    Locations = e.ClientProviderLocations.Select(cl => cl.ClientLocation).ToList() ?? new List<ClientLocation>(),
                    ScheduledVisitsPerDayKpi = e.ScheduledVisitsPerDayKpi,
                    DaysToBillKpi = e.DaysToBillKpi,
                    PatientsSeenPerDayKpi = e.PatientsSeenPerDayKpi,
                    NoShowRateKpi = e.NoShowRateKpi,
                };

                var clientLocationCriteriaSpec = new GenericByClientIdSpecification<ClientProvider>(_clientId);

                //var patientFilterSpec = new PatientFilterSpecification(request.SearchString, _clientId);
                var data = await _unitOfWork.Repository<ClientProvider>().Entities
                    .Include(x => x.Person)
                    .ThenInclude(x => x.Address)
                    .Include(x => x.ClientProviderLocations)
                    .ThenInclude(y => y.ClientLocation)
                    .Include(s => s.Specialty)
                    //.Specify(patientFilterSpec)
                    .Specify(new ClientProviderBySearchStringSpecification(request.SearchString, _clientId))
                    .Select(expression)
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }   

}

