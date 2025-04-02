using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAll
{
    public class GetAllDocumentTypeQuery : IRequest<Result<List<GetAllDocumentTypeResponse>>>
    {

    }

    public class GetAllDocumentTypeHandler : IRequestHandler<GetAllDocumentTypeQuery, Result<List<GetAllDocumentTypeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private int _clientId;

        public GetAllDocumentTypeHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork  = unitOfWork ;
            _mapper = mapper;
            _clientId = currentUserService.ClientId;
        }

        public async Task<Result<List<GetAllDocumentTypeResponse>>> Handle(GetAllDocumentTypeQuery request, CancellationToken cancellationToken)
        {
            var types = await _unitOfWork .Repository<DocumentType>().Entities
                .Where(x => x.Clients.Any(c => c.Id == _clientId))
                .Select(x => _mapper.Map<GetAllDocumentTypeResponse>(x))
                .ToListAsync();

            return await Result<List<GetAllDocumentTypeResponse>>.SuccessAsync(types);
        }
    }
}
