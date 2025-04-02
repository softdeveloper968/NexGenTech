using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Upsert
{
    public class UpsertChargeEntryTransactionCommand : BaseChargeEntryTransactionCommand
    {
        public string UiPathUniqueReference { get; set; }
    }

    public class UpsertChargeEntryTransactionCommandHandler : IRequestHandler<UpsertChargeEntryTransactionCommand, Result<int>>
    {
        //private readonly IChargeEntryTransactionRepository _chargeEntryTransactionRepository;
        //private readonly IChargeEntryTransactionHistoryRepository _chargeEntryTransactionHistoryRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork<int> _unitOfWork { get; set; }
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public UpsertChargeEntryTransactionCommandHandler(
            IChargeEntryTransactionRepository chargeEntryTransactionRepository, 
            IChargeEntryTransactionHistoryRepository chargeEntryTransactionHstoryRepository, 
            IUnitOfWork<int> unitOfWork, IMapper mapper, 
            ICurrentUserService currentUserService)
        {
            //_chargeEntryTransactionRepository = chargeEntryTransactionRepository;
            //_chargeEntryTransactionHistoryRepository = chargeEntryTransactionHstoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpsertChargeEntryTransactionCommand request, CancellationToken cancellationToken)
        {
            ChargeEntryTransaction chargeEntryTransaction = null;
            ChargeEntryTransactionHistory chargeEntryTransactionHistory;
            string responseMessage = string.Empty;

            try
            {
                if (request.Id > 0)                
                    chargeEntryTransaction = await _unitOfWork.Repository<ChargeEntryTransaction>().GetByIdAsync(request.Id);

                if (chargeEntryTransaction != null)
                {
                    chargeEntryTransaction = _mapper.Map<ChargeEntryTransaction>(request);
                    chargeEntryTransactionHistory = _mapper.Map<ChargeEntryTransactionHistory>(chargeEntryTransaction);
                    chargeEntryTransactionHistory.DbOperationId = DbOperationEnum.Update;
                    chargeEntryTransactionHistory.ChargeEntryTransactionId = chargeEntryTransaction.Id;

                    await _unitOfWork.Repository<ChargeEntryTransactionHistory>().AddAsync(chargeEntryTransactionHistory).ConfigureAwait(true);
                    await _unitOfWork.Repository<ChargeEntryTransaction>().UpdateAsync(chargeEntryTransaction).ConfigureAwait(true);

                    await _unitOfWork.Commit(cancellationToken);
                    responseMessage = $"Updated Transaction - Request TransactionId: {request.Id}, Found TransactionId: {chargeEntryTransaction.Id}, ChargeEntryBatchId = {request.ChargeEntryBatchId}";
                }
                else
                {
                    chargeEntryTransaction = _mapper.Map<ChargeEntryTransaction>(request);
                    chargeEntryTransaction.ClientId = _clientId;
                    chargeEntryTransactionHistory = _mapper.Map<ChargeEntryTransactionHistory>(chargeEntryTransaction);
                    chargeEntryTransactionHistory.DbOperationId = DbOperationEnum.Insert;
                    chargeEntryTransaction.ChargeEntryTransactionHistories.Add(chargeEntryTransactionHistory);

                    chargeEntryTransaction = await _unitOfWork.Repository<ChargeEntryTransaction>().AddAsync(chargeEntryTransaction);

                    await _unitOfWork.Commit(cancellationToken);
                    responseMessage = $"Inserted Charge Entry Transaction Id = {chargeEntryTransaction.Id} - ChargeEntryBatchId = {request.ChargeEntryBatchId}";
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.InnerException != null ? ex.InnerException.Message : ex.Message + Environment.NewLine + responseMessage);
            }

            return await Result<int>.SuccessAsync(chargeEntryTransaction.Id, responseMessage);
        }
    }
}
