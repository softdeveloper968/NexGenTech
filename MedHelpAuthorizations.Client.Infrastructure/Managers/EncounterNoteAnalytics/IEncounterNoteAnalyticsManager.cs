using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.Base;
using MedHelpAuthorizations.Shared.Models.EncounterNote;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.EncounterNoteAnalytics
{
    public interface IEncounterNoteAnalyticsManager : IManager
    {
        Task<IResult<string>> GetResponseFromENAApi();

        Task<IResult<List<ProviderChartingResponse>>> GeProviderChartingResponse(EncounterNoteFilter encounterNoteFilter); //EN-738

        Task<string> GetProviderCleanReport(EncounterNoteFilter encounterNoteFilter); //EN-745
        Task<IResult<ExecutiveDashboardResponse>> GetExecutiveDashboardResponse(EncounterNoteFilter encounterNoteFilter, decimal complianceAccuracy);
        Task<IResult<List<CodingComplianceResponse>>> GetCodingDashboardResponse(EncounterNoteFilter encounterNoteFilter);
        Task<string> GetCodingComplianceReport(EncounterNoteFilter encounterNoteFilter, decimal? codingAccuray);
        Task<IResult<List<ProviderGradingResponseByProvderId>>> GeProviderChartingResponseByProviderId(EncounterNoteFilter encounterNoteFilter, int providerId); //EN-794
        Task<string> ExportProviderChartingResponseByProviderId(EncounterNoteFilter encounterNoteFilter, int providerId);
        Task<IResult<ChartKpiDashboardResponse>> GetChartKpiDashboarResponse(EncounterNoteFilter encounterNoteFilter, decimal complianceAccuracy);//EN-819
    }
}
