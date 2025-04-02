using AutoMapper;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Reports.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using self_pay_eligibility_api.Domain.Entities.Enums.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class ARAgingReportQueryService : IARAgingReportQueryService
    {
        private readonly IStringLocalizer<ARAgingReportQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITenantInfo _tenantInfo;
        private readonly IExcelService _excelService;
        private int _clientId => _currentUserService.ClientId;

        public ARAgingReportQueryService(ApplicationContext context, IDbContextFactory<ApplicationContext> contextFactory, IUnitOfWork<int> unitOfWork, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IMapper mapper, IStringLocalizer<ARAgingReportQueryService> localizer, ICurrentUserService currentUserService, IConfiguration configuration, ITenantInfo tenantInfo, IExcelService excelService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _tenantInfo = tenantInfo;
            _localizer = localizer;
            this._currentUserService = currentUserService;
            _excelService = excelService;
        }

        public async Task<List<ARAgingSummaryReportResponse>> GetARAgingReportTotalsAsync(ARAgingSummaryClaimReportDetailsQuery filters, string filterReportBy = StoreProcedureTitle.BilledOnDate)
        {
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = CreateARAgingReportSpCommand(StoreProcedureTitle.spGetARAgingSummaryReport, conn, filters, filterReportBy);
                    var claimStatusUploadedTotalsTask = ExecuteARAgingReportSpCommand(cmd);

                    await Task.WhenAll(claimStatusUploadedTotalsTask);

                    return claimStatusUploadedTotalsTask.Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<List<ExportQueryResponse>> GetARAgingReportExportDetailsAsync(ARAgingReportExportDetailsQuery request, int filterDayGroupby, string filterReportBy = StoreProcedureTitle.BilledOnDate, string connStr = null)
        {

            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = CreateARAgingExportDetailsReportSpCommand(storedProcedureName: StoreProcedureTitle.spGetARAgingReportExportDetails, connectionState: conn, requestParams: request, filterDayGroupby: filterDayGroupby, filterReportBy: filterReportBy);
                    var claimStatusUploadedTotalsTask = await ExecuteARAgingReportExportSpCommand(cmd);

                    return claimStatusUploadedTotalsTask;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<List<ExportQueryResponse>> GetARAgingReportExportSummaryAsync(ARAgingReportExportDetailsQuery request, int filterDayGroupby, string filterReportBy = StoreProcedureTitle.BilledOnDate, string connStr = null)
        {
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = CreateARAgingExportDetailsReportSpCommand(storedProcedureName: StoreProcedureTitle.spGetARAgingReportExportSummary, connectionState: conn, requestParams: request, filterDayGroupby: filterDayGroupby, filterReportBy: filterReportBy);
                    var claimStatusUploadedTotalsTask =await ExecuteARAgingReportExportSummarySpCommand(cmd);

                    return claimStatusUploadedTotalsTask;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private SqlCommand CreateARAgingExportDetailsReportSpCommand(string storedProcedureName, SqlConnection connectionState, ARAgingReportExportDetailsQuery requestParams, int filterDayGroupby, string filterReportBy)
        {
            SqlCommand cmd = new(storedProcedureName, connectionState)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, _clientId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, requestParams.ClientInsuranceIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, requestParams.ClientLocationIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, requestParams.ClientProviderIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.filterDayGroupby, filterDayGroupby);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.filterType, filterReportBy ?? StoreProcedureTitle.BilledOnDate);
            return cmd;
        }
        private SqlCommand CreateARAgingReportSpCommand(string spName, SqlConnection conn, ARAgingSummaryClaimReportDetailsQuery filters, string filterReportCol)
        {
            SqlCommand cmd = new(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, _clientId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, filters.ClientLocationIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, filters.ClientProviderIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.filterReportBy, filterReportCol ?? StoreProcedureTitle.BilledOnDate);
            return cmd;
        }
        private async Task<List<ARAgingSummaryReportResponse>> ExecuteARAgingReportSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ARAgingSummaryReportResponse> _agingReportTotalsByDayGroup = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var _agingReportTotal = new ARAgingSummaryReportResponse();
                        var rows = reader.GetSchemaTable().Rows;

                        //_agingReportTotal.Quantity = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? (reader[StoredProcedureColumnsHelper.Quantity] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.Quantity]) : default(int);
                        _agingReportTotal.InsuranceTitle = HasColumn(rows, StoredProcedureColumnsHelper.InsuranceName) ? (reader[StoredProcedureColumnsHelper.InsuranceName] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.InsuranceName]) : default(string);
                        _agingReportTotal.InsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.InsuranceId) ? (reader[StoredProcedureColumnsHelper.InsuranceId] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.InsuranceId]) : default(int);
                        _agingReportTotal.LocationId = HasColumn(rows, StoredProcedureColumnsHelper.LocationId) ? (reader[StoredProcedureColumnsHelper.LocationId] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.LocationId]) : default(int);
                        _agingReportTotal.ProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ProviderId) ? (reader[StoredProcedureColumnsHelper.ProviderId] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.ProviderId]) : default(int);
                        _agingReportTotal.LocationName = HasColumn(rows, StoredProcedureColumnsHelper.LocationName) ? (reader[StoredProcedureColumnsHelper.LocationName] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.LocationName]) : default(string);
                        _agingReportTotal.ProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ProviderName) ? (reader[StoredProcedureColumnsHelper.ProviderName] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.ProviderName]) : default(string);
                        _agingReportTotal.ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? (reader[StoredProcedureColumnsHelper.ChargedSum] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]) : 0.00m;
                        _agingReportTotal.AgeGroup_0_30 = HasColumn(rows, StoredProcedureColumnsHelper.AgeGroup_0_30) ? (reader[StoredProcedureColumnsHelper.AgeGroup_0_30] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AgeGroup_0_30]) : 0.00m;
                        _agingReportTotal.AgeGroup_31_60 = HasColumn(rows, StoredProcedureColumnsHelper.AgeGroup_31_60) ? (reader[StoredProcedureColumnsHelper.AgeGroup_31_60] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AgeGroup_31_60]) : 0.00m;
                        _agingReportTotal.AgeGroup_61_90 = HasColumn(rows, StoredProcedureColumnsHelper.AgeGroup_61_90) ? (reader[StoredProcedureColumnsHelper.AgeGroup_61_90] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AgeGroup_61_90]) : 0.00m;
                        _agingReportTotal.AgeGroup_91_120 = HasColumn(rows, StoredProcedureColumnsHelper.AgeGroup_91_120) ? (reader[StoredProcedureColumnsHelper.AgeGroup_91_120] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AgeGroup_91_120]) : 0.00m;
                        _agingReportTotal.AgeGroup_121_150 = HasColumn(rows, StoredProcedureColumnsHelper.AgeGroup_121_150) ? (reader[StoredProcedureColumnsHelper.AgeGroup_121_150] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AgeGroup_121_150]) : 0.00m;
                        _agingReportTotal.AgeGroup_151_180 = HasColumn(rows, StoredProcedureColumnsHelper.AgeGroup_151_180) ? (reader[StoredProcedureColumnsHelper.AgeGroup_151_180] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AgeGroup_151_180]) : 0.00m;
                        _agingReportTotal.AgeGroup_181_Plus = HasColumn(rows, StoredProcedureColumnsHelper.AgeGroup_Above_181) ? (reader[StoredProcedureColumnsHelper.AgeGroup_Above_181] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.AgeGroup_Above_181]) : 0.00m;

                        _agingReportTotalsByDayGroup.Add(_agingReportTotal);
                    }

                    reader.Close();
                }

                return _agingReportTotalsByDayGroup;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<List<ExportQueryResponse>> ExecuteARAgingReportExportSpCommand(SqlCommand cmd)
        {
            try
            {
                List<ExportQueryResponse> _agingReportTotalsByDayGroup = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var _agingReportTotal = new ExportQueryResponse
                        {
                            PatientLastName = HasColumn(rows, StoredProcedureColumnsHelper.Last_Name) ? (reader[StoredProcedureColumnsHelper.Last_Name] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.Last_Name]) : default(string),
                            PatientFirstName = HasColumn(rows, StoredProcedureColumnsHelper.First_Name) ? (reader[StoredProcedureColumnsHelper.First_Name] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.First_Name]) : default(string),
                            DateOfBirthString = HasColumn(rows, StoredProcedureColumnsHelper.DOB) ? (reader[StoredProcedureColumnsHelper.DOB] == DBNull.Value ? default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.DOB]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            PayerName = HasColumn(rows, StoredProcedureColumnsHelper.Insurance_Name) ? (reader[StoredProcedureColumnsHelper.Insurance_Name] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.Insurance_Name]) : default(string),
                            PolicyNumber = HasColumn(rows, StoredProcedureColumnsHelper.Policy_Number) ? (reader[StoredProcedureColumnsHelper.Policy_Number] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.Policy_Number]) : default(string),
                            ServiceType = HasColumn(rows, StoredProcedureColumnsHelper.ServiceType) ? (reader[StoredProcedureColumnsHelper.ServiceType] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.ServiceType]) : default(string),
                            DateOfServiceFromString = HasColumn(rows, StoredProcedureColumnsHelper.DOS_From) ? (reader[StoredProcedureColumnsHelper.DOS_From] == DBNull.Value ? default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.DOS_From]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            ProcedureCode = HasColumn(rows, StoredProcedureColumnsHelper.CPT_Code) ? (reader[StoredProcedureColumnsHelper.CPT_Code] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.CPT_Code]) : default(string),
                            BilledAmount = HasColumn(rows, StoredProcedureColumnsHelper.Billed_Amt) ? (reader[StoredProcedureColumnsHelper.Billed_Amt] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.Billed_Amt]) : 0.00m,
                            ClaimBilledOnString = HasColumn(rows, StoredProcedureColumnsHelper.Billed_On) ? (reader[StoredProcedureColumnsHelper.Billed_On] == DBNull.Value ? default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.Billed_On]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            ClaimLineItemStatus = HasColumn(rows, StoredProcedureColumnsHelper.Lineitem_Status) ? (reader[StoredProcedureColumnsHelper.Lineitem_Status] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.Lineitem_Status]) : default(string),
                            ExceptionReasonCategory = HasColumn(rows, StoredProcedureColumnsHelper.Exception_Category) ? (reader[StoredProcedureColumnsHelper.Exception_Category] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.Exception_Category]) : default(string),
                            ExceptionReason = HasColumn(rows, StoredProcedureColumnsHelper.Exception_Reason) ? (reader[StoredProcedureColumnsHelper.Exception_Reason] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.Exception_Reason]) : default(string),
                            ClientLocationName = HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationName) ? (reader[StoredProcedureColumnsHelper.ClientLocationName] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.ClientLocationName]) : default(string),
                            ClientLocationNpi = HasColumn(rows, StoredProcedureColumnsHelper.ClientLocationNpi) ? (reader[StoredProcedureColumnsHelper.ClientLocationNpi] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.ClientLocationNpi]) : default(string),
                            ClaimLineItemStatusId = reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId] == DBNull.Value
                            ? default(ClaimLineItemStatusEnum?)
                            : (ClaimLineItemStatusEnum)reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.ClaimLineItemStatusId))
                        };
                        _agingReportTotalsByDayGroup.Add(_agingReportTotal);
                    }

                    reader.Close();
                }

                return _agingReportTotalsByDayGroup;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<List<ExportQueryResponse>> ExecuteARAgingReportExportSummarySpCommand(SqlCommand cmd)
        {
            try
            {
                List<ExportQueryResponse> _agingReportTotalsByDayGroup = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var _agingReportTotal = new ExportQueryResponse
                        {
                            ExceptionReasonCategory = HasColumn(rows, StoredProcedureColumnsHelper.Exception_Category) ? (reader[StoredProcedureColumnsHelper.Exception_Category] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.Exception_Category]) : default(string),
                            ExceptionCount = HasColumn(rows, StoredProcedureColumnsHelper.Exception_Category_Count) ? (reader[StoredProcedureColumnsHelper.Exception_Category_Count] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.Exception_Category_Count]) : default(int),
                            SumBilledAmount = HasColumn(rows, StoredProcedureColumnsHelper.Billed_Amount) ? (reader[StoredProcedureColumnsHelper.Billed_Amount] == DBNull.Value ? default(decimal) : (decimal)reader[StoredProcedureColumnsHelper.Billed_Amount]) : default(decimal)
                        };

                        _agingReportTotalsByDayGroup.Add(_agingReportTotal);
                    }

                    reader.Close();
                }

                return _agingReportTotalsByDayGroup;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private bool HasColumn(DataRowCollection rows, string ColumnName)
        {
            bool exist = false;
            foreach (DataRow row in rows)
            {
                if (row[StoredProcedureColumnsHelper.ColumnName].ToString() == ColumnName)
                    return true;
                else continue;
            }
            return exist;
        }

        /// <summary>
        /// get ARAging chart data  AA-130
        /// </summary>
        /// <param name="summaryData"></param>
        /// <returns></returns>
        public async Task<ARAgingDataResponse> GetARAgingChartTotalsAsync(ARAgingDataQuery filters, string filterReportBy = StoreProcedureTitle.BilledOnDate)
        {
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = CreateARAgingChartTotalsSpCommand(StoreProcedureTitle.spGetARAgingTotals, conn, filters, filterReportBy);
                    var claimStatusUploadedTotalsTask = ExecuteARAgingChartTotalsSpCommand(cmd, filterReportBy);

                    await Task.WhenAll(claimStatusUploadedTotalsTask);

                    return await claimStatusUploadedTotalsTask;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="conn"></param>
        /// <param name="filters"></param>
        /// <param name="filterReportCol"></param>
        /// <returns></returns>
        private SqlCommand CreateARAgingChartTotalsSpCommand(string spName, SqlConnection conn, ARAgingDataQuery filters, string filterReportCol)
        {
            SqlCommand cmd = new(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, _clientId);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, filters.ClientInsuranceIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, filters.ClientLocationIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, filters.ClientProviderIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, filters.ExceptionReasonCategoryIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, filters.AuthTypeIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, filters.ClientProviderIds ?? string.Empty);
            cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.filterReportBy, filterReportCol ?? StoreProcedureTitle.BilledOnDate);
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private async Task<ARAgingDataResponse> ExecuteARAgingChartTotalsSpCommand(SqlCommand cmd, string filterReportBy)
        {
            try
            {
                ARAgingDataResponse _agingReportTotalsByDayGroup = new ARAgingDataResponse
                {
                    ARAgingData = new List<ARAgingData>()
                };
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var _agingReportTotal = new ARAgingData()
                        {
                            Quantity = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? (reader[StoredProcedureColumnsHelper.Quantity] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.Quantity]) : default(int),
                            ClaimLineItemStatusId = reader[StoredProcedureColumnsHelper.ClaimLineItemStatusId] == DBNull.Value
                            ? default(ClaimLineItemStatusEnum?)
                            : (ClaimLineItemStatusEnum)reader.GetInt32(reader.GetOrdinal(StoredProcedureColumnsHelper.ClaimLineItemStatusId)),
                            InsuranceTitle = HasColumn(rows, StoredProcedureColumnsHelper.InsuranceName) ? (reader[StoredProcedureColumnsHelper.InsuranceName] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.InsuranceName]) : default(string),
                            InsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.InsuranceId) ? (reader[StoredProcedureColumnsHelper.InsuranceId] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.InsuranceId]) : default(int),
                            LocationId = HasColumn(rows, StoredProcedureColumnsHelper.LocationId) ? (reader[StoredProcedureColumnsHelper.LocationId] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.LocationId]) : default(int),
                            ProviderId = HasColumn(rows, StoredProcedureColumnsHelper.ProviderId) ? (reader[StoredProcedureColumnsHelper.ProviderId] == DBNull.Value ? default(int) : (int)reader[StoredProcedureColumnsHelper.ProviderId]) : default(int),
                            LocationName = HasColumn(rows, StoredProcedureColumnsHelper.LocationName) ? (reader[StoredProcedureColumnsHelper.LocationName] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.LocationName]) : default(string),
                            ProviderName = HasColumn(rows, StoredProcedureColumnsHelper.ProviderName) ? (reader[StoredProcedureColumnsHelper.ProviderName] == DBNull.Value ? default(string) : (string)reader[StoredProcedureColumnsHelper.ProviderName]) : default(string),
                            ChargedSum = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? (reader[StoredProcedureColumnsHelper.ChargedSum] == DBNull.Value ? 0.00m : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]) : 0.00m,
                            DateOfServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.DOS_From) ? (reader[StoredProcedureColumnsHelper.DOS_From] == DBNull.Value ? default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.DOS_From]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),
                            ClaimBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.Billed_On) ? (reader[StoredProcedureColumnsHelper.Billed_On] == DBNull.Value ? default(string) : ((DateTime?)reader[StoredProcedureColumnsHelper.Billed_On]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)
                        };

                        _agingReportTotalsByDayGroup.ARAgingData.Add(_agingReportTotal);
                    }

                    reader.Close();
                }

                _agingReportTotalsByDayGroup = ClaimFiltersHelpers.CreateARAgingChartDataByAgeDayGroup(_agingReportTotalsByDayGroup, filterReportBy);

                return _agingReportTotalsByDayGroup;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        #region Export Helpers

        public ExportSummaryReportResponse GetGrandTotal(List<ExportSummaryReportResponse> summaryData)
        {
            return new ExportSummaryReportResponse()
            {
                ExceptionReasonCategory = ExportHelper.Grand_Total,
                ExceptionCount = summaryData.Sum(z => z.ExceptionCount),
                SumBilledAmount = summaryData.Sum(z => z.SumBilledAmount)
            };
        }
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelARAgingReportDetails()
        {
            return new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer[StoredProcedureColumnsHelper.Last_Name], item => item.PatientLastName },
                { _localizer[StoredProcedureColumnsHelper.First_Name], item => item.PatientFirstName },
                { _localizer[StoredProcedureColumnsHelper.DOB], item => item.DateOfBirth },
                { _localizer[StoredProcedureColumnsHelper.Insurance_Name], item => item.PayerName },
                { _localizer[StoredProcedureColumnsHelper.Policy_Number], item => item.PolicyNumber },
                { _localizer[StoredProcedureColumnsHelper.ServiceType], item => item.ServiceType },
                { _localizer[StoredProcedureColumnsHelper.DOS_From], item => item.DateOfServiceFrom },
                { _localizer[StoredProcedureColumnsHelper.CPT_Code], item => item.ProcedureCode },
                { _localizer[StoredProcedureColumnsHelper.Billed_Amt], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US")))},
                { _localizer[StoredProcedureColumnsHelper.Billed_On], item => item.ClaimBilledOn },
                { _localizer[StoredProcedureColumnsHelper.Lineitem_Status], item => item.ClaimLineItemStatus },
                { _localizer[StoredProcedureColumnsHelper.Exception_Category], item => item.ExceptionReasonCategory },
                { _localizer[StoredProcedureColumnsHelper.Exception_Reason], item => item.ExceptionReason },
                { _localizer[StoredProcedureColumnsHelper.ClientLocationName], item => item.ClientLocationName },
                { _localizer[StoredProcedureColumnsHelper.ClientLocationNpi], item => item.ClientLocationNpi },
            };
        }
        public Dictionary<string, Func<ExportQueryResponse, object>> GetExcelARAgingReportSummary()
        {
            return new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer[ExportHelper.Exception_Reason_Category], item => item.ExceptionReasonCategory },
                { _localizer[ExportHelper.Count_of_Exception_Category], item => item.ExceptionCount },
                { _localizer[ExportHelper.Sum_of_Billed_Amount], item => item.SumBilledAmount.ToString("C", new CultureInfo("en-US")) },
            };
        }
        public void GetFilterDayGroupBy(ARAgingReportDayGroupEnum aRAgingReportDayGroupEnum, out int filterDayGroupby)
        {
            filterDayGroupby = 30;
            switch (aRAgingReportDayGroupEnum)
            {
                case ARAgingReportDayGroupEnum.AgeGroup_0_30:
                    filterDayGroupby = 30;
                    break;
                case ARAgingReportDayGroupEnum.AgeGroup_31_60:
                    filterDayGroupby = 31;
                    break;
                case ARAgingReportDayGroupEnum.AgeGroup_61_90:
                    filterDayGroupby = 61;
                    break;
                case ARAgingReportDayGroupEnum.AgeGroup_91_120:
                    filterDayGroupby = 91;
                    break;
                case ARAgingReportDayGroupEnum.AgeGroup_121_150:
                    filterDayGroupby = 121;
                    break;
                case ARAgingReportDayGroupEnum.AgeGroup_151_180:
                    filterDayGroupby = 151;
                    break;
                case ARAgingReportDayGroupEnum.AgeGroup_181_plus:
                    filterDayGroupby = 181;
                    break;
                default:
                    filterDayGroupby = 30;
                    break;
            }
        }
        public List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineTwoExportReportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelSummaryReportDetails, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails)
        {
            return new List<Dictionary<string, Func<ExportQueryResponse, object>>>()
            {
                excelSummaryReportDetails.ToDictionary(summaryKey => summaryKey.Key, summary => (Func<ExportQueryResponse, object>)(exp => summary.Value((ExportQueryResponse)exp))),
                excelReportDetails.ToDictionary(detailKey => detailKey.Key, detail => (Func<ExportQueryResponse, object>)(exp => detail.Value((ExportQueryResponse)exp)))
            };
        }
        public List<ExportQueryResponse> UpdateExportDetails(List<ExportQueryResponse> summaryData)
        {
            var totalDetails = summaryData.FirstOrDefault(x => x.ExceptionReasonCategory == null);
            if (totalDetails != null)
            {
                summaryData.RemoveAll(x => x.ExceptionReasonCategory == null);

                var otherDetails = summaryData.FirstOrDefault(x => x.ExceptionReasonCategory == ClaimStatusExceptionReasonCategoryEnum.Other.GetDescription());
                if (otherDetails != null)
                {
                    var index = summaryData.IndexOf(otherDetails);
                    summaryData[index].ExceptionCount += totalDetails.ExceptionCount;
                    summaryData[index].SumBilledAmount += totalDetails.SumBilledAmount;
                }
                else
                {
                    summaryData.Add(new ExportQueryResponse { ExceptionReasonCategory = ClaimStatusExceptionReasonCategoryEnum.Other.GetDescription(), ExceptionCount = totalDetails.ExceptionCount, SumBilledAmount = totalDetails.SumBilledAmount });
                }
            }
            return summaryData;
        }

        #endregion

    }
}