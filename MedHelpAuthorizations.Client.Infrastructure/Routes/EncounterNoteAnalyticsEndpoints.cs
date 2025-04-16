using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.Base;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class EncounterNoteAnalyticsEndpoints
    {
        public static string test = "api/Grading/test";

        public static string ProviderCharting(EncounterNoteFilter encounterNoteFilter)
        {
            
            return $"api/v1/ProviderGrading/providerCharting?{encounterNoteFilter}";
        }

        public static string ExportProviderCleanReport(EncounterNoteFilter encounterNoteFilter)
        {
            return $"api/v1/ProviderGrading/exportProviderCleanReport?{encounterNoteFilter}";
        }

        public static string GetExecutiveDashboardDetail(EncounterNoteFilter encounterNoteFilter, decimal complianceAccuracy)
        {
             return $"api/v1/ExecutiveDashboard/getExecutiveDashboardDetail?{encounterNoteFilter}&complianceAccuracy={complianceAccuracy}";
        }

        public static string GetCodingCompliancesDashboardDetail(EncounterNoteFilter encounterNoteFilter) //EN-714
        {
            return $"api/v1/CodingCompliance/getCodingComplianceDetails?{encounterNoteFilter}";
        }

        public static string ExportCodingCompliancesReport(EncounterNoteFilter encounterNoteFilter, decimal? codingAccuray)
        {
            return $"api/v1/CodingCompliance/exportCodingComplianceReport?{encounterNoteFilter}&codingAccuray={codingAccuray}";
        }

        public static string ProviderChartingByProviderId(EncounterNoteFilter encounterNoteFilter, int providerId)
        {
            return $"api/v1/ProviderGrading/getproviderChartingDetailByProviderId?{encounterNoteFilter}&providerId={providerId}";
        }

        public static string ProviderChartingReportByProviderId(EncounterNoteFilter encounterNoteFilter, int providerId)
        {
            return $"api/v1/ProviderGrading/exportproviderChartingReportByProviderId?{encounterNoteFilter}&providerId={providerId}";
        }

        public static string GetChartKpiDashboardDetail(EncounterNoteFilter encounterNoteFilter, decimal complianceAccuracy)
        {
            return $"api/v1/ChartKpiDashboard/getChartKpiDashboardDetail?{encounterNoteFilter}&complianceAccuracy={complianceAccuracy}";
        }

    }
}
