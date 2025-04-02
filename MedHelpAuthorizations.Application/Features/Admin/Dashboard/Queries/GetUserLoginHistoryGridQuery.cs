using MedHelpAuthorizations.Application.Features.Admin.Dashboard.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Dashboard.Queries
{
    public class GetUserLoginHistoryGridQuery : IRequest<PaginatedResult<GetUserLoginHistoryGridResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
    }

    public class GetUserLoginHistoryGridQueryHandler : IRequestHandler<GetUserLoginHistoryGridQuery, PaginatedResult<GetUserLoginHistoryGridResponse>>
    {
        private readonly ITokenService _tokenService;

        public GetUserLoginHistoryGridQueryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<PaginatedResult<GetUserLoginHistoryGridResponse>> Handle(GetUserLoginHistoryGridQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var gridReq = new Shared.Requests.LoginHistoryRequest.UserLoginHistoryRequest()
                {

                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    QuickSearch = request.QuickSearch,
                };

                var res = (await _tokenService.GetUserLoginHistoryGrid(gridReq));

                return res;
            }
            catch (Exception ex)
            {
                return PaginatedResult<GetUserLoginHistoryGridResponse>.Failure("Fail to load data");
            }
        }
    }
}
