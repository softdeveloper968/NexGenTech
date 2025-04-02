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
    public class DeleteApiKeyCommand : IRequest<Result<int>>
    {
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public int Id { get; set; }
        public DeleteApiKeyCommand(int tenantId, int clientId, int id)
        {
            TenantId = tenantId;
            ClientId = clientId;
            Id = id;
        }
    }

    public class DeleteApiKeyCommandHandler : IRequestHandler<DeleteApiKeyCommand, Result<int>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public DeleteApiKeyCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<int>> Handle(DeleteApiKeyCommand command, CancellationToken cancellationToken)
        {
            var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(command.TenantId);
            var authorization = await unitOfWork.Repository<ClientApiIntegrationKey>().GetByIdAsync(command.Id);
            await unitOfWork.Repository<ClientApiIntegrationKey>().DeleteAsync(authorization);
            await unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(authorization.Id);
        }
    }
}
