using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.ClientApiKey.Commands
{
    public class AddEditAdminClientApiKeyCommand : ApiKeyViewModel, IRequest<Result<int>>
    {
        public int TenantId { get; set; }
    }
    public class AddEditAdminClientApiKeyCommandHandler : IRequestHandler<AddEditAdminClientApiKeyCommand, Result<int>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAdminClientApiKeyCommandHandler> _localizer;

        public AddEditAdminClientApiKeyCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory, IMapper mapper, IStringLocalizer<AddEditAdminClientApiKeyCommandHandler> localizer)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditAdminClientApiKeyCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(command.TenantId);

                if (command.Id == 0)
                {

                    var apiKeyData = _mapper.Map<ClientApiIntegrationKey>(command);
                    await unitOfWork.Repository<ClientApiIntegrationKey>().AddAsync(apiKeyData);
                    await unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(apiKeyData.Id, _localizer["Client ApiKey Saved"]);
                }
                else
                {
                    var apiKeyData = await unitOfWork.Repository<ClientApiIntegrationKey>().GetByIdAsync(command.Id);
                    if (apiKeyData != null)
                    {
                        _mapper.Map(command, apiKeyData);

                        await unitOfWork.Repository<ClientApiIntegrationKey>().UpdateAsync(apiKeyData);
                        await unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(apiKeyData.Id, _localizer["Client ApiKey Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["ApiKey Not Found!"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
