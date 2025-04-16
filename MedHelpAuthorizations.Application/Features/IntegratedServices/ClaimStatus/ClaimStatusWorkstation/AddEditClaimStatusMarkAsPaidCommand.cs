using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation
{
    public class AddEditClaimStatusMarkAsPaidCommand : IAddEditClaimStatusWorkstationCommand, IRequest<Result<int>> { }

    public class AddEditClaimStatusMarkAsPaidCommandHandler : IRequestHandler<AddEditClaimStatusMarkAsPaidCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        private readonly IClaimStatusTransactionLineItemStatusChangeRepository _claimStatusTransactionLineItemStatusChangeRepository;
        private readonly IClaimLineItemStatusRepository _claimLineItemStatusRepository;

        public AddEditClaimStatusMarkAsPaidCommandHandler(IUnitOfWork<int> unitOfWork,
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

        public async Task<Result<int>> Handle(AddEditClaimStatusMarkAsPaidCommand command, CancellationToken cancellationToken)
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
                            claimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId = claimStatusChangeRecord.Id;
                        }
                    }

                    claimStatusTransaction.CheckDate = command.CheckDate;
                    claimStatusTransaction.LineItemPaidAmount = command.LineItemPaidAmount;//Paid Number
                    claimStatusTransaction.CheckNumber = command.CheckNumber;
                    claimStatusTransaction.CopayAmount = command.CopayAmount;
                    claimStatusTransaction.CobAmount = command.CobAmount;
                    claimStatusTransaction.CoinsuranceAmount = command.CoinsuranceAmount;
                    claimStatusTransaction.ClaimLineItemStatusId = command.ClaimLineItemStatusId; //Updated Claim Status As Paid
                    claimStatusTransaction.ClaimLineItemStatusValue = command.ClaimLineItemStatusValue; //Mark as 'Manual Edit'

                    await _unitOfWork.Repository<ClaimStatusTransaction>().UpdateAsync(claimStatusTransaction);
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
