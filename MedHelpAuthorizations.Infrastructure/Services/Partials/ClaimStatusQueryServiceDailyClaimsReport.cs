using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using Microsoft.Data.SqlClient;
using System.Data;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MudBlazor;
using MedHelpAuthorizations.Application.Features.Reports.DailyClaimReports;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Infrastructure.Common;
using MedHelpAuthorizations.Shared.Interfaces.IntegratedServices;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class ClaimStatusQueryService
    {
        public async Task<ClaimStatusDashboardResponse> GetDailyClaimsStatusTotalsAsync(DailyClaimReportDetailsQuery filters, string connStr)
        {
            ClaimStatusDashboardResponse claimStatusDashboardResponse = new()
            {
                ClaimStatusTransactionTotals = new List<ClaimStatusTotal>(),
                ClaimStatusUploadedTotals = new List<ClaimStatusTotal>(),
                ClaimStatusInProcessTotals = new List<ClaimStatusTotal>(),
                DenialReasonTotals = new List<ClaimStatusTotal>(),
            };
            if (filters.ClientId == 0 || filters.ClientId == null)
            {
                filters.ClientId = _currentUserService.ClientId;
            }
            if (string.IsNullOrWhiteSpace(connStr))
            {
                connStr = _tenantInfo.ConnectionString;
            }

            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = CreateDailyClaimStatusReportSpCommand(StoreProcedureTitle.spGetClaimStatusUploadedTotals, conn, filters);
                    var claimStatusUploadedTotalsTask = ExecuteDailyClaimTotalsSpCommand(cmd);

                    cmd = CreateDailyClaimStatusReportSpCommand(StoreProcedureTitle.spGetClaimStatusTransactionTotals, conn, filters);
                    var claimStatusInitialTotalsTask = ExecuteDailyClaimTotalsSpCommand(cmd);

                    cmd = CreateDailyClaimStatusReportSpCommand(StoreProcedureTitle.spGetClaimStatusInProcessTotals, conn, filters);
                    var claimStatusInProcessTotalsTask = ExecuteDailyClaimTotalsSpCommand(cmd);

                    //Denied
                    filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName(StoreProcedureTitle.Denied);
                    cmd = CreateDailyClaimStatusReportSpCommand(StoreProcedureTitle.spGetClaimStatusTransactionTotals, conn, filters);
                    var claimStatusInitialDenialTotalsTask = ExecuteDailyClaimTotalsSpCommand(cmd);


                    await Task.WhenAll(claimStatusUploadedTotalsTask, claimStatusInitialTotalsTask, claimStatusInProcessTotalsTask, claimStatusInitialDenialTotalsTask);

                    claimStatusDashboardResponse.ClaimStatusUploadedTotals = claimStatusUploadedTotalsTask.Result;
                    claimStatusDashboardResponse.ClaimStatusTransactionTotals = claimStatusInitialTotalsTask.Result;
                    claimStatusDashboardResponse.ClaimStatusInProcessTotals = claimStatusInProcessTotalsTask.Result;
                    claimStatusDashboardResponse.DenialReasonTotals = claimStatusInitialDenialTotalsTask.Result;
                }

                return claimStatusDashboardResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private SqlCommand CreateDailyClaimStatusReportSpCommand(string spName, SqlConnection conn, DailyClaimReportDetailsQuery filters)
        {
            SqlCommand cmd = new(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            var clientId = _clientId;
            if (filters?.ClientId != null)
            {
                clientId = filters.ClientId.Value;
            }
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, clientId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, filters.CommaDelimitedLineItemStatusIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ProcedureCodes ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedFrom, filters.ReceivedFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedTo, filters.ReceivedTo);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, filters.DateOfServiceFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, filters.DateOfServiceTo);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateFrom, filters.TransactionDateFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateTo, filters.TransactionDateTo);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, filters.ClaimBilledFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, filters.ClaimBilledTo);
            ///Handle Pagination.       
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.PageNumber, filters.PageNumber);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.RowsOfPage, filters.PageSize);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.SortTypeCol, filters.SortType);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.searchString, filters.SearchText);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.filterReportBy, filters.FilterReportBy ?? string.Empty);
            return cmd;
        }
        private async Task<List<ClaimStatusTotal>> ExecuteDailyClaimTotalsSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ClaimStatusTotal> claimStatusTotals = new List<ClaimStatusTotal>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    var parameterList = ExportCommonMethod.GetSQLCommandParametersWithValues(cmd);

                    while (reader.Read())
                    {
                        var claimStatusUploadTotal = new ClaimStatusTotal();
                        var rows = reader.GetSchemaTable().Rows;
                        claimStatusUploadTotal.Quantity = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? (reader[StoredProcedureColumnsHelper.Quantity] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.Quantity]) : default(int);
                        claimStatusUploadTotal.ClaimLineItemStatus = HasColumn(rows, StoredProcedureColumnsHelper.ClaimLineItemStatus) ? (reader[StoredProcedureColumnsHelper.ClaimLineItemStatus] == DBNull.Value ? string.Empty : reader[StoredProcedureColumnsHelper.ClaimLineItemStatus] as string) : string.Empty;
                        claimStatusUploadTotal.ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? (reader[StoredProcedureColumnsHelper.ChargedSum] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]) : 0.00m;
                        //EN-435
                        claimStatusUploadTotal.AllowedAmountSum = HasColumn(rows, StoredProcedureColumnsHelper.AllowedAmountSum) ? (reader[StoredProcedureColumnsHelper.AllowedAmountSum] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmountSum]) : 0.00m;
                        claimStatusUploadTotal.FilterDailyClaimReport = HasColumn(rows, StoredProcedureColumnsHelper.filterReportByDate) ? (reader[StoredProcedureColumnsHelper.filterReportByDate] == DBNull.Value ? null : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.filterReportByDate])) : null;

                        claimStatusTotals.Add(claimStatusUploadTotal);
                    }

                    reader.Close();
                }

                return claimStatusTotals;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private List<string> GetColumnNamesFromDataRow(DataRowCollection rows)
        {
            return rows.Cast<DataRow>().Select(row => row[StoredProcedureColumnsHelper.ColumnName].ToString()).ToList();
        }
        private bool HasColumn(DataRowCollection rows, string ColumnName)
        {
            bool exist = false;
            foreach (DataRow row in rows)
            {
                if (row["ColumnName"].ToString() == ColumnName)
                    return true;
                else continue;
            }
            return exist;
        }

        public DailyClaimStatusReportResponse GetDailyClaimStatusReportResponse(DateTime day, List<ClaimStatusTotal> claimStatusUploadedTotals, List<ClaimStatusTotal> claimStatusTransactionTotals, List<ClaimStatusTotal> claimStatusInProcessTotals, List<ClaimStatusTotal> claimStatusDenialReasonTotals, bool hasLastPage = false, string claimDateTitleForLastDate = "")
        {
            var claimStatusDailyReport = new DailyClaimStatusReportResponse();

            // Extract Sum of Totals
            var uploadedSum = claimStatusUploadedTotals.Sum(x => x.Quantity);
            var chargedSumForUploadedClaims = claimStatusUploadedTotals.Sum(z => z.ChargedSum);
            bool hasUploadedClaims = uploadedSum > 0;
            var allowedAmountSum = claimStatusUploadedTotals.Sum(x => x.AllowedAmountSum);

            // Set claim date
            claimStatusDailyReport.ClaimDate = hasLastPage ? claimDateTitleForLastDate : day.ToString(ClaimFiltersHelpers._dateFormat);
            
            ///Total claim received for the day.
            claimStatusDailyReport.ClaimReceived = uploadedSum;
            
            ///Total charged Amount for these claims.
            claimStatusDailyReport.ChargeAmount = chargedSumForUploadedClaims;

            // Convert read-only collections to HashSet for optimized lookups
            var errorClaimLineItemStatuses = new HashSet<string>(ReadOnlyObjects.ErrorClaimLineItemStatuses.Select(y => y.ToString()));
            var paidApprovedClaimLineItemStatuses = new HashSet<string>(ReadOnlyObjects.PaidApprovedClaimLineItemStatuses.Select(y => y.ToString()));
            var openClaimLineItemStatuses = new HashSet<string>(ReadOnlyObjects.OpenClaimLineItemStatuses.Select(y => y.ToString()));
            var zeroPayBundledClaimLineItemStatuses = new HashSet<string>(ReadOnlyObjects.ZeroPayBundledClaimLineItemStatuses.Select(y => y.ToString()));

            // Calculate Reviewed claims and percentage
            claimStatusDailyReport.Reviewed = claimStatusTransactionTotals.Where(x => !errorClaimLineItemStatuses.Contains(x.ClaimLineItemStatus)).Sum(z => z.Quantity);
            claimStatusDailyReport.ReviewedPercentage = hasUploadedClaims ? Math.Round((claimStatusDailyReport.Reviewed / (decimal)uploadedSum) * 100, 2) : 0;

            // Calculate InProcess claims and percentage
            claimStatusDailyReport.InProcess = claimStatusInProcessTotals.Sum(x => x.Quantity);
            claimStatusDailyReport.InProcessPercentage = hasUploadedClaims ? Math.Round((claimStatusDailyReport.InProcess / (decimal)uploadedSum) * 100, 2) : 0;

            // Calculate ApprovedPaid claims and percentage
            claimStatusDailyReport.ApprovedPaid = claimStatusTransactionTotals.Where(x => paidApprovedClaimLineItemStatuses.Contains(x.ClaimLineItemStatus)).Sum(x => x.Quantity);
            claimStatusDailyReport.ApprovePaidPercentage = hasUploadedClaims ? Math.Round((claimStatusDailyReport.ApprovedPaid / (decimal)uploadedSum) * 100, 2) : 0;

            // Calculate Denied claims and percentage
            claimStatusDailyReport.Denied = claimStatusDenialReasonTotals.Sum(x => x.Quantity);
            claimStatusDailyReport.DeniedPercentage = hasUploadedClaims ? Math.Round((claimStatusDailyReport.Denied / (decimal)uploadedSum) * 100, 2) : 0;

            // Calculate NotAdjudicated claims and percentage
            claimStatusDailyReport.NotAdjudicated = claimStatusTransactionTotals.Where(x => openClaimLineItemStatuses.Contains(x.ClaimLineItemStatus)).Sum(x => x.Quantity);
            claimStatusDailyReport.NotAdjudicatedPercentage = hasUploadedClaims ? Math.Round((claimStatusDailyReport.NotAdjudicated / (decimal)uploadedSum) * 100, 2) : 0;

            // Calculate ZeroPaid claims and percentage
            claimStatusDailyReport.ZeroPaid = claimStatusTransactionTotals.Where(x => zeroPayBundledClaimLineItemStatuses.Contains(x.ClaimLineItemStatus)).Sum(x => x.Quantity);
            claimStatusDailyReport.ZeroPaidPercentage = hasUploadedClaims ? Math.Round((claimStatusDailyReport.ZeroPaid / (decimal)uploadedSum) * 100, 2) : 0;

            // Calculate AllowedAmount and percentage
            //% Allowed: charge amount ($)/allowed Amount ($)
            claimStatusDailyReport.AllowedAmount = allowedAmountSum;
            claimStatusDailyReport.AllowedAmountPercentage = allowedAmountSum > 0 ? Math.Round((chargedSumForUploadedClaims / (decimal)allowedAmountSum) * 100, 2) : 0;

            return claimStatusDailyReport;
        }


    }
}