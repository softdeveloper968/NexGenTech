using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation
{
    public class AddEditClaimStatusChangeStatusCommand : IAddEditClaimStatusWorkstationCommand, IRequest<Result<int>> { }

    public class AddEditClaimStatusChangeStatusCommandHandler : IRequestHandler<AddEditClaimStatusChangeStatusCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        private readonly IClaimStatusTransactionLineItemStatusChangeRepository _claimStatusTransactionLineItemStatusChangeRepository;
        private readonly IClaimLineItemStatusRepository _claimLineItemStatusRepository;

        public AddEditClaimStatusChangeStatusCommandHandler(IUnitOfWork<int> unitOfWork,
                                            IMapper mapper, IClaimStatusTransactionLineItemStatusChangeRepository ClaimStatusTransactionLineItemStatusChangeRepository,
                                            IClaimLineItemStatusRepository ClaimLineItemStatusRepository,
                                            ICurrentUserService currentUserService, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _claimLineItemStatusRepository = ClaimLineItemStatusRepository;
            _claimStatusTransactionLineItemStatusChangeRepository = ClaimStatusTransactionLineItemStatusChangeRepository;
        }

        public async Task<Result<int>> Handle(AddEditClaimStatusChangeStatusCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ClaimStatusTransaction claimStatusTransaction;
                claimStatusTransaction = await _unitOfWork.Repository<ClaimStatusTransaction>().GetByIdAsync(command.Id);


                if (claimStatusTransaction != null)
                {
                    int newStatusRank = 0;
                    var previousStatus = await _claimLineItemStatusRepository.GetByIdAsync((ClaimLineItemStatusEnum)claimStatusTransaction.ClaimLineItemStatusId);
                    int previousRank = previousStatus?.Rank ?? 0;
                    if (command.ClaimLineItemStatusId != null)
                    {
                        var newClaimLineItemStatus = await _claimLineItemStatusRepository.GetByIdAsync((ClaimLineItemStatusEnum)command.ClaimLineItemStatusId);
                        newStatusRank = newClaimLineItemStatus.Rank;

                    }

                    if (newStatusRank < previousRank
                        && claimStatusTransaction.ClaimLineItemStatusId != null)
                    {
                        return await Result<int>.FailAsync("ClaimStatus Rank is less than previous status!");
                    }
                    else
                    {
                        var claimStatusChangeRecord = _claimStatusTransactionLineItemStatusChangeRepository.GetByIdAsync(claimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId ?? 0).Result;

                        //if logged then update.
                        if (claimStatusChangeRecord != null)
                        {
                            claimStatusChangeRecord.PreviousClaimLineItemStatusId = claimStatusTransaction.ClaimLineItemStatusId;
                            claimStatusChangeRecord.UpdatedClaimLineItemStatusId = (ClaimLineItemStatusEnum)command.ClaimLineItemStatusId;

                            if (claimStatusChangeRecord.WriteoffAmount != command.WriteoffAmount)
                            {
                                //Update WriteOff Amount denial reasons.
                                claimStatusChangeRecord.WriteoffAmount = command.WriteoffAmount ?? null;
                            }
                            await _claimStatusTransactionLineItemStatusChangeRepository.UpdateAsync(claimStatusChangeRecord).ConfigureAwait(true);
                        }
                        //else add entry.
                        else
                        {
                            var claimStatusChangeRequest = new ClaimStatusTransactionLineItemStatusChangẹ(_clientId,
                                (ClaimLineItemStatusEnum)claimStatusTransaction.ClaimLineItemStatusId, (ClaimLineItemStatusEnum)command.ClaimLineItemStatusId, DbOperationEnum.Insert);

                            await _claimStatusTransactionLineItemStatusChangeRepository.InsertAsync(claimStatusChangeRequest).ConfigureAwait(true);
                        }
                    }

                    ///Update new Claim Status and Add Manual Edit entry in record.
                    claimStatusTransaction.ClaimLineItemStatusId = command.ClaimLineItemStatusId;
                    claimStatusTransaction.ClaimLineItemStatusValue = command.ClaimLineItemStatusValue;

                    ///Update Exception Reason Category Id, If claim line Item Status Value is Denied.///AA-70
                    if (command.ClaimLineItemStatusId.HasValue && ReadOnlyObjects.ReadOnlyObjects.DeniedClaimLineItemStatuses.Contains<ClaimLineItemStatusEnum>(command.ClaimLineItemStatusId.Value))
                    {
                        claimStatusTransaction.ClaimStatusExceptionReasonCategoryId = command.ExceptionReasonCategoryId;

                    }

                    //Update WriteOff Amount 
                    claimStatusTransaction.WriteoffAmount = command.WriteoffAmount;

                    await _unitOfWork.Repository<ClaimStatusTransaction>().UpdateAsync(claimStatusTransaction);
                    var transactionHistory = _mapper.Map<ClaimStatusTransactionHistory>(claimStatusTransaction);
                    transactionHistory.DbOperationId = DbOperationEnum.Insert;///Added db operationId when adding history.

                    await _unitOfWork.Repository<ClaimStatusTransactionHistory>().AddAsync(transactionHistory);

                    ///Todo Include workstation notes in Add Edit Workstation status transaction.
                    if (command.WorkstationTransactionNote != null)
                    {
                        //await _mediator.Send(command.WorkstationTransactionNotes);
                        var reasonNoteModel = _mapper.Map<ClaimStatusWorkstationNotes>(command.WorkstationTransactionNote);
                        //add workstation notes
                        await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().AddAsync(reasonNoteModel);
                    }

                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(claimStatusTransaction.Id, $"ClaimStatusTransaction for {claimStatusTransaction.Id} Updated");
                }
                else
                {
                    return await Result<int>.FailAsync("ClaimStatusTransaction Not Found!");
                }

            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }

    }
}
