using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.ChartReviewCompliance
{
    internal class ChartReviewComplianceManager : IChartReviewComplianceManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ChartReviewComplianceManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
    }
}
