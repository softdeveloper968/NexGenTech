using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.Delete
{
	public class DeleteFeeScheduleEntryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteFeeEnteryCommandHandler : IRequestHandler<DeleteFeeScheduleEntryCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteFeeEnteryCommandHandler> _localizer;

        public DeleteFeeEnteryCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteFeeEnteryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteFeeScheduleEntryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var feeEnter = await _unitOfWork.Repository<ClientFeeScheduleEntry>().Entities.Include(x => x.ClaimStatusBatchClaims).FirstOrDefaultAsync(x => x.Id == command.Id);
                if(feeEnter != null)
                {
                    await _unitOfWork.Repository<ClientFeeScheduleEntry>().DeleteAsync(feeEnter);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(feeEnter.Id, _localizer["ClientFeeScheduleEntry Deleted"]);
                }
                return await Result<int>.FailAsync(_localizer["Error"]);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}
