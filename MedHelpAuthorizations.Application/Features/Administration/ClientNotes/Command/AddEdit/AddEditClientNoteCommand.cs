using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Command.AddEdit
{
    public class AddEditClientNoteCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsOnlyView { get; set; }
    }
    public class AddEditClientNoteCommandHandler : IRequestHandler<AddEditClientNoteCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditClientNoteCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private string _userId => _currentUserService.UserId;
        private int _clientId => _currentUserService.ClientId;
        public AddEditClientNoteCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditClientNoteCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(AddEditClientNoteCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var clientNote = _mapper.Map<ClientNote>(command);
                clientNote.ClientId = _clientId;
                await _unitOfWork.Repository<ClientNote>().AddAsync(clientNote);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(clientNote.Id, _localizer["ClientNote Saved"]);
            }
            else
            {
                var encounterType = await _unitOfWork.Repository<ClientNote>().GetByIdAsync(command.Id);
                if (encounterType != null)
                {
                    _mapper.Map(command, encounterType);
                    encounterType.CreatedBy = _userId;
                    await _unitOfWork.Repository<ClientNote>().UpdateAsync(encounterType);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(encounterType.Id, _localizer["ClientNote Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ClientNote Not Found!"]);
                }
            }
        }
    }
}
