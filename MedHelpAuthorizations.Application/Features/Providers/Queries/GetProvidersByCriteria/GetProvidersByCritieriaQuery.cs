using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Providers.GetByCriteria
{
    public class GetProvidersByCriteriaQuery : IRequest<PaginatedResult<GetProvidersByCriteriaResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string AddressStreetLine1 { get; set; }
        public DateTime? DateOfBirth { get; set; } = null;
        public SpecialtyEnum? SpecialtyId { get; set; }
        public string Npi { get; set; }
        public string TaxId { get; set; }
        public string Upin { get; set; }
        public string ExternalId { get; set; }
        public int ClientId { get; set; } 
    }

    public class GetProvidersByCriteriaHandler : IRequestHandler<GetProvidersByCriteriaQuery, PaginatedResult<GetProvidersByCriteriaResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetProvidersByCriteriaHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService    )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;

        }

        public async Task<PaginatedResult<GetProvidersByCriteriaResponse>> Handle(GetProvidersByCriteriaQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientProvider, GetProvidersByCriteriaResponse>> expression = e => _mapper.Map<GetProvidersByCriteriaResponse>(e);

            var data = await _unitOfWork.Repository<ClientProvider>()
                .Entities
                .Specify(new ProviderByCriteriaSpecification(request))
                .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            //var mappedProvider = _mapper.Map<GetProvidersByCriteriaResponse>(data);

            return data;
        }
    }
}
