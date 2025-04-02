using MedHelpAuthorizations.Application.Features.Admin.Dashboard.Models;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetTenantAssociatedUserLoginHistory : IRequest<PaginatedResult<GetTenantUsersUserLoginHistoryGridResponse>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
        public GetTenantAssociatedUserLoginHistory(int tenantId, int pageNumber, int pageSize, string quickSearch)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            QuickSearch = quickSearch;
        }
    }

    public class GetTenantAssociatedUserLoginHistoryHandler : IRequestHandler<GetTenantAssociatedUserLoginHistory, PaginatedResult<GetTenantUsersUserLoginHistoryGridResponse>>
    {
        private readonly ITokenService _tokenService;

        public GetTenantAssociatedUserLoginHistoryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<PaginatedResult<GetTenantUsersUserLoginHistoryGridResponse>> Handle(GetTenantAssociatedUserLoginHistory request, CancellationToken cancellationToken)
        {
            var res = await _tokenService.GetUserLoginHistoryGridForTenantAsync(new Shared.Requests.LoginHistoryRequest.TenantUsersLoginHistoryRequest()
            {
                TenantId = request.TenantId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                QuickSearch = request.QuickSearch,
            });

            return res;
        }
    }
}
