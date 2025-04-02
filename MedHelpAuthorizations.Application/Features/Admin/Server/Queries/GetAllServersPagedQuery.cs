using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.Server.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Server.Queries
{
    public class GetAllServersPagedQuery : IRequest<PaginatedResult<ServerInfoReponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
        public GetAllServersPagedQuery(int pageNumber, int pageSize, string quickSearch)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            QuickSearch = quickSearch;
        }
    }
    public class GetAllServersPagedQueryHandler : IRequestHandler<GetAllServersPagedQuery, PaginatedResult<ServerInfoReponse>>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public GetAllServersPagedQueryHandler(IAdminUnitOfWork adminUnitOfWork, IUserService userService, ICurrentUserService currentUserService)
        {
            _adminUnitOfWork = adminUnitOfWork;
            _userService = userService;
            _currentUserService = currentUserService;
        }
        public async Task<PaginatedResult<ServerInfoReponse>> Handle(GetAllServersPagedQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Domain.IdentityEntities.Server, ServerInfoReponse>> expression = e => new ServerInfoReponse
            {
                ServerId = e.Id,
                ServerName = e.ServerName,
                ServerAddress = e.ServerAddress,
                AuthenticationType = (int)e.AuthenticationType,
                ServerType = (int)e.ServerType,
                Username = e.Username,
                CreatedByName = _userService.GetNameAsync(e.CreatedBy).Result,
                CreatedOn = e.CreatedOn,
                LastModifiedByName = string.IsNullOrEmpty(e.LastModifiedBy) ? "" : _userService.GetNameAsync(e.LastModifiedBy).Result,
                LastModifiedOn = e.LastModifiedOn
            };

            try
            {
                var data = await _adminUnitOfWork.Repository<Domain.IdentityEntities.Server, int>().Entities
                    .Select(expression)
                    .Where(x => string.IsNullOrEmpty(request.QuickSearch) || 
                    (
                        x.ServerName.Contains(request.QuickSearch) ||
                        x.ServerAddress.Contains(request.QuickSearch) ||
                        x.Username.Contains(request.QuickSearch)
                    ))
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            catch (Exception ex)
            {
                return PaginatedResult<ServerInfoReponse>.Failure("Something went wrong.");
            }
        }
    }
}
