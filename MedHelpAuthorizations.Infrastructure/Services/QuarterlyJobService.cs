using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Client.Shared.Models.ChartObjects;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using static MedHelpAuthorizations.Infrastructure.Services.QuarterlyJobService;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class QuarterlyJobService : IQuarterlyJobService
    {
        private readonly ITenantManagementService _tenantManagementService;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private IUnitOfWork<int> _unitOfWork;

        public QuarterlyJobService(ITenantManagementService tenantManagementService, ITenantRepositoryFactory tenantRepositoryFactory, IUnitOfWork<int> unitOfWork)
        {
            _tenantManagementService = tenantManagementService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CalculateQuarterlyAverageCollection(CancellationToken stoppingToken)
        {
            try
            {
                // Lists to store average collection percentages
                var ClientInsuranceAverageCollectionPercentages = new List<ClientInsuranceAverageCollectionPercentage>();
                var ClaimLevelAverageCollectionPercentages = new List<ClaimLevelAverageCollectionPercentage>();

                // Retrieve all tenants
                var tenants = await _tenantManagementService.GetAllActiveAsync();
                if (tenants.Any())
                {
                    foreach (var tenant in tenants)
                    {
                        // Retrieve repositories for the current tenant
                        var _claimStatusBatchClaimRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchClaimsRepository>(tenant.Identifier);
                        var _claimStatusbatchRepository = await _tenantRepositoryFactory.GetAsync<IClaimStatusBatchRepository>(tenant.Identifier);
                        var _clientInsuranceAverageCollectionPercentageRepository = await _tenantRepositoryFactory.GetAsync<IClientInsuranceAverageCollectionPercentageRepository>(tenant.Identifier);

                        // Retrieve all batch IDs for the current tenant
                        var batchIds = await _claimStatusbatchRepository.ClaimStatusBatches
                                                                        .Where(x => x.CreatedOn > DateTime.UtcNow.AddMonths(-3))
                                                                        .Select(x => x.Id)
                                                                        .ToListAsync();

                        foreach (var batchId in batchIds)
                        {
                            // Retrieve claim level average collection percentages for each batch
                            var _claimLevelAverageCollectionPercentage = await _claimStatusBatchClaimRepository.ClaimStatusBatchClaims
                                .Include(i => i.ClaimStatusTransaction)
                                .Where(i => i.ClaimStatusBatchId == batchId
                                && i.ClaimBilledOn > DateTime.UtcNow.AddMonths(-3))
                                .Specify(new ClaimStatusBatchClaimNotSupplantedSpecification())
                                .GroupBy(i => new { i.ClaimLevelMd5Hash, i.ClientInsuranceId })
                                .Select(group => new ClaimLevelAverageCollectionPercentage()
                                {
                                    ClaimLevelMd5Hash = group.Key.ClaimLevelMd5Hash,
                                    LineItemPaidAmount = group.Sum(y => y.ClaimStatusTransaction != null ? y.ClaimStatusTransaction.LineItemPaidAmount : 0.00m),
                                    LineItemChargeAmount = group.Sum(y => y.ClaimStatusTransaction != null ? y.ClaimStatusTransaction.LineItemChargeAmount : 0.00m),
                                    ClientInsuranceId = (int)group.Key.ClientInsuranceId,
                                }).ToListAsync();

                            //ClaimLevelAverageCollectionPercentages.AddRange(_claimLevelAverageCollectionPercentage);
                            var _clientInsuranceAverageCollectionPercentages = _claimLevelAverageCollectionPercentage
                                .GroupBy(i => i.ClientInsuranceId)
                                .Select(group => new ClientInsuranceAverageCollectionPercentage()
                                {
                                    ClientInsuranceId = group.Key,
                                    Quarter = DateTime.UtcNow.GetQuarter(),
                                    Year = DateTime.UtcNow.Year.ToString(),
                                    CollectionPercentage = (group.Sum(x => x.LineItemChargeAmount) > 0 && group.Sum(x => x.LineItemChargeAmount) > 0) ? Math.Abs((decimal)(group.Sum(x => x.LineItemPaidAmount ?? 0.00m) / group.Sum(x => x.LineItemChargeAmount ?? 0.00m))) : 0.00m
                                }).ToList();

                            // Add or update average collection percentages in the repository
                            if (_clientInsuranceAverageCollectionPercentages.Any())
                            {
                                await AddupdateAverageCollectionPercentages(_clientInsuranceAverageCollectionPercentages, _clientInsuranceAverageCollectionPercentageRepository, stoppingToken);
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task AddupdateAverageCollectionPercentages(List<ClientInsuranceAverageCollectionPercentage> AverageCollectionPercentages, IClientInsuranceAverageCollectionPercentageRepository ClientInsuranceAverageCollectionPercentageRepository, CancellationToken cancellationToken)
        {
            try
            {
                if(AverageCollectionPercentages.Any())
                {
                    //we need to check to see that entry for the current quarter is already data in the records.
                    //if exist then update values
                    foreach (var AverageCollectionPercentage in AverageCollectionPercentages)
                    {
                        // Retrieve existing average collection percentage from the repository
                        var existingAverageCollectionPercentage = await ClientInsuranceAverageCollectionPercentageRepository.GetDataByQuarterAndClientInsurance(AverageCollectionPercentage.Quarter, AverageCollectionPercentage.Year, AverageCollectionPercentage.ClientInsuranceId);

                        if (existingAverageCollectionPercentage != null)
                        {
                            // If exists, update the values
                            existingAverageCollectionPercentage.CollectionPercentage = AverageCollectionPercentage.CollectionPercentage;
                            await ClientInsuranceAverageCollectionPercentageRepository.UpdateAsync(existingAverageCollectionPercentage);
                            await ClientInsuranceAverageCollectionPercentageRepository.Commit(cancellationToken);
                        }
                        else
                        {
                            // If not exists, insert a new entry
                            var response = await ClientInsuranceAverageCollectionPercentageRepository.AddAsync(AverageCollectionPercentage);
                            await ClientInsuranceAverageCollectionPercentageRepository.Commit(cancellationToken);
                        }
                    }
                    //await _unitOfWork.Commit(cancellationToken);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
