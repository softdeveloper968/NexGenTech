using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Client.Commands
{
    public class DeleteClientCommand : IRequest<Result<int>>
    {
        public int TenantId { get; set; }
        public int ClientId { get; }

        public DeleteClientCommand(int tenantId, int clientId)
        {
            TenantId = tenantId;
            ClientId = clientId;
        }
    }
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Result<int>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public DeleteClientCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<int>> Handle(DeleteClientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(command.TenantId);

                var Client = await unitOfWork.Repository<Domain.Entities.Client>().GetByIdAsync(command.ClientId);
                List<EmployeeClient> employeeClients = await unitOfWork.Repository<EmployeeClient>().GetAllAsync();
                employeeClients = employeeClients.Where(ec => ec.ClientId == Client.Id).ToList();

                if (employeeClients.Any())
                {
                    employeeClients.ToList().ForEach(async employeeClient =>
                    {
                        await unitOfWork.Repository<EmployeeClient>().DeleteAsync(employeeClient);
                    });
                    await unitOfWork.Commit(cancellationToken);
                }

                await unitOfWork.Repository<Domain.Entities.Client>().DeleteAsync(Client);
                await unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Client.Id, "Client Deleted");
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
