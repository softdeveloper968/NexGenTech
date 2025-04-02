using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetById
{
    public class GetClientUserNotificationByIdQuey : IRequest<Result<GetAllClientUserNotificationResponse>>
    {
        public string fileName { get; set; }
    }

    public class GetClientUserNotificationByIdQueyHandler : IRequestHandler<GetClientUserNotificationByIdQuey, Result<GetAllClientUserNotificationResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        private int _clientId => _currentUserService.ClientId;

        public GetClientUserNotificationByIdQueyHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetAllClientUserNotificationResponse>> Handle(GetClientUserNotificationByIdQuey query, CancellationToken cancellationToken)
        {
            var clientnotification = await _unitOfWork.Repository<ClientUserNotification>().Entities.FirstOrDefaultAsync(x => x.FileName == query.fileName && x.ClientId == _clientId );
            var mappedclientnotification = _mapper.Map<GetAllClientUserNotificationResponse>(clientnotification);
            return await Result<GetAllClientUserNotificationResponse>.SuccessAsync(mappedclientnotification);
        }
    }
}
