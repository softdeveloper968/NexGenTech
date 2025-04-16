using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.UpdateActiveInactiveRpaInsurance.Commands
{
    public class UpdateActiveInactiveRpaInsuranceCommand : IRequest<Result<bool>>
    {
        public int TenantId { get; set; }
        public int RPAInsuranceId { get; set; }
        public bool IsActive { get; set; }
        public UpdateActiveInactiveRpaInsuranceCommand(int tenantId, int rpaInsuranceId, bool isActive)
        {
            this.TenantId = tenantId;
            this.RPAInsuranceId = rpaInsuranceId;
            this.IsActive = isActive;
        }
    }

    public class UpdateActiveInactiveRpaInsuranceCommandHandler : IRequestHandler<UpdateActiveInactiveRpaInsuranceCommand, Result<bool>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public UpdateActiveInactiveRpaInsuranceCommandHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            this._tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<Result<bool>> Handle(UpdateActiveInactiveRpaInsuranceCommand request, CancellationToken cancellationToken)
        {
            var rpaInsuranceRepository = _tenantRepositoryFactory.GetUnitOfWork<int>(request.TenantId).Repository<RpaInsurance>();

            var rpaInsurance = rpaInsuranceRepository.Entities.FirstOrDefault(x => x.Id == request.RPAInsuranceId);

            if (rpaInsurance != null)
            {
                if (request.IsActive)
                {
                    rpaInsurance.InactivatedOn = null;
                    await rpaInsuranceRepository.UpdateAsync(rpaInsurance);
                    await rpaInsuranceRepository.Commit(cancellationToken);
                    return Result<bool>.Success("RPA Insurance Activated Successfully.");
                }
                else
                {
                    rpaInsurance.InactivatedOn = DateTime.UtcNow;
                    await rpaInsuranceRepository.UpdateAsync(rpaInsurance);
                    await rpaInsuranceRepository.Commit(cancellationToken);
                    return Result<bool>.Success("RPA Insurance Inactivated Successfully.");
                }
            }

            return Result<bool>.Fail("No such record found.");
        }
    }
}
