using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.Executive;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Executive
{
    public class ExecutiveManager : IExecutiveManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ExecutiveManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthChargesAsync(GetExecutiveSummaryDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.CurrentMonthCharges(query.ClientId));
                var data = await response.ToResult<IEnumerable<ExecutiveSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthDenialsAsync(GetExecutiveCurrentMonthDenialsDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.CurrentMonthDenials(query.ClientId));
                var data = await response.ToResult<IEnumerable<ExecutiveSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthPaymentsAsync(GetExecutiveCurrentMonthPaymentsQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.CurrentMonthPayments(query.ClientId));
                var data = await response.ToResult<IEnumerable<ExecutiveSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysLocation>>> GetCurrentAROverPercentageLocationAsync(CurrentAROverPercentageNintyDaysLocationQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.GetCurrentAROverPercentageLocation(query.ClientId));
                var data = await response.ToResult<IEnumerable<CurrentAROverPercentageNintyDaysLocation>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysPayer>>> GetCurrentAROverPercentagePayerAsync(GetCurrentAROverNintyDaysPayerByLocationQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.GetCurrentAROverNintyDaysPayerByLocation(query.ClientId, query.LocationId));
                var data = await response.ToResult<IEnumerable<CurrentAROverPercentageNintyDaysPayer>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<ExecutiveClaimRate>>> GetCleanClaimRateAsync(GetExecutiveCleanClaimRateQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.GetCleanClaimRate(query.ClientId));
                var data = await response.ToResult<IEnumerable<ExecutiveClaimRate>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<ExecutiveClaimRate>>> GetDenialClaimRateAsync(GetExecutiveDenialRateQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.GetExecutiveDenialRate(query.ClientId));
                var data = await response.ToResult<IEnumerable<ExecutiveClaimRate>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<MonthlyDaysInAR>> GetMonthlyDaysInARAsync(GetExecutiveMonthlyDaysInARQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.GetMonthlyDaysInAR(query.ClientId));
                var data = await response.ToResult<MonthlyDaysInAR>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<MonthlyEmployeeSummary>>> GetCurrentMonthEmployeeWorkAsync(GetExecutiveCurrentMonthEmployeeWorkQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.GetCurrentMonthEmployeeWork(query.ClientId));
                var data = await response.ToResult<IEnumerable<MonthlyEmployeeSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
