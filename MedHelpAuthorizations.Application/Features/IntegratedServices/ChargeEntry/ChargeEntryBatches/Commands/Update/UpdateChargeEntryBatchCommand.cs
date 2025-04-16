using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Update
{
    public class UpdateChargeEntryBatchCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }       
        public bool IsStarted { get; set; } = false;
        public string ProcessStartedByHostIpAddress { get; set; }
        public string ProcessStartedByRpaCode { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsAborted { get; set; } = false;
        public string AbortedReason { get; set; } 
        public bool IsDeleted { get; set; } = false;
        public bool IsMaxConsecutiveIssueResolved { get; set; } = true;

        public class UpdateChargeEntryBatchCommandHandler : IRequestHandler<UpdateChargeEntryBatchCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IChargeEntryBatchRepository _chargeEntryBatchRepository;
            private readonly IRpaTypeRepository _rpaTypeRepository;
            private readonly IMapper _mapper;

            public UpdateChargeEntryBatchCommandHandler(IChargeEntryBatchRepository chargeEntryBatchRepository, IUnitOfWork<int> unitOfWork, IRpaTypeRepository rpaTypeRepository, IMapper mapper)
            {
                _chargeEntryBatchRepository = chargeEntryBatchRepository;
                _unitOfWork = unitOfWork;
                _rpaTypeRepository = rpaTypeRepository;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdateChargeEntryBatchCommand command, CancellationToken cancellationToken)
            {
                ChargeEntryBatchHistory chargeEntryBatchHistory;
                var chargeEntryBatch = await _chargeEntryBatchRepository.GetByIdAsync(command.Id);

                if (chargeEntryBatch == null)
                {
                    return Result<int>.Fail($"ChargeEntryBatch Not Found.");
                }
                else
                {
                    if (command.IsDeleted)
                    {
                        chargeEntryBatch.IsDeleted = true;
                    }
                    else if (command.IsStarted)
                    {
                        if (chargeEntryBatch.ProcessStartDateTimeUtc != null)
                        {
                            return await Result<int>.FailAsync("Cannot complete the batch start request because the batch has already been marked as 'Started'.");
                        }
                        if (chargeEntryBatch.IsDeleted)
                        {
                            return await Result<int>.FailAsync($"ChargeEntryBatchId: { chargeEntryBatch.Id } has been marked as deleted and should not be processed.");
                        }
                        chargeEntryBatch.CompletedDateTimeUtc = null;
                        chargeEntryBatch.AbortedOnUtc = null;
                        chargeEntryBatch.AbortedReason = null;
                        chargeEntryBatch.ProcessStartDateTimeUtc = DateTime.UtcNow;
                        chargeEntryBatch.ProcessStartedByHostIpAddress = command.ProcessStartedByHostIpAddress;
                        chargeEntryBatch.ProcessStartedByRpaCode = command.ProcessStartedByRpaCode;
                    }
                    else if (command.IsCompleted)
                    {
                        if (chargeEntryBatch.CompletedDateTimeUtc != null)
                        {
                            return await Result<int>.FailAsync("Cannot mark the batch as completed because the batch is already marked as completed.");
                        }
                        else
                        {
                            chargeEntryBatch.CompletedDateTimeUtc = DateTime.UtcNow;
                        }
                    }
                    else if (command.IsAborted)
                    {
                        if (chargeEntryBatch.AbortedOnUtc != null)
                        {
                            return await Result<int>.FailAsync("Cannot mark the batch as aborted because the batch is already marked as aborted.");
                        }
                        else
                        {
                            var rpaType = await _rpaTypeRepository.GetByIdAsync(chargeEntryBatch.ChargeEntryRpaConfiguration.RpaTypeId);
                            rpaType.IsMaxConsecutiveIssueResolved = command.IsMaxConsecutiveIssueResolved;
                            
                            chargeEntryBatch.AbortedOnUtc = DateTime.UtcNow;
                            chargeEntryBatch.AbortedReason = command.AbortedReason;
                        }
                    }
                    else if (command.IsStarted)
                    {
                            chargeEntryBatch.ProcessStartDateTimeUtc = DateTime.UtcNow;
                    }

                    await _chargeEntryBatchRepository.UpdateAsync(chargeEntryBatch);

                    chargeEntryBatchHistory = _mapper.Map<ChargeEntryBatchHistory>(chargeEntryBatch);
                    chargeEntryBatchHistory.DbOperationId = DbOperationEnum.Update;
                    chargeEntryBatch.ChargeEntryBatchHistories.Add(chargeEntryBatchHistory);

                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(chargeEntryBatch.Id);
                }
            }
        }
    }
}
