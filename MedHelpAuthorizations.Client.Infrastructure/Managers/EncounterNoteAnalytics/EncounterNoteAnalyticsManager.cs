using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.Base;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients.AIEncounterNoteAnalysis;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Models.EncounterNote;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClientToCurl;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.EncounterNoteAnalytics
{
    public class EncounterNoteAnalyticsManager : IEncounterNoteAnalyticsManager
    {
        private readonly IEcounterNoteApiInternalHttpClient _client;

        public EncounterNoteAnalyticsManager(IEcounterNoteApiInternalHttpClient client)
        {
            _client = client;
        }

        public async Task<IResult<string>> GetResponseFromENAApi()
        {
            try
            {
                var response = await _client.GetAsync(EncounterNoteAnalyticsEndpoints.test);
                return await response.ToResult<string>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IResult<List<ProviderChartingResponse>>> GeProviderChartingResponse(EncounterNoteFilter encounterNoteFilter)
        {
                var endpoint = EncounterNoteAnalyticsEndpoints.ProviderCharting(encounterNoteFilter);
                var response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
                return await response.ToResult<List<ProviderChartingResponse>>();
        }

        public async Task<string> GetProviderCleanReport(EncounterNoteFilter encounterNoteFilter)
        {
            try
            {
                var endpoint = EncounterNoteAnalyticsEndpoints.ExportProviderCleanReport(encounterNoteFilter);
                var response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IResult<ExecutiveDashboardResponse>> GetExecutiveDashboardResponse(EncounterNoteFilter encounterNoteFilter, decimal complianceAccuracy)
        {
            var curlScript = string.Empty;
            HttpResponseMessage response = null;
            try
            {
                var endpoint = EncounterNoteAnalyticsEndpoints.GetExecutiveDashboardDetail(encounterNoteFilter, complianceAccuracy);
                response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
                return await response.ToResult<ExecutiveDashboardResponse>();
            }
            catch (Exception ex)
            {
                curlScript = _client.Client.GenerateCurlInString(response.RequestMessage); //Put into a variable
                throw;
            }
        }

        public async Task<IResult<List<CodingComplianceResponse>>> GetCodingDashboardResponse(EncounterNoteFilter encounterNoteFilter)
        {
            try
            {
                var endpoint = EncounterNoteAnalyticsEndpoints.GetCodingCompliancesDashboardDetail(encounterNoteFilter);
                var response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
                return await response.ToResult<List<CodingComplianceResponse>>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> GetCodingComplianceReport(EncounterNoteFilter encounterNoteFilter, decimal? codingAccuray)
        {
            try
            {
                var endpoint = EncounterNoteAnalyticsEndpoints.ExportCodingCompliancesReport(encounterNoteFilter, codingAccuray);
                var response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IResult<List<ProviderGradingResponseByProvderId>>> GeProviderChartingResponseByProviderId(EncounterNoteFilter encounterNoteFilter, int providerId)
        {
            var endpoint = EncounterNoteAnalyticsEndpoints.ProviderChartingByProviderId(encounterNoteFilter, providerId);
            var response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
            return await response.ToResult<List<ProviderGradingResponseByProvderId>>();
        }

        public async Task<string> ExportProviderChartingResponseByProviderId(EncounterNoteFilter encounterNoteFilter, int providerId)
        {
            try
            {
                var endpoint = EncounterNoteAnalyticsEndpoints.ProviderChartingReportByProviderId(encounterNoteFilter, providerId);
                var response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IResult<ChartKpiDashboardResponse>> GetChartKpiDashboarResponse(EncounterNoteFilter encounterNoteFilter, decimal complianceAccuracy)//EN-819
        {
            try
            {
                var endpoint = EncounterNoteAnalyticsEndpoints.GetChartKpiDashboardDetail(encounterNoteFilter,complianceAccuracy);
                var response = await _client.PostAsJsonAsync(endpoint, encounterNoteFilter);
                return await response.ToResult<ChartKpiDashboardResponse>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
