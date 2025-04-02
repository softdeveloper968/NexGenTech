using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.CustomDashboard.Queries
{
    /// <summary>
    /// Summary: Represents a query to retrieve custom dashboard items.
    /// </summary>
    public class CustomDashboardItemsQuery : IRequest<Result<List<DashboardItem>>>
    {

    }

    /// <summary>
    /// Summary: Handles the CustomDashboardItemsQuery to fetch custom dashboard items.
    /// </summary>
    public class CustomDashboardItemsQueryHandler : IRequestHandler<CustomDashboardItemsQuery, Result<List<DashboardItem>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<CustomDashboardItemsQueryHandler> _localizer;

        /// <summary>
        /// Summary: Initializes a new instance of the CustomDashboardItemsQueryHandler class.
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="unitOfWork"></param>
        // Parameters:
        //   localizer: The IStringLocalizer for localization.
        //   unitOfWork: The unit of work for database operations.
        public CustomDashboardItemsQueryHandler(IStringLocalizer<CustomDashboardItemsQueryHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Summary: Handles the CustomDashboardItemsQuery to fetch custom dashboard items.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        // Parameters:
        //   request: The CustomDashboardItemsQuery request.
        //   cancellationToken: The cancellation token.
        // Returns: A Result containing a list of DashboardItem entities.
        public async Task<Result<List<DashboardItem>>> Handle(CustomDashboardItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the list of DashboardItem entities from the database.
                var response = await _unitOfWork.Repository<DashboardItem>().Entities.ToListAsync();

                // Return a successful Result with the list of DashboardItem entities.
                return await Result<List<DashboardItem>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
