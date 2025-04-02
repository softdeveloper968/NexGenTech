using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using self_pay_eligibility_api.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteClaimStatusBatch.Commands
{
    public class DeleteClaimStatusBatchCommand : IRequest<Result<int>>
    {
        public int? ClaimLineItemStatusId { get; set; }
        public int? ClientInsuranceId { get; set; }
        public int? RpaInsuranceId { get; set; }
        public string ProcedureCode { get; set; }
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public int TenantId { get; set; }
        public int ClaimStatusBatchId { get; set; }

        public DeleteClaimStatusBatchCommand(int tenantId, int claimStatusBatchId, int? claimLineItemStatusId = null, int? rpaInsuranceId = null,
                                              int? clientInsuranceId = null, string procedureCode = null, DateTime? dateOfServiceFrom = null, DateTime? dateOfServiceTo = null)
        {
            TenantId = tenantId;
            ClaimStatusBatchId = claimStatusBatchId;
            ClaimLineItemStatusId = claimLineItemStatusId;
            RpaInsuranceId = rpaInsuranceId;
            ClientInsuranceId = clientInsuranceId;
            ProcedureCode = procedureCode;
            DateOfServiceFrom = dateOfServiceFrom;
            DateOfServiceTo = dateOfServiceTo;
        }
    }

    internal class DeleteClaimStatusBatchCommandHandler : IRequestHandler<DeleteClaimStatusBatchCommand, Result<int>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public DeleteClaimStatusBatchCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<Result<int>> Handle(DeleteClaimStatusBatchCommand command, CancellationToken cancellationToken)
        {
            try
            {
                string claimDeletionMessage = "Claim Status Batch is Not Set as Deleted";
                var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(command.TenantId);
                var claimStatusBatchClaimsRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchClaimsRepository>(command.TenantId);

                var claimStatusBatch = await claimStatusRepo.Entities
                    .FirstOrDefaultAsync(c => c.Id == command.ClaimStatusBatchId, cancellationToken);

                if (claimStatusBatch == null)
                {
                    return await Result<int>.FailAsync("No matching claim status batch found.");
                }

                bool hasFilters = command.ClientInsuranceId != null || command.RpaInsuranceId != null ||
                                  command.ClaimLineItemStatusId != null || !string.IsNullOrWhiteSpace(command.ProcedureCode) ||
                                  command.DateOfServiceFrom != null || command.DateOfServiceTo != null;

                var allClaimsInBatch = await claimStatusBatchClaimsRepo.Entities
                    .Where(c => c.ClaimStatusBatchId == command.ClaimStatusBatchId)
                    .ToListAsync(cancellationToken);

                List<ClaimStatusBatchClaim> claimsToDelete;
                if (hasFilters)
                {
                    claimsToDelete = await claimStatusBatchClaimsRepo.Entities
                        .Include(x => x.ClaimStatusTransaction)
                        .ThenInclude(t => t.ClaimLineItemStatus)
                        .Include(c => c.ClientInsurance)
                        .ThenInclude(ci => ci.RpaInsurance)
                        .Where(c => c.ClaimStatusBatchId == command.ClaimStatusBatchId &&
                            (command.ClientInsuranceId == null || c.ClientInsurance.Id == command.ClientInsuranceId) &&
                            (command.RpaInsuranceId == null || c.ClientInsurance.RpaInsuranceId == command.RpaInsuranceId) &&
                            (command.ClaimLineItemStatusId == null ||
                             c.ClaimStatusTransaction.ClaimLineItemStatusId == (ClaimLineItemStatusEnum?)command.ClaimLineItemStatusId) &&
                            (string.IsNullOrWhiteSpace(command.ProcedureCode) || c.ProcedureCode == command.ProcedureCode) &&
                            (command.DateOfServiceFrom == null || c.DateOfServiceFrom >= command.DateOfServiceFrom) &&
                            (command.DateOfServiceTo == null || c.DateOfServiceTo <= command.DateOfServiceTo))
                        .ToListAsync(cancellationToken);
                }
                else
                {
                    claimsToDelete = allClaimsInBatch;
                }

                if (claimsToDelete == null || !claimsToDelete.Any())
                {
                    return await Result<int>.FailAsync("No matching claims found to delete.");
                }

                int deletedClaimsCount = 0;
                foreach (var claim in claimsToDelete)
                {
                    if (!claim.IsDeleted)
                    {
                        claim.IsDeleted = true;
                        deletedClaimsCount++;
                    }
                }
                if (deletedClaimsCount > 0)
                {
                    await claimStatusBatchClaimsRepo.Commit(cancellationToken);
                }

                bool allClaimsDeleted = allClaimsInBatch.All(c => c.IsDeleted);

                if (hasFilters && claimsToDelete.Count == allClaimsInBatch.Count && allClaimsDeleted)
                {
                    claimStatusBatch.IsDeleted = true;
                    claimDeletionMessage = $"No. of Claims Deleted {deletedClaimsCount} out of {allClaimsInBatch.Count()} Claims";
                    await claimStatusRepo.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(deletedClaimsCount, claimDeletionMessage);
                }
                else if (!hasFilters && allClaimsDeleted)
                {
                    claimStatusBatch.IsDeleted = true;
                    claimDeletionMessage = $"No. of Claims Deleted {deletedClaimsCount} out of {allClaimsInBatch.Count()} Claims";
                    await claimStatusRepo.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(deletedClaimsCount, claimDeletionMessage);
                }

                return await Result<int>.FailAsync($"{claimDeletionMessage} pending Claims = {allClaimsInBatch.Count() - deletedClaimsCount}");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync($"Could not delete batch because: {ex.Message}");
            }
        }

    }
}
