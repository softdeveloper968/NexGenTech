using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Documents.Queries.GetById
{
    public class GetByIdDocumentsQuery : IRequest<Result<GetByIdDocumentsResponse>>
    {
        public int Id { get; set; }       

        public GetByIdDocumentsQuery(int id)
        {
            Id = id;
        }
    }

    public class GetByIdDocumentsQueryHandler : IRequestHandler<GetByIdDocumentsQuery, Result<GetByIdDocumentsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetByIdDocumentsQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<GetByIdDocumentsResponse>> Handle(GetByIdDocumentsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Document, GetByIdDocumentsResponse>> expression = e => new GetByIdDocumentsResponse
            {
                Id = e.Id,
                Title = e.Title,
                CreatedBy = e.CreatedBy,
                DocumentDate = e.DocumentDate,
                IsPublic = e.IsPublic,
                CreatedOn = e.CreatedOn,
                Description = e.Description,
                DocumentTypeId = e.DocumentType.Id,
                DocumentTypeName = e.DocumentType.Name,
                URL = e.URL
            };

            var data = await _unitOfWork.Repository<Document>()
                .Entities
                .Select(expression)
                .FirstOrDefaultAsync(x => x.Id == request.Id);               
            
            return await Result<GetByIdDocumentsResponse>.SuccessAsync(data);
        }
    }
}