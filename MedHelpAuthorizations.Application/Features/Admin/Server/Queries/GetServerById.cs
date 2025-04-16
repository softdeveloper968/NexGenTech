using MedHelpAuthorizations.Application.Features.Admin.Server.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Server.Queries
{
    public class GetServerById : IRequest<IResult<ServerInfoReponse>>
    {
        public int ServerId { get; set; }
        public GetServerById(int serverId)
        {
            ServerId = serverId;
        }
    }

    public class GetServerByIdHandler : IRequestHandler<GetServerById, IResult<ServerInfoReponse>>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;

        public GetServerByIdHandler(IAdminUnitOfWork adminUnitOfWork)
        {
            _adminUnitOfWork = adminUnitOfWork;
        }
        public async Task<IResult<ServerInfoReponse>> Handle(GetServerById request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.IdentityEntities.Server, ServerInfoReponse>> expression = e => new ServerInfoReponse
            {
                ServerId = e.Id,
                ServerName = e.ServerName,
                ServerAddress = e.ServerAddress,
                AuthenticationType = (int)e.AuthenticationType,
                ServerType = (int)e.ServerType,
                Username = e.Username
            };

            var data = await _adminUnitOfWork.Repository<Domain.IdentityEntities.Server, int>().Entities
               .Select(expression)
               .FirstOrDefaultAsync(x => x.ServerId == request.ServerId);


            if (data == null)
            {
                return Result<ServerInfoReponse>.Fail("Server details not found.");
            }

            return Result<ServerInfoReponse>.Success(data);
        }
    }
}
