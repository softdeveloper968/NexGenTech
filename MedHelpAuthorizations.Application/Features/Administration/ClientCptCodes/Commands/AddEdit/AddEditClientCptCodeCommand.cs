using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Commands.AddEdit
{
    public partial class AddEditClientCptCodeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int? CptCodeGroupId { get; set; }
        public int ClientId { get; set; }
        public decimal? ScheduledFee { get; set; } = 0.00m;
        
        [Required]
        public string LookupName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Code { get; set; }
        public string CodeVersion { get; set; }
        public TypeOfServiceEnum? TypeOfServiceId { get; set; }

    }

    public class AddEditClientCptCodeCommandHandler : IRequestHandler<AddEditClientCptCodeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditClientCptCodeCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditClientCptCodeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, ICurrentUserService currentUserService, IStringLocalizer<AddEditClientCptCodeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditClientCptCodeCommand command, CancellationToken cancellationToken)
        {
            command.ClientId = _clientId;

            if (command.Id == 0)
            {
                var cptCode = _mapper.Map<ClientCptCode>(command);
                await _unitOfWork.Repository<ClientCptCode>().AddAsync(cptCode);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(cptCode.Id, _localizer["ClientCptCode Saved"]);
            }
            else
            {
                var cptCode = await _unitOfWork.Repository<ClientCptCode>().GetByIdAsync(command.Id);
                if (cptCode != null)
                {
                    _mapper.Map(command, cptCode);

                    await _unitOfWork.Repository<ClientCptCode>().UpdateAsync(cptCode);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(cptCode.Id, _localizer["ClientCptCode Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ClientCptCode Not Found!"]);
                }
            }
        }
    }
}