using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.RestoreClaimStatusBatch.Commands
{
    public class RestoreClaimStatusBatchCommand : IRequest<Result<bool>>
    {
        public int TenantId { get; set; }
        public int Id { get; set; }
        public RestoreClaimStatusBatchCommand(int tenantId, int id)
        {
            TenantId = tenantId;
            Id = id;
        }
    }

    public class RestoreClaimStatusBatchCommandHandler : IRequestHandler<RestoreClaimStatusBatchCommand, Result<bool>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public RestoreClaimStatusBatchCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<bool>> Handle(RestoreClaimStatusBatchCommand request, CancellationToken cancellationToken)
        {
            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);

            var claimStatusBatch = await claimStatusRepo.Entities.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (claimStatusBatch != null)
            {
                await claimStatusRepo.RestoreAsync(claimStatusBatch);
                return Result<bool>.Success(true);
            }
            return Result<bool>.Fail("Failed to Restore");
        }
    }
}
