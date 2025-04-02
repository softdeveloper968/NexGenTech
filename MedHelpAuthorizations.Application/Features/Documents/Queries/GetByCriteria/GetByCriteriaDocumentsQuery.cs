using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Documents.Queries.GetByCriteria
{
    public class GetByCriteriaDocumentsQuery : IRequest<PaginatedResult<GetByCriteriaDocumentsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PatientId { get; set; }

        public int AuthorizationId { get; set; } = 0;

        public GetByCriteriaDocumentsQuery()
        {            
        }
    }

    public class GetByCriteriaDocumentsQueryHandler : IRequestHandler<GetByCriteriaDocumentsQuery, PaginatedResult<GetByCriteriaDocumentsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        private readonly IMapper _mapper;

        public GetByCriteriaDocumentsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetByCriteriaDocumentsResponse>> Handle(GetByCriteriaDocumentsQuery request, CancellationToken cancellationToken)
        {          
            var docSpec = new DocumentFilterByCriteriaSpecification(request.PatientId, request.AuthorizationId);
            var data = await _unitOfWork.Repository<Document>().Entities
               .Specify(docSpec)
               .ProjectTo<GetByCriteriaDocumentsResponse>(_mapper.ConfigurationProvider)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }
    }
}