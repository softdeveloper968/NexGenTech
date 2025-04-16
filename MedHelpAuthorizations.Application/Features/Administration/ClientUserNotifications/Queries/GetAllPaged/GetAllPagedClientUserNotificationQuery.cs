using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetRecent
{
    public class GetRecentClientUserNotificationQuery : IRequest<Result<List<GetAllClientUserNotificationResponse>>>
    {
        public int ClientId { get; set; }
        public int MaxResults { get; set; }
    }

    public class GetRecentClientUserNotificationQueryHandler : IRequestHandler<GetRecentClientUserNotificationQuery, Result<List<GetAllClientUserNotificationResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        private int _clientId => _currentUserService.ClientId;

        public GetRecentClientUserNotificationQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClientUserNotificationResponse>>> Handle(GetRecentClientUserNotificationQuery request, CancellationToken cancellationToken)
        {
            // Assign ClientId from the current user service
            request.ClientId = _clientId;

            //// Define the date range for the last 10 days
            //DateTime last10Days = DateTime.UtcNow.AddDays(-10);

            // Define the mapping expression
            Expression<Func<ClientUserNotification, GetAllClientUserNotificationResponse>> expression = e =>
                _mapper.Map<GetAllClientUserNotificationResponse>(e);

            // Fetch the filtered data
            var data = await _unitOfWork.Repository<ClientUserNotification>().Entities
                .Where(x => x.ClientId == request.ClientId) // Last 10 days filter
                .OrderByDescending(x => x.Id) // Latest first
                .Take(request.MaxResults) // Limit results to max
                .Select(expression)
                .ToListAsync(cancellationToken);

            // Wrap the result in a standardized response
            return await Result<List<GetAllClientUserNotificationResponse>>.SuccessAsync(data);
        }
    }
}
