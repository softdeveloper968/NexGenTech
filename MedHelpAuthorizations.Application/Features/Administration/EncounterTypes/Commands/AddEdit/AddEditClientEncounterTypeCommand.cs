using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Commands.AddEdit
{
    public class AddEditClientEncounterTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

    }

    public class AddEditClientEncounterTypeCommandHandler : IRequestHandler<AddEditClientEncounterTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditClientEncounterTypeCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public AddEditClientEncounterTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditClientEncounterTypeCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditClientEncounterTypeCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var encounterType = _mapper.Map<ClientEncounterType>(command);
                encounterType.ClientId = _clientId;
                await _unitOfWork.Repository<ClientEncounterType>().AddAsync(encounterType);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(encounterType.Id, _localizer["EncounterType Saved"]);
            }
            else
            {
                var encounterType = await _unitOfWork.Repository<ClientEncounterType>().GetByIdAsync(command.Id);
                if (encounterType != null)
                {
                    _mapper.Map(command, encounterType);

                    await _unitOfWork.Repository<ClientEncounterType>().UpdateAsync(encounterType);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(encounterType.Id, _localizer["EncounterType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["AuthType Not Found!"]);
                }
            }
        }
    }
}
