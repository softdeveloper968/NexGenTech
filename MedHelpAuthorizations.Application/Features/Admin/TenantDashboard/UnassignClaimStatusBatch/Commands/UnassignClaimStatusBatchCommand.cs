using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.UnassignClaimBatch.Commands
{
    public class UnassignClaimStatusBatchCommand : IRequest<Result<bool>>
    {
        public int TenantId { get; set; }
        public int Id { get; set; }
        public UnassignClaimStatusBatchCommand(int tenantId, int id)
        {
            TenantId = tenantId;
            Id = id;
        }
    }
    public class UnassignClaimStatusBatchCommandHandler : IRequestHandler<UnassignClaimStatusBatchCommand, Result<bool>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public UnassignClaimStatusBatchCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<bool>> Handle(UnassignClaimStatusBatchCommand request, CancellationToken cancellationToken)
        {
            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);

            var claimStatusBatch = await claimStatusRepo.Entities.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (claimStatusBatch != null)
            {
                claimStatusBatch.AssignedDateTimeUtc = null;
                claimStatusBatch.AssignedToHostName = null;
                claimStatusBatch.AssignedToIpAddress = null;
                claimStatusBatch.AssignedToRpaCode = null;
                claimStatusBatch.AssignedToRpaLocalProcessIds = "";
                claimStatusBatch.CompletedDateTimeUtc = null;
                claimStatusBatch.AbortedOnUtc = null;
                claimStatusBatch.AbortedReason = null;

                await claimStatusRepo.UpdateAsync(claimStatusBatch);
                
                return Result<bool>.Success(true);
            }

            return Result<bool>.Fail();

        }
    }
}

