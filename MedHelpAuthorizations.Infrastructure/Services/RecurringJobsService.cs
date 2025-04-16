using Hangfire;
using Hangfire.Server;
//using MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.DfStaging;
using MedHelpAuthorizations.Application.Interfaces.Services.ExternalApis;
using MedHelpAuthorizations.Infrastructure.Services.JobsManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;



namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class RecurringJobsService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IRecurringJobManager _recurringJobs;
        private readonly ILogger<RecurringJobScheduler> _logger;
        private readonly IJobCronManager _jobCronManager;
        public RecurringJobsService(IServiceProvider services,
        IRecurringJobManager recurringJobs,
        ILogger<RecurringJobScheduler> logger, IJobCronManager jobCronManager)
        {
            _services = services;
            _recurringJobs = recurringJobs ?? throw new ArgumentNullException(nameof(recurringJobs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jobCronManager = jobCronManager ?? throw new ArgumentNullException(nameof(jobCronManager));
        }

        /// <summary>
        ///  Dafault corn value if Enviroment is Debug
        /// </summary>
        public const string DefaultNonProductionEnvironmentCron = "0 0 31 2 0";
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {

                    INightlyJobService nightlyJobService;
                    IQuarterlyJobService quarterlyJobService; //EN-91
                    IExternalApiJobService externalApiJobService;
                    IDataPipeJobService dataPipeJobService;
                    IManuallyRunJobService manualJobService;
                    try
                    {
                        nightlyJobService = scope.ServiceProvider.GetRequiredService<INightlyJobService>();
                        quarterlyJobService = scope.ServiceProvider.GetRequiredService<IQuarterlyJobService>();
                        externalApiJobService = scope.ServiceProvider.GetRequiredService<IExternalApiJobService>();
                        dataPipeJobService = scope.ServiceProvider.GetRequiredService<IDataPipeJobService>();
                        manualJobService = scope.ServiceProvider.GetRequiredService<IManuallyRunJobService>();
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                    //DataPipe Jobs
                    _recurringJobs.AddOrUpdate("DataPipe Manual Job", () => dataPipeJobService.DoTransformDfStagingRecords(), "0 0 31 2 0", new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //UserAlerts
                    _recurringJobs.AddOrUpdate("Cleanup Notifications", () => nightlyJobService.DoUserAlertsScheduledCleanup(), GetDefaultCronJobValue("0 0 * * *"));

                    // CLAIM STATUS 
                    _recurringJobs.AddOrUpdate("Nightly (11:00 PM) Re-Queueing Unresolved Claim Status Batches", () => nightlyJobService.DoQueueUnresolvedClaimStatusBatches(180), GetDefaultCronJobValue("0 03 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("Morning (05:00 AM) Re-Queueing Unresolved Claim Status Batches", () => nightlyJobService.DoQueueUnresolvedClaimStatusBatches(180), GetDefaultCronJobValue("0 10 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("Morning (05:00 AM) Failed Claim Status Batch Configurations Report", () => nightlyJobService.DoSendFailedClientInsuranceRpaConfigurationsEmail(), GetDefaultCronJobValue("0 10 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("Morning (02:00 AM) Daily Claim Status Report", () => nightlyJobService.DoSendDailyClaimStatusReport(), GetDefaultCronJobValue("0 07 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    // Claim Status APIs
                    _recurringJobs.AddOrUpdate("All ClaimStatus Api Processing Early AM", () => externalApiJobService.ProcessApiClaimStatus(), GetDefaultCronJobValue("0 07 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });
                    _recurringJobs.AddOrUpdate("Test UHC Claim Summary By ClaimNumber new", () => externalApiJobService.TestUhcServiceGetClaimSummaryByClaimNumber(), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });
                    _recurringJobs.AddOrUpdate("Test UHC Claim Summary By MemberNumber new", () => externalApiJobService.TestUhcServiceGetClaimSummaryByMember(), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });
                    _recurringJobs.AddOrUpdate("Test UHC Claim Detail By Member new", () => externalApiJobService.TestUhcServiceGetClaimDetailByMember(), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });
                    _recurringJobs.AddOrUpdate("Test UHC Claim Detail By ClaimNumber new", () => externalApiJobService.TestUhcServiceGetClaimDetailByClaimNumber(), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //RpaConfiguration Maintenance
                    _recurringJobs.AddOrUpdate("Reset ClientInsuranceRpaConfiguration.CurrentDailyClaimCount", () => nightlyJobService.DoResetRpaConfigurationCurrentDayClaimCount(stoppingToken), GetDefaultCronJobValue("0 05 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Send Unchecked Claims Email TO No-Reply.
                    _recurringJobs.AddOrUpdate("Send Unchecked Claims Email TO No-Reply", () => nightlyJobService.DoSendUncheckedClaimsEmail(), GetDefaultCronJobValue("0 07 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Send Yesterday DenialClaims To Employees.
                    _recurringJobs.AddOrUpdate("Send Yesterday DenialClaims To Employees", () => nightlyJobService.DoSendYesterdayDenialClaimsToEmployees(), GetDefaultCronJobValue("0 12 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Send 'Days Wait Lapsed' Claims Email
                    _recurringJobs.AddOrUpdate("Send 'Days Wait Lapsed' Claims Email", () => nightlyJobService.DoSendDaysWaitLapsedClaimsEmail(), GetDefaultCronJobValue("0 10 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    // DEMO DATA MAINTENANCE
                    _recurringJobs.AddOrUpdate("Morning (7:00 AM) Demo Audit Dates update", () => nightlyJobService.DoUpdateDemoDataAuditDates(stoppingToken), GetDefaultCronJobValue("0 12 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    // Manual Run - "0 0 31 2 0" Cron for manual - wont be scheduled automatically
                    //_recurringJobs.AddOrUpdate("Update ClientId References", () => manualJobService.DoAddMissingClientIdReferencesOnTables(), "0 0 31 2 0", TimeZoneInfo.Local);
                    _recurringJobs.AddOrUpdate("DoResetUnknownDaysWaitLapsedClaims", () => manualJobService.DoResetUnknownDaysWaitLapsedClaims(), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("Update Normalized Claim Numbers", () => manualJobService.UpdateNormalizedClaimsAsync(stoppingToken), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //EN-35

                    _recurringJobs.AddOrUpdate("Write Off Claims", () => manualJobService.ProcessClaimsManually(), GetDefaultCronJobValue("0 0 31 2 0"), TimeZoneInfo.Local);

                    //Send Scheduled Today Eligibility Email To Employees.
                    _recurringJobs.AddOrUpdate("Send Scheduled Today Eligibility Email to Employees", () => nightlyJobService.DoSendScheduledTodayEligibilityEmailReport(), GetDefaultCronJobValue("0 11 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Send Scheduled Tomorrow Eligibility Email To Employees.
                    _recurringJobs.AddOrUpdate("Send Scheduled Tomorrow Eligibility Email to Employees", () => nightlyJobService.DoSendScheduledTomorrowEligibilityEmailReport(), GetDefaultCronJobValue("0 13 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Send Scheduled Check Completed Yesterday Eligibility Email To Employees.
                    _recurringJobs.AddOrUpdate("Send Check Completed Yesterday Eligibility Email To Employee", () => nightlyJobService.DoSendCheckCompletedYesterdayEligibilityEmailReport(), GetDefaultCronJobValue("0 09 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Send Approved for More Than 6 Days Claims Email To Billing Managers or ARFollowUp Employees
                    _recurringJobs.AddOrUpdate("Send 'Approved for More Than 6 Days' Claims Email", () => nightlyJobService.DoSendApprovedForMoreThanSixDaysClaimsEmail(), GetDefaultCronJobValue("0 10 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Send Unknown Status Claims Email to AIT internal for review --> EN-37 
                    _recurringJobs.AddOrUpdate("Send 'Unknown Status Claims' Email", () => nightlyJobService.DoSendUnknownClaimsEmail(), GetDefaultCronJobValue("0 10 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    //Calculates average collection amount quarterly
                    _recurringJobs.AddOrUpdate("Calculate Quarterly Average Collection", () => quarterlyJobService.CalculateQuarterlyAverageCollection(stoppingToken), GetDefaultCronJobValue("0 0 1 */3 *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //EN-91
                                                                                                                                                                                                                                                                                                                         //_recurringJobs.AddOrUpdate("Calculate Quarterly Average Collection Percentage", () => quarterlyJobService.CalculateQuarterlyAverageCollection(stoppingToken), "0 0 1 */3 *", TimeZoneInfo.Local); //EN-91

                    //Update the dev demo data
                    _recurringJobs.AddOrUpdate("Update the dev demo data", () => manualJobService.DoUpdateDevDemoDataAuditDates(stoppingToken), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //EN-91

                    _recurringJobs.AddOrUpdate("Update the claims Data", () => manualJobService.UpdateClaimStatusBatchClaimsByCptCode(stoppingToken), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //EN-91

                    _recurringJobs.AddOrUpdate("Add/Update SystemDefaultReport For EmployeeClients", () => nightlyJobService.AddUpdateSystemDefaultReportForEmployeeClient(), GetDefaultCronJobValue("0 10 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });//EN-107

                    //This crashes server. Need to fix and bring back
                    _recurringJobs.AddOrUpdate("Send NotFound Unavailable Claims Report", () => nightlyJobService.CheckStatusPercentageThreshold(), GetDefaultCronJobValue("0 06 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });//EN-107
                    //_recurringJobs.AddOrUpdate("Send NotFound Unavailable Claims Report", () => nightlyJobService.CheckStatusPercentageThreshold(), "0 01 * * *", TimeZoneInfo.Local);//EN-107

                    _recurringJobs.AddOrUpdate("Process FeeSchedule Entries", () => manualJobService.ProcessFeeScheduleEntriesForAllTenants(), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //EN-91
                    _recurringJobs.AddOrUpdate("Pre Summarized ClaimsData Refresh Service", () => manualJobService.PreSummarizedClaimsDataRefreshService(stoppingToken), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //EN-91
                    _recurringJobs.AddOrUpdate("Failed Jobs", () => nightlyJobService.RetrieveFailedJobs(), GetDefaultCronJobValue("0 0 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //EN-359


                    //Send In Process Claims Email to AIT internal for review 
                    _recurringJobs.AddOrUpdate("Send 'In Process Claims' Email", () => nightlyJobService.DoSendInProcessClaimsReport(), GetDefaultCronJobValue("0 10 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("UpdateClaimStatusExceptionReasonCategory", () => nightlyJobService.UpdateClaimStatusExceptionReasonCategory(), GetDefaultCronJobValue("0 0 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("Get claims by RPA Config", () => manualJobService.GetClaimLineItemForUnPaidClaims(stoppingToken), GetDefaultCronJobValue("0 0 31 2 0"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    #region Month closing jobs

                    _recurringJobs.AddOrUpdate("FillMonthCashCollection", () => nightlyJobService.FillMonthCashCollection(), GetDefaultCronJobValue("59 23 28-31 * * [ \"$(date +\\%d -d tomorrow)\" == \"01\" ]"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("FillMonthlyDenialData", () => nightlyJobService.FillMonthlyDenialData(), GetDefaultCronJobValue("59 23 28-31 * * [ \"$(date +\\%d -d tomorrow)\" == \"01\" ]"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("FillGetMonthlyReceivablesData", () => nightlyJobService.FillGetMonthlyReceivablesData(), GetDefaultCronJobValue("59 23 28-31 * * [ \"$(date +\\%d -d tomorrow)\" == \"01\" ]"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });

                    _recurringJobs.AddOrUpdate("FillGetMonthlyARData", () => nightlyJobService.FillGetMonthlyARData(), GetDefaultCronJobValue("59 23 28-31 * * [ \"$(date +\\%d -d tomorrow)\" == \"01\" ]"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });
                    #endregion

                    _recurringJobs.AddOrUpdate("GetMonthlyDaysInAR", () => nightlyJobService.GetMonthlyDaysInARorAllTenantsAsync(), GetDefaultCronJobValue("59 23 28-31 * * [ \"$(date +\\%d -d tomorrow)\" == \"01\" ]"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed }); //En-735

                    _recurringJobs.AddOrUpdate("UpdateOutstandingBalancesForAllTenantsAsync", () => nightlyJobService.UpdateOutstandingBalancesForAllTenantsAsync(), GetDefaultCronJobValue("0 0 * * *"), new RecurringJobOptions() { TimeZone = TimeZoneInfo.Utc, MisfireHandling = MisfireHandlingMode.Relaxed });
                }
            }
            catch (Exception e)
            {
                _logger.LogError("An exception occurred while creating recurring jobs.", e);
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// Returns the appropriate cron job expression based on the current environment.
        /// In a production environment, it returns the provided cron job expression.
        /// In a non-production environment, it returns a default cron expression that
        /// </summary>
        /// <param name="cronJob">The cron job expression intended for the production environment.</param>
        /// <returns>
        /// The original cron job expression if in production; otherwise, a default cron job 
        /// expression that prevents the job from running.
        /// </returns>
        private string GetDefaultCronJobValue(string cronJob)
        {
            // Use the IsProductionEnvironment property to determine the cron expression to return
            return _jobCronManager.IsProductionEnvironment ? cronJob : DefaultNonProductionEnvironmentCron;
        }

    }
}