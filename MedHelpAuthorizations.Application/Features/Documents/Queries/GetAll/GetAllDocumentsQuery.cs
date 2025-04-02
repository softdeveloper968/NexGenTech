using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Documents.Queries.GetAll
{
    public class GetAllDocumentsQuery : IRequest<PaginatedResult<GetAllDocumentsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetAllDocumentsQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllDocumentsQueryHandler : IRequestHandler<GetAllDocumentsQuery, PaginatedResult<GetAllDocumentsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        private readonly ICurrentUserService _currentUserService;

        public GetAllDocumentsQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllDocumentsResponse>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Document, GetAllDocumentsResponse>> expression = e => new GetAllDocumentsResponse
                {
                    Id = e.Id,
                    Title = e.Title,
                    CreatedBy = e.CreatedBy,
                    IsPublic = e.IsPublic,
                    CreatedOn = e.CreatedOn,
                    Description = e.Description,
                    DocumentTypeId = e.DocumentType.Id,
                    DocumentTypeName = e.DocumentType.Name,
                    URL = e.URL
                };
                var docSpec = new DocumentFilterSpecification(request.SearchString, _currentUserService.UserId);
                var data = await _unitOfWork.Repository<Document>()?.Entities
                   ?.Specify(docSpec)
                   ?.Select(expression)
                   ?.ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return data;
            }
            catch (Exception ex)
            {
                throw;
                //return (PaginatedResult<GetAllDocumentsResponse>)await Result.FailAsync(
                //    $"Getting Documents failed" + ex.InnerException != null
                //        ? ex.InnerException.Message
                //        : ex.Message);
            }            
        }
    }
}