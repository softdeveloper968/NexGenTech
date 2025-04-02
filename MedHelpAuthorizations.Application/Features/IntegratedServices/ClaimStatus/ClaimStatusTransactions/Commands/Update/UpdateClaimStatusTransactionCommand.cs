using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Base;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Update
{
    public class UpdateClaimStatusTransactionCommand : BaseClaimStatusTransactionCommand
    {
    }
    public class UpdateClaimStatusTransactionCommandHandler : IRequestHandler<UpdateClaimStatusTransactionCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClaimStatusTransactionRepository _claimStatusTransactionRepository;
        private readonly IClaimStatusTransactionHistoryRepository _claimStatusTransactionHstoryRepository;
        private readonly IMapper _mapper;

        public UpdateClaimStatusTransactionCommandHandler(IClaimStatusTransactionRepository claimStatusTransactionRepository, IClaimStatusTransactionHistoryRepository claimStatusTransactionHstoryRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _claimStatusTransactionRepository = claimStatusTransactionRepository;
            _claimStatusTransactionHstoryRepository = claimStatusTransactionHstoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateClaimStatusTransactionCommand command, CancellationToken cancellationToken)
        {
            var claimStatusTransaction = await _claimStatusTransactionRepository.GetByIdAsync(command.Id);

            if (claimStatusTransaction == null)
            {
                return Result<int>.Fail($"ClaimStatusTransaction Not Found.");
            }
            else
            {
                claimStatusTransaction = _mapper.Map(command, claimStatusTransaction);
                await _claimStatusTransactionRepository.UpdateAsync(claimStatusTransaction);
                await _unitOfWork.Commit(cancellationToken);

                // Map the Updated ClaimStatusTransaction to a new ClaimsStatusHistory object and insert
                var claimStatusTransactionHistory = _mapper.Map<ClaimStatusTransactionHistory>(claimStatusTransaction);
                claimStatusTransactionHistory.DbOperationId = DbOperationEnum.Update;
                await _claimStatusTransactionHstoryRepository.InsertAsync(claimStatusTransactionHistory);

                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(claimStatusTransaction.Id);
            }
        }
    }
    //public class UpdateClaimStatusTransactionCommandHandler : IRequestHandler<UpdateClaimStatusTransactionCommand, Result<int>>
    //{
    //    private readonly IUnitOfWork<int> _unitOfWork;
    //    private readonly IClaimStatusTransactionRepository _claimStatusTransactionRepository;
    //    private readonly IClaimStatusTransactionHistoryRepository _claimStatusTransactionHstoryRepository;
    //    private readonly IMapper _mapper;
    //    private readonly IMediator _mediator;

    //    public UpdateClaimStatusTransactionCommandHandler(IClaimStatusTransactionRepository claimStatusTransactionRepository, IClaimStatusTransactionHistoryRepository claimStatusTransactionHistoryRepository, IMapper mapper, IMediator mediator)
    //    {
    //        _claimStatusTransactionRepository = claimStatusTransactionRepository;
    //        _claimStatusTransactionHstoryRepository = claimStatusTransactionHistoryRepository;
    //        _mapper = mapper;
    //        _mediator = mediator;
    //    }

    //    public async Task<Result<int>> Handle(UpdateClaimStatusTransactionCommand command, CancellationToken cancellationToken)
    //    {
    //        UpsertClaimStatusTransactionCommand upsertTransactionCommand = _mapper.Map<UpsertClaimStatusTransactionCommand>(command);

    //        return await _mediator.Send(upsertTransactionCommand);
    //    }
    //}
}
