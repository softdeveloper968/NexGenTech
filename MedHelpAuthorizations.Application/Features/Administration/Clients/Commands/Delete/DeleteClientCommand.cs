using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.Delete
{
    public class DeleteClientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientCommandHandler> _localizer;

        public DeleteClientCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var Client = await _unitOfWork.Repository<Domain.Entities.Client>().GetByIdAsync(command.Id);
                List<EmployeeClient> employeeClients = await _unitOfWork.Repository<EmployeeClient>().GetAllAsync();
                employeeClients = employeeClients.Where(ec => ec.ClientId == Client.Id).ToList();
               
                if (employeeClients.Any())
                {
                    //List<ClientEmployeeDepartment> clientEmployeeDepartments = await _unitOfWork.Repository<ClientEmployeeDepartment>().GetAllAsync();
                    //clientEmployeeDepartments = clientEmployeeDepartments.Where(cd => cd.EmployeeClientId == employeeClients?.FirstOrDefault().Id)?.ToList();

                    employeeClients.ToList().ForEach (async employeeClient =>
                    {
                        await _unitOfWork.Repository<EmployeeClient>().DeleteAsync(employeeClient);
                    });
                    await _unitOfWork.Commit(cancellationToken);

                    //if (clientEmployeeDepartments.Any())
                    //{
                    //    clientEmployeeDepartments.ToList().ForEach(async clientDepartment =>
                    //    {
                    //        await _unitOfWork.Repository<ClientEmployeeDepartment>().DeleteAsync(clientDepartment);
                    //    });
                    //    await _unitOfWork.Commit(cancellationToken);
                    //}
                }
                
                await _unitOfWork.Repository<Domain.Entities.Client>().DeleteAsync(Client);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Client.Id, _localizer["Client Deleted"]);
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }
    }
}