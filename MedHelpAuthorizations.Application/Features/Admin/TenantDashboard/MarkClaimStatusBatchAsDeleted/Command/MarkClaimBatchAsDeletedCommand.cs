using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.MarkClaimStatusBatchAsDeleted.Command
{
    public class MarkClaimBatchAsDeletedCommand : IRequest<Result<int>>
    {
        public int? ClaimLineItemStatusId { get; set; }
        public int? ClientInsuranceId { get; set; }
        public int? RpaInsuranceId { get; set; }
        public string ProcedureCode { get; set; }
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public int TenantId { get; set; }
        public int ClaimStatusBatchId { get; set; }

        public MarkClaimBatchAsDeletedCommand(int tenantId, int claimStatusBatchId, int? claimLineItemStatusId = null, int? rpaInsuranceId = null,
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

    internal class MarkClaimBatchAsDeletedCommandHandler : IRequestHandler<MarkClaimBatchAsDeletedCommand, Result<int>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public MarkClaimBatchAsDeletedCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory, ICurrentUserService currentUserService, IUnitOfWork<int> unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<Result<int>> Handle(MarkClaimBatchAsDeletedCommand command, CancellationToken cancellationToken)
        {
            try
            {
                string claimDeletionMessge = "Claim Staus Batch is Not Set as Deleted";
                var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(command.TenantId);
                var claimStatusBatches = await claimStatusRepo.Entities
                    .Include(c => c.ClaimStatusBatchClaims)
                    .ThenInclude(cb => cb.ClaimStatusTransaction)
                    .Where(c => c.Id == command.ClaimStatusBatchId &&
                        (command.RpaInsuranceId == null ||
                         c.ClientInsurance != null && c.ClientInsurance.RpaInsuranceId == command.RpaInsuranceId) &&
                        (command.ClientInsuranceId == null ||
                         (c.ClientInsurance != null && c.ClientInsurance.Id == command.ClientInsuranceId)) &&
                        (command.ClaimLineItemStatusId == null ||
                         c.ClaimStatusBatchClaims.Any(cb => cb.ClaimStatusTransaction != null &&
                                                            cb.ClaimStatusTransaction.ClaimLineItemStatusId == (ClaimLineItemStatusEnum?)command.ClaimLineItemStatusId)) &&
                        (string.IsNullOrWhiteSpace(command.ProcedureCode) ||
                         c.ClaimStatusBatchClaims.Any(cb => cb.ProcedureCode == command.ProcedureCode)) &&
                        (command.DateOfServiceFrom == null ||
                         c.ClaimStatusBatchClaims.Any(cb => cb.DateOfServiceFrom != null &&
                                                            cb.DateOfServiceFrom >= command.DateOfServiceFrom)) &&
                        (command.DateOfServiceTo == null ||
                         c.ClaimStatusBatchClaims.Any(cb => cb.DateOfServiceTo != null &&
                                                            cb.DateOfServiceTo <= command.DateOfServiceTo))
                     ).ToListAsync(cancellationToken);


                if (claimStatusBatches == null || !claimStatusBatches.Any())
                {
                    return await Result<int>.FailAsync("No matching claim status batches found.");
                }

                int deletedClaimsCount = 0;

                foreach (var batch in claimStatusBatches)
                {
                    foreach (var claim in batch.ClaimStatusBatchClaims)
                    {
                        if (!claim.IsDeleted)
                        {
                            claim.IsDeleted = true;
                            await claimStatusRepo.Commit(cancellationToken);
                            deletedClaimsCount++;
                        }
                    }
                    if (batch.ClaimStatusBatchClaims.All(c => c.IsDeleted))
                    {
                        batch.IsDeleted = true;
                        await claimStatusRepo.Commit(cancellationToken);
                        claimDeletionMessge = $"Claim StatusBatch Id :{command.ClaimStatusBatchId} is Deleted";
                    }
                }

                var res = claimStatusBatches;

                return await Result<int>.SuccessAsync(deletedClaimsCount, claimDeletionMessge);
            }
            catch (Exception ex)
            {

                return await Result<int>.FailAsync($"Could not Delete batch because : {ex.Message}");
            }

        }

    }
}
