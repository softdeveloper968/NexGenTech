using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Application.Features.Patients.Queries.GetAllPaged
{
    public class GetAllPatientsQuery : IRequest<PaginatedResult<GetAllPagedPatientsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public GetAllPatientsQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, PaginatedResult<GetAllPagedPatientsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllPatientsQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllPagedPatientsResponse>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Patient, GetAllPagedPatientsResponse>> expression = e => new GetAllPagedPatientsResponse
            {
                Id = e.Id,
            };
            var patientFilterSpec = new PatientFilterSpecification(request.SearchString, _clientId);
            var data = await _unitOfWork.Repository<Patient>().Entities
                .Specify(patientFilterSpec)
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }
    }
}