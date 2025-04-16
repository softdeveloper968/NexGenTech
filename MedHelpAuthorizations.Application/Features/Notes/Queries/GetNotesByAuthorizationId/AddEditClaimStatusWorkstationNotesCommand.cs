using AutoMapper;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId
{
    public class AddEditClaimStatusWorkstationNotesCommand : GetClaimStatusWorkstationNotesResponse, IRequest<Result<int>>
    {
    }

    public class AddEditClaimStatusWorkstationNotesCommandHandler : IRequestHandler<AddEditClaimStatusWorkstationNotesCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditClaimStatusWorkstationNotesCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubService _hubService;

        private int _clientId => _currentUserService.ClientId;

        public AddEditClaimStatusWorkstationNotesCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper,
            IStringLocalizer<AddEditClaimStatusWorkstationNotesCommandHandler> localizer,
            ICurrentUserService currentUserService, IHubService hubService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _hubService = hubService;
        }

        public async Task<Result<int>> Handle(AddEditClaimStatusWorkstationNotesCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var note = _mapper.Map<ClaimStatusWorkstationNotes>(command);
                note.ClientId = _clientId;
                await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().AddAsync(note);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(note.Id, _localizer["ClaimStatus Workstation Notes Saved"]);
            }
            else
            {
                var note = await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().GetByIdAsync(command.Id);
                if (note != null)
                {
                    _mapper.Map(command, note);
                    note.ClientId = _clientId;
                    await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().UpdateAsync(note);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(note.Id, _localizer["ClaimStatus Workstation Notes Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ClaimStatus Workstation Notes Not Found!"]);
                }
            }
        }
    }
}
