using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Corporate
{
    public class CorporateDashboardManager : ICorporateDashboardManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public CorporateDashboardManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<IEnumerable<MonthlyClientSummary>>> GetCurrentMonthChargesAsync(GetCurrentMonthChargesQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.CurrentMonthCharges, query);
                var data = await response.ToResult<IEnumerable<MonthlyClientSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        public async Task<IResult<IEnumerable<MonthlyClientSummary>>> GetCurrentMonthPaymentsAsync(GetCurrentMonthPaymentsQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.CurrentMonthPayments, query);
                var data = await response.ToResult<IEnumerable<MonthlyClientSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<MonthlyClientSummary>>> GetCurrentMonthDenialsAsync(GetCurrentMonthDenialsQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.CurrentMonthDenials, query);
                var data = await response.ToResult<IEnumerable<MonthlyClientSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<MonthlyEmployeeSummary>>> GetCurrentMonthEmployeeWorkAsync(GetCurrentMonthEmployeeWorkQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.CurrentMonthEmployeeWork, query);
                var data = await response.ToResult<IEnumerable<MonthlyEmployeeSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysClient>>> GetCurrentAROverPercentageClientAsync(GetCurrentAROverPercentageNintyDaysClientQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.CurrentAROverPercentageClient, query);
                var data = await response.ToResult<IEnumerable<CurrentAROverPercentageNintyDaysClient>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<IEnumerable<CurrentAROverPercentageNintyDaysPayer>>> GetCurrentAROverPercentagePayerAsync(GetCurrentAROverPercentageNintyDaysPayerQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.CurrentAROverPercentagePayer, query);
                var data = await response.ToResult<IEnumerable<CurrentAROverPercentageNintyDaysPayer>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<MonthlyDaysInAR>> GetMonthlyDaysInARAsync(GetMonthlyDaysInARQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.MonthlyDaysInAR, query);
                var data = await response.ToResult<MonthlyDaysInAR>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        public async Task<IResult<List<ClaimRate>>> GetCleanClaimRateAsync(GetCleanClaimRateQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.CleanClaimRate, query);
                var data = await response.ToResult<List<ClaimRate>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        public async Task<IResult<List<ClaimRate>>> GetDenialClaimRateAsync(GetDenialRateQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.DenialClaimRate, query);
                var data = await response.ToResult<List<ClaimRate>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<ClaimStatusDashboardData>> GetClientStatTotalsDataAsync(GetClientStatTotalsDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.ClientStatTotals, query);
                var data = await response.ToResult<ClaimStatusDashboardData>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<List<FinancialSummaryData>>> GetClientFinancialSummaryDataAsync(GetClientFinancialSummayDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.ClientFinancialSummary, query);
                var data = await response.ToResult<List<FinancialSummaryData>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetClientDenialReasonsByInsuranceDataAsync(GetClientDenialReasonsByInsuranceDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.ClientDenialReasons, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<ClaimSummary>> GetClientClaimSummaryDataAsync(GetClientClaimSummaryDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.ClientClaimSummary, query);
                var data = await response.ToResult<ClaimSummary>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult<List<AverageDaysByPayer>>> GetClientAverageDaysToPayByPayerDataAsync(GetClientAverageDaysToPayByPayerQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.AverageDaysToPayByPayer, query);
                var data = await response.ToResult<List<AverageDaysByPayer>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ChargesByPayer>>> GetClientChargesByPayersDataAsync(GetClientChargesByPayersQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.ChargesByPayer, query);
                var data = await response.ToResult<List<ChargesByPayer>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<ClaimSummary>> GetClientClaimStatusDataAsync(GetClientClaimStatusDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.ClaimStatus, query);
                var data = await response.ToResult<ClaimSummary>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetClientClaimsInProcessDataAsync(GetClientClaimsInProcessQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.ClaimsInProcess, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetClientAvgAllowedAmtTotalsAsync(GetClientAvgAllowedAmtTotalsQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(CorporateDashboardEndpoints.AvgAllowedAmt, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
