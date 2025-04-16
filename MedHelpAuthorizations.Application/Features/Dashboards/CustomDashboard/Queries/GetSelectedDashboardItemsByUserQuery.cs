using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.CustomDashboard.Queries
{
    /// <summary>
    /// Summary: Represents a query to retrieve selected dashboard items by the current user.
    /// </summary>
    public class GetSelectedDashboardItemsByUserQuery : IRequest<Result<List<UserDashboardItem>>>
    {

    }

    /// <summary>
    /// Summary: Handles the GetSelectedDashboardItemsByUserQuery to fetch selected dashboard items by the current user.
    /// </summary>
    public class GetSelectedDashboardItemsByUserQueryHandler : IRequestHandler<GetSelectedDashboardItemsByUserQuery, Result<List<UserDashboardItem>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<GetSelectedDashboardItemsByUserQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private string _userId => _currentUserService.UserId;

        /// <summary>
        /// Summary: Initializes a new instance of the GetSelectedDashboardItemsByUserQueryHandler class.
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="currentUserService"></param>
        /// <param name="unitOfWork"></param>
        public GetSelectedDashboardItemsByUserQueryHandler(IStringLocalizer<GetSelectedDashboardItemsByUserQueryHandler> localizer, ICurrentUserService currentUserService, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Summary: Handles the GetSelectedDashboardItemsByUserQuery to fetch selected dashboard items by the current user.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<List<UserDashboardItem>>> Handle(GetSelectedDashboardItemsByUserQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the list of selected UserDashboardItem entities for the current user from the database.
            var response = await _unitOfWork.Repository<UserDashboardItem>().Entities
                .Include(i => i.DashboardItem)
                .Where(i => i.UserId == _userId && i.IsActive).ToListAsync();

            // Return a successful Result with the list of selected UserDashboardItem entities.
            return await Result<List<UserDashboardItem>>.SuccessAsync(response);
        }
    }
}
