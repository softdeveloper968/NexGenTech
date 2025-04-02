using AutoMapper;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ClientFeeScheduleService : IClientFeeScheduleService
	{
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository { get; set; }
        private IClaimStatusTransactionRepository _claimStatusTransactionRepository { get; set; }
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
		private readonly ITenantInfo _tenantInfo;

		public ClientFeeScheduleService(
            ITenantRepositoryFactory tenantRepositoryFactory,
            IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository,
            IClaimStatusQueryService claimStatusQueryService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IClaimStatusTransactionRepository claimStatusTransactionRepository,
            IUnitOfWork<int> unitOfWork,
			 ITenantInfo tenantInfo)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _claimStatusQueryService = claimStatusQueryService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _claimStatusTransactionRepository = claimStatusTransactionRepository;
            _unitOfWork = unitOfWork;
			_tenantInfo = tenantInfo;
		}

        /// <summary>
        /// Processes claims manually by checking and updating their status based on fee schedule information. (AA-315)
        /// </summary>
        /// <remarks>
        /// This method iterates through all batches of claims, retrieves claims that are not mapped to fee schedules,
        /// and checks if they require a write-off based on fee schedule conditions. If a claim requires a write-off,
        /// it either creates a new transaction or updates an existing one accordingly, and then updates the claim status.
        /// </remarks>
        public async Task<bool> ProcessFeeScheduleMatchedClaim(ClaimStatusBatchClaim claim, string tenantIdentifier = null, int? clientFeeScheduleEntryId = null)
        {
            var mappedToFeeSchedule = false;
           // if(claim != null && claim.ClientId != 35) { return false; }
            try
            {
                string connStr = null;
                if (!string.IsNullOrWhiteSpace(tenantIdentifier))
                {
                    _claimStatusTransactionRepository = _tenantRepositoryFactory.GetClaimStatusTransactionRepository(tenantIdentifier);
                    _claimStatusBatchClaimsRepository = _tenantRepositoryFactory.GetClaimStatusBatchClaimsRepository(tenantIdentifier);
                    connStr = _claimStatusTransactionRepository.TenantInfo?.ConnectionString;
                }

                //EN-214
                if (clientFeeScheduleEntryId != null)
                {
                    mappedToFeeSchedule = true;
                    // Associate the claim with the appropriate ClientFeeScheduleEntry
                    claim.ClientFeeScheduleEntryId = clientFeeScheduleEntryId;
                    await AddUpdateClaimStatusTransactionAsContractual(claim);
                }
                else
                {

                    var feeScheduleEntry = await _claimStatusQueryService.GetClientFeeScheduleEntry(claim.ProcedureCode, (int)claim.ClientInsuranceId, (DateTime)claim.DateOfServiceFrom, claim.ClientProvider?.ProviderLevelId, claim.ClientProvider?.SpecialtyId, connStr);

                    if (feeScheduleEntry != null)
                    {
                        mappedToFeeSchedule = true;
                        // Associate the claim with the appropriate ClientFeeScheduleEntry
                        claim.ClientFeeScheduleEntryId = feeScheduleEntry.ClientFeeScheduleEntryId;

                        if (!feeScheduleEntry.IsReimbursable)
                        {
                            await AddUpdateClaimStatusTransactionAsContractual(claim);
                        }
                    }
                }
                // Update the claim with the ClientFeeScheduleEntryId and write-off transaction
                await _claimStatusBatchClaimsRepository.UpdateAsync(claim);
                await _claimStatusBatchClaimsRepository.Commit(new CancellationToken());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mappedToFeeSchedule;
        }


        /// <summary>
        /// Updates the ClaimStatusTransaction for a write-off scenario.
        /// If the claim does not have an existing ClaimStatusTransaction, a new one is created.
        /// </summary>
        /// <param name="claim">The ClaimStatusBatchClaim to be updated.</param>
        private async Task AddUpdateClaimStatusTransactionAsContractual(ClaimStatusBatchClaim claim)
        {
            if (claim.ClaimStatusTransactionId == null)
            {
                // Create a new ClaimStatusTransaction for write-off
                ClaimStatusTransaction newTransaction = new ClaimStatusTransaction()
                {
                    ClaimStatusBatchClaimId = claim.Id,
                    ClaimLineItemStatusId = ClaimLineItemStatusEnum.Contractual,
                    ClaimStatusTransactionBeginDateTimeUtc = DateTime.UtcNow,
                    ClaimStatusTransactionEndDateTimeUtc = DateTime.UtcNow,
                    ExceptionReason = "Fee Schedule Contractual",
                    IsDeleted = false,
                    CreatedBy = _currentUserService.UserId,
                    CreatedOn = DateTime.UtcNow,
                    TotalAllowedAmount = 0.0m,
                    ClaimLineItemStatusValue = ClaimLineItemStatusEnum.Contractual.ToString(),
                    WriteoffAmount = claim.BilledAmount,
                    ClientId = claim.ClientId,
                };

                // Map the new transaction to a history record
                var historyRecord = _mapper.Map<ClaimStatusTransaction, ClaimStatusTransactionHistory>(newTransaction);
                historyRecord.DbOperationId = DbOperationEnum.Insert;

                newTransaction.ClaimStatusTransactionHistories.Add(historyRecord);

                await _claimStatusTransactionRepository.InsertAsync(newTransaction);
                await _claimStatusTransactionRepository.Commit(new CancellationToken());

                // Update the claim with the newly created transaction
                claim.ClaimStatusTransactionId = newTransaction.Id;
            }
            else
            {
                var transaction = await _claimStatusTransactionRepository.GetByIdAsync(claim.ClaimStatusTransactionId ?? 0);

                if (transaction != null)
                {
                    transaction.ClaimLineItemStatusId = ClaimLineItemStatusEnum.Contractual;
                    transaction.ClaimLineItemStatusValue = ClaimLineItemStatusEnum.Contractual.ToString();

                    await _claimStatusTransactionRepository.UpdateAsync(transaction);
                    await _claimStatusTransactionRepository.Commit(new CancellationToken());
                }
            }
        }

        public async Task<decimal> GetLatestPaidAmountForPayerCptDos(int clientInsuranceId, int? clientCptCode, int clientId, DateTime DateOfService)
        {
            var specification = new GetClaimsByUnmappedFeeScheduleSpecification(clientInsuranceId, clientCptCode, clientId, DateOfService);

            // Get the claim with the highest ClaimBilledOn date
            var latestClaim = await _unitOfWork.Repository<ClaimStatusBatchClaim>()
                .Entities
                .Include(c => c.ClaimStatusTransaction)
                .Specify(specification)
                .OrderByDescending(x => x.ClaimBilledOn)
                .FirstOrDefaultAsync();

            // If a claim is found, return its TotalAllowedAmount, otherwise return 0
            return latestClaim != null ? latestClaim.ClaimStatusTransaction.TotalAllowedAmount ?? 0 : 0;
        }


		public async Task<List<FeeScheduleCriteriaResultViewModel>> GetClaimStatusAveragePaidAmountAsync(FeeScheduleCriteriaModel feeScheduleCriteriaModel, int clientId = 0, string connStr = null)
		{
			if (clientId == 0)
			{
				clientId = _currentUserService.ClientId;
			}

			if (string.IsNullOrWhiteSpace(connStr))
			{
				connStr = _tenantInfo.ConnectionString;
			}

			List<FeeScheduleCriteriaResultViewModel> response = new List<FeeScheduleCriteriaResultViewModel>();
			SqlConnection conn = new SqlConnection(connStr);

			try
			{
				await using (conn)
				{
					await conn.OpenAsync();
					SqlCommand cmd = CreateClaimStatusSpCommand("spGetAutoCalculateFeeScheduleData", conn, clientId, feeScheduleCriteriaModel);
					response = await ExecuteEmployeeClaimStatusSpCommandAndGetAveragePaidAmount(cmd);
				}
				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}


		private SqlCommand CreateClaimStatusSpCommand(string spName, SqlConnection conn, int clientId, FeeScheduleCriteriaModel feeScheduleCriteriaModel)
		{
			SqlCommand cmd = new SqlCommand(spName, conn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.AddWithValue("@clientInsuranceIds", feeScheduleCriteriaModel.ClientInsuranceIds ?? string.Empty);
			cmd.Parameters.AddWithValue("@clientFeeScheduleStartDate", feeScheduleCriteriaModel.ClientFeeScheduleStartDate);
			cmd.Parameters.AddWithValue("@clientFeeScheduleEndDate", feeScheduleCriteriaModel.ClientFeeScheduleEndDate);
			cmd.Parameters.AddWithValue("@specialtyIds", feeScheduleCriteriaModel.SpecialtyIds ?? string.Empty);
			cmd.Parameters.AddWithValue("@providerLevelIds", feeScheduleCriteriaModel.ProviderLevelIds ?? string.Empty);
			cmd.Parameters.AddWithValue("@clientId", clientId);

			return cmd;
		}

		private async Task<List<FeeScheduleCriteriaResultViewModel>> ExecuteEmployeeClaimStatusSpCommandAndGetAveragePaidAmount(SqlCommand command)
		{
			try
			{
				List<FeeScheduleCriteriaResultViewModel> response = new List<FeeScheduleCriteriaResultViewModel>();

				using (command)
				{
					if (command.Connection.State != ConnectionState.Open)
						await command.Connection.OpenAsync();

					SqlDataReader reader = await command.ExecuteReaderAsync();

					while (reader.Read())
					{
						var dataItem = new FeeScheduleCriteriaResultViewModel();
						dataItem.AverageLineItemPaidAmount = reader["AverageLineItemPaidAmount"] == DBNull.Value ? 0 : (decimal)reader["AverageLineItemPaidAmount"];
						dataItem.ProcedureCode = reader[StoredProcedureColumnsHelper.ProcedureCode] as string;
						dataItem.ChargedSum = reader["ChargedSum"] != DBNull.Value ? (decimal)reader["ChargedSum"] : default(decimal);
						dataItem.AverageBilledAmount = reader["AvergaeBilledAmount"] != DBNull.Value ? (decimal)reader["AvergaeBilledAmount"] : default(decimal);
						dataItem.ChargedSum = reader["ChargedSum"] != DBNull.Value ? (decimal)reader["ChargedSum"] : default(decimal);
						dataItem.ClientCptCodeId = reader["ClientCptCodeId"] != DBNull.Value ? (int)reader["ClientCptCodeId"] : default(int);
						response.Add(dataItem);
					}
					reader.Close();
				}
				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

        /// <summary>
        /// Updates the existing ClaimStatusTransaction to "Unknown" status if it is currently "Contractual".
        /// If no existing transaction is found, creates a new ClaimStatusTransaction with "Unknown" status.
        /// </summary>
        /// <param name="claim">The ClaimStatusBatchClaim object containing the claim details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateOrCreateClaimStatusTransactionAsync(ClaimStatusBatchClaim claim, int clientFeeScheduleEntryId)
        {
            claim.ClientFeeScheduleEntryId = clientFeeScheduleEntryId;
            await _claimStatusBatchClaimsRepository.UpdateAsync(claim);
            await _claimStatusBatchClaimsRepository.Commit(new CancellationToken());

            if (claim.ClaimStatusTransaction != null)
            {
                var transaction = await _claimStatusTransactionRepository.GetByIdAsync(claim.ClaimStatusTransactionId ?? 0);

                if (transaction != null && transaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Contractual)
                {
                    // Update the transaction to "Unknown" status
                    transaction.ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unknown;
                    transaction.ClaimLineItemStatusValue = ClaimLineItemStatusEnum.Unknown.ToString();

                    await _claimStatusTransactionRepository.UpdateAsync(transaction);
                    await _claimStatusTransactionRepository.Commit(new CancellationToken());
                }
                
            }
            else
            {
                // Create a new "Unknown" transaction if it doesn't exist
                await CreateNewUnknownTransaction(claim);
            }
        }

        private async Task CreateNewUnknownTransaction(ClaimStatusBatchClaim claim)
        {
            // Create a new ClaimStatusTransaction for unknown status
            ClaimStatusTransaction newTransaction = new ClaimStatusTransaction()
            {
                ClaimStatusBatchClaimId = claim.Id,
                ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unknown,
                ClaimStatusTransactionBeginDateTimeUtc = DateTime.UtcNow,
                ClaimStatusTransactionEndDateTimeUtc = DateTime.UtcNow,
                ExceptionReason = "Fee Schedule Unknown",
                IsDeleted = false,
                CreatedBy = _currentUserService.UserId,
                CreatedOn = DateTime.UtcNow,
                TotalAllowedAmount = 0.0m,
                ClaimLineItemStatusValue = ClaimLineItemStatusEnum.Unknown.ToString(),
                WriteoffAmount = claim.BilledAmount,
                ClientId = claim.ClientId,
            };

            // Map the new transaction to a history record
            var historyRecord = _mapper.Map<ClaimStatusTransaction, ClaimStatusTransactionHistory>(newTransaction);
            historyRecord.DbOperationId = DbOperationEnum.Insert;

            newTransaction.ClaimStatusTransactionHistories.Add(historyRecord);

            // Insert the new transaction and commit
            await _claimStatusTransactionRepository.InsertAsync(newTransaction);
            await _claimStatusTransactionRepository.Commit(new CancellationToken());

            // Update the claim with the newly created transaction
            claim.ClaimStatusTransactionId = newTransaction.Id;
            await _claimStatusBatchClaimsRepository.UpdateAsync(claim);
            await _claimStatusBatchClaimsRepository.Commit(new CancellationToken());
        }
    }
}
