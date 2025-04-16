using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Domain.IdentityEntities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Server.Commands
{
    public class AddEditServerCommand : IRequest<IResult<int>>
    {
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public int ServerType { get; set; }
        public int AuthenticationType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class AddEditServerCommandHandler : IRequestHandler<AddEditServerCommand, IResult<int>>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;

        public AddEditServerCommandHandler(IAdminUnitOfWork adminUnitOfWork)
        {
            _adminUnitOfWork = adminUnitOfWork;
        }

        public async Task<IResult<int>> Handle(AddEditServerCommand request, CancellationToken cancellationToken)
        {
            var serverRepo = _adminUnitOfWork.Repository<Domain.IdentityEntities.Server, int>();
            if (request.ServerId == 0)
            {
                var response = await serverRepo.AddAsync(new Domain.IdentityEntities.Server
                {
                    ServerName = request.ServerName,
                    ServerAddress = request.ServerAddress,
                    ServerType = (ServerType)request.ServerType,
                    AuthenticationType = (AuthenticationType)request.AuthenticationType,
                    Username = request.AuthenticationType == (int)AuthenticationType.Credentials ? request.Username : "",
                    Password = request.AuthenticationType == (int)AuthenticationType.Credentials ? request.Password : "",
                });
                return Result<int>.Success(response.Id);
            }
            else
            {
                var server = await serverRepo.GetByIdAsync(request.ServerId);

                if (server == null)
                {
                    return Result<int>.Fail("Server not found.");
                }

                server.ServerName = request.ServerName;
                server.ServerAddress = request.ServerAddress;
                server.ServerType = (ServerType)request.ServerType;
                server.AuthenticationType = (AuthenticationType)request.AuthenticationType;
                server.Username = request.AuthenticationType == (int)AuthenticationType.Credentials ? request.Username : "";
                server.Password = request.AuthenticationType == (int)AuthenticationType.Credentials ? request.Password : "";

                await serverRepo.UpdateAsync(server);

                await _adminUnitOfWork.Commit(cancellationToken);

                return Result<int>.Success(server.Id);
            }
        }
    }
}
