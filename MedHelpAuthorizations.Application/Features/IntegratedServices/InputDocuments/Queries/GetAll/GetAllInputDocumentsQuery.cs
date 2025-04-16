using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Queries.GetAll
{
    public class GetAllInputDocumentsQuery : IRequest<PaginatedResult<GetAllInputDocumentsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllInputDocumentsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllInputDocumentsQueryHandler : IRequestHandler<GetAllInputDocumentsQuery, PaginatedResult<GetAllInputDocumentsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllInputDocumentsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllInputDocumentsResponse>> Handle(GetAllInputDocumentsQuery query, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<InputDocument>()
                .Entities
                .Specify(new InputDocumentsByClientIdSpecification(_currentUserService.ClientId))
                .OrderByDescending(x => x.Id)
                .Select(x => new GetAllInputDocumentsResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ClientId = x.ClientId,
                    URL = x.URL,
                    InputDocumentTypeId = x.InputDocumentTypeId,
                    DocumentDate = x.DocumentDate,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    IsPublic = x.IsPublic,
                    IsDeleted = x.IsDeleted,
                    ImportStatus = x.ImportStatus,
                    ClaimStatusBatches = x.ClaimStatusBatches,
                    AttemptedImportCount = x.AttemptedImportCount ?? 0,
                    ActualImportCount = x.ActualImportCount ?? 0,
                    ErrorMessage = x.ErrorMessage,
                    ImportDocumentErrorMessagesCount = x.ImportDocumentMessages.Count(m => m.MessageType == InputDocumentMessageTypeEnum.Errored),
                    ImportDocumentUnMatchedLocationAndProviderMessagesCount = x.ImportDocumentMessages.Count(m => m.MessageType == InputDocumentMessageTypeEnum.UnmatchedLocation || m.MessageType == InputDocumentMessageTypeEnum.UnmatchedProvider),
                    ImportDocumentRepeatMessagesCount = x.ImportDocumentMessages.Count(m => m.MessageType == InputDocumentMessageTypeEnum.FileDuplicates || m.MessageType == InputDocumentMessageTypeEnum.UnSupplantableDuplicates)
                })
                .ToPaginatedListAsync(query.PageNumber, query.PageSize);

            return data;
        }
    }
}
