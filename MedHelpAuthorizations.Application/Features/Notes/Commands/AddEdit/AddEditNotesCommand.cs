using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Common;

namespace MedHelpAuthorizations.Application.Features.Notes.Commands.AddEdit
{
    public partial class AddEditNotesCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int? AuthorizationId { get; set; }
        public string NoteUserId { get; set; }
        public string NoteContent { get; set; }
    }

    public class AddEditBrandCommandHandler : IRequestHandler<AddEditNotesCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditBrandCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubService _hubService;

        private int _clientId => _currentUserService.ClientId;

        public AddEditBrandCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, 
            IStringLocalizer<AddEditBrandCommandHandler> localizer, 
            ICurrentUserService currentUserService, IHubService hubService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _hubService = hubService;
        }

        public async Task<Result<int>> Handle(AddEditNotesCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var note = _mapper.Map<Note>(command);
                note.ClientId = _clientId;
                await _unitOfWork.Repository<Note>().AddAsync(note);
                await _unitOfWork.Commit(cancellationToken);

                var alert = new UserAlert()
                {
                    UserId = note.NoteUserId,
                    AlertType = Domain.Entities.Enums.AlertTypeEnum.NotesAdded,
                    PreviewText = note.NoteContent, 
                    IsViewed = false,
                    IsRemoved = false,
                    ResourceType = "note",
                    ResourceId = $"{note.Id}"
                };

                await _unitOfWork.Repository<UserAlert>().AddAsync(alert);

                await _unitOfWork.Commit(cancellationToken);
                _hubService.SendAlert(note.Id.ToString(), note.NoteUserId, _currentUserService.UserId);
                return await Result<int>.SuccessAsync(note.Id, _localizer["Notes Saved"]);
            }
            else
            {
                var note = await _unitOfWork.Repository<Note>().GetByIdAsync(command.Id);
                if (note != null)
                {
                    _mapper.Map(command, note);
                    await _unitOfWork.Repository<Note>().UpdateAsync(note);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(note.Id, _localizer["Notes Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Notes Not Found!"]);
                }
            }
        }
    }
}