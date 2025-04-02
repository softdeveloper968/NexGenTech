using System.Threading;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface INightlyJobService
    {
        // Task<bool> DoSendMailExpiringAuthorization(int expiringDays = 30);
        // Task<bool> DoSendMailCurrentAuthorizations();
        
        Task<bool> DoUserAlertsScheduledCleanup();
        Task<bool> DoQueueUnresolvedClaimStatusBatches(int maxDaysOld = 180);
        //Task<bool> DoKickOffUiPathChargeEntryProcesses();
        Task<bool> DoSendFailedClientInsuranceRpaConfigurationsEmail();
        Task<bool> DoSendDailyClaimStatusReport();
        Task<bool> DoUpdateDemoDataAuditDates(CancellationToken cancellationToken);
        Task<bool> DoResetRpaConfigurationCurrentDayClaimCount(CancellationToken cancellationToken);
        Task<bool> DoSendYesterdayDenialClaimsToEmployees();
        Task<bool> DoSendUncheckedClaimsEmail();
        Task<bool> DoSendDaysWaitLapsedClaimsEmail();
        Task<bool> DoSendCheckCompletedYesterdayEligibilityEmailReport();
        Task<bool> DoSendScheduledTomorrowEligibilityEmailReport();
        Task<bool> DoSendScheduledTodayEligibilityEmailReport();
        Task<bool> DoSendApprovedForMoreThanSixDaysClaimsEmail();
        Task<bool> DoSendUnknownClaimsEmail(); //EN-37
         Task<bool> AddUpdateSystemDefaultReportForEmployeeClient();//EN-111
        Task<bool> CheckStatusPercentageThreshold(); //EN-131
		//Task<bool> ProcessUhcApiClaimStatus();
        Task<bool> RetrieveFailedJobs(); //EN-359

        Task<bool> DoSendInProcessClaimsReport();
        Task<bool> UpdateClaimStatusExceptionReasonCategory(); // EN-544
        Task<bool> FillMonthCashCollection(); //EN-668
        Task<bool> FillMonthlyDenialData(); //EN-668
        Task<bool> FillGetMonthlyReceivablesData(); //EN-668
        Task<bool> FillGetMonthlyARData(); //EN-668

        Task<bool> GetMonthlyDaysInARorAllTenantsAsync(); //EN-735

        Task<bool> UpdateOutstandingBalancesForAllTenantsAsync();

    }
}