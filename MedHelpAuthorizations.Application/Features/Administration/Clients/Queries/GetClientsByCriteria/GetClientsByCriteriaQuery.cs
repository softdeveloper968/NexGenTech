using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using AutoMapper;

namespace MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetClientsByCriteria
{
    public class GetClientsByCriteriaQuery : IRequest<PaginatedResult<GetClientsByCriteriaResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public string ClientCode { get; set; }

        public GetClientsByCriteriaQuery()
        {
            //PageNumber = pageNumber;
            //PageSize = pageSize;            
        }
    }

    public class GetClientsByCriteriaQueryHandler : IRequestHandler<GetClientsByCriteriaQuery, PaginatedResult<GetClientsByCriteriaResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientsByCriteriaQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetClientsByCriteriaResponse>> Handle(GetClientsByCriteriaQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.Client, GetClientsByCriteriaResponse>> expression = e => _mapper.Map<GetClientsByCriteriaResponse>(e);
            var ClientCriteriaSpec = new ClientCriteriaSpecification(request);
            var data = await _unitOfWork.Repository<Domain.Entities.Client>().Entities
               .Specify(ClientCriteriaSpec)
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }
    }
}