using AutoMapper;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClaimStatusBatchClaimsRepository : RepositoryAsync<ClaimStatusBatchClaim, int>, IClaimStatusBatchClaimsRepository
    {

        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;//AA-321

        public ClaimStatusBatchClaimsRepository(ApplicationContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ClaimStatusBatchClaimsRepository(ApplicationContext dbContext, IMapper mapper, ITenantInfo tenantInfo) : base(dbContext, tenantInfo)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ClaimStatusBatchClaim> ClaimStatusBatchClaims => _dbContext.ClaimStatusBatchClaims;

        public async Task<List<ClaimStatusBatchClaim>> GetInitialClaimStatusByBatchIdAsync(int batchId)
        {
            return await ClaimStatusBatchClaims
                .Include(x => x.ClaimStatusTransaction)
                .Where(c => c.ClaimStatusBatchId == batchId
                            && (c.ClaimStatusTransaction == null))
                .ToListAsync();
        }

        public async Task<List<ClaimStatusBatchClaim>> GetClaimStatusByBatchIdAsync(int batchId)
        {
            return await ClaimStatusBatchClaims
                .Include(x => x.ClaimStatusTransaction)
                .Where(c => c.ClaimStatusBatchId == batchId)
                .ToListAsync();
        }

        #region Get Un-Resolved BatchId by pageination

        public async Task<List<GetClaimStatusBatchClaimsByBatchIdResponse>> GetUnresolvedByBatchIdAsync(int batchId, int returnQuantityCap = 2000)//, int pageNumber = 1, int pageSize = 100000)
        {
            try
            {
                List<GetClaimStatusBatchClaimsByBatchIdResponse> result = await ClaimStatusBatchClaims?
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(t => t.ClaimStatusTransactionLineItemStatusChangẹ)
                .Include(c => c.ClaimStatusTransaction)
                    .ThenInclude(cs => cs.ClaimLineItemStatus)
                .Include(c => c.ClientLocation)
                    .ThenInclude(l => l.ClientLocationInsuranceIdentifiers)
                .Include(c => c.ClientProvider)
                .Include(c => c.Patient)
                    .ThenInclude(p => p.Person)
                .Specify(new ApprovedClaimLineItemStatusWaitPeriodSpecification())
                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                .Specify(new ClaimStatusBatchClaimByBatchIdSpecification(batchId))
                .Specify(new ClaimStatusClaimBilledOnQualificationFilterSpecification())
                .Specify(new ClaimStatusMaxDaysPipelineQualificationFilterSpecification())
                //.Specify(new ClaimStatusAttemptsQualificationFilterSpecification())
                .Specify(new ClaimStatusDaysBetweenAttemptsQualificationFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedWrongPayerFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedPolicyNumberFilterSpecification())
                .OrderBy(c => c.ClaimStatusTransactionId.HasValue)
                .ThenBy(c => c.ClaimStatusTransaction != null && c.ClaimStatusTransaction.ClaimLineItemStatus != null ? c.ClaimStatusTransaction.ClaimLineItemStatus.Rank : 0)
                ?.Take(returnQuantityCap)
                //.Select(c => _mapper.Map<GetClaimStatusBatchClaimsByBatchIdResponse>(c))
                .Select(c => new GetClaimStatusBatchClaimsByBatchIdResponse()
                {
                    Id = c.Id,
                    CurrentLineItemStatusId = c.ClaimStatusTransaction != null ? c.ClaimStatusTransaction.ClaimLineItemStatusId : null,
                    CurrentExceptionReason = c.ClaimStatusTransaction != null ? c.ClaimStatusTransaction.ExceptionReason : null,
                    LastStatusChangedOn = (c.ClaimStatusTransaction != null && c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ != null) ? c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn : null,
                    ClaimStatusBatchId = c.ClaimStatusBatchId,
                    ClaimStatusTransactionId = c.ClaimStatusTransactionId,
                    ClaimNumber = c.ClaimNumber,
                    PayerClaimNumber = c.ClaimStatusTransaction != null ? c.ClaimStatusTransaction.ClaimNumber : null,
                    PatientLastName = (c.Patient != null && c.Patient.Person != null) ? c.Patient.Person.LastName : string.Empty,
                    PatientFirstName = (c.Patient != null && c.Patient.Person != null) ? c.Patient.Person.FirstName : string.Empty,
                    DateOfBirth = c.DateOfBirth,
                    RenderingNpi = c.ClientProvider != null ? c.ClientProvider.Npi : string.Empty,
                    GroupNpi = c.GroupNpi ?? (c.Client.NpiNumber != null ? c.Client.NpiNumber.ToString() : null),
                    PolicyNumber = c.PolicyNumber,
                    PolicyNumberUpdatedOn = c.PolicyNumberUpdatedOn,
                    EligibilityPolicyNumber = c.ClaimStatusTransaction != null ? c.ClaimStatusTransaction.EligibilityPolicyNumber : string.Empty,
                    DateOfServiceFrom = c.DateOfServiceFrom,
                    DateOfServiceTo = c.DateOfServiceTo,
                    ProcedureCode = c.ProcedureCode,
                    Modifiers = c.Modifiers,
                    ClaimBilledOn = c.ClaimBilledOn,
                    BilledAmount = c.BilledAmount,
                    Quantity = c.Quantity,
                    IsDeleted = c.IsDeleted,
                    ClientLocationNpi = c.ClientLocation != null ? c.ClientLocation.Npi : string.Empty,
                    ClientLocationInsuranceIdentifierString = c.ClientLocation != null ? c.ClientLocation.GetLocationIdentifierStringForInsuranceId(c.ClientLocation, c.ClientInsuranceId) : string.Empty,
                    PayerIdentifier = c.ClientInsurance != null ? c.ClientInsurance.PayerIdentifier : string.Empty,
                    TaxId = c.Client.TaxId != null ? c.Client.TaxId.ToString().PadLeft(9, '0') : string.Empty
                })
                ?.ToListAsync();

                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        #endregion

        //private string GetLocationIdentifierForInsuranceId(ClientLocationInsuranceIdentifier clientLocationInsuranceIdentifier)
        //{
        //    return clientLocationInsuranceIdentifier?.Identifier;
        //}

        public async Task<int> GetUnresolvedCountByBatchIdAsync(int batchId)
        {
            //This does not care about claimBilledOnDate, DaysToWait between attempts
            return await ClaimStatusBatchClaims
                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                .Specify(new ClaimStatusBatchClaimByBatchIdSpecification(batchId))
                .Specify(new ClaimStatusMaxDaysPipelineQualificationFilterSpecification())
                //.Specify(new ClaimStatusAttemptsQualificationFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedWrongPayerFilterSpecification())
                .Specify(new ClaimStatusOmitDeniedPolicyNumberFilterSpecification())
                .CountAsync();
        }

        public async Task<int> InsertAsync(ClaimStatusBatchClaim claimStatusBatchClaim)
        {
            await _dbContext.ClaimStatusBatchClaims.AddAsync(claimStatusBatchClaim);
            return claimStatusBatchClaim.Id;
        }

        //public async Task<List<ClaimStatusBatchClaim>> GetByClaimStatusBatchClaimRootIdAsync(int claimStatusBatchClaimRootId)
        //{
        //    return await _repository.Entities
        //                    .Include(c => c.ClaimStatusBatchClaimRoot)
        //                    .Include(c => c.ClaimStatusBatch)
        //                    .Include(c => c.ClaimStatusTransaction)
        //                        .ThenInclude(s => s.ClaimLineItemStatus)
        //                    .Include(c => c.ClaimStatusTransaction.ClaimStatusTransactionHistories)
        //                    .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
        //                    .Where(c => c.ClaimStatusBatchClaimRootId == claimStatusBatchClaimRootId)
        //                    .ToListAsync();
        //}

        public async Task<ClaimStatusBatchClaim> GetActiveByClaimNumberAndPatientIdAsync(string claimNumber, int? patientId)
        {
            if (string.IsNullOrWhiteSpace(claimNumber) || patientId == null)
                return null;

            return await ClaimStatusBatchClaims
                            //.Include(c => c.ClaimStatusBatchClaimRoot)
                            .Include(c => c.ClaimStatusBatch)
                            .Include(c => c.ClaimStatusTransaction)
                                .ThenInclude(s => s.ClaimLineItemStatus)
                            //.Include(c => c.ClaimStatusTransaction.ClaimStatusTransactionHistories)
                            .Include(c => c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ)
                            .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                            .Where(c => (c.ClaimNumber != null 
                                        && c.ClaimNumber.ToUpper().Trim() == claimNumber.ToUpper().Trim())   
                                        && c.PatientId == patientId)
                            .FirstOrDefaultAsync();

        }

        public async Task<List<ClaimStatusBatchClaim>> GetByEntryMd5HashAsync(string entryMd5HashAsync)
        {
            return await ClaimStatusBatchClaims
                            //.Include(c => c.ClaimStatusBatchClaimRoot)
                            //.Include(c => c.ClaimStatusBatch)
                            //.Include(c => c.ClaimStatusTransaction)
                            //    .ThenInclude(s => s.ClaimLineItemStatus)
                            //.Include(c => c.ClaimStatusTransaction.ClaimStatusTransactionHistories)
                            .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                            .Where(c => c.EntryMd5Hash == entryMd5HashAsync)
                            .ToListAsync();
        }

        //public async Task UpdateSupplantedByRootIdAsync(int claimStatusBatchClaimRootId)
        //{
        //    var batchClaims = await GetByClaimStatusBatchClaimRootIdAsync(claimStatusBatchClaimRootId);

        //    foreach (var bc in batchClaims)
        //    {
        //        bc.IsSupplanted = true;
        //        await _repository.UpdateAsync(bc);
        //    }
        //}

        public async Task UpdateSupplantedByEntryMd5HashAsync(string entryMd5HashAsync)
        {
            var batchClaims = await GetByEntryMd5HashAsync(entryMd5HashAsync);

            foreach (var bc in batchClaims)
            {
                bc.IsSupplanted = true;
                _dbContext.ClaimStatusBatchClaims.Update(bc);
                await _dbContext.SaveChangesAsync();
            }
            //return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSupplantedByIdAsync(int id)
        {
            var bc = await GetByIdAsync(id);
            if (bc != null)
            {
                bc.IsSupplanted = true;
                _dbContext.ClaimStatusBatchClaims.Update(bc);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<bool> IsSupplantableAsync(ClaimStatusBatchClaim originalClaim, ClaimStatusBatchClaim replacementClaim)
        {
            //If something changed in data, denied previously, or expired - supplant
            if (!originalClaim.Equals(replacementClaim))
                return Task.FromResult(true);
            if (originalClaim == null)
                return Task.FromResult(true);
            if (originalClaim.ClaimStatusTransactionId == null)
                return Task.FromResult(false);
            if (originalClaim.ClaimStatusTransaction.LastModifiedOn == null)
                return Task.FromResult(false);
            if (originalClaim.ClaimStatusTransaction.ClaimLineItemStatusId == null)
                return Task.FromResult(false);
            if (ReadOnlyObjects.DeniedClaimLineItemStatuses.Contains(originalClaim.ClaimStatusTransaction.ClaimLineItemStatusId ?? 0))
                return Task.FromResult(true);
            //Max Attempts
            var currentStatusHxCount = originalClaim.ClaimStatusTransaction?.ClaimStatusTransactionHistories?.Count(x => x.CreatedOn > DateTime.UtcNow.AddMonths(-3) && x.ClaimLineItemStatusId == originalClaim.ClaimStatusTransaction?.ClaimLineItemStatusId);
            if (currentStatusHxCount < originalClaim.ClaimStatusTransaction.ClaimLineItemStatus.MinimumResolutionAttempts)
                return Task.FromResult(false);
            //Max Pipeline Days
            if ((DateTime.Now.Date - originalClaim.ClaimStatusTransaction.LastModifiedOn.Value.Date).Days >= originalClaim.ClaimStatusTransaction.ClaimLineItemStatus.MaximumPipelineDays)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        public async Task<ClaimStatusBatchClaim> GetActiveByEntryMd5HashAsync(string entryMd5HashAsync)
        {
            return await ClaimStatusBatchClaims
                            //.Include(c => c.ClaimStatusBatchClaimRoot)
                            .Include(c => c.ClaimStatusBatch)
                            .Include(c => c.ClaimStatusTransaction)
                                .ThenInclude(s => s.ClaimLineItemStatus)
                            .Include(c => c.ClaimStatusTransaction.ClaimStatusTransactionHistories)
                            .Include(c => c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ)
                            .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                            .Where(c => c.EntryMd5Hash == entryMd5HashAsync )
                            .FirstOrDefaultAsync();

        }

        public async Task<List<ClaimStatusBatchClaim>> GetClaimsUnMappedToFeeScheduleByBatchIdAsync(int batchId)
        {
            return await ClaimStatusBatchClaims
                .Include(x => x.ClientProvider)
                .Include(x => x.ClaimStatusTransaction)
                .Specify(new ClaimStatusBatchClaimByBatchIdSpecification(batchId))
                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                .Specify(new ClaimStatusBatchClaimUnMappedToFeeScheduleSpecification())
                .ToListAsync();
        }



        //public async Task<List<ClaimStatusBatchClaim>> GetDaysWaitLapsedByClientIdAsync(int clientId)
        //{
        //    return await _repository.Entities
        //         .Include(c => c.ClaimStatusTransaction)
        //             .ThenInclude(t => t.ClaimStatusTransactionLineItemStatusChangẹ)
        //         .Include(c => c.ClaimStatusTransaction)
        //             .ThenInclude(cs => cs.ClaimLineItemStatus)
        //         .Include(c => c.Patient)
        //             .ThenInclude(p => p.Person)
        //         .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
        //         .Specify(new GenericByClientIdSpecification<ClaimStatusBatchClaim>(clientId))
        //         .Specify(new ClaimStatusClaimBilledOnQualificationFilterSpecification())
        //         .Specify(new ClaimStatusMaxDaysPipelineQualificationFilterSpecification())
        //         .Specify(new ClaimStatusAttemptsQualificationFilterSpecification())
        //         .Specify(new ClaimStatusDaysWaitLapsedFilterSpecification())
        //         .Specify(new ClaimStatusOmitDeniedWrongPayerFilterSpecification())
        //         .OrderBy(c => c.ClaimStatusTransactionId.HasValue)
        //         .ThenBy(c => c.ClaimStatusTransaction.ClaimLineItemStatus.Rank)
        //         .ToListAsync();
        //}

        //public async Task<ClaimStatusBatchClaim> GetActiveByRootIdAsync(int claimStatusBatchClaimRootId)
        //{
        //    return await _repository.Entities
        //                    .Include(c => c.ClaimStatusBatchClaimRoot)
        //                    .Include(c => c.ClaimStatusBatch)
        //                    .Include(c => c.ClaimStatusTransaction)
        //                        .ThenInclude(s => s.ClaimLineItemStatus)
        //                    .Include(c => c.ClaimStatusTransaction.ClaimStatusTransactionHistories)
        //                    .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
        //                    .Where(c => c.ClaimStatusBatchClaimRootId == claimStatusBatchClaimRootId)
        //                    .FirstOrDefaultAsync();
        //}

        //AA-228
        public async Task<List<ClaimStatusDashboardInProcessDetailsResponse>> GetUncheckedClaims(int ClientId)
        {
            try
            {
                return await ClaimStatusBatchClaims
                                        .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                                        .Specify(new ClaimStatusBatchClaimNotDeletedSpecification())
                                        .Specify(new ClaimStatusUncheckedClaimsSpecification())
                                        .Specify(new Application.Specifications.GenericByClientIdSpecification<ClaimStatusBatchClaim>(ClientId))
                                        .Select(c => new ClaimStatusDashboardInProcessDetailsResponse()
                                        {
                                            PatientLastName = c.Patient.Person.LastName,
                                            PatientFirstName = c.Patient.Person.FirstName,
                                            DateOfBirth = c.Patient.Person.DateOfBirth != null ? c.Patient.Person.DateOfBirth.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                                            PolicyNumber = c.PolicyNumber,
                                            ServiceType = c.ClaimStatusBatch.AuthType.Name,
                                            PayerName = c.ClaimStatusBatch.ClientInsurance.LookupName,
                                            OfficeClaimNumber = c.ClaimNumber,
                                            ProcedureCode = c.ProcedureCode,
                                            DateOfServiceFrom = c.DateOfServiceFrom != null ? c.DateOfServiceFrom.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                                            ClaimBilledOn = c.ClaimBilledOn != null ? c.ClaimBilledOn.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                                            BilledAmount = c.BilledAmount ?? 0.00m,
                                            BatchNumber = c.ClaimStatusBatch.BatchNumber,
                                            AitClaimReceivedDate = c.CreatedOn.ToLocalTime().ToShortDateString(),
                                            AitClaimReceivedTime = c.CreatedOn.ToLocalTime().ToShortTimeString(),
                                            ClientLocationName = c.ClientLocation.Name,
                                            ClientLocationNpi = c.ClientLocation.Npi
                                        })
                                        .OrderBy(c => c.PayerName)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //AA-228
        public async Task<List<ClaimStatusDaysWaitLapsedDetailResponse>> GetDaysWaitLapsedByClientIdAsync(int clientId)
        {
            try
            {
                return await ClaimStatusBatchClaims
                                        .Include(c => c.ClaimStatusTransaction)
                                            .ThenInclude(t => t.ClaimStatusTransactionLineItemStatusChangẹ)
                                        .Include(c => c.ClaimStatusTransaction)
                                            .ThenInclude(cs => cs.ClaimLineItemStatus)
                                        .Include(c => c.Patient)
                                            .ThenInclude(p => p.Person)
                                        .Specify(new ApprovedClaimLineItemStatusWaitPeriodSpecification())
                                        .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                                        .Specify(new ClaimStatusNotDeletedSpecification())
                                        .Specify(new Application.Specifications.GenericByClientIdSpecification<ClaimStatusBatchClaim>(clientId))
                                        .Specify(new ClaimStatusClaimBilledOnQualificationFilterSpecification())
                                        .Specify(new ClaimStatusMaxDaysPipelineQualificationFilterSpecification())
                                        //.Specify(new ClaimStatusAttemptsQualificationFilterSpecification())
                                        .Specify(new ClaimStatusDaysBetweenAttemptsQualificationFilterSpecification())
                                        .Specify(new ClaimStatusDaysWaitLapsedFilterSpecification())
                                        .Specify(new ClaimStatusOmitDeniedWrongPayerFilterSpecification())
                                        .Select(c => new ClaimStatusDaysWaitLapsedDetailResponse()
                                        {
                                            ClaimStatusTransactionId = c.ClaimStatusTransactionId,
                                            ClaimStatusTransactionLineItemStatusChangẹId = c.ClaimStatusTransaction != null ? c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId : null,
                                            PatientLastName = c.Patient.Person.LastName,
                                            PatientFirstName = c.Patient.Person.FirstName,
                                            DateOfBirth = c.Patient.Person.DateOfBirth != null ? c.Patient.Person.DateOfBirth.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                                            PolicyNumber = c.PolicyNumber,
                                            ServiceType = c.ClaimStatusBatch.AuthType.Name,
                                            PayerName = c.ClaimStatusBatch.ClientInsurance.LookupName,
                                            OfficeClaimNumber = c.ClaimNumber,
                                            ProcedureCode = c.ProcedureCode,
                                            DateOfServiceFrom = c.DateOfServiceFrom != null ? c.DateOfServiceFrom.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                                            ClaimBilledOn = c.ClaimBilledOn != null ? c.ClaimBilledOn.Value.ToString(ClaimFiltersHelpers._dateFormat) : String.Empty,
                                            BilledAmount = c.BilledAmount ?? 0.00m,
                                            BatchNumber = c.ClaimStatusBatch.BatchNumber,
                                            ClaimLineItemStatus = GetClaimLineItemStatusString(c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatusId),
                                            StatusLastCheckedOn = c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn == null
                                                                    ? c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.CreatedOn.ToString(ClaimFiltersHelpers._dateFormat)
                                                                    : c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.Value.ToString(ClaimFiltersHelpers._dateFormat),
                                            DaysLapsed = String.Format("{0:0.##}", (DateTime.UtcNow - (c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn == null
                                                                    ? c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.CreatedOn
                                                                    : c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.Value)
                                                                    .AddDays(c.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatus.DaysWaitBetweenAttempts))
                                                                    .TotalDays),
                                            AitClaimReceivedDate = c.CreatedOn.ToLocalTime().ToShortDateString(),
                                            AitClaimReceivedTime = c.CreatedOn.ToLocalTime().ToShortTimeString(),
                                        })
                                        .OrderBy(c => c.PayerName)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static string GetClaimLineItemStatusString(ClaimLineItemStatusEnum? claimLineItemStatusEnum)
        {
            if (claimLineItemStatusEnum != null)
                return claimLineItemStatusEnum.ToString();

            return string.Empty;
        }

        public async Task<List<ChargeEntryRpaConfiguration>> GetUiPathChargeEntryConfigurations()
        {
            try
            {
                return await _dbContext.ChargeEntryRpaConfigurations
                    .Specify(new ActiveChargeEntryConfigurationSpecification())
                    .Specify(new ChargeEntryConfigurationUiPathSpecification())
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<ClaimStatusBatchClaim>> GetClaimsToUpdateAsync(int batchId)
        {
            return await ClaimStatusBatchClaims
                .Where(x => !x.IsDeleted
                            && !x.IsSupplanted
                            && x.ClaimStatusBatchId == batchId
                            && string.IsNullOrEmpty(x.NormalizedClaimNumber)
                )
                .ToListAsync();
        }

        public async Task<List<ClaimStatusBatchClaim>> GetClaimsByCriteriaAsync(
                                                                          List<int> clientInsuranceIds,
                                                                          int clientCptCodeId,
                                                                          DateTime clientFeeScheduleStartDate,
                                                                          DateTime? clientFeeScheduleEndDate,
                                                                          List<SpecialtyEnum> SpecialtyId = null,
                                                                          List<ProviderLevelEnum> ProviderLevelId = null
                                                                          )
        {
            var specification = new ClaimStatusBatchClaimByFeeScheduleCriteriaSpecification(clientInsuranceIds, clientCptCodeId, clientFeeScheduleStartDate, clientFeeScheduleEndDate, SpecialtyId, ProviderLevelId);

            return await ClaimStatusBatchClaims
                .Include(x => x.ClaimStatusTransaction)
                               .Specify(specification)
                               .ToListAsync();
        }
         //EN-232
        public async Task<List<ClaimStatusBatchClaim>> GetClaimsByFeeScheduleCriteriaAsync(
                                                                          List<int> clientInsuranceIds,
                                                                          int clientCptCodeId,
                                                                          DateTime? clientFeeScheduleStartDate,
                                                                          DateTime? clientFeeScheduleEndDate,
                                                                          List<SpecialtyEnum> SpecialtyId = null,
                                                                          List<ProviderLevelEnum> ProviderLevelId = null
                                                                          )
        {
            var specification = new GetClaimsByFeeScheduleCriteriaSpecification(clientInsuranceIds, clientCptCodeId, clientFeeScheduleStartDate, clientFeeScheduleEndDate, SpecialtyId, ProviderLevelId);

            return await ClaimStatusBatchClaims
                .Include(x => x.ClaimStatusTransaction)
                               .Specify(specification)
                               .ToListAsync();
        }
    }
}
