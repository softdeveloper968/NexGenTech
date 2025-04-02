using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.UserAlerts.Commands.AddEdit
{
    public class AddEditUserAlertCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AlertTypeEnum AlertType { get; set; }
        public string ResourceType { get; set; }
        public string ResourceId { get; set; }
        public bool IsViewed { get; set; }
        public bool IsRemoved { get; set; }
        public string TargetUrl { get; set; }
    }

    public class AddEditAlertCommandHandler : IRequestHandler<AddEditUserAlertCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAlertCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditAlertCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditAlertCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditUserAlertCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var alert = _mapper.Map<UserAlert>(command);
                await _unitOfWork.Repository<UserAlert>().AddAsync(alert);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(alert.Id, _localizer["Alert Saved"]);
            }
            else
            {
                var alert = await _unitOfWork.Repository<UserAlert>().GetByIdAsync(command.Id);
                if (alert != null)
                {
                    _mapper.Map(command, alert);
                    await _unitOfWork.Repository<UserAlert>().UpdateAsync(alert);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(alert.Id, _localizer["Alert Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Alert Not Found!"]);
                }
            }
        }
    }
}

