using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Commands
{
    public class AddEditClientUserNotificationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string FileUrl { get; set; }
        public bool IsDownload { get; set; } = false;
        public string FileStatus { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AddEditClientUserNotificationCommandHandler : IRequestHandler<AddEditClientUserNotificationCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientUserNotificationCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public AddEditClientUserNotificationCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClientUserNotificationCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditClientUserNotificationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.FileStatus =="In-Progress")
                {
                    var notificationData = _mapper.Map<ClientUserNotification>(command);
                    notificationData.ClientId = _clientId;
                    notificationData.CreatedOn = DateTime.UtcNow;
                    notificationData = await _unitOfWork.Repository<ClientUserNotification>().AddAsync(notificationData);
                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(notificationData.Id, _localizer["Notification Is Saved"]);
                }
                else
                {
                    var notificationData = await _unitOfWork.Repository<ClientUserNotification>().Entities
                        .FirstOrDefaultAsync(x => x.FileName == command.FileName);

                    if (notificationData != null)
                    {
                        if (command.IsDownload)
                        {
                            notificationData.IsDownload = true;
                        }
                        else
                        {
                            notificationData.ClientId = _clientId;
                            notificationData.FileStatus = command.FileStatus;
                            notificationData.FileUrl = command.FileUrl;
                            notificationData.IsDownload = command.IsDownload;
                            notificationData.ErrorMessage = command.ErrorMessage;
                        }
                        
                        await _unitOfWork.Repository<ClientUserNotification>().UpdateAsync(notificationData);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(notificationData.Id, _localizer["Notification Is Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Notification Is Not Found!"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[$"Error Saving Notification! {ex.Message}"]);
            }
        }
    }
}
