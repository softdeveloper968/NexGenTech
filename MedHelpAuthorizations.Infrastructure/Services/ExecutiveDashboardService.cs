using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.Executive;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ExecutiveDashboardService : IExecutiveDashboardService
    {
        private readonly ITenantInfo _tenantInfo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public ExecutiveDashboardService(ITenantInfo tenantInfo, ICurrentUserService currentUserService, IUserService userService)
        {
            _tenantInfo = tenantInfo;
            _currentUserService = currentUserService;
            _userService = userService;

        }

        #region Executive dashboard current month charges data 
        public async Task<List<ExecutiveSummary>> GetExecutiveCurrentMonthDataAsync(IExecutiveDashboardQueryBase query)
        {
            List<ExecutiveSummary> response = new List<ExecutiveSummary>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetExecutiveCurrentMonthCharges, conn, query);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var claimsSummaryTask = ExecuteCurrentMonthChargesSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(claimsSummaryTask);
                    response = claimsSummaryTask.Result;

                }
                return response;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public SqlCommand CreateMonthlySummarySpCommand(string spName, SqlConnection conn, IExecutiveDashboardQueryBase query)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.

                return cmd;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ExecutiveSummary>> ExecuteCurrentMonthChargesSp(SqlCommand cmd)
        {
            try
            {
                List<ExecutiveSummary> currentMonthCharges = new List<ExecutiveSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new ExecutiveSummary()
                        {
                            LocationId = GetIntValue(reader, rows, ExecutiveDashboardConstants.ClientLocationId),
                            LocationName = GetStringValue(reader, rows, ExecutiveDashboardConstants.ClientLocationName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Visit = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Charges = GetDecimalValue(reader, rows, CorporateDashboardConstants.Charges),
                            AvgCharges = GetDecimalValue(reader, rows, CorporateDashboardConstants.AvgCharges),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),
                        };
                        currentMonthCharges.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentMonthCharges ?? new List<ExecutiveSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Executive dashboard current month denials data

        public async Task<List<ExecutiveSummary>> GetCurrentMonthDenialTotalsAsync(IExecutiveDashboardQueryBase query)
        {
            List<ExecutiveSummary> response = new List<ExecutiveSummary>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetExecutiveCurrentMonthDenials, conn, query);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var claimsSummaryTask = ExecuteCurrentMonthDenialsSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(claimsSummaryTask);
                    response = claimsSummaryTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ExecutiveSummary>> ExecuteCurrentMonthDenialsSp(SqlCommand cmd)
        {
            try
            {
                List<ExecutiveSummary> currentMonthCharges = new List<ExecutiveSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new ExecutiveSummary()
                        {
                            LocationId = GetIntValue(reader, rows, ExecutiveDashboardConstants.ClientLocationId),
                            LocationName = GetStringValue(reader, rows, ExecutiveDashboardConstants.ClientLocationName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Visit = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Denials = GetDecimalValue(reader, rows, CorporateDashboardConstants.Denials),
                            AvgDenials = GetDecimalValue(reader, rows, CorporateDashboardConstants.AvgDenial),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),

                        };
                        currentMonthCharges.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentMonthCharges ?? new List<ExecutiveSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Executive dashbaord current month payments data
        public async Task<List<ExecutiveSummary>> GetCurrentMonthPaymentTotalsAsync(IExecutiveDashboardQueryBase query)
        {
            List<ExecutiveSummary> response = new List<ExecutiveSummary>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetExecutiveCurrentMonthPayments, conn, query);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var claimsSummaryTask = ExecuteCurrentMonthPaymentsSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(claimsSummaryTask);
                    response = claimsSummaryTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ExecutiveSummary>> ExecuteCurrentMonthPaymentsSp(SqlCommand cmd)
        {
            try
            {
                List<ExecutiveSummary> currentMonthCharges = new List<ExecutiveSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new ExecutiveSummary()
                        {
                            LocationId = GetIntValue(reader, rows, ExecutiveDashboardConstants.ClientLocationId),
                            LocationName = GetStringValue(reader, rows, ExecutiveDashboardConstants.ClientLocationName),
                            Visit = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Payments = GetDecimalValue(reader, rows, CorporateDashboardConstants.Payments),
                            AvgAllowedAmt = GetDecimalValue(reader, rows, CorporateDashboardConstants.AvgAllowedAmt),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),

                        };
                        currentMonthCharges.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentMonthCharges ?? new List<ExecutiveSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Current AR over % 90 Days
        public async Task<List<CurrentAROverPercentageNintyDaysLocation>> GetCurrentAROverPercentageNintyDaysLocationAsync(IExecutiveDashboardQueryBase query)
        {
            List<CurrentAROverPercentageNintyDaysLocation> response = new List<CurrentAROverPercentageNintyDaysLocation>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetCurrentAROverPercentageNintyDaysLocation, conn, query);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var currentAROverPercentage90Days = ExecuteCurrentAROverPercentageNintyDaysLocationSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(currentAROverPercentage90Days);
                    response = currentAROverPercentage90Days.Result;

                }
                return response;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<List<CurrentAROverPercentageNintyDaysLocation>> ExecuteCurrentAROverPercentageNintyDaysLocationSp(SqlCommand cmd)
        {
            try
            {
                List<CurrentAROverPercentageNintyDaysLocation> currentAROverPercentage90Days = new List<CurrentAROverPercentageNintyDaysLocation>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new CurrentAROverPercentageNintyDaysLocation()
                        {
                            LocationId = GetIntValue(reader, rows, ExecutiveDashboardConstants.ClientLocationId),
                            LocationName = GetStringValue(reader, rows, ExecutiveDashboardConstants.ClientLocationName),
                            Claims = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Charges = GetDecimalValue(reader, rows, CorporateDashboardConstants.Charges),
                            ARTotals = GetDecimalValue(reader, rows, CorporateDashboardConstants.ARTotals),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),
                        };
                        currentAROverPercentage90Days.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentAROverPercentage90Days ?? new List<CurrentAROverPercentageNintyDaysLocation>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region current AR Over percentage 90 days Payer
        public async Task<List<CurrentAROverPercentageNintyDaysPayer>> GetCurrentAROverPercentageNintyDaysPayerAsync(GetCurrentAROverNintyDaysPayerByLocationQuery query)
        {
            List<CurrentAROverPercentageNintyDaysPayer> response = new List<CurrentAROverPercentageNintyDaysPayer>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateCurrentARSpCommand(StoreProcedureTitle.spCalculateMonthlyDaysInARForClientInsurance, conn, query);
                    //cmd.Parameters.AddWithValue("@ClientId", query.ClientId);
                    //cmd.Parameters.AddWithValue("@ClientLocationId", query.LocationId);

                    //execute the sp
                    var currentAROverPercentage90Days = ExecuteCurrentAROverPercentageNintyDaysPayerSp(cmd);
                    

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(currentAROverPercentage90Days);
                    response = currentAROverPercentage90Days.Result;

                }
                return response;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public SqlCommand CreateCurrentARSpCommand(string spName, SqlConnection conn, GetCurrentAROverNintyDaysPayerByLocationQuery query)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.
                cmd.Parameters.AddWithValue("@ClientId", query.ClientId);
                cmd.Parameters.AddWithValue("@ClientLocationId", query.LocationId);

                return cmd;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CurrentAROverPercentageNintyDaysPayer>> ExecuteCurrentAROverPercentageNintyDaysPayerSp(SqlCommand cmd)
        {
            try
            {
                List<CurrentAROverPercentageNintyDaysPayer> currentAROverPercentage90Days = new List<CurrentAROverPercentageNintyDaysPayer>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new CurrentAROverPercentageNintyDaysPayer()
                        {
                            PayerId = GetIntValue(reader, rows, CorporateDashboardConstants.PayerId),
                            PayerName = GetStringValue(reader, rows, CorporateDashboardConstants.PayerName),
                            Claims = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Charges = GetDecimalValue(reader, rows, CorporateDashboardConstants.Charges),
                            //PercentageAR = GetDecimalValue(reader, rows, CorporateDashboardConstants.PercentageAR),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),

                        };
                        currentAROverPercentage90Days.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentAROverPercentage90Days ?? new List<CurrentAROverPercentageNintyDaysPayer>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Clean Claim Rate
        public async Task<List<ExecutiveClaimRate>> GetCleanClaimRateAsync(GetExecutiveCleanClaimRateQuery query)
        {
            List<ExecutiveClaimRate> response = new List<ExecutiveClaimRate>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateClaimRateSpCommand(StoreProcedureTitle.spGetExecutiveClaimRate, conn, (int)ClaimStatusTypeEnum.PaidClaimStatusType);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var monthlyDaysInAR = ExecuteCleanClaimRateSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(monthlyDaysInAR);
                    response = monthlyDaysInAR.Result;

                }
                return response;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public SqlCommand CreateClaimRateSpCommand(string spName, SqlConnection conn, int claimStatusType)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.
                cmd.Parameters.AddWithValue(CorporateDashboardConstants.ClaimStatusTypeId, claimStatusType);

                return cmd;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ExecutiveClaimRate>> ExecuteCleanClaimRateSp(SqlCommand cmd)
        {
            try
            {
                List<ExecutiveClaimRate> cleanClaimRates = new List<ExecutiveClaimRate>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var cleanClaimRate = new ExecutiveClaimRate()
                        {
                            LocationId = GetIntValue(reader, rows, ExecutiveDashboardConstants.ClientLocationId),
                            LocationName = GetStringValue(reader, rows, ExecutiveDashboardConstants.ClientLocationName),
                            Visits = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            OnInitialReview = GetIntValue(reader, rows, CorporateDashboardConstants.PaidOnInitialReviewCol),
                            ClaimPercentageRate = GetDecimalValue(reader, rows, CorporateDashboardConstants.CleanClaimPercentageRateCol)
                        };
                        cleanClaimRates.Add(cleanClaimRate);
                    }
                    reader.Close();
                }
                return cleanClaimRates ?? new List<ExecutiveClaimRate>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Denial claim rate
        public async Task<List<ExecutiveClaimRate>> GetDenialClaimRateAsync(GetExecutiveDenialRateQuery query)
        {
            List<ExecutiveClaimRate> response = new List<ExecutiveClaimRate>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateClaimRateSpCommand(StoreProcedureTitle.spGetExecutiveClaimRate, conn, (int)ClaimStatusTypeEnum.DeniedClaimStatusType);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var denialClaimRates = ExecuteDenialClaimRateSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(denialClaimRates);
                    response = denialClaimRates.Result;

                }
                return response;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ExecutiveClaimRate>> ExecuteDenialClaimRateSp(SqlCommand cmd)
        {
            try
            {
                List<ExecutiveClaimRate> denialClaimRates = new List<ExecutiveClaimRate>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var denialClaimRate = new ExecutiveClaimRate()
                        {
                            LocationId = GetIntValue(reader, rows, ExecutiveDashboardConstants.ClientLocationId),
                            LocationName = GetStringValue(reader, rows, ExecutiveDashboardConstants.ClientLocationName),
                            Visits = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            OnInitialReview = GetIntValue(reader, rows, CorporateDashboardConstants.PaidOnInitialReviewCol),
                            ClaimPercentageRate = GetDecimalValue(reader, rows, CorporateDashboardConstants.CleanClaimPercentageRateCol)
                        };
                        denialClaimRates.Add(denialClaimRate);
                    }
                    reader.Close();
                }
                return denialClaimRates ?? new List<ExecutiveClaimRate>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Monthly days in AR
        public async Task<MonthlyDaysInAR> GetMonthlyDaysInARAsync(GetExecutiveMonthlyDaysInARQuery query)
        {
            MonthlyDaysInAR response = new MonthlyDaysInAR();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spCalculateExecutiveMonthlyDaysInAR, conn, query);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var monthlyDaysInAR = ExecuteMonthlyDaysInARSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(monthlyDaysInAR);
                    response = monthlyDaysInAR.Result;

                }
                return response;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<MonthlyDaysInAR> ExecuteMonthlyDaysInARSp(SqlCommand cmd)
        {
            try
            {
                MonthlyDaysInAR monthlyDaysInAR = new MonthlyDaysInAR();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        monthlyDaysInAR = new MonthlyDaysInAR()
                        {
                            CurrentMonthlyDaysInAR = GetIntValue(reader, rows, CorporateDashboardConstants.CurrentMonthDaysInAR),
                            PreviousMonthlyDaysInAR = GetIntValue(reader, rows, CorporateDashboardConstants.PreviousMonthlyDaysInAR)
                        };
                    }
                    reader.Close();
                }
                return monthlyDaysInAR ?? new MonthlyDaysInAR();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region current month employee work
        public async Task<List<MonthlyEmployeeSummary>> GetCurrentMonthEmployeeWorkAsync(IExecutiveDashboardQueryBase query)
        {
            List<MonthlyEmployeeSummary> response = new List<MonthlyEmployeeSummary>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetExecutiveCurrentMonthEmployeeWork, conn, query);
                    cmd.Parameters.AddWithValue("@ClientId", query.ClientId);

                    //execute the sp
                    var claimsSummaryTask = ExecuteCurrentMonthEmployeeWorkSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(claimsSummaryTask);
                    response = claimsSummaryTask.Result;

                }

                //response = await GetMonthlyEmployeeWork();
                return response ?? new List<MonthlyEmployeeSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<List<MonthlyEmployeeSummary>> ExecuteCurrentMonthEmployeeWorkSp(SqlCommand cmd)
        {
            try
            {
                List<MonthlyEmployeeSummary> currentMonthCharges = new List<MonthlyEmployeeSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new MonthlyEmployeeSummary()
                        {
                            EmployeeId = GetIntValue(reader, rows, CorporateDashboardConstants.EmployeeId),
                            EmployeeLastCommaFirst = await _userService.GetNameAsync(GetStringValue(reader, rows, CorporateDashboardConstants.UserId)),
                            Visit = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Charges = GetDecimalValue(reader, rows, CorporateDashboardConstants.Charges),
                            AvgCharges = GetDecimalValue(reader, rows, CorporateDashboardConstants.AvgCharges),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals)
                        };
                        currentMonthCharges.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentMonthCharges ?? new List<MonthlyEmployeeSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region helper methods
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
        private int GetIntValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) && reader[columnName] != DBNull.Value
                ? (int)reader[columnName]
                : 0;
        }

        private decimal GetDecimalValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) && reader[columnName] != DBNull.Value
                ? (decimal)reader[columnName]
                : 0.00m;
        }

        private string GetStringValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ? reader[columnName] as string
                     : default(string);
        }

        private string GetDateStringValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ?
                    (reader[columnName] == DBNull.Value ?
                    default(string) : ((DateTime?)reader[columnName]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string);
        }
        private T GetEnumValue<T>(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            if (HasColumn(rows, columnName) && reader[columnName] != DBNull.Value)
            {
                object value = reader[columnName];
                if (typeof(T).IsEnum)
                {
                    // If T is an enum type, parse the value to the enum type
                    return (T)Enum.Parse(typeof(T), value.ToString());
                }
                else
                {
                    // Otherwise, directly cast to the specified type
                    return (T)value;
                }
            }
            else
            {
                return default(T);
            }
        }

        //public async Task<IEnumerable<int>> GetAssginedClientsAsync()
        //{
        //    IUserClientRepository userClientRepository = _tenantRepositoryFactory.GetUserClientRepository(_tenantInfo.Identifier);

        //    var allowedClients = (await userClientRepository.GetClientsForUser(_currentUserService.UserId))
        //        .Select(x => x.Id).ToList();

        //    return allowedClients;
        //}
        #endregion
    }
}
