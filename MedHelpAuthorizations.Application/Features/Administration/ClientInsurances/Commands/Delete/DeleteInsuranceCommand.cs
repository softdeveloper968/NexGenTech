using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Domain;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq.Expressions;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Commands.Delete
{
    public class DeleteInsuranceCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteInsuranceCommandHandler : IRequestHandler<DeleteInsuranceCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteInsuranceCommandHandler> _localizer;

        public DeleteInsuranceCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteInsuranceCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteInsuranceCommand command, CancellationToken cancellationToken)
        {
            var insurance = await _unitOfWork.Repository<ClientInsurance>().GetByIdAsync(command.Id);

            try
            {
                Expression<Func<ClientInsuranceAverageCollectionPercentage, bool>> ciacpExpression = x =>
                   x.ClientInsuranceId == insurance.Id;

                Expression<Func<Domain.Entities.ClientInsuranceFeeSchedule, bool>> cifsExpression = x =>
                       x.ClientInsuranceId == insurance.Id;

                Expression<Func<EmployeeClientInsurance, bool>> eciExpression = x =>
                       x.ClientInsuranceId == insurance.Id;

                Expression<Func<InsuranceCard, bool>> icExpression = x =>
                       x.ClientInsuranceId == insurance.Id;

                Expression<Func<ClientInsuranceRpaConfiguration, bool>> rpacfgExpression = x =>
                       x.ClientInsuranceId == insurance.Id;

                Expression<Func<InputDocument, bool>> idExpression = x =>
                       x.ClientInsuranceId == insurance.Id;

                _unitOfWork.Repository<ClientInsuranceAverageCollectionPercentage>().ExecuteDelete(ciacpExpression);
                _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().ExecuteDelete(cifsExpression);
                _unitOfWork.Repository<EmployeeClientInsurance>().ExecuteDelete(eciExpression);
                _unitOfWork.Repository<InsuranceCard>().ExecuteDelete(icExpression);
                //Update Batch set AssignedRpaConfigurationId to null
                _unitOfWork.Repository<ClaimStatusBatch>().ExecuteUpdate(b => b.ClientInsuranceId == insurance.Id, u => { u.AssignedClientRpaConfigurationId = null; });
                _unitOfWork.Repository<ClientInsuranceRpaConfiguration>().ExecuteDelete(rpacfgExpression);
                _unitOfWork.Repository<InputDocument>().ExecuteDelete(idExpression);
                await _unitOfWork.Commit(cancellationToken);

                await _unitOfWork.Repository<ClientInsurance>().DeleteAsync(insurance);
                await _unitOfWork.Commit(cancellationToken);
            }
            catch(Exception ex)
            {
                return await Result<int>.FailAsync("Cannot delete ClientInsurance do to referenced by other Entities.");
            }
            
            return await Result<int>.SuccessAsync(insurance.Id, _localizer["Insurance Deleted"]);
        }
    }
}