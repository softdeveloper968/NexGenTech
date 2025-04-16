using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ReferringProviders.GetByCriteria
{
    public class GetReferringProvidersByCriteriaQuery : IRequest<PaginatedResult<GetReferringProvidersByCriteriaResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

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
    }

    public class GetReferringProvidersByCriteriaHandler : IRequestHandler<GetReferringProvidersByCriteriaQuery, PaginatedResult<GetReferringProvidersByCriteriaResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        public GetReferringProvidersByCriteriaHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ;
            _mapper = mapper;
        }      

        public Task<PaginatedResult<GetReferringProvidersByCriteriaResponse>> Handle(GetReferringProvidersByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.Repository<ReferringProvider>()
                .Entities
                //.Specify(new ReferringProviderByCriteriaSpecification(request))
                .Select(x => _mapper.Map<GetReferringProvidersByCriteriaResponse>(request))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }
    }
}
