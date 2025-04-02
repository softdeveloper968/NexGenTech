using MedHelpAuthorizations.Application.Features.MonthClose.Queries;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.MonthClose
{
    public class MonthCloseManager : IMonthCloseManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public MonthCloseManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        // Get Monthly Cash Collection Data
        public async Task<PaginatedResult<MonthlyCashCollectionData>> GetMonthlyCashCollectionDataAsync(IMonthCloseDashboardQuery request)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(Routes.MonthCloseEndPoints.GetMonthlyCashCollectionData(request));
                return await response.ToPaginatedResult<MonthlyCashCollectionData>();
            }
            catch (Exception)
            {

                throw;
            }

        }

        // Get Monthly Receivables Data
        public async Task<PaginatedResult<MonthlyReceivablesData>> GetMonthlyReceivablesDataAsync(IMonthCloseDashboardQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.MonthCloseEndPoints.GetMonthlyReceivablesData(request));
            return await response.ToPaginatedResult<MonthlyReceivablesData>();
        }

        // Get Monthly AR Data
        public async Task<PaginatedResult<MonthlyARData>> GetMonthlyARDataAsync(IMonthCloseDashboardQuery request)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(Routes.MonthCloseEndPoints.GetMonthlyARData(request));

                return await response.ToPaginatedResult<MonthlyARData>();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        // Get Monthly Denial Data
        public async Task<PaginatedResult<MonthlyDenialData>> GetMonthlyDenialDataAsync(IMonthCloseDashboardQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.MonthCloseEndPoints.GetMonthlyDenialData(request));
            return await response.ToPaginatedResult<MonthlyDenialData>();
        }


    }

}
