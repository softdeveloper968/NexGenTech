using System;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Enums;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClaimStatusBatchRepository : RepositoryAsync<ClaimStatusBatch, int>, IClaimStatusBatchRepository
    {
        private readonly ApplicationContext _dbContext;
        public ClaimStatusBatchRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ClaimStatusBatchRepository(ApplicationContext dbContext, ITenantInfo tenantInfo) : base(dbContext, tenantInfo)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ClaimStatusBatch> ClaimStatusBatches => _dbContext.ClaimStatusBatches;

        public async Task DeleteAsync(ClaimStatusBatch claimStatusBatch)
        {
            _dbContext.ClaimStatusBatches.Remove(claimStatusBatch);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ClaimStatusBatch> GetByIdAsync(int claimStatusBatchId)
        {
            return await _dbContext.ClaimStatusBatches
                //.Include(x => x.ClaimStatusBatchHistories)
                .Include(x => x.ClientInsurance)
                    .ThenInclude(x => x.RpaInsurance)
                .Include(x => x.ClientInsurance.Client)
                .Include(x => x.AuthType)
                .Include(b => b.InputDocument)
                    .ThenInclude(x => x.ClientLocation)
                        .ThenInclude(x => x.ClientLocationInsuranceIdentifiers)
                .FirstOrDefaultAsync(p => p.Id == claimStatusBatchId);
        }

        public async Task<List<ClaimStatusBatch>> GetUnprocessedByCriteriaAsync(GetClaimStatusUnprocessedBatchesQuery query, ApiIntegrationEnum? apiIntegrationId = null)
        {
            //var batchesCurrentlyInProcessByHost = await _repository.Entities
            //    .Include(x => x.ClientInsurance)
            //        .ThenInclude(x => x.RpaInsurance)
            //    .Include(x => x.ClientInsurance.Client)
            //    .CountAsync(b => b.ClientInsurance.RpaInsuranceId == query.RpaInsuranceId
            //                      && b.ClaimStatusBatchClaims.Count > 0
            //                      && b.AssignedDateTimeUtc != null
            //                      //&& b.AssignedToHostName == query.AssignedToHostName
            //                      && b.CompletedDateTimeUtc == null
            //                      && !b.IsDeleted);

            var baseQuery = _dbContext.ClaimStatusBatches
                //.Include(x => x.ClaimStatusBatchHistories)
                .Include(b => b.ClientInsurance)
                    .ThenInclude(b => b.RpaInsurance)
                .Include(b => b.Client)
                    .ThenInclude(c => c.ClientApiIntegrationKeys)
                .Include(b => b.AuthType)
                .Include(b => b.InputDocument)
                    .ThenInclude(x => x.ClientLocation)
                        .ThenInclude(x => x.ClientLocationInsuranceIdentifiers)
                //.Take(Math.Max(query.BatchesReturnedLimit - batchesCurrentlyInProcessByHost, 0)) // Do not allow a negative number
                .Specify(new ClaimStatusBatchesInitialAnalysisSpecification(query.IsForInitialAnalysis))
                .Specify(new ClaimStatusBatchNotDeletedResolvedOrExpiredSpecification())
                .Where(b => b.Client.IsActive && b.ClientInsurance.RpaInsurance != null
                                              && b.ClientInsurance.RpaInsuranceId == query.RpaInsuranceId
                                              && (!b.ClientInsurance.RpaInsurance.InactivatedOn.HasValue
                                                  || b.ClientInsurance.RpaInsurance.InactivatedOn.Value.AddHours(3) < DateTime.UtcNow)
                                              && b.ClaimStatusBatchClaims.Count > 0
                                              && b.AssignedDateTimeUtc == null
                                              && b.CompletedDateTimeUtc == null
                                              && b.AbortedOnUtc == null
                                              && !b.IsDeleted);

			if (apiIntegrationId != null)
			{
				baseQuery = baseQuery.Where(x => x.Client.ClientApiIntegrationKeys.Any(y => y.ApiIntegrationId == apiIntegrationId));
			}

			var result = await baseQuery
							.OrderBy(b => b.LastModifiedOn.HasValue)
					        .ThenBy(B => B.LastModifiedOn)
                            .ToListAsync();

			return result;
                    
		}

        //AA-318
        /// <summary>
        /// Retrieves a list of Claim Status Batches associated with a specific RPA Insurance ID, subject to various filtering conditions.
        /// </summary>
        /// <param name="query">A query object containing filter and options for the retrieval.</param>
        /// <returns>A list of Claim Status Batches that match the specified criteria.</returns>
        public async Task<List<ClaimStatusBatch>> GetByRpaInsuranceId(GetClaimStatusBatchesByRpaInsuranceIdQuery query, ApiIntegrationEnum? apiIntegrationId = null)
        {
            try
            {
                var baseQuery = _dbContext.ClaimStatusBatches
                    .Include(b => b.ClientInsurance)
                        .ThenInclude(b => b.RpaInsurance)
                    .Include(b => b.Client)
					    .ThenInclude(c => c.ClientApiIntegrationKeys)
					.Include(b => b.AuthType)
                    .Include(b => b.InputDocument)
                    .ThenInclude(x => x.ClientLocation)
                        .ThenInclude(x => x.ClientLocationInsuranceIdentifiers)
                     .Specify(new IsActiveClientsSpecification<ClaimStatusBatch>())
                    .Specify(new ClaimStatusBatchesInitialAnalysisSpecification(query.IsForInitialAnalysis))
                    .Specify(new ClaimStatusBatchNotDeletedResolvedOrExpiredSpecification())
                    .Where(b => b.ClientInsurance.RpaInsurance != null
                        && b.ClientInsurance.RpaInsurance.Id == query.RpaInsuranceId
                        && (!b.ClientInsurance.RpaInsurance.InactivatedOn.HasValue
                            || b.ClientInsurance.RpaInsurance.InactivatedOn.Value.AddHours(3) < DateTime.UtcNow)
                        && b.ClientInsurance.RpaInsurance.Name != "Unknown"
                        && b.ClaimStatusBatchClaims.Any() // Check if there are any related claims
                        && !b.IsDeleted);

                if(apiIntegrationId != null)
                {
                    //Uses api for claim status
					baseQuery = baseQuery.Where(x => x.Client.ClientApiIntegrationKeys.Any(y => y.ApiIntegrationId == apiIntegrationId));
				}
                else
                {
                    //Doesn't use api for claim status. 
					baseQuery = baseQuery.Where(x => x.ClientInsurance.RpaInsurance != null && x.ClientInsurance.RpaInsurance.ApiIntegrationId == null);
				}
				if (query.IncludeAssignedBatches == false)
                {
                    baseQuery = baseQuery.Specify(new ClaimStatusBatchesIncludeAssignedSpecification());
                }

                var result = await baseQuery
                    .OrderBy(b => b.LastModifiedOn)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex; // Rethrow the exception or return a specific result.
            }
        }


        public async Task<List<ClaimStatusBatch>> GetAllUnprocessedAsync(bool isForInitialAnalysis = false)
        {
            try
            {
                return await _dbContext.ClaimStatusBatches
                .Include(b => b.ClientInsurance)
                    .ThenInclude(r => r.RpaInsurance)
                .Include(b => b.Client)
                .Include(b => b.AuthType)
                .Include(b => b.InputDocument)
                    .ThenInclude(x => x.ClientLocation)
                        .ThenInclude(x => x.ClientLocationInsuranceIdentifiers)
                .Include(b => b.ClientInsurance)
                    .ThenInclude(b => b.RpaInsurance)
                        .ThenInclude(c => c.RpaInsuranceGroup)
                 .Specify(new IsActiveClientsSpecification<ClaimStatusBatch>())
                .Specify(new ClaimStatusBatchesInitialAnalysisSpecification(isForInitialAnalysis))
                .Specify(new ClaimStatusBatchNotDeletedResolvedOrExpiredSpecification())
                .Where(b => b.Client.IsActive && b.ClientInsurance.RpaInsurance != null
                                              && (!b.ClientInsurance.RpaInsurance.InactivatedOn.HasValue
                                                  || b.ClientInsurance.RpaInsurance.InactivatedOn.Value.AddHours(3) < DateTime.UtcNow)
                                              && b.ClientInsurance.RpaInsurance.Name != "Unknown"
                                              && b.ClaimStatusBatchClaims.Count > 0
                                              && b.AssignedDateTimeUtc == null
                                              && !b.IsDeleted)
                //.OrderByDescending(b => b.ClaimStatusBatchHistories.Count(x => x.AbortedOnUtc == null))
                //.ThenBy((b => b.Priority.HasValue))
                //.ThenByDescending(b => b.Priority)
                .OrderBy(b => b.LastModifiedOn.HasValue)
                    .ThenBy(B => B.LastModifiedOn)
                .ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<List<ClaimStatusBatch>> GetCompletedCleanupByHostName(string hostName)
        {
            return await _dbContext.ClaimStatusBatches
                //.Include(x => x.ClaimStatusBatchHistories)
                .Include(b => b.ClientInsurance)
                    .ThenInclude(b => b.RpaInsurance)
                .Include(b => b.Client)
                .Include(b => b.AuthType)
                .Include(b => b.InputDocument)
                    .ThenInclude(x => x.ClientLocation)
                        .ThenInclude(x => x.ClientLocationInsuranceIdentifiers)
                .Where(b => b.AssignedToHostName == hostName
                            && b.CompletedDateTimeUtc != null
                            && !string.IsNullOrWhiteSpace(b.AssignedToRpaLocalProcessIds))
                .ToListAsync();
        }

        public async Task<List<ClaimStatusBatch>> GetNotCompletedCleanupByHostName(string hostName)
        {
            var lastTransactionInfo = await _dbContext.ClaimStatusBatches
                //.Include(x => x.ClaimStatusBatchHistories)
                .Include(b => b.Client)
                .Include(b => b.ClaimStatusBatchClaims)
                    .ThenInclude(y => y.ClaimStatusTransaction)
                .Where(b => b.AssignedDateTimeUtc <= DateTime.UtcNow.AddMinutes(-10)
                            && b.AssignedToHostName == hostName
                            && b.CompletedDateTimeUtc == null
                            && b.AbortedOnUtc == null
                            && !string.IsNullOrWhiteSpace(b.AssignedToRpaLocalProcessIds))
                .ToListAsync();

            return lastTransactionInfo;
        }

        public async Task<List<ClaimStatusBatch>> GetListAsync()
        {
            return await _dbContext.ClaimStatusBatches
                .Include(x => x.ClientInsurance)
                    .ThenInclude(y => y.RpaInsurance)
                .Include(x => x.Client)
                .Include(x => x.ClaimStatusBatchHistories)
                .Include(x => x.InputDocument)
                    .ThenInclude(x => x.ClientLocation)
                        .ThenInclude(x => x.ClientLocationInsuranceIdentifiers)
                 .Specify(new IsActiveClientsSpecification<ClaimStatusBatch>())
                .ToListAsync();
        }

        public async Task<List<ClaimStatusBatch>> GetUnresolvedBatchesByDaysOldAsync(int maxDaysOld = 180)
        {
            return await _dbContext.ClaimStatusBatches
                .Specify(new ClaimStatusBatchNotDeletedResolvedOrExpiredSpecification())
                .Where(b => b.Client.IsActive && b.CreatedOn.AddDays(maxDaysOld) >= DateTime.UtcNow
                                              && (b.CompletedDateTimeUtc != null || b.AbortedOnUtc != null))
                .ToListAsync();
        }

        public async Task<int> InsertAsync(ClaimStatusBatch claimStatusBatch)
        {
            await _dbContext.ClaimStatusBatches.AddAsync(claimStatusBatch);
            await _dbContext.SaveChangesAsync();
            return claimStatusBatch.Id;
        }

        public async Task UpdateAsync(ClaimStatusBatch claimStatusBatch)
        {
            _dbContext.ClaimStatusBatches.Update(claimStatusBatch);
            await _dbContext.SaveChangesAsync();

        }

        public async Task RestoreAsync(ClaimStatusBatch claimStatusBatch)
        {
            claimStatusBatch.IsDeleted = false;
            _dbContext.ClaimStatusBatches.Update(claimStatusBatch);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ClaimStatusBatch>> GetAbortedBatchesListAsync()
        {
            return await _dbContext.ClaimStatusBatches
                .Include(x => x.ClientInsurance)
                    .ThenInclude(x => x.RpaInsurance)
                .Include(x => x.ClientInsurance.Client)
                .Where(x => x.AbortedOnUtc != null)
                .ToListAsync();
        }

        public async Task<List<ClaimStatusBatch>> GetBatchesByClientInsuranceAndAuthTypeAsync(int ClientInsuranceId, int? AuthTypeId)
        {
            return await _dbContext.ClaimStatusBatches
                .Include(x => x.ClaimStatusBatchHistories)
                .Include(x => x.ClientInsurance)
                    .ThenInclude(x => x.RpaInsurance)
                .Include(x => x.ClientInsurance.Client)
                .Where(x => x.Client.IsActive && x.AbortedOnUtc != null && x.ClientInsuranceId == ClientInsuranceId && x.AuthTypeId == AuthTypeId)
                .ToListAsync();
        }

        public async Task<List<ClaimStatusBatch>> GetByClientIdAsyncAsync(int clientId)
        {
            try
            {
                return await _dbContext.ClaimStatusBatches
                            .Include(x => x.ClientInsurance)
                            .Include(x => x.ClaimStatusBatchHistories)
                            .Include(x => x.ClaimStatusBatchClaims)
                            .Where(x => x.ClientId == clientId)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //EN-65
        public async Task<List<ClaimStatusBatch>> RetrieveValidBatchIdsAsync()
        {
            return await _dbContext.ClaimStatusBatches
                //.Include(x => x.ClaimStatusBatchClaims)
                .Include(x => x.ClientInsurance)
                    .ThenInclude(y => y.ClientInsuranceFeeSchedules)
                .Where(x => x.ClientInsurance.ClientInsuranceFeeSchedules.Any()
                            && x.ClaimStatusBatchClaims.Any(y => y.ClientFeeScheduleEntryId == null))
                .ToListAsync();
        }

        public async Task<List<int>> GetNonDeletedBatchIdsAsync()
        {
            return await _dbContext.ClaimStatusBatches
                .Where(x => !x.IsDeleted)
                .Select(x => x.Id)
                .ToListAsync();
        }


        public async Task<ClaimStatusBatchesPaginationModel> GetAbortedClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            var dataQuery = from cb in _dbContext.ClaimStatusBatches
                            join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                            join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                            join c in _dbContext.Clients on cb.ClientId equals c.Id
                            where cb.AbortedOnUtc != null && (string.IsNullOrEmpty(quickSearch) ||
                                (c.ClientCode.Contains(quickSearch) ||
                                cb.BatchNumber.Contains(quickSearch) ||
                                rpai.Name.Contains(quickSearch) ||
                                cb.AbortedOnUtc.ToString().Contains(quickSearch) ||
                                cb.CreatedOn.ToString().Contains(quickSearch) ||
                                cb.LastModifiedOn.ToString().Contains(quickSearch))
                            )
                            select new ClaimStatusBatchesModel
                            {
                                Id = cb.Id,
                                ClientCode = c.ClientCode,
                                AbortedOnUtc = cb.AbortedOnUtc,
                                AbortedReason = cb.AbortedReason,
                                AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                AssignedToHostName = cb.AssignedToHostName,
                                AssignedToIpAddress = cb.AssignedToIpAddress,
                                AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                BatchNumber = cb.BatchNumber,
                                CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                CreatedOn = cb.CreatedOn,
                                LastModifiedOn = cb.LastModifiedOn,
                                Payer = ci.Name,
                                RPA = rpai.Name,
                                LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                            };

            var skipRecords = (pageNumber - 1) * pageSize;

            var data = await dataQuery
                         .OrderByMappings(sortField, sortOrder)
                         .Skip(skipRecords)
                         .Take(pageSize)
                         .ToListAsync();

            var countQuery = from cb in _dbContext.ClaimStatusBatches
                             join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                             join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                             join c in _dbContext.Clients on cb.ClientId equals c.Id
                             where cb.AbortedOnUtc != null && (string.IsNullOrEmpty(quickSearch) || (
                                c.ClientCode.Contains(quickSearch) ||
                                cb.BatchNumber.Contains(quickSearch) ||
                                rpai.Name.Contains(quickSearch) ||
                                cb.AbortedOnUtc.ToString().Contains(quickSearch) ||
                                cb.CreatedOn.ToString().Contains(quickSearch) ||
                                cb.LastModifiedOn.ToString().Contains(quickSearch))
                            )
                             select new ClaimStatusBatchesModel
                             {
                                 Id = cb.Id,
                                 ClientCode = c.ClientCode,
                                 AbortedOnUtc = cb.AbortedOnUtc,
                                 AbortedReason = cb.AbortedReason,
                                 AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                 AssignedToHostName = cb.AssignedToHostName,
                                 AssignedToIpAddress = cb.AssignedToIpAddress,
                                 AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                 BatchNumber = cb.BatchNumber,
                                 CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                 CreatedOn = cb.CreatedOn,
                                 LastModifiedOn = cb.LastModifiedOn,
                                 Payer = ci.Name,
                                 RPA = rpai.Name,
                                 LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                             };


            var totalCount = await countQuery.OrderByMappings(sortField, sortOrder).CountAsync();

            var retVal = new ClaimStatusBatchesPaginationModel();
            retVal.Data = data;
            retVal.TotalRows = totalCount;

            return retVal;
        }

        public async Task<ClaimStatusBatchesPaginationModel> GetCompletedClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            var dataQuery = from cb in _dbContext.ClaimStatusBatches
                            join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                            join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                            join c in _dbContext.Clients on cb.ClientId equals c.Id
                            where cb.CompletedDateTimeUtc != null && (string.IsNullOrEmpty(quickSearch) || (
                                c.ClientCode.Contains(quickSearch) ||
                                cb.BatchNumber.Contains(quickSearch) ||
                                rpai.Name.Contains(quickSearch) ||
                                cb.CompletedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.AssignedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.CreatedOn.ToString().Contains(quickSearch) ||
                                cb.LastModifiedOn.ToString().Contains(quickSearch)
                            ))
                            select new ClaimStatusBatchesModel
                            {
                                Id = cb.Id,
                                ClientCode = c.ClientCode,
                                AbortedOnUtc = cb.AbortedOnUtc,
                                AbortedReason = cb.AbortedReason,
                                AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                AssignedToHostName = cb.AssignedToHostName,
                                AssignedToIpAddress = cb.AssignedToIpAddress,
                                AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                BatchNumber = cb.BatchNumber,
                                CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                CreatedOn = cb.CreatedOn,
                                LastModifiedOn = cb.LastModifiedOn,
                                Payer = ci.Name,
                                RPA = rpai.Name,
                                AllClaimStatusesResolvedOrExpired = cb.AllClaimStatusesResolvedOrExpired,
                                LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                            };


            var skipRecords = (pageNumber - 1) * pageSize;

            var data = await dataQuery
                         .OrderByMappings(sortField, sortOrder)
                         .Skip(skipRecords)
                         .Take(pageSize)
                         .ToListAsync();

            var countQuery = from cb in _dbContext.ClaimStatusBatches
                             join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                             join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                             join c in _dbContext.Clients on cb.ClientId equals c.Id
                             where cb.CompletedDateTimeUtc != null && (string.IsNullOrEmpty(quickSearch) || (
                                c.ClientCode.Contains(quickSearch) ||
                                cb.BatchNumber.Contains(quickSearch) ||
                                rpai.Name.Contains(quickSearch) ||
                                cb.CompletedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.AssignedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.CreatedOn.ToString().Contains(quickSearch) ||
                                cb.LastModifiedOn.ToString().Contains(quickSearch)
                            ))
                             select new ClaimStatusBatchesModel
                             {
                                 Id = cb.Id,
                                 ClientCode = c.ClientCode,
                                 AbortedOnUtc = cb.AbortedOnUtc,
                                 AbortedReason = cb.AbortedReason,
                                 AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                 AssignedToHostName = cb.AssignedToHostName,
                                 AssignedToIpAddress = cb.AssignedToIpAddress,
                                 AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                 BatchNumber = cb.BatchNumber,
                                 CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                 CreatedOn = cb.CreatedOn,
                                 LastModifiedOn = cb.LastModifiedOn,
                                 Payer = ci.Name,
                                 RPA = rpai.Name,
                                 AllClaimStatusesResolvedOrExpired = cb.AllClaimStatusesResolvedOrExpired,
                                 LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                             };


            var totalCount = await countQuery.OrderByMappings(sortField, sortOrder).CountAsync();

            var retVal = new ClaimStatusBatchesPaginationModel();
            retVal.Data = data;
            retVal.TotalRows = totalCount;

            return retVal;
        }

        public async Task<ClaimStatusBatchesPaginationModel> GetInProgressClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            var dataQuery = from cb in _dbContext.ClaimStatusBatches
                            join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                            join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                            join c in _dbContext.Clients on cb.ClientId equals c.Id
                            where cb.CompletedDateTimeUtc != null && cb.AbortedOnUtc != null && (string.IsNullOrEmpty(quickSearch) || (
                                c.ClientCode.Contains(quickSearch) ||
                                cb.BatchNumber.Contains(quickSearch) ||
                                rpai.Name.Contains(quickSearch) ||
                                cb.AbortedOnUtc.ToString().Contains(quickSearch) ||
                                cb.CompletedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.AssignedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.CreatedOn.ToString().Contains(quickSearch) ||
                                cb.LastModifiedOn.ToString().Contains(quickSearch)
                            ))
                            orderby cb.AssignedDateTimeUtc descending
                            select new ClaimStatusBatchesModel
                            {
                                Id = cb.Id,
                                ClientCode = c.ClientCode,
                                AbortedOnUtc = cb.AbortedOnUtc,
                                AbortedReason = cb.AbortedReason,
                                AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                AssignedToHostName = cb.AssignedToHostName,
                                AssignedToIpAddress = cb.AssignedToIpAddress,
                                AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                BatchNumber = cb.BatchNumber,
                                CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                CreatedOn = cb.CreatedOn,
                                LastModifiedOn = cb.LastModifiedOn,
                                Payer = ci.Name,
                                RPA = rpai.Name,
                                AllClaimStatusesResolvedOrExpired = cb.AllClaimStatusesResolvedOrExpired,
                                LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                            };

            var skipRecords = (pageNumber - 1) * pageSize;

            var data = await dataQuery
                         .OrderByMappings(sortField, sortOrder)
                         .Skip(skipRecords)
                         .Take(pageSize)
                         .ToListAsync();

            var countQuery = from cb in _dbContext.ClaimStatusBatches
                             join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                             join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                             join c in _dbContext.Clients on cb.ClientId equals c.Id
                             where cb.CompletedDateTimeUtc != null && cb.AbortedOnUtc != null
                             orderby cb.AssignedDateTimeUtc != null && (string.IsNullOrEmpty(quickSearch) || (
                                c.ClientCode.Contains(quickSearch) ||
                                cb.BatchNumber.Contains(quickSearch) ||
                                rpai.Name.Contains(quickSearch) ||
                                cb.AbortedOnUtc.ToString().Contains(quickSearch) ||
                                cb.CompletedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.AssignedDateTimeUtc.ToString().Contains(quickSearch) ||
                                cb.CreatedOn.ToString().Contains(quickSearch) ||
                                cb.LastModifiedOn.ToString().Contains(quickSearch)
                            ))
                             select new ClaimStatusBatchesModel
                             {
                                 Id = cb.Id,
                                 ClientCode = c.ClientCode,
                                 AbortedOnUtc = cb.AbortedOnUtc,
                                 AbortedReason = cb.AbortedReason,
                                 AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                 AssignedToHostName = cb.AssignedToHostName,
                                 AssignedToIpAddress = cb.AssignedToIpAddress,
                                 AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                 BatchNumber = cb.BatchNumber,
                                 CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                 CreatedOn = cb.CreatedOn,
                                 LastModifiedOn = cb.LastModifiedOn,
                                 Payer = ci.Name,
                                 RPA = rpai.Name,
                                 AllClaimStatusesResolvedOrExpired = cb.AllClaimStatusesResolvedOrExpired,
                                 LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                             };


            var totalCount = await countQuery.OrderByMappings(sortField, sortOrder).CountAsync();

            var retVal = new ClaimStatusBatchesPaginationModel();
            retVal.Data = data;
            retVal.TotalRows = totalCount;

            return retVal;
        }

        public async Task<ClaimStatusBatchesPaginationModel> GetDeletedClaimStatusBatchesAsync(int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            var dataQuery = from cb in _dbContext.ClaimStatusBatches
                            join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                            join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                            join c in _dbContext.Clients on cb.ClientId equals c.Id
                            where cb.IsDeleted && (string.IsNullOrEmpty(quickSearch) || (
                            c.ClientCode.Contains(quickSearch) ||
                            cb.BatchNumber.Contains(quickSearch) ||
                            rpai.Name.Contains(quickSearch) ||
                            cb.AbortedOnUtc.ToString().Contains(quickSearch) ||
                            cb.CompletedDateTimeUtc.ToString().Contains(quickSearch) ||
                            cb.AssignedDateTimeUtc.ToString().Contains(quickSearch) ||
                            cb.CreatedOn.ToString().Contains(quickSearch) ||
                            cb.LastModifiedOn.ToString().Contains(quickSearch)
                        ))
                            select new ClaimStatusBatchesModel
                            {
                                Id = cb.Id,
                                ClientCode = c.ClientCode,
                                AbortedOnUtc = cb.AbortedOnUtc,
                                AbortedReason = cb.AbortedReason,
                                AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                AssignedToHostName = cb.AssignedToHostName,
                                AssignedToIpAddress = cb.AssignedToIpAddress,
                                AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                BatchNumber = cb.BatchNumber,
                                CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                CreatedOn = cb.CreatedOn,
                                LastModifiedOn = cb.LastModifiedOn,
                                Payer = ci.Name,
                                RPA = rpai.Name,
                                AllClaimStatusesResolvedOrExpired = cb.AllClaimStatusesResolvedOrExpired,
                                LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                            };

            var skipRecords = (pageNumber - 1) * pageSize;

            var data = await dataQuery
                         .IgnoreQueryFilters()
                         .OrderByMappings(sortField, sortOrder)
                         .Skip(skipRecords)
                         .Take(pageSize)
                         .ToListAsync();

            var countQuery = from cb in _dbContext.ClaimStatusBatches.Where(x => x.IsDeleted == true)
                             join ci in _dbContext.ClientInsurances on cb.ClientInsuranceId equals ci.Id
                             join rpai in _dbContext.RpaInsurances on ci.RpaInsuranceId equals rpai.Id
                             join c in _dbContext.Clients on cb.ClientId equals c.Id
                             where cb.IsDeleted && (string.IsNullOrEmpty(quickSearch) || (
                            c.ClientCode.Contains(quickSearch) ||
                            cb.BatchNumber.Contains(quickSearch) ||
                            rpai.Name.Contains(quickSearch) ||
                            cb.AbortedOnUtc.ToString().Contains(quickSearch) ||
                            cb.CompletedDateTimeUtc.ToString().Contains(quickSearch) ||
                            cb.AssignedDateTimeUtc.ToString().Contains(quickSearch) ||
                            cb.CreatedOn.ToString().Contains(quickSearch) ||
                            cb.LastModifiedOn.ToString().Contains(quickSearch)
                        ))
                             select new ClaimStatusBatchesModel
                             {
                                 Id = cb.Id,
                                 ClientCode = c.ClientCode,
                                 AbortedOnUtc = cb.AbortedOnUtc,
                                 AbortedReason = cb.AbortedReason,
                                 AssignedDateTimeUtc = cb.AssignedDateTimeUtc,
                                 AssignedToHostName = cb.AssignedToHostName,
                                 AssignedToIpAddress = cb.AssignedToIpAddress,
                                 AssignedToRpaLocalProcessIds = cb.AssignedToRpaLocalProcessIds,
                                 BatchNumber = cb.BatchNumber,
                                 CompletedDateTimeUtc = cb.CompletedDateTimeUtc,
                                 CreatedOn = cb.CreatedOn,
                                 LastModifiedOn = cb.LastModifiedOn,
                                 Payer = ci.Name,
                                 RPA = rpai.Name,
                                 AllClaimStatusesResolvedOrExpired = cb.AllClaimStatusesResolvedOrExpired,
                                 LineItems = cb.ClaimStatusBatchClaims.DefaultIfEmpty().Count(),
                             };

            var totalCount = await countQuery
                                .IgnoreQueryFilters()
                                .OrderByMappings(sortField, sortOrder)
                                .CountAsync();

            var retVal = new ClaimStatusBatchesPaginationModel();
            retVal.Data = data;
            retVal.TotalRows = totalCount;
            return retVal;
        }
    }
}
