using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using Microsoft.Data.SqlClient;
using System.Data;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Infrastructure.Common;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class ClaimStatusQueryService
    {
        public async Task<ClaimStatusDashboardResponse> GetInitialClaimsStatusTotalsAsync(IClaimStatusDashboardInitialQuery filters)
        {
            ClaimStatusDashboardResponse claimStatusDashboardResponse = new()
            {
                ClaimStatusTransactionTotals = new List<ClaimStatusTotal>(),
                ClaimStatusUploadedTotals = new List<ClaimStatusTotal>(),
                ClaimStatusInProcessTotals = new List<ClaimStatusTotal>(),
                DenialReasonTotals = new List<ClaimStatusTotal>(),
            };

            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    //SqlCommand cmd = CreateDashboardSpCommand(StoreProcedureTitle.spGetInitialClaimStatusUploadedTotalsTask, conn, filters);
                    //var claimStatusUploadedTotalsTask = ExecuteTotalsSpCommand(cmd);

                    var cmd = CreateDashboardSpCommand(StoreProcedureTitle.spGetInitialClaimStatusVisits, conn, filters);
                    var claimStatusDashboardData = ExecuteClaimStatusVisitsSp(cmd);
                    await Task.WhenAll(claimStatusDashboardData);
                    claimStatusDashboardResponse.ClaimStatusDashboardData = claimStatusDashboardData.Result;
                }

                return claimStatusDashboardResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ClaimStatusTrendsResponse> GetInitialClaimsStatusTrendsAsync(IClaimStatusDashboardInitialQuery filters)
        {
            var claimStatusTrendsResponse = new ClaimStatusTrendsResponse()
            {
                ClaimStatusTrendsChargeTotals = new List<ClaimStatusTrendTotal>(),
                ClaimStatusTrendsTotals = new List<ClaimStatusTrendTotal>()
            };

            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    //Uploaded Charge Trends
                    var cmd = CreateDashboardTrendsSpCommand(StoreProcedureTitle.spGetClaimStatusUploadedChargeTrendsByDay, conn, filters);
                    var claimStatusTrendsUploadedTotalsTask = ExecuteTrendsTotalsSpCommand(cmd);

                    //Trend Totals
                    cmd = CreateDashboardTrendsSpCommand(StoreProcedureTitle.spGetClaimStatusTrendsByDay, conn, filters);
                    var claimStatusTrendsTotalsTask = ExecuteTrendsTotalsSpCommand(cmd);

                    await Task.WhenAll(claimStatusTrendsUploadedTotalsTask, claimStatusTrendsTotalsTask);

                    claimStatusTrendsResponse.ClaimStatusTrendsChargeTotals = claimStatusTrendsUploadedTotalsTask.Result;
                    claimStatusTrendsResponse.ClaimStatusTrendsTotals = claimStatusTrendsTotalsTask.Result;
                }

                return claimStatusTrendsResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<ExportQueryResponse>> GetInitialClaimStatusDetailsAsync(IInitialClaimStatusDashboardDetailsQuery filters, int clientId, string connString)
        {
            List<ExportQueryResponse> claimStatusDetails = new();

            try
            {
                //filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName(filters.FlattenedLineItemStatus);

                await using (SqlConnection conn = new SqlConnection(connString))
                {
                    await conn.OpenAsync();
                    //SqlCommand cmd = CreateDashboardSpCommand(StoreProcedureTitle.spGetInitialClaimStatusDetails, conn, filters);
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spDynamicExportDashboardQuery, conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId,dashboardType: ExportStoredProcedureColumnHelper.DashBoardType);

                    claimStatusDetails = await ExecuteClaimDynamicExportSpCommand(cmd);

                }

                return claimStatusDetails;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<ExportQueryResponse>> GetInitialClaimStatusDenialDetailsAsync(IInitialClaimStatusDashboardDetailsQuery filters, int clientId, string connString)
        {
            List<ExportQueryResponse> claimStatusDetails = new();

            SqlConnection conn = new SqlConnection(connString);
            try
            {

                filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName("Denied");

                await using (conn)
                {
                    await conn.OpenAsync();
                    //SqlCommand cmd = CreateDynamicSpCommand(StoreProcedureTitle.spGetInitialClaimStatusDetails, conn, filters);
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spDynamicExportDashboardQuery, conn, clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId, dashboardType: ExportStoredProcedureColumnHelper.DashBoardType);

                    //var data_1 = await ExecuteDashboardDynamicExportSpCommand(cmd);
                    claimStatusDetails = await ExecuteClaimDynamicExportSpCommand(cmd);

                    //SqlCommand cmd_ = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetInitialClaimStatusDetails, conn, _clientId, filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId);
                    //claimStatusDetails = await ExecuteInitialClaimStatusDetailsSpCommand(cmd_);

                }

                return claimStatusDetails;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
            }
        }

        private SqlCommand CreateDashboardTrendsSpCommand(string spName, SqlConnection conn, IClaimStatusDashboardInitialQuery filters)
        {
            SqlCommand cmd = new SqlCommand(spName, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, _clientId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, filters.CommaDelimitedLineItemStatusIds ?? String.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ProcedureCodes ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, filters.DateOfServiceFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, filters.DateOfServiceTo);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, filters.ClaimBilledFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, filters.ClaimBilledTo);

            return cmd;
        }

        private SqlCommand CreateDashboardSpCommand(string spName, SqlConnection conn, IClaimStatusDashboardInitialQuery filters)
        {
            SqlCommand cmd = new SqlCommand(spName, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, _clientId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, filters.CommaDelimitedLineItemStatusIds ?? String.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ProcedureCodes ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedFrom, filters.ReceivedFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedTo, filters.ReceivedTo);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, filters.DateOfServiceFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, filters.DateOfServiceTo);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateFrom, filters.TransactionDateFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateTo, filters.TransactionDateTo);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, filters.ClaimBilledFrom);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, filters.ClaimBilledTo);
            cmd.Parameters.AddWithValue(StoredProcedureColumnsHelper.PatientId, filters.PatientId);

            return cmd;
        }

        //TODO : Need to update sp AA-178
        private async Task<List<ClaimStatusDashboardDetailsResponse>> ExecuteInitialClaimStatusDetailsSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ClaimStatusDashboardDetailsResponse> claimStatusDetails = new List<ClaimStatusDashboardDetailsResponse>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var claimDetail = new ClaimStatusDashboardDetailsResponse()
                        {
                            PatientLastName = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PatientLastName) ? reader[StoredProcedureColumnsHelper.PatientLastName] as string : default(string),
                            PatientFirstName = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PatientFirstName) ? reader[StoredProcedureColumnsHelper.PatientFirstName] as string : default(string),
                            ClientProviderName = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ProviderName) ? reader[StoredProcedureColumnsHelper.ProviderName] as string : default(string),
                            DateOfBirth = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.DateOfBirth)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.DateOfBirth]),
                            PolicyNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PolicyNumber) ? reader[StoredProcedureColumnsHelper.PolicyNumber] as string : default(string),
                            ServiceType = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ServiceType) ? reader[StoredProcedureColumnsHelper.ServiceType] as string : default(string),
                            PayerName = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PayerName) ? reader[StoredProcedureColumnsHelper.PayerName] as string : default(string),
                            OfficeClaimNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.OfficeClaimNumber) ? reader[StoredProcedureColumnsHelper.OfficeClaimNumber] as string : default(string),
                            ProcedureCode = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string : default(string),
                            //  Quantity = reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.Quantity)),
                            DateOfServiceFrom = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.DateOfServiceFrom)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.DateOfServiceFrom]),
                            DateOfServiceTo = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.DateOfServiceTo)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.DateOfServiceTo]),
                            ClaimBilledOn = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClaimBilledOn)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.ClaimBilledOn]),
                            BilledAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.BilledAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.BilledAmount],
                            PayerClaimNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PayerClaimNumber) ? reader[StoredProcedureColumnsHelper.PayerClaimNumber] as string : default(string),
                            PayerLineItemControlNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PayerLineItemControlNumber) ? reader[StoredProcedureColumnsHelper.PayerLineItemControlNumber] as string : default(string),
                            ClaimLineItemStatus = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClaimLineItemStatus) ? reader[StoredProcedureColumnsHelper.ClaimLineItemStatus] as string : default(string),
                            ClaimLineItemStatusValue = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClaimLineItemStatusValue) ? reader[StoredProcedureColumnsHelper.ClaimLineItemStatusValue] as string : default(string),
                            ExceptionReasonCategory = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ExceptionReasonCategory) ? reader[StoredProcedureColumnsHelper.ExceptionReasonCategory] as string : default(string),
                            ExceptionReason = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ExceptionReason) ? reader[StoredProcedureColumnsHelper.ExceptionReason] as string : default(string),
                            ExceptionRemark = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ExceptionRemark) ? reader[StoredProcedureColumnsHelper.ExceptionRemark] as string : default(string),
                            ReasonCode = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ReasonCode) ? reader[StoredProcedureColumnsHelper.ReasonCode] as string : default(string),
                            TotalAllowedAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.AllowedAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmount],
                            NonAllowedAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.NonAllowedAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.NonAllowedAmount],
                            LineItemPaidAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.LineItemPaidAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.LineItemPaidAmount],
                            CheckPaidAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.CheckPaidAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CheckPaidAmount],
                            CheckDate = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.CheckDate)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.CheckDate]),
                            CheckNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.CheckNumber) ? reader[StoredProcedureColumnsHelper.CheckNumber] as string : default(string),
                            ReasonDescription = reader[StoredProcedureColumnsHelper.ReasonDescription] as string,
                            RemarkCode = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.RemarkCode) ? reader[StoredProcedureColumnsHelper.RemarkCode] as string : default(string),
                            RemarkDescription = reader[StoredProcedureColumnsHelper.RemarkDescription] as string,
                            CoinsuranceAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.CoinsuranceAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CoinsuranceAmount],
                            CopayAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.CopayAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CopayAmount],
                            DeductibleAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.DeductibleAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.DeductibleAmount],
                            CobAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.CobAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.CobAmount],
                            PenalityAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PenalityAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.PenalityAmount],
                            EligibilityStatus = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.EligibilityStatus) ? reader[StoredProcedureColumnsHelper.EligibilityStatus] as string : default(string),
                            EligibilityInsurance = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.EligibilityInsurance) ? reader[StoredProcedureColumnsHelper.EligibilityInsurance] as string : default(string),
                            EligibilityPolicyNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.EligibilityPolicyNumber) ? reader[StoredProcedureColumnsHelper.EligibilityPolicyNumber] as string : default(string),
                            EligibilityFromDate = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.EligibilityFromDate)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.EligibilityFromDate]),
                            VerifiedMemberId = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.VerifiedMemberId) ? reader[StoredProcedureColumnsHelper.VerifiedMemberId] as string : default(string),
                            CobLastVerified = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.CobLastVerified)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.CobLastVerified]),
                            LastActiveEligibleDateRange = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.LastActiveEligibleDateRange) ? reader[StoredProcedureColumnsHelper.LastActiveEligibleDateRange] as string : default(string),
                            PrimaryPayer = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PrimaryPayer) ? reader[StoredProcedureColumnsHelper.PrimaryPayer] as string : default(string),
                            PrimaryPolicyNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PrimaryPolicyNumber) ? reader[StoredProcedureColumnsHelper.PrimaryPolicyNumber] as string : default(string),
                            BatchNumber = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.BatchNumber) ? reader[StoredProcedureColumnsHelper.BatchNumber] as string : default(string),
                            AitClaimReceivedDate = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.AitClaimReceivedDate)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.AitClaimReceivedDate]).ToString(ClaimFiltersHelpers._dateFormat_02),
                            AitClaimReceivedTime = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.AitClaimReceivedDate)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.AitClaimReceivedDate]).ToString(ClaimFiltersHelpers._timeFormat),
                            TransactionDate = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.TransactionDate)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.TransactionDate]).ToString(ClaimFiltersHelpers._dateFormat_02),
                            TransactionTime = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.TransactionDate)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.TransactionDate]).ToString(ClaimFiltersHelpers._timeFormat),
                            PartA_EligibilityFrom = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartA_EligibilityFrom)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartA_EligibilityFrom]),
                            PartA_EligibilityTo = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartA_EligibilityTo)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartA_EligibilityTo]),
                            PartA_DeductibleFrom = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartA_DeductibleFrom)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartA_DeductibleFrom]),
                            PartA_DeductibleTo = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartA_DeductibleToDate)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartA_DeductibleToDate]),
                            PartA_RemainingDeductible = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartA_RemainingDeductible)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.PartA_RemainingDeductible],
                            PartB_EligibilityFrom = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartB_EligibilityFrom)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartB_EligibilityFrom]),
                            PartB_EligibilityTo = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartB_EligibilityTo)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartB_EligibilityTo]),
                            PartB_DeductibleFrom = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartB_DeductibleFrom)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartB_DeductibleFrom]),
                            PartB_DeductibleTo = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartB_DeductibleTo)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PartB_DeductibleTo]),
                            PartB_RemainingDeductible = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PartB_RemainingDeductible)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.PartB_RemainingDeductible],
                            OtCapYearFrom = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.OtCapYearFrom)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.OtCapYearFrom]),
                            OtCapYearTo = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.OtCapYearTo)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.OtCapYearTo]),
                            OtCapUsedAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.OtCapUsedAmount)
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.OtCapUsedAmount],
                            PtCapYearFrom = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PtCapYearFrom)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PtCapYearFrom]),
                            PtCapYearTo = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PtCapYearTo)
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.PtCapYearTo]),
                            PtCapUsedAmount = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PtCapUsedAmount)
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.PtCapUsedAmount],
                            ClientLocationName = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationName) ? reader[StoredProcedureColumnsHelper.ClientLocationName] as string : default(string),
                            ClientLocationNpi = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationNpi) ? reader[StoredProcedureColumnsHelper.ClientLocationNpi] as string : default(string),
                            PaymentType = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PaymentType)
                                ? null
                                : reader[StoredProcedureColumnsHelper.PaymentType].ToString(), //AA-324
                                                                                               // LastHistoryCreatedOn = reader[StoredProcedureColumnsHelper.Last_History_Created_On] == System.DBNull.Value
                                                                                               //? null
                                                                                               //                                                                        : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.Last_History_Created_On]), //EN-127
                            ClaimStatusBatchClaimId = reader[StoredProcedureColumnsHelper.ClaimStatusBatchClaimId] == DBNull.Value ? default(int?) : (int?)reader[StoredProcedureColumnsHelper.ClaimStatusBatchClaimId], //EN-127
                        };

                        claimStatusDetails.Add(claimDetail);
                    }

                    reader.Close();
                }

                return claimStatusDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<List<ExportQueryResponse>> ExecuteInitialInProcessDetailsSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ExportQueryResponse> claimStatusInProcessDetails = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var inProcessClaimDetail = new ExportQueryResponse()
                        {
                            PatientLastName = reader[StoredProcedureColumnsHelper.PatientLastName] as string,
                            PatientFirstName = reader[StoredProcedureColumnsHelper.PatientFirstName] as string,
                            ClientProviderName = reader[StoredProcedureColumnsHelper.ProviderName] as string,
                            DateOfBirthString = reader[StoredProcedureColumnsHelper.DateOfBirth] == System.DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.DateOfBirth]).ToString(ClaimFiltersHelpers._dateFormat_02),
                            PolicyNumber = reader[StoredProcedureColumnsHelper.PolicyNumber] as string,
                            ServiceType = reader[StoredProcedureColumnsHelper.ServiceType] as string,
                            PayerName = reader[StoredProcedureColumnsHelper.PayerName] as string,
                            OfficeClaimNumber = reader[StoredProcedureColumnsHelper.OfficeClaimNumber] as string,
                            ProcedureCode = reader[StoredProcedureColumnsHelper.ProcedureCode] as string,
                            //Quantity = reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.Quantity)),
                            DateOfServiceFromString = reader[StoredProcedureColumnsHelper.DateOfServiceFrom] == System.DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.DateOfServiceFrom]).ToString(ClaimFiltersHelpers._dateFormat_02),
                            DateOfServiceToString = reader[StoredProcedureColumnsHelper.DateOfServiceTo] == System.DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.DateOfServiceTo]).ToString(ClaimFiltersHelpers._dateFormat_02),
                            ClaimBilledOnString = reader[StoredProcedureColumnsHelper.ClaimBilledOn] == System.DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.ClaimBilledOn]).ToString(ClaimFiltersHelpers._dateFormat_02),
                            BilledAmount = reader[StoredProcedureColumnsHelper.BilledAmount] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.BilledAmount],
                            BatchNumber = reader[StoredProcedureColumnsHelper.BatchNumber] as string,
                            AitClaimReceivedDate = reader[StoredProcedureColumnsHelper.CreatedOn] == System.DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.CreatedOn]).ToString(ClaimFiltersHelpers._dateFormat_02),
                            AitClaimReceivedTime = reader[StoredProcedureColumnsHelper.CreatedOn] == System.DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader[StoredProcedureColumnsHelper.CreatedOn]).ToLocalTime().ToShortTimeString(),
                            ClientLocationName = reader[StoredProcedureColumnsHelper.ClientLocationName] as string,
                            ClientLocationNpi = reader[StoredProcedureColumnsHelper.ClientLocationNpi] as string,
                            PaymentType = reader.IsDBNull(reader.GetOrdinal(StoredProcedureColumnsHelper.PaymentType))
                                ? null
                                : reader[StoredProcedureColumnsHelper.PaymentType].ToString(), //AA-324
                        };

                        claimStatusInProcessDetails.Add(inProcessClaimDetail);
                    }

                    reader.Close();
                }

                return claimStatusInProcessDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<List<ClaimStatusTrendTotal>> ExecuteTrendsTotalsSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ClaimStatusTrendTotal> claimStatusTrendTotals = new List<ClaimStatusTrendTotal>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var claimStatusTrendTotal = new ClaimStatusTrendTotal();
                        claimStatusTrendTotal.ClaimBilledOn = Convert.ToDateTime(reader[StoredProcedureColumnsHelper.ClaimBilledOn]);
                        claimStatusTrendTotal.DateOfServiceFrom = Convert.ToDateTime(reader[StoredProcedureColumnsHelper.DateOfServiceFrom]);
                        claimStatusTrendTotal.ProcedureCode = reader[StoredProcedureColumnsHelper.ProcedureCode] as string;
                        claimStatusTrendTotal.ServiceType = reader[StoredProcedureColumnsHelper.ServiceType] as string;
                        claimStatusTrendTotal.ServiceTypeId = reader[StoredProcedureColumnsHelper.ServiceTypeId] == DBNull.Value ? default(int) : reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.ServiceTypeId));
                        claimStatusTrendTotal.ClientInsuranceLookupName = reader[StoredProcedureColumnsHelper.ClientInsuranceLookupName] as string;
                        claimStatusTrendTotal.ClientInsuranceId = reader[StoredProcedureColumnsHelper.ClientInsuranceId] == DBNull.Value ? default(int) : reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.ClientInsuranceId));
                        claimStatusTrendTotal.ChargedSum = reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum];
                        claimStatusTrendTotal.PaidAmountSum = reader[StoredProcedureColumnsHelper.PaidAmountSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.PaidAmountSum];
                        claimStatusTrendTotal.Quantity = reader[StoredProcedureColumnsHelper.Quantity] == DBNull.Value ? default(int) : reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.Quantity));
                        claimStatusTrendTotal.ClaimLineItemStatusId = reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId] == DBNull.Value
                            ? default(ClaimLineItemStatusEnum?)
                            : (ClaimLineItemStatusEnum)reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.ClaimLineItemStatusId));
                        claimStatusTrendTotal.ExceptionReasonCategoryId = reader[StoredProcedureColumnsHelper.ExceptionReasonCategoryId] == DBNull.Value
                            ? default(ClaimStatusExceptionReasonCategoryEnum?)
                            : (ClaimStatusExceptionReasonCategoryEnum)reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.ExceptionReasonCategoryId));
                        claimStatusTrendTotal.WeekNumberBilledOn = reader[StoredProcedureColumnsHelper.WeekNumberBilledOn] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.WeekNumberBilledOn];
                        claimStatusTrendTotal.WeekYearBilledOn = reader[StoredProcedureColumnsHelper.WeekYearBilledOn] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.WeekYearBilledOn];
                        claimStatusTrendTotal.WeekNumberServiceDate = reader[StoredProcedureColumnsHelper.WeekNumberServiceDate] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.WeekNumberServiceDate];
                        claimStatusTrendTotal.WeekYearServiceDate = reader[StoredProcedureColumnsHelper.WeekYearServiceDate] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.WeekYearServiceDate];
                        claimStatusTrendTotal.FirstDateOfWeekBilledOn = Convert.ToDateTime(reader[StoredProcedureColumnsHelper.FirstDateOfWeekBilledOn]);
                        claimStatusTrendTotal.FirstDateOfWeekServiceDate = Convert.ToDateTime(reader[StoredProcedureColumnsHelper.FirstDateOfWeekServiceDate]);

                        claimStatusTrendTotals.Add(claimStatusTrendTotal);
                    }

                    reader.Close();
                }

                return claimStatusTrendTotals;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<List<ClaimStatusTotal>> ExecuteTotalsSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ClaimStatusTotal> claimStatusTotals = new List<ClaimStatusTotal>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var claimStatusUploadTotal = new ClaimStatusTotal();
                        var rows = reader.GetSchemaTable().Rows;
                        claimStatusUploadTotal.ProcedureCode = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ProcedureCode) ? reader[StoredProcedureColumnsHelper.ProcedureCode] as string : default(string);
                        claimStatusUploadTotal.ClientInsuranceName = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string : default(string);
                        claimStatusUploadTotal.AllowedAmountSum = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.AllowedAmountSum) ? (reader[StoredProcedureColumnsHelper.AllowedAmountSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.AllowedAmountSum]) : 0.0m;

                        claimStatusUploadTotal.NonAllowedAmountSum = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.NonAllowedAmountSum) ?
                            (reader[StoredProcedureColumnsHelper.NonAllowedAmountSum] == System.DBNull.Value
                                ? 0.00m
                                : (decimal)reader[StoredProcedureColumnsHelper.NonAllowedAmountSum]) : 0.0m;
                        ;
                        claimStatusUploadTotal.ChargedSum = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? (reader[StoredProcedureColumnsHelper.ChargedSum] == DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]) : 0.0m;
                        claimStatusUploadTotal.PaidAmountSum = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.PaidAmountSum) ? (reader[StoredProcedureColumnsHelper.PaidAmountSum] == System.DBNull.Value
                            ? 0.00m
                            : (decimal)reader[StoredProcedureColumnsHelper.PaidAmountSum]) : 0.0m;
                        claimStatusUploadTotal.Quantity = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ?
                            (reader[StoredProcedureColumnsHelper.Quantity] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.Quantity]) : default(int);
                        claimStatusUploadTotal.ClaimLineItemStatus = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClaimLineItemStatus) ? (reader[StoredProcedureColumnsHelper.ClaimLineItemStatus] as string) : default(string);
                        claimStatusUploadTotal.ClaimStatusExceptionReasonCategory = ClaimFiltersHelpers.HasColumn(rows, StoredProcedureColumnsHelper.ClaimStatusExceptionReasonCategory) ? (reader[StoredProcedureColumnsHelper.ClaimStatusExceptionReasonCategory] as string) : default(string);

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

        //TODO : Update sp to include provider name AA-178
        public async Task<List<ExportQueryResponse>> GetInitialInProcessDetailsAsync(IInitialClaimStatusDashboardDetailsQuery filters, int clientId, string connStr)
        {
            List<ExportQueryResponse> claimStatusInProcessDetails = new();

            SqlConnection conn = new SqlConnection(connStr);

            filters.FlattenedLineItemStatus = "In-Process";
            filters.CommaDelimitedLineItemStatusIds = ReadOnlyObjects.GetDelimitedLineItemStatusesFromFlattenedName(filters.FlattenedLineItemStatus);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    //SqlCommand cmd = CreateDashboardSpCommand(StoreProcedureTitle.spGetInitialClaimStatusInProcessDetails, conn, filters);
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetInitialClaimStatusInProcessDetails, conn, clientId, delimitedLineItemStatusIds: filters.CommaDelimitedLineItemStatusIds, filters.ClientInsuranceIds, filters.AuthTypeIds, filters.ProcedureCodes, filters.ExceptionReasonCategoryIds, filters.ClientProviderIds, filters.ClientLocationIds, filters.DateOfServiceFrom, filters.DateOfServiceTo, filters.ClaimBilledFrom, filters.ClaimBilledTo, filters.ReceivedFrom, filters.ReceivedTo, filters.TransactionDateFrom, filters.TransactionDateTo, filters.PatientId, filters.ClaimStatusBatchId, flattenedLineItemStatus: null, dashboardType: filters.DashboardType);
                    claimStatusInProcessDetails = await ExecuteInitialInProcessDetailsSpCommand(cmd);
                }

                return claimStatusInProcessDetails;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
            }
        }

    }
}