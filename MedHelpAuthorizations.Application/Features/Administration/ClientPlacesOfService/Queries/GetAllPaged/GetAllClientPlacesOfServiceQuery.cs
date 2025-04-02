using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientPlacesOfService.Queries.GetAllPaged
{
    public class GetAllClientPlacesOfServiceQuery : IRequest<PaginatedResult<GetAllClientPlacesOfServiceResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllClientPlacesOfServiceQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllClientPlacesOfServiceQueryHandler : IRequestHandler<GetAllClientPlacesOfServiceQuery, PaginatedResult<GetAllClientPlacesOfServiceResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public GetAllClientPlacesOfServiceQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllClientPlacesOfServiceResponse>> Handle(GetAllClientPlacesOfServiceQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.ClientPlaceOfService, GetAllClientPlacesOfServiceResponse>> expression = e => _mapper.Map<GetAllClientPlacesOfServiceResponse>(e);
            var clientPlaceOfServiceCriteriaSpec = new ClientPlaceOfServiceByClientIdSpecification(_clientId);

            var data = await _unitOfWork.Repository<Domain.Entities.ClientPlaceOfService>().Entities
               .Specify(clientPlaceOfServiceCriteriaSpec)
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}
