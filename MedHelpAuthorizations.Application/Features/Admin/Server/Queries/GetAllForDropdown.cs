using MedHelpAuthorizations.Application.Features.Admin.Server.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Shared.Wrapper;
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
    public class GetAllForDropdown : IRequest<IResult<IEnumerable<BasicServerInfoResponse>>>
    {
    }

    public class GetAllForDropdownHandler: IRequestHandler<GetAllForDropdown, IResult<IEnumerable<BasicServerInfoResponse>>>

    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;

        public GetAllForDropdownHandler(IAdminUnitOfWork adminUnitOfWork)
        {
            _adminUnitOfWork = adminUnitOfWork;
        }

        public async Task<IResult<IEnumerable<BasicServerInfoResponse>>> Handle(GetAllForDropdown request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.IdentityEntities.Server, BasicServerInfoResponse>> expression = e => new BasicServerInfoResponse
            {
                ServerId = e.Id,
                ServerName = e.ServerName
            };

            try
            {
                var data = await _adminUnitOfWork.Repository<Domain.IdentityEntities.Server, int>().Entities
                    .Select(expression)
                    .ToListAsync();

                return Result<IEnumerable<BasicServerInfoResponse>>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<BasicServerInfoResponse>>.Fail("Something went wrong.");
            }
        }
    }
}
