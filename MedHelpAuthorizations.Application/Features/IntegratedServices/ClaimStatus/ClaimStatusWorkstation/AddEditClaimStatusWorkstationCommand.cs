using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation
{
    public partial class AddEditClaimStatusWorkstationCommand : BaseClaimStatusTransactionCommand
    {
        public AddEditClaimStatusWorkstationNotesCommand WorkstationTransactionNote { get; set; }
        public decimal? WriteoffAmount { get; set; }
    }
    public class AddEditClaimStatusWorkstationCommandHandler : IRequestHandler<AddEditClaimStatusWorkstationCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        private readonly IClaimStatusTransactionLineItemStatusChangeRepository _claimStatusTransactionLineItemStatusChangeRepository;
        private readonly IClaimLineItemStatusRepository _claimLineItemStatusRepository;

        public AddEditClaimStatusWorkstationCommandHandler(IUnitOfWork<int> unitOfWork,
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

        public async Task<Result<int>> Handle(AddEditClaimStatusWorkstationCommand command, CancellationToken cancellationToken)
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
                            await _claimStatusTransactionLineItemStatusChangeRepository.UpdateAsync(claimStatusChangeRecord).ConfigureAwait(true);
                        }
                        //else add entry.
                        else
                        {
                            var claimStatusChangeRequest = new ClaimStatusTransactionLineItemStatusChangẹ(_clientId,
                                (ClaimLineItemStatusEnum)claimStatusTransaction.ClaimLineItemStatusId, (ClaimLineItemStatusEnum)command.ClaimLineItemStatusId, DbOperationEnum.Insert);

                            await _claimStatusTransactionLineItemStatusChangeRepository.InsertAsync(claimStatusChangeRequest).ConfigureAwait(true);
                            claimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId = claimStatusChangeRequest.Id;
                        }
                    }

                    claimStatusTransaction.CheckDate = command.CheckDate;
                    claimStatusTransaction.LineItemPaidAmount = command.LineItemPaidAmount;//Paid Number
                    claimStatusTransaction.CheckNumber = command.CheckNumber;
                    claimStatusTransaction.CopayAmount = command.CopayAmount;
                    claimStatusTransaction.CobAmount = command.CobAmount;
                    claimStatusTransaction.CoinsuranceAmount = command.CoinsuranceAmount;
                    claimStatusTransaction.ClaimLineItemStatusId = command.ClaimLineItemStatusId;
                    claimStatusTransaction.ClaimLineItemStatusValue = command.ClaimLineItemStatusValue;

                    await _unitOfWork.Repository<ClaimStatusTransaction>().UpdateAsync(claimStatusTransaction);

                    ///Todo Include workstation notes in Add Edit Workstation status transaction.
                    if (command.WorkstationTransactionNote != null)
                    {
                        //await _mediator.Send(command.WorkstationTransactionNotes);
                        var reasonNoteModel = _mapper.Map<ClaimStatusWorkstationNotes>(command.WorkstationTransactionNote);
                        //add workstation notes
                        await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().AddAsync(reasonNoteModel);
                    }

                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(claimStatusTransaction.Id, "ClaimStatusTransaction Updated");
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
