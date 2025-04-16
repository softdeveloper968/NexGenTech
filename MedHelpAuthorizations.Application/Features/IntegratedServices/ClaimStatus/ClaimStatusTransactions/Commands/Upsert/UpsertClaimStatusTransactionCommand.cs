using System.Threading;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Base;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert
{
	using MedHelpAuthorizations.Application.Interfaces.Services;

	public class UpsertClaimStatusTransactionCommand : BaseClaimStatusTransactionCommand
	{
		public bool ForceExceptionReason { get; set; } = false;
	}

	public class UpsertClaimStatusTransactionCommandHandler : IRequestHandler<UpsertClaimStatusTransactionCommand, Result<int>>
	{
		private readonly IClaimStatusTransactionService _claimStatusTransactionService;

		public UpsertClaimStatusTransactionCommandHandler(IClaimStatusTransactionService claimStatusTransactionService)
		{
			_claimStatusTransactionService = claimStatusTransactionService;			
		}

		public async Task<Result<int>> Handle(UpsertClaimStatusTransactionCommand request, CancellationToken cancellationToken)
		{			
			string responseMessage = string.Empty;
			int transactionId;

			try
			{
				transactionId = await _claimStatusTransactionService.UpsertClaimStatusTransaction(request);
				responseMessage = $"Upserted Transaction - Request TransactionId: {request.Id}, transactionId: {transactionId}, ClaimStatusBatchClaimId = {request.ClaimStatusBatchClaimId}";
								
			}
			catch (Exception ex)
			{
				return await Result<int>.FailAsync(ex.InnerException != null ? ex.InnerException.Message : ex.Message + Environment.NewLine + ex.InnerException?.Message + Environment.NewLine + responseMessage);
			}

			return await Result<int>.SuccessAsync(transactionId, responseMessage);
		}
	}
}
