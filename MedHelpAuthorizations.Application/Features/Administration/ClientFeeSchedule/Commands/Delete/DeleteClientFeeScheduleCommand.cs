using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Extensions;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Delete
{
    public class DeleteClientFeeScheduleCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    public class DeleteClientFeeScheduleCommandHandler : IRequestHandler<DeleteClientFeeScheduleCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientFeeScheduleCommandHandler> _localizer;

        public DeleteClientFeeScheduleCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientFeeScheduleCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientFeeScheduleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.Id != 0)
                {
                    #region update ClaimStatusBatchClaim EN-330
                    var clientFeeScheduleEntries = await _unitOfWork.Repository<ClientFeeScheduleEntry>().Entities.Where(x => x.ClientFeeScheduleId == command.Id).ToListAsync();

                    if (clientFeeScheduleEntries != null && clientFeeScheduleEntries.Any())
                    {
                        // Executes a bulk update operation on the ClaimStatusBatchClaim entities
                        _unitOfWork.Repository<ClaimStatusBatchClaim>().ExecuteUpdate(c => c.ClientFeeScheduleEntryId.HasValue && clientFeeScheduleEntries.Select(x => x.Id).Contains(c.ClientFeeScheduleEntryId.Value),
                               u =>
                               {
                                   u.ClientFeeScheduleEntryId = null;
                               });
                    }
                    #endregion

                    #region Remove the FeeSchedule EN-330
                    Expression<Func<ClientFeeScheduleEntry, bool>> cfeExpression = x =>
                     x.ClientFeeScheduleId == command.Id;

                    Expression<Func<Domain.Entities.ClientInsuranceFeeSchedule, bool>> clfsExpression = x =>
                      x.ClientFeeScheduleId == command.Id;

                    Expression<Func<ClientFeeScheduleProviderLevel, bool>> cfplExpression = x =>
                      x.ClientFeeScheduleId == command.Id;

                    Expression<Func<ClientFeeScheduleSpecialty, bool>> cfsExpression = x =>
                      x.ClientFeeScheduleId == command.Id;


                    _unitOfWork.Repository<ClientFeeScheduleEntry>().ExecuteDelete(cfeExpression);

                    _unitOfWork.Repository<Domain.Entities.ClientInsuranceFeeSchedule>().ExecuteDelete(clfsExpression);

                    _unitOfWork.Repository<ClientFeeScheduleProviderLevel>().ExecuteDelete(cfplExpression);

                    _unitOfWork.Repository<ClientFeeScheduleSpecialty>().ExecuteDelete(cfsExpression);

                    var clientFeeSchedule = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().GetByIdAsync(command.Id);
                    await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().DeleteAsync(clientFeeSchedule);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(clientFeeSchedule.Id, _localizer["Client Fee Schedule Deleted"]);
                    #endregion
                }
                return await Result<int>.FailAsync(["Error"]);

            }
            catch (Exception ex)
            {

                return await Result<int>.FailAsync(["Error"]);
            }
           
        }
    }
}
