using AutoMapper;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.CreateAddress;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.CustomDashboard.Commands
{
    // Summary: Represents a command to save user dashboard items.
    public class SaveUserDashboardItemsCommand : IRequest<Result<int>>
    {
        public List<UserDashboardItem> UserDashboardItems { get; set; }
    }

    // Summary: Handles the SaveUserDashboardItemsCommand to save user dashboard items.
    public class SaveUserDashboardItemsCommandHandler : IRequestHandler<SaveUserDashboardItemsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SaveUserDashboardItemsCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private string _userId => _currentUserService.UserId;

        // Summary: Initializes a new instance of the SaveUserDashboardItemsCommandHandler class.
        // Parameters:
        //   unitOfWork: The unit of work for database operations.
        //   mapper: The AutoMapper for mapping objects.
        //   currentUserService: The service providing information about the current user.
        //   localizer: The IStringLocalizer for localization.
        public SaveUserDashboardItemsCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<SaveUserDashboardItemsCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        // Summary: Handles the SaveUserDashboardItemsCommand to save user dashboard items.
        // Parameters:
        //   request: The SaveUserDashboardItemsCommand request.
        //   cancellationToken: The cancellation token.
        // Returns: A Result indicating the success or failure of the save operation.
        public async Task<Result<int>> Handle(SaveUserDashboardItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the existing user dashboard items from the database
                var existingItems = _unitOfWork.Repository<UserDashboardItem>()
                    .Entities
                    .Where(i => i.UserId == _userId)
                    .ToList();

                // Update the existing user dashboard items based on the request
                // For each item in the existing items, check if there is a corresponding updated item in the request.
                // If an updated item exists, apply its properties to the existing item.
                // If no updated item exists, deactivate the existing item.
                _unitOfWork.Repository<UserDashboardItem>().ExecuteUpdate(i => i.UserId == _userId,
                    item =>
                    {
                        var updatedItem = request.UserDashboardItems.FirstOrDefault(x => x.DashboardItemId == item.DashboardItemId);

                        if (updatedItem != null)
                        {
                            // Update properties of the existing item
                            item.IsActive = updatedItem.IsActive;
                            item.Order = updatedItem.Order;
                        }
                        else
                        {
                            // If the item is not in the request, deactivate it
                            item.IsActive = false;
                        }
                    });

                // Identify and add new user dashboard items that were not present in the existing items
                var newItems = request.UserDashboardItems
                    .Where(x => !existingItems.Any(e => e.DashboardItemId == x.DashboardItemId))
                    .ToList()
                    .Select(item => new UserDashboardItem()
                    {
                        UserId = item.UserId,
                        DashboardItemId = item.DashboardItemId,
                        IsActive = true,
                        Order = item.Order
                    });

                // If there are new items, add them to the database
                if (newItems.Any())
                {
                    _unitOfWork.Repository<UserDashboardItem>().AddRange(newItems);
                }

                // Commit the changes to the database and return a successful Result with a message indicating success
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(1, _localizer["Dashboard Configuration Saved"]);
            }
            catch (Exception ex)
            {
                // Return a failed Result with an error message
                //return await Result<int>.FailAsync(_localizer["Failed saving configuration!"]);
                throw;
            }
        }
    }
}
