using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Commands.AddEdit
{
    public partial class AddEditAuthTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }

    public class AddEditAuthTypeCommandHandler : IRequestHandler<AddEditAuthTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditAuthTypeCommandHandler> _localizer;

        public AddEditAuthTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditAuthTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditAuthTypeCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var patient = _mapper.Map<AuthType>(command);
                await _unitOfWork.Repository<AuthType>().AddAsync(patient);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(patient.Id, _localizer["AuthType Saved"]);
            }
            else
            {
                var authz = await _unitOfWork.Repository<AuthType>().GetByIdAsync(command.Id);
                if (authz != null)
                {
                    _mapper.Map(command, authz);

                    await _unitOfWork.Repository<AuthType>().UpdateAsync(authz);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(authz.Id, _localizer["AuthType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["AuthType Not Found!"]);
                }
            }
        }
    }
}