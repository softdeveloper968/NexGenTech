using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAll;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAllPaged
{
    public class GetAllPagedDocumentTypeQuery : IRequest<PaginatedResult<GetAllDocumentTypeResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllPagedDocumentTypeHandler : IRequestHandler<GetAllPagedDocumentTypeQuery, PaginatedResult<GetAllDocumentTypeResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private int _clientId;

        public GetAllPagedDocumentTypeHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork  = unitOfWork ;
            _mapper = mapper;
            _clientId = currentUserService.ClientId;
        }

        public async Task<PaginatedResult<GetAllDocumentTypeResponse>> Handle(GetAllPagedDocumentTypeQuery request, CancellationToken cancellationToken)
        {
            var types = await _unitOfWork .Repository<DocumentType>().Entities
                .Where(x => x.Clients.Any(c => c.Id == _clientId))
                .Select(x => _mapper.Map<GetAllDocumentTypeResponse>(x))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return types;
        }
    }
}
