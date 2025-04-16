using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Commands.AddEdit
{
    public partial class AddEditClientApiKeyCommand : ApiKeyViewModel, IRequest<Result<int>>
    { 
    }
    public class AddEditClientApiKeyCommandHandler : IRequestHandler<AddEditClientApiKeyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientApiKeyCommandHandler> _localizer;

        public AddEditClientApiKeyCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClientApiKeyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditClientApiKeyCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {

                var apiKeyData = _mapper.Map<ClientApiIntegrationKey>(command);
                await _unitOfWork.Repository<ClientApiIntegrationKey>().AddAsync(apiKeyData);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(apiKeyData.Id, _localizer["Client ApiKey Saved"]);
            }
            else
            {
                var apiKeyData = await _unitOfWork.Repository<ClientApiIntegrationKey>().GetByIdAsync(command.Id);
                if (apiKeyData != null)
                {
                    _mapper.Map(command, apiKeyData);

                    await _unitOfWork.Repository<ClientApiIntegrationKey>().UpdateAsync(apiKeyData);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(apiKeyData.Id, _localizer["Client ApiKey Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ApiKey Not Found!"]);
                }
            }
        }
    }
}
