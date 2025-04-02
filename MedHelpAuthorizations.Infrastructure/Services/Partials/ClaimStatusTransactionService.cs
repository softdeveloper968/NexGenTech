using AutoMapper;
using Azure;
using Finbuckle.MultiTenant;
using Hangfire.Common;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Domain.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ClaimStatusTransactionService : IClaimStatusTransactionService
    {
        private readonly ApplicationContext _context;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;
        private IUnitOfWork<int> _unitOfWork;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IStringLocalizer<ClaimStatusQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITenantInfo _tenantInfo;
        private IClaimStatusTransactionRepository _claimStatusTransactionRepository;
        private IClaimStatusTransactionHistoryRepository _claimStatusTransactionHistoryRepository;
        private IClaimStatusExceptionReasonCategoryMapRepository _claimStatusExceptionReasonCategoryMapRepository;
        private IClaimStatusTransactionLineItemStatusChangeRepository _claimStatusTransactionLineItemStatusChangeRepository;
        private IClaimLineItemStatusRepository _claimLineItemStatusRepository;
        private bool _forceUpdateExceptionReason = false;
        private bool _toggleMemberNotFoundAndRetryMemberNotFound = false;
        protected int _clientId;

        public ClaimStatusTransactionService(
            IUnitOfWork<int> unitOfWork,
            IMapper mapper,
            IStringLocalizer<ClaimStatusQueryService> localizer,
            ICurrentUserService currentUserService,
            IConfiguration configuration,
            ITenantInfo tenantInfo,
            ITenantRepositoryFactory tenantRepositoryFactory,
			IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository,
			IClaimStatusTransactionRepository claimStatusTransactionRepository,
            IClaimStatusTransactionHistoryRepository claimStatusTransactionHistoryRepository,
            IClaimStatusExceptionReasonCategoryMapRepository claimStatusExceptionReasonCategoryMapRepository,
            IClaimStatusTransactionLineItemStatusChangeRepository claimStatusTransactionLineItemStatusChangeRepository,
            IClaimLineItemStatusRepository claimLineItemStatusRepository)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _tenantInfo = tenantInfo;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
			_claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
			_claimStatusTransactionRepository = claimStatusTransactionRepository;
            _claimStatusTransactionHistoryRepository = claimStatusTransactionHistoryRepository;
            _claimStatusExceptionReasonCategoryMapRepository = claimStatusExceptionReasonCategoryMapRepository;
            _claimLineItemStatusRepository = claimLineItemStatusRepository;
            _claimStatusTransactionLineItemStatusChangeRepository = claimStatusTransactionLineItemStatusChangeRepository;
			_clientId = _currentUserService.ClientId;
		}

		public async Task<int> UpsertClaimStatusTransaction(UpsertClaimStatusTransactionCommand request, string tenantIdentifier = null)
		{
			ClaimStatusBatchClaim claimStatusBatchClaim = new ClaimStatusBatchClaim();
			ClaimStatusTransactionHistory claimStatusTransactionHistory;
			ClaimStatusExceptionReasonCategoryMap claimStatusExceptionReasonCategoryMap;
			var cancellationToken = new CancellationToken();
			string responseMessage = string.Empty;

			if (!string.IsNullOrWhiteSpace(tenantIdentifier))
			{
				_claimStatusTransactionRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionRepository>(tenantIdentifier);
				_claimStatusTransactionHistoryRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionHistoryRepository>(tenantIdentifier);
				_claimStatusExceptionReasonCategoryMapRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusExceptionReasonCategoryMapRepository>(tenantIdentifier);
				_unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
				_claimStatusTransactionLineItemStatusChangeRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusTransactionLineItemStatusChangeRepository>(tenantIdentifier);
				_claimLineItemStatusRepository = await _tenantRepositoryFactory.GetAsync<IClaimLineItemStatusRepository>(tenantIdentifier);
			}
			//Only lookup a Exception Category For "Denial Type" Claim line ite m statuses.. 
			if (request.ClaimLineItemStatusId != null && ReadOnlyObjects.DeniedClaimLineItemStatuses.ToList().Contains((ClaimLineItemStatusEnum)request.ClaimLineItemStatusId))
			{
				//If bot overrides
				if (request.ExceptionReasonCategoryId == null)
				{
					claimStatusExceptionReasonCategoryMap = await _claimStatusExceptionReasonCategoryMapRepository.GetByExceptionCategoryReasonAsync(request.ExceptionReason ?? string.Empty)
															?? new ClaimStatusExceptionReasonCategoryMap()
															{
																ClaimStatusExceptionReasonCategoryId = ClaimStatusExceptionReasonCategoryEnum.Other,
																ClaimStatusExceptionReasonText = request.ExceptionReason?.ToUpper().Trim()
															};

					request.ExceptionReasonCategoryId = claimStatusExceptionReasonCategoryMap.ClaimStatusExceptionReasonCategoryId;
				}
			}
			else
			{
				request.ExceptionReasonCategoryId = null;
			}


			claimStatusBatchClaim = _unitOfWork.Repository<ClaimStatusBatchClaim>().Entities
				.Include(x => x.ClientInsurance)
				//.Include(bc => bc.ClaimStatusTransaction)
				//    .ThenInclude(t => t.ClaimStatusTransactionHistories)
				//.Include(bc => bc.ClaimStatusTransaction)
				//    .ThenInclude(t => t.ClaimLineItemStatus)
				.Where(bc => bc.Id == request.ClaimStatusBatchClaimId).FirstOrDefault();
			if (claimStatusBatchClaim == null)
			{
				throw new Exception($"Error: ClaimStatusBatchClaim was not found. Transaction will not be recorded. ID = {request.ClaimStatusBatchClaimId}");
			}

			if (claimStatusBatchClaim.ClientId != _clientId)
			{
				_clientId = (int)claimStatusBatchClaim.ClientId;
			}

			request.ClientId = _clientId;

            // Calculate penalty amount if not provided and ClientInsurance.AutoCalcPenalty is true
            if (!request.PenalityAmount.HasValue && claimStatusBatchClaim.ClientInsurance?.AutoCalcPenalty == true && request.TotalAllowedAmount.HasValue && (request.DeductibleAmount != null || request.CobAmount != null || request.CoinsuranceAmount != null || request.CopayAmount != null ||request.LineItemPaidAmount != null ))
            {
                decimal calculatedPenalty = request.TotalAllowedAmount.Value -
                    ((request.LineItemPaidAmount ?? 0) +
                     (request.CoinsuranceAmount ?? 0) +
                     (request.CopayAmount ?? 0) +
                     (request.CobAmount ?? 0) +
                     (request.DeductibleAmount ?? 0));

                request.PenalityAmount = calculatedPenalty > 0 ? calculatedPenalty : 0;
            }


            if (claimStatusBatchClaim.ClaimStatusTransactionId != null)
			{
				var claimStatusTransaction = await _claimStatusTransactionRepository.GetByIdAsync((int)claimStatusBatchClaim.ClaimStatusTransactionId);
				request.Id = claimStatusTransaction.Id;
				request.ClaimStatusTransactionLineItemStatusChangẹId = claimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId;
				_forceUpdateExceptionReason = request.ForceExceptionReason && (claimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Denied
											&& claimStatusTransaction.ClaimLineItemStatusId == request.ClaimLineItemStatusId
											&& ((claimStatusTransaction.ExceptionReason?.ToUpper()?.Trim() ?? string.Empty)
												!= (request.ExceptionReason?.ToUpper()?.Trim() ?? string.Empty)));

				// Used to be able to update transaction with either MemberNotFound or RetryMemberNotfound when needed and without regard to status Rank. 
				var toggleMemberNotFoundList = new List<ClaimLineItemStatusEnum>() { ClaimLineItemStatusEnum.MemberNotFound, ClaimLineItemStatusEnum.RetryMemberNotFound };
				_toggleMemberNotFoundAndRetryMemberNotFound = toggleMemberNotFoundList.Contains(request.ClaimLineItemStatusId ?? 0)
																&& toggleMemberNotFoundList.Contains(claimStatusTransaction.ClaimLineItemStatusId ?? 0)
																&& request.ClaimLineItemStatusId != claimStatusTransaction.ClaimLineItemStatusId;
				ClaimLineItemStatus previousLineItemStatus = claimStatusTransaction.ClaimLineItemStatus;

				var newLineItemStatus = await _claimLineItemStatusRepository.GetByIdAsync(request.ClaimLineItemStatusId ?? ClaimLineItemStatusEnum.Unknown);
				int newStatusRank = newLineItemStatus?.Rank ?? 0;
				int previousRank = claimStatusTransaction.ClaimLineItemStatus?.Rank ?? 0; //****

				// Need to preserve current transaction date  and not update with new values if request.ClaimLineitemStatusId == ClaimLineItemStatusEnum.Error/null and the current transaction is not Error/null
				// Do create a new  transaction History record though
				// Requested claim line item status Rank is <= then previous status Rank
				// transaction.LastStatusChangeRecord Modified date Controls the Aging between checks. Transaction .modifiedOn controls the length of time it has been in that status
				bool updateChangeRecord = (newStatusRank >= previousRank) || (newStatusRank != previousRank && previousLineItemStatus.Id != ClaimLineItemStatusEnum.Paid);
				bool updateTransactionRecord = (newStatusRank > previousRank) || _forceUpdateExceptionReason || (newStatusRank > 13 && (newStatusRank != previousRank && previousLineItemStatus.Id != ClaimLineItemStatusEnum.Paid)); // && (newLineItemStatus.Rank >= 9));

				//Handle certain scenarios where we need to move claims to new statuses and other scenarios that will never get updated.  
				if (!updateTransactionRecord)
				{
					//The highest ranked status / most accurate status is the one recorded on the transaction
					//If it is still the same and hasn't changed since last time we checked
					//we can check dates and decide to move to different status or whatever else if not progressing. 
					if (claimStatusTransaction.ClaimLineItemStatusId == request.ClaimLineItemStatusId)
					{
						switch (request.ClaimLineItemStatusId)
						{
							case ClaimLineItemStatusEnum.Unavailable:
								//If the claim has been unavailble to review on insurance site for 10 days after it was billed
								// - move to NotOnFile status that will make it show in denial totals.. Unavailable status shows in non-adjudicated reporting
								if (claimStatusBatchClaim.ClaimBilledOn == null || (claimStatusBatchClaim.ClaimBilledOn.Value.AddDays(20) < DateTime.UtcNow.Date))
								{
									request.ClaimLineItemStatusId = ClaimLineItemStatusEnum.NotOnFile;
									request.ExceptionReason = "Claim still unavailable in payers system after 10 days since billed.";
									updateChangeRecord = true; // Force update change record logic
									updateTransactionRecord = true; // Force update Transaction record logic
								}
								break;

							default:
								break;
						}
					}
				}

				// If status Rank is not > than - we want to just make a history record only so we can age the claim in current status for the ClaimLineItemStatus values to do their job. 
				if ((updateChangeRecord || updateTransactionRecord))
				{

					//Check claimStatusChange already logged in db.
					var claimStatusChangeRecord = _claimStatusTransactionLineItemStatusChangeRepository.GetByIdAsync(claimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId ?? 0).Result;

					//if logged and different then update.
					if (updateChangeRecord)
					{
						if (claimStatusChangeRecord != null)
						{

							claimStatusChangeRecord.PreviousClaimLineItemStatusId = claimStatusTransaction.ClaimLineItemStatusId;
							claimStatusChangeRecord.UpdatedClaimLineItemStatusId = (ClaimLineItemStatusEnum)request.ClaimLineItemStatusId;
							claimStatusChangeRecord.LastModifiedOn = DateTime.UtcNow;

							await _claimStatusTransactionLineItemStatusChangeRepository.UpdateAsync(claimStatusChangeRecord).ConfigureAwait(true);
						}
						//else add entry.
						else
						{
							claimStatusChangeRecord = new ClaimStatusTransactionLineItemStatusChangẹ(_clientId, (ClaimLineItemStatusEnum)claimStatusTransaction.ClaimLineItemStatusId, (ClaimLineItemStatusEnum)request.ClaimLineItemStatusId, DbOperationEnum.Insert);

							var statusChangeId = await _claimStatusTransactionLineItemStatusChangeRepository.InsertAsync(claimStatusChangeRecord).ConfigureAwait(true);
							await _unitOfWork.Commit(cancellationToken);
							//responseMessage = $"Inserted claimStatus Transaction LineItem StatusChangẹ - ClaimStatusBatchClaimId = {request.ClaimStatusBatchClaimId}";

							claimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId = claimStatusChangeRecord.Id;
						}
					}

					request.ClaimStatusTransactionLineItemStatusChangẹId = claimStatusChangeRecord.Id;

					//We only want to update transaction if rank is higher. We do still want to record the status change lastModifiedOn date for attempts between attempts. 

					if (updateTransactionRecord)
					{
						// Need a better way. I can;t figure out why the mapping is overwriting thcreated on., Will revist to make more stream,lined
						var createdOn = claimStatusTransaction.CreatedOn;
						claimStatusTransaction = _mapper.Map<ClaimStatusTransaction>(request);
						claimStatusTransaction.CreatedOn = createdOn;
						await _unitOfWork.Repository<ClaimStatusTransaction>().UpdateAsync(claimStatusTransaction);
					}


					// Ticket:TAPI-118
					// Create an Entity/table that will be used to log when a transaction has changed ClaimLineItemStatsId.
					// Record on adding transaction which would be a null previous status and whatever the new status is.
					// We do not want to update if there is an error status. We do not want to overwrite a valid previous with an error status.
					// Requested claim line item status Rank is > then previous status Rank.
					//if (claimStatusTransaction.ClaimLineItemStatusId != request.ClaimLineItemStatusId)

					claimStatusTransactionHistory = _mapper.Map<ClaimStatusTransactionHistory>(claimStatusTransaction);
				}
				else
				{

					// Do not update the transaction that has a non-Error / non-null LineitemStatus. transaction data in tact
					// Map request data to the history though
					claimStatusTransactionHistory = _mapper.Map<ClaimStatusTransactionHistory>(request);
				}
				claimStatusTransactionHistory.DbOperationId = DbOperationEnum.Update;

				//await _unitOfWork.Repository<ClaimStatusTransaction>().UpdateAsync(claimStatusTransaction);
				await _unitOfWork.Repository<ClaimStatusTransactionHistory>().AddAsync(claimStatusTransactionHistory);

				await _unitOfWork.Commit(cancellationToken);

				//responseMessage = $"Updated Transaction - Request TransactionId: {request.Id}, Found TransactionId: {claimStatusTransaction.Id}, ClaimStatusBatchClaimId = {request.ClaimStatusBatchClaimId}";
			}
			else
			{
				//Use transactionRepository to add transaction and attach a lineStatusChange to that then attach to BatchClaim and save all. 
				var claimStatusTransaction = _mapper.Map<ClaimStatusTransaction>(request);
				claimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ
											= new ClaimStatusTransactionLineItemStatusChangẹ(_clientId, (ClaimLineItemStatusEnum)request.ClaimLineItemStatusId, DbOperationEnum.Insert);
				claimStatusTransactionHistory = _mapper.Map<ClaimStatusTransactionHistory>(claimStatusTransaction);
				claimStatusTransactionHistory.DbOperationId = DbOperationEnum.Insert;
				claimStatusTransaction.ClaimStatusTransactionHistories.Add(claimStatusTransactionHistory);

				await _unitOfWork.Repository<ClaimStatusTransaction>().AddAsync(claimStatusTransaction);

				claimStatusBatchClaim.ClaimStatusTransaction = claimStatusTransaction;


				await _unitOfWork.Commit(cancellationToken);
				//responseMessage = $"Inserted Transaction - ClaimStatusBatchClaimId = {request.ClaimStatusBatchClaimId}";
			}

			return claimStatusBatchClaim.ClaimStatusTransactionId ?? 0;
		}
	}       
}