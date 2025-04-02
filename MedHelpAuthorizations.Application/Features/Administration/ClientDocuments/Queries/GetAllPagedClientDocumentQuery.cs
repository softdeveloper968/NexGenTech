using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Queries;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientDocument.Queries
{
    public class GetAllPagedClientDocumentQuery : IRequest<PaginatedResult<GetAllPagedClientDocumentsResponse>> //EN-791
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetAllPagedClientDocumentQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    internal class GetAllPagedClientDocumentQueryHandler : IRequestHandler<GetAllPagedClientDocumentQuery, PaginatedResult<GetAllPagedClientDocumentsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        private int _clientId => _currentUserService.ClientId;

        public GetAllPagedClientDocumentQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllPagedClientDocumentsResponse>> Handle(GetAllPagedClientDocumentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var clientDocuments = await _unitOfWork.Repository<Domain.Entities.ClientDocument>()
                .Entities
                .Where(x => x.ClientId == _clientId)
                .Specify(new ClientDocumentBySearchStringSpecification(request.SearchString,_clientId))
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => _mapper.Map<GetAllPagedClientDocumentsResponse>(x))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                if (clientDocuments.Data?.Any() ?? false)
                {
                    // Fetch all users in a single query to avoid multiple DB calls
                    var userIds = clientDocuments.Data.Select(d => d.CreatedBy).Distinct().ToList();
                    var users = await _userManager.Users
                        .Where(u => userIds.Contains(u.Id))
                        .Select(u => new { u.Id, u.FirstName, u.LastName })
                        .ToDictionaryAsync(u => u.Id); // Convert to Dictionary for O(1) lookups

                    foreach (var data in clientDocuments.Data)
                    {
                        if (users.TryGetValue(data.CreatedBy, out var user))
                        {
                            data.CreatedBy = $"{user.LastName}, {user.FirstName}";
                            data.UserId = user.Id;
                        }
                    }
                }

                return clientDocuments;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
