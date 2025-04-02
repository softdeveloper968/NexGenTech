using MedHelpAuthorizations.Domain.Entities.Enums;
using MudBlazor;
using System;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class DailyMonthlyReportEndpoints
    {
        public static string GetDailyClaimReportByCriteria(DateTime? startDate, DateTime? endDate, DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? dateOfServiceFrom, DateTime? dateOfServiceTo, DateTime? transactionDateFrom, DateTime? transactionDateTo, DateTime? claimBilledFrom, DateTime? claimBilledTo, int clientInsuranceId, int authTypeId, int claimStatusBatchId, ClaimStatusExceptionReasonCategoryEnum? exceptionReasonCategory, string procedureCode, int pageNumber, int pageSize, string sortType, string searchText)
        {
            return $"api/v1/tenant/Reports/GetDailyClaimReportByCriteria?startDate={startDate}&endDate={endDate}&createdOnFrom=" +
                $"{createdOnFrom}&createdOnTo={createdOnTo}&dateOfServiceFrom={dateOfServiceFrom}" +
                $"&dateOfServiceTo={dateOfServiceTo}&transactionDateFrom={transactionDateFrom}" +
                $"&transactionDateTo={transactionDateTo}&claimBilledFrom={claimBilledFrom}" +
                $"&claimBilledTo={claimBilledTo}&clientInsuranceId={clientInsuranceId}" +
                $"&authTypeId={authTypeId}&claimStatusBatchId={claimStatusBatchId}" +
                $"&exceptionReasonCategory={exceptionReasonCategory}&procedureCode={procedureCode}" +
                $"&pageNumber={pageNumber}&pageSize={pageSize}&sortType={sortType}&searchText={searchText}";
        }
        public static string GetDailyClaimReportByCriteria_NEW = "api/v1/tenant/Reports/GetDailyClaimReportByCriteria";
       
    }
}
