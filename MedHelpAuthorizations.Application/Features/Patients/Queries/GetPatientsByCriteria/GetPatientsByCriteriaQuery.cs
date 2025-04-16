using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria
{
    public class GetPatientsByCriteriaQuery : IRequest<PaginatedResult<GetPatientsByCriteriaResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string AccountNumber { get; set; }
        public string ExternalId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public string InsuranceGroupNumber { get; set; }
        public int? ClientInsuranceId { get; set; }
        public string SocialSecurityNumber { get; set; } //AA-218
        public DateTime? BirthDate { get; set; }
        public bool? IsAddedThisMonth { get; set; }
        public bool? IsActive { get; set; }
        public bool? BenefitsNotChecked { get; set; }

        public GetPatientsByCriteriaQuery()
        {
            //PageNumber = pageNumber;
            //PageSize = pageSize;            
        }
    }

    public class GetPatientsByCriteriaQueryHandler : IRequestHandler<GetPatientsByCriteriaQuery, PaginatedResult<GetPatientsByCriteriaResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetPatientsByCriteriaQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetPatientsByCriteriaResponse>> Handle(GetPatientsByCriteriaQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Patient, GetPatientsByCriteriaResponse>> expression = e => _mapper.Map<GetPatientsByCriteriaResponse>(e);
            //TODO: Add in the missing query Criteria in the specification
            var patientCriteriaSpec = new PatientCriteriaSpecification(request, _clientId);
        
            var data = await _unitOfWork.Repository<Patient>().Entities
               .Specify(patientCriteriaSpec)
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            
            return data;
        }
    }
}