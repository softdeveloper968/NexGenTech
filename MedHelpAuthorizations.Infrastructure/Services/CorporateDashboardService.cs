using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Common;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Requests.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using self_pay_eligibility_api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class CorporateDashboardService : ICorporateDashboardService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITenantInfo _tenantInfo;
        private IUnitOfWork<int> _unitOfWork;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IUserService _userService;
        private readonly ITenantCryptographyService _tenantCryptographyService;


        public CorporateDashboardService(IUnitOfWork<int> unitOfWork,
            ITenantRepositoryFactory tenantRepositoryFactory,
            IUserService userService,
            ICurrentUserService currentUserService,
            ITenantInfo tenantInfo,
            ITenantCryptographyService tenantCryptographyService
        )
        {
            _unitOfWork = unitOfWork;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _userService = userService;
            _currentUserService = currentUserService;
            _tenantInfo = tenantInfo;
            _tenantCryptographyService = tenantCryptographyService;
        }

        public SqlCommand CreateMonthlySummarySpCommand(string spName, SqlConnection conn, ICorporateDashboardQueryBase query)
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

        public SqlCommand CreateCurrentARSpCommand(string spName, SqlConnection conn, GetCurrentAROverPercentageNintyDaysPayerQuery query)
        {
            try
            {
                // Create a new SqlCommand with the specified stored procedure name and database connection.
                SqlCommand cmd = new SqlCommand(spName, conn);

                // Set the command type to stored procedure.
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the SqlCommand based on the provided filters.
                cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, query.ClientId);

                return cmd;
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

        #region current month charges
        public async Task<List<MonthlyClientSummary>> GetCurrentMonthChargesTotalsAsync(ICorporateDashboardQueryBase query)
        {
            List<MonthlyClientSummary> response = new List<MonthlyClientSummary>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetCurrentMonthCharges, conn, query);
                    cmd.Parameters.AddWithValue("@UserId", _currentUserService.UserId);

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

        public async Task<List<MonthlyClientSummary>> ExecuteCurrentMonthChargesSp(SqlCommand cmd)
        {
            try
            {
                List<MonthlyClientSummary> currentMonthCharges = new List<MonthlyClientSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new MonthlyClientSummary()
                        {
                            ClientId = GetIntValue(reader, rows, CorporateDashboardConstants.ClientId),
                            ClientName = GetStringValue(reader, rows, CorporateDashboardConstants.ClientName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Visit = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Charges = GetDecimalValue(reader, rows, CorporateDashboardConstants.Charges),
                            AvgCharges = GetDecimalValue(reader, rows, CorporateDashboardConstants.AvgCharges),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),
                            TenantClientString = _tenantCryptographyService.Encrypt(_tenantInfo.Identifier, GetIntValue(reader, rows, CorporateDashboardConstants.ClientId))
                        };
                        currentMonthCharges.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentMonthCharges ?? new List<MonthlyClientSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region current month payments
        public async Task<List<MonthlyClientSummary>> GetCurrentMonthPaymentTotalsAsync(ICorporateDashboardQueryBase query)
        {
            List<MonthlyClientSummary> response = new List<MonthlyClientSummary>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetCurrentMonthPayments, conn, query);
                    cmd.Parameters.AddWithValue("@UserId", _currentUserService.UserId);

                    //execute the sp
                    var claimsSummaryTask = ExecuteCurrentMonthPaymentsSp(cmd);

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

        public async Task<List<MonthlyClientSummary>> ExecuteCurrentMonthPaymentsSp(SqlCommand cmd)
        {
            try
            {
                List<MonthlyClientSummary> currentMonthCharges = new List<MonthlyClientSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new MonthlyClientSummary()
                        {
                            ClientId = GetIntValue(reader, rows, CorporateDashboardConstants.ClientId),
                            ClientName = GetStringValue(reader, rows, CorporateDashboardConstants.ClientName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Visit = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Payments = GetDecimalValue(reader, rows, CorporateDashboardConstants.Payments),
                            AvgAllowedAmt = GetDecimalValue(reader, rows, CorporateDashboardConstants.AvgAllowedAmt),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),
                            TenantClientString = _tenantCryptographyService.Encrypt(_tenantInfo.Identifier, GetIntValue(reader, rows, CorporateDashboardConstants.ClientId))

                        };
                        currentMonthCharges.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentMonthCharges ?? new List<MonthlyClientSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region current month denials
        public async Task<List<MonthlyClientSummary>> GetCurrentMonthDenialTotalsAsync(ICorporateDashboardQueryBase query)
        {
            List<MonthlyClientSummary> response = new List<MonthlyClientSummary>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetCurrentMonthDenials, conn, query);
                    cmd.Parameters.AddWithValue("@UserId", _currentUserService.UserId);

                    //execute the sp
                    var claimsSummaryTask = ExecuteCurrentMonthDenialsSp(cmd);

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

        public async Task<List<MonthlyClientSummary>> ExecuteCurrentMonthDenialsSp(SqlCommand cmd)
        {
            try
            {
                List<MonthlyClientSummary> currentMonthCharges = new List<MonthlyClientSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new MonthlyClientSummary()
                        {
                            ClientId = GetIntValue(reader, rows, CorporateDashboardConstants.ClientId),
                            ClientName = GetStringValue(reader, rows, CorporateDashboardConstants.ClientName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Visit = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Denials = GetDecimalValue(reader, rows, CorporateDashboardConstants.Denials),
                            AvgDenials = GetDecimalValue(reader, rows, CorporateDashboardConstants.AvgDenial),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),
                            TenantClientString = _tenantCryptographyService.Encrypt(_tenantInfo.Identifier, GetIntValue(reader, rows, CorporateDashboardConstants.ClientId))

                        };
                        currentMonthCharges.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentMonthCharges ?? new List<MonthlyClientSummary>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region current month employee work
        public async Task<List<MonthlyEmployeeSummary>> GetCurrentMonthEmployeeWorkAsync(ICorporateDashboardQueryBase query)
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
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetCurrentMonthEmployeeWork, conn, query);
                    cmd.Parameters.AddWithValue("@CurrentUserId", _currentUserService.UserId);

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

        #region current AR Over percentage 90 days Client
        public async Task<List<CurrentAROverPercentageNintyDaysClient>> GetCurrentAROverPercentageNintyDaysClientAsync(ICorporateDashboardQueryBase query)
        {
            List<CurrentAROverPercentageNintyDaysClient> response = new List<CurrentAROverPercentageNintyDaysClient>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spGetCurrentAROverPercentageNintyDaysClient, conn, query);
                    cmd.Parameters.AddWithValue("@UserId", _currentUserService.UserId);

                    //execute the sp
                    var currentAROverPercentage90Days = ExecuteCurrentAROverPercentageNintyDaysClientSp(cmd);

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

        public async Task<List<CurrentAROverPercentageNintyDaysClient>> ExecuteCurrentAROverPercentageNintyDaysClientSp(SqlCommand cmd)
        {
            try
            {
                List<CurrentAROverPercentageNintyDaysClient> currentAROverPercentage90Days = new List<CurrentAROverPercentageNintyDaysClient>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var clientCharges = new CurrentAROverPercentageNintyDaysClient()
                        {
                            ClientId = GetIntValue(reader, rows, CorporateDashboardConstants.ClientId),
                            ClientName = GetStringValue(reader, rows, CorporateDashboardConstants.ClientName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Claims = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            Charges = GetDecimalValue(reader, rows, CorporateDashboardConstants.Charges),
                            ARTotals = GetDecimalValue(reader, rows, CorporateDashboardConstants.ARTotals),
                            PrevMonth = GetDecimalValue(reader, rows, CorporateDashboardConstants.LastMonthTotals),
                            TenantClientString = _tenantCryptographyService.Encrypt(_tenantInfo.Identifier, GetIntValue(reader, rows, CorporateDashboardConstants.ClientId))

                        };
                        currentAROverPercentage90Days.Add(clientCharges);
                    }
                    reader.Close();
                }
                return currentAROverPercentage90Days ?? new List<CurrentAROverPercentageNintyDaysClient>();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region current AR Over percentage 90 days Payer
        public async Task<List<CurrentAROverPercentageNintyDaysPayer>> GetCurrentAROverPercentageNintyDaysPayerAsync(GetCurrentAROverPercentageNintyDaysPayerQuery query)
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

        #region Monthly days in AR
        public async Task<MonthlyDaysInAR> GetMonthlyDaysInARAsync(GetMonthlyDaysInARQuery query)
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
                    SqlCommand cmd = CreateMonthlySummarySpCommand(StoreProcedureTitle.spCalculateMonthlyDaysInAR, conn, query);
                    cmd.Parameters.AddWithValue("@UserId", _currentUserService.UserId);

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

        #region Clean Claim Rate
        public async Task<List<ClaimRate>> GetCleanClaimRateAsync(GetCleanClaimRateQuery query)
        {
            List<ClaimRate> response = new List<ClaimRate>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateClaimRateSpCommand(StoreProcedureTitle.spGetClaimRate, conn, (int)ClaimStatusTypeEnum.PaidClaimStatusType);
                    cmd.Parameters.AddWithValue("@UserId", _currentUserService.UserId);

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

        public async Task<List<ClaimRate>> ExecuteCleanClaimRateSp(SqlCommand cmd)
        {
            try
            {
                List<ClaimRate> cleanClaimRates = new List<ClaimRate>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var cleanClaimRate = new ClaimRate()
                        {
                            ClientId = GetIntValue(reader, rows, CorporateDashboardConstants.ClientId),
                            ClientName = GetStringValue(reader, rows, CorporateDashboardConstants.ClientName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Visits = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            OnInitialReview = GetIntValue(reader, rows, CorporateDashboardConstants.PaidOnInitialReviewCol),
                            ClaimPercentageRate = GetDecimalValue(reader, rows, CorporateDashboardConstants.CleanClaimPercentageRateCol),
                            TenantClientString = _tenantCryptographyService.Encrypt(_tenantInfo.Identifier, GetIntValue(reader, rows, CorporateDashboardConstants.ClientId))

                        };
                        cleanClaimRates.Add(cleanClaimRate);
                    }
                    reader.Close();
                }
                return cleanClaimRates ?? new List<ClaimRate>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Denial Claim Rate
        public async Task<List<ClaimRate>> GetDenialClaimRateAsync(GetDenialRateQuery query)
        {
            List<ClaimRate> response = new List<ClaimRate>();
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp
                    SqlCommand cmd = CreateClaimRateSpCommand(StoreProcedureTitle.spGetClaimRate, conn, (int)ClaimStatusTypeEnum.DeniedClaimStatusType);
                    cmd.Parameters.AddWithValue("@UserId", _currentUserService.UserId);

                    //execute the sp
                    var monthlyDaysInAR = ExecuteDenialClaimRateSp(cmd);

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

        public async Task<List<ClaimRate>> ExecuteDenialClaimRateSp(SqlCommand cmd)
        {
            try
            {
                List<ClaimRate> cleanClaimRates = new List<ClaimRate>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var cleanClaimRate = new ClaimRate()
                        {
                            ClientId = GetIntValue(reader, rows, CorporateDashboardConstants.ClientId),
                            ClientName = GetStringValue(reader, rows, CorporateDashboardConstants.ClientName),
                            ClientCode = GetStringValue(reader, rows, CorporateDashboardConstants.ClientCode),
                            Visits = GetIntValue(reader, rows, CorporateDashboardConstants.Visits),
                            OnInitialReview = GetIntValue(reader, rows, CorporateDashboardConstants.PaidOnInitialReviewCol),
                            ClaimPercentageRate = GetDecimalValue(reader, rows, CorporateDashboardConstants.CleanClaimPercentageRateCol),
                            TenantClientString = _tenantCryptographyService.Encrypt(_tenantInfo.Identifier, GetIntValue(reader, rows, CorporateDashboardConstants.ClientId))

                        };
                        cleanClaimRates.Add(cleanClaimRate);
                    }
                    reader.Close();
                }
                return cleanClaimRates ?? new List<ClaimRate>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region services not in use, will remove later
        #region Client stat totals
        public async Task<ClaimStatusDashboardData> GetClientStatTotalsDataAsync(GetClientStatTotalsDataQuery query)
        {
            ClaimStatusDashboardData response = new ClaimStatusDashboardData();
            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {

                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp to get cash projection for last 7 days by passing the required parameters to the sp
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimStatusVisits, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);
                    //execute the sp
                    var dashboardData = await ExecuteClaimStatusVisitsSp(cmd);

                    //wait for the task to complete and then map the result into response
                    response = dashboardData;

                }
                return response;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ClaimStatusDashboardData> ExecuteClaimStatusVisitsSp(SqlCommand cmd) //EN-312
        {
            try
            {
                ClaimStatusDashboardData ClaimStatusDashboardData = new ClaimStatusDashboardData();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        ClaimStatusDashboardData = new ClaimStatusDashboardData()
                        {
                            ChargedVisit = GetIntValue(reader, rows, ReportConstants.Quantity),
                            ChargedTotals = GetDecimalValue(reader, rows, ReportConstants.BilledAmount),
                            AllowedAmountSum = GetDecimalValue(reader, rows, ReportConstants.AllowedAmountSum),
                            PaidAmountSum = GetDecimalValue(reader, rows, ReportConstants.PaidAmountSum),
                            AdjudicatedVisits = GetIntValue(reader, rows, ReportConstants.AdjudicatedVisits),
                            AdjudicatedTotals = GetDecimalValue(reader, rows, ReportConstants.AdjudicatedTotals),
                            AllowedVisits = GetIntValue(reader, rows, ReportConstants.AllowedVisits),
                            AllowedTotals = GetDecimalValue(reader, rows, ReportConstants.AllowedTotals),
                            PaidVisits = GetIntValue(reader, rows, ReportConstants.PaidVisits),
                            PaidTotals = GetDecimalValue(reader, rows, ReportConstants.PaidTotals),
                            ContractualTotals = GetDecimalValue(reader, rows, ReportConstants.ContractualTotals),
                            ContractualVisits = GetIntValue(reader, rows, ReportConstants.ContractualVisits),
                            DenialVisits = GetIntValue(reader, rows, ReportConstants.DenialVisits),
                            DenialTotals = GetDecimalValue(reader, rows, ReportConstants.DenialTotals),
                            WriteOffVisits = GetIntValue(reader, rows, ReportConstants.WriteOffVisits),
                            WriteOffTotals = GetDecimalValue(reader, rows, ReportConstants.WriteOffTotals),
                            SecPsTotals = GetDecimalValue(reader, rows, ReportConstants.SecPs),
                            OpenVisits = GetIntValue(reader, rows, ReportConstants.OpenVisits),
                            OpenTotals = GetDecimalValue(reader, rows, ReportConstants.TotalOpen),
                            InProcessVisits = GetIntValue(reader, rows, ReportConstants.InProcessVisits),
                            InProcessTotals = GetDecimalValue(reader, rows, ReportConstants.TotalInProcess),
                            TotalProcessed = GetDecimalValue(reader, rows, ReportConstants.TotalProcessed),
                        };
                    }
                    reader.Close();
                }
                return ClaimStatusDashboardData;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Client Financial Summary data
        public async Task<ClaimSummary> GetClientFinancialSummaryDataAsync(GetClientFinancialSummayDataQuery query)
        {
            ClaimSummary response = new ClaimSummary();
            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {

                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp to get cash projection for last 7 days by passing the required parameters to the sp
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetFinancialSummaryTotals, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);
                    //execute the sp
                    var dashboardData = await ExecuteFinancialSymmarySp(cmd);

                    //wait for the task to complete and then map the result into response
                    response = dashboardData;

                }
                return response;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ClaimSummary> ExecuteFinancialSymmarySp(SqlCommand cmd) //AA-331
        {
            try
            {
                ClaimSummary financialSummary = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        financialSummary.ChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedTotals);
                        financialSummary.ChargedVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity);
                        financialSummary.PaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidTotals);
                        financialSummary.PaymentVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.PaidVisits);
                        financialSummary.ContractualTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ContractualTotals);
                        financialSummary.ContractualVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ContractualVisits);
                        financialSummary.WriteOffAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.WriteOffTotals);
                        financialSummary.WriteOffVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.WriteOffVisits);
                    }
                    reader.Close();
                }
                return financialSummary;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Client denial reasons by insurance 
        public async Task<List<ClaimSummary>> GetDenialReasonsByInsuranceDataAsync(GetClientDenialReasonsByInsuranceDataQuery query)
        {
            List<ClaimSummary> response = new List<ClaimSummary>();
            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {

                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp to get cash projection for last 7 days by passing the required parameters to the sp
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetDenialsByInsuranceDateWise, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);
                    //execute the sp
                    var dashboardData = await ExecuteDenialReasonsByInsuranceDataSp(cmd);

                    //wait for the task to complete and then map the result into response
                    response = dashboardData;

                }
                return response;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ClaimSummary>> ExecuteDenialReasonsByInsuranceDataSp(SqlCommand cmd) //AA-331
        {
            try
            {
                List<ClaimSummary> denialReasonsByInsurances = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var denialReasonsByInsurance = new ClaimSummary();

                        denialReasonsByInsurance.ClientInsuranceName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceName);
                        denialReasonsByInsurance.ClientInsuranceId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceId);
                        denialReasonsByInsurance.ChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedSum);
                        denialReasonsByInsurance.Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.DenialVisits);

                        denialReasonsByInsurances.Add(denialReasonsByInsurance);
                    }
                    reader.Close();
                }
                return denialReasonsByInsurances;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region
        public async Task<ClaimSummary> GetClientClaimSummaryDataAsync(GetClientClaimSummaryDataQuery query)
        {
            ClaimSummary response = new ClaimSummary();

            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimSummaryTotals, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);


                    //execute the sp
                    var claimsSummaryTask = ExecuteClaimsSymmarySp(cmd);

                    //wait for the task to complete and then map the result into response
                    await claimsSummaryTask;
                    response = claimsSummaryTask.Result;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ClaimSummary> ExecuteClaimsSymmarySp(SqlCommand cmd)
        {
            try
            {
                ClaimSummary dataItem = new();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        dataItem = new ClaimSummary()
                        {
                            ChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ChargedTotals),
                            ChargedVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.TotalVisits),
                            PaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaymentTotals),
                            PaymentVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.PaymentVisits),
                            DenialTotals = GetDecimalValue(reader, rows, ReportConstants.DenialTotals),
                            DenialVisits = GetIntValue(reader, rows, ReportConstants.DenialVisits),
                            InProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.InProcessTotals),
                            InProcessVisits = GetIntValue(reader, rows, ReportConstants.InProcessVisits),
                            OpenTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedTotals),
                            OpenVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedVisits),
                            LastMonthPaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthPaymentTotals),
                            LastMonthDenialTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthDenialTotals),
                            LastMonthInProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthInProcessTotals),
                            LastMonthNotAdjudicatedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthNotAdjudicatedTotals),
                            LastMonthChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.LastMonthChargedTotals),
                            CurrentMonthPaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthPaymentTotals),
                            CurrentMonthDenialTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthDenialTotals),
                            CurrentMonthInProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthInProcessTotals),
                            CurrentMonthNotAdjudicatedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthNotAdjudicatedTotals),
                            CurrentMonthChargedTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.CurrentMonthChargedTotals)
                        };
                    }
                    reader.Close();
                }
                return dataItem;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Get client average days by payer
        public async Task<List<AverageDaysByPayer>> GetClientAverageDasyByPayerDataAsync(GetClientAverageDaysToPayByPayerQuery query)
        {

            List<AverageDaysByPayer> response = new List<AverageDaysByPayer>();

            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetAvgDaysToPayByPayerData, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);

                    //execute the sp
                    var averageDaysToPayByPayerTask = await ExecuteAverageDaysToPayByPayerSp(cmd);

                    response = averageDaysToPayByPayerTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<AverageDaysByPayer>> ExecuteAverageDaysToPayByPayerSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<AverageDaysByPayer> averageDaysByPayers = new List<AverageDaysByPayer>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var daysByPayer = new AverageDaysByPayer()
                        {
                            PayerId = HasColumn(rows, StoredProcedureColumnsHelper.PayerId) ? reader[StoredProcedureColumnsHelper.PayerId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.PayerId]
                                            : 0,
                            PayerName = HasColumn(rows, StoredProcedureColumnsHelper.PayerName) ? reader[StoredProcedureColumnsHelper.PayerName] as string
                                            : default(string),
                            DaysToPayByBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.BilledToPayment) && reader[StoredProcedureColumnsHelper.BilledToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.BilledToPayment],
                            DaysToPayByServiceFrom = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToPayment) && reader[StoredProcedureColumnsHelper.ServiceToPayment] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToPayment],
                            DaysServiceToBilledOn = HasColumn(rows, StoredProcedureColumnsHelper.ServiceToBilled) && reader[StoredProcedureColumnsHelper.ServiceToBilled] == System.DBNull.Value
                            ? 0
                            : (int)reader[StoredProcedureColumnsHelper.ServiceToBilled]
                        };
                        averageDaysByPayers.Add(daysByPayer);
                    }
                    reader.Close();
                }
                return averageDaysByPayers;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Get client charges by payer
        public async Task<List<ChargesByPayer>> GetClientChargesByPayerDataAsync(GetClientChargesByPayersQuery query)
        {

            List<ChargesByPayer> response = new List<ChargesByPayer>();

            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spChargesByPayer, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);

                    //execute the sp
                    var averageDaysToPayByPayerTask = await ExecuteChargesByPayerSp(cmd);

                    response = averageDaysToPayByPayerTask;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ChargesByPayer>> ExecuteChargesByPayerSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<ChargesByPayer> chargesByPayers = new List<ChargesByPayer>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ChargesByPayer()
                        {
                            PayerName = HasColumn(rows, StoredProcedureColumnsHelper.PayerName) ? reader[StoredProcedureColumnsHelper.PayerName] as string
                                            : default(string),
                            PayerId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,
                            ChargesTotal = HasColumn(rows, StoredProcedureColumnsHelper.ChargedTotals) ? reader[StoredProcedureColumnsHelper.ChargedTotals] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedTotals]
                                            : 0.00m,
                            ClaimCount = HasColumn(rows, StoredProcedureColumnsHelper.Quantity) ? reader[StoredProcedureColumnsHelper.Quantity] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.Quantity]
                                            : 0,

                        };
                        chargesByPayers.Add(charges);
                    }
                    reader.Close();
                }
                return chargesByPayers;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Get client claim status data
        public async Task<ClaimSummary> GetClientClaimStatusDataAsync(GetClientClaimStatusDataQuery query)
        {
            ClaimSummary response = new ClaimSummary();

            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimStatusTotals, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);

                    //execute the sp
                    var claimStatusData = await ExecuteClaimsStatusTotalsTaskSp(cmd);

                    response = claimStatusData;

                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ClaimSummary> ExecuteClaimsStatusTotalsTaskSp(SqlCommand cmd) //AA-137
        {
            try
            {
                ClaimSummary claimsStatusTotals = new ClaimSummary();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        claimsStatusTotals.PaymentVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.PaidVisits);
                        claimsStatusTotals.PaymentTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidTotals);
                        claimsStatusTotals.DenialTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.DenialTotals);
                        claimsStatusTotals.DenialVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.DenialVisits);
                        claimsStatusTotals.WriteOffAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.WriteOffTotals);
                        claimsStatusTotals.WriteOffVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.WriteOffVisits);
                        claimsStatusTotals.OpenTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedTotals);
                        claimsStatusTotals.OpenVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.NotAdjudicatedVisits);
                        claimsStatusTotals.ContractualTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.ContractualTotals);
                        claimsStatusTotals.ContractualVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ContractualVisits);
                        claimsStatusTotals.InProcessTotals = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.InProcessTotals);
                        claimsStatusTotals.InProcessVisits = GetIntValue(reader, rows, StoredProcedureColumnsHelper.InProcessVisits);
                    }
                    reader.Close();
                }
                return claimsStatusTotals;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region
        public async Task<List<ClaimSummary>> GetClientClaimsInProcessDataAsync(GetClientClaimsInProcessQuery query)
        {
            List<ClaimSummary> response = new List<ClaimSummary>();

            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetClaimInProcessDateWise, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);

                    //execute the sp
                    var claimsInProcessTask = ExecuteClaimsInProcessSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(claimsInProcessTask);
                    response = claimsInProcessTask.Result;
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ClaimSummary>> ExecuteClaimsInProcessSp(SqlCommand cmd)
        {
            try
            {
                List<ClaimSummary> claimsInProcess = new List<ClaimSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ClaimSummary()
                        {
                            ClientInsuranceName = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceName) ? reader[StoredProcedureColumnsHelper.ClientInsuranceName] as string
                                            : default(string),
                            ClientInsuranceId = HasColumn(rows, StoredProcedureColumnsHelper.ClientInsuranceId) ? reader[StoredProcedureColumnsHelper.ClientInsuranceId] == System.DBNull.Value
                                            ? 0
                                            : (int)reader[StoredProcedureColumnsHelper.ClientInsuranceId]
                                            : 0,
                            ChargedTotals = HasColumn(rows, StoredProcedureColumnsHelper.ChargedSum) ? reader[StoredProcedureColumnsHelper.ChargedSum] == System.DBNull.Value
                                            ? 0.00m
                                            : (decimal)reader[StoredProcedureColumnsHelper.ChargedSum]
                                            : 0.00m,
                            //BilledOnDate = HasColumn(rows, StoreProcedureTitle.BilledOnDate) ?
                            //                (reader[StoreProcedureTitle.BilledOnDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.BilledOnDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfServiceFrom = HasColumn(rows, StoreProcedureTitle.DateOfService) ?
                            //                (reader[StoreProcedureTitle.DateOfService] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfService]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //ReceivedFrom = HasColumn(rows, StoreProcedureTitle.ReceivedDate) ?
                            //                (reader[StoreProcedureTitle.ReceivedDate] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.ReceivedDate]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string),

                            //DateOfTransactionFrom = HasColumn(rows, StoreProcedureTitle.DateOfTransaction) ?
                            //                (reader[StoreProcedureTitle.DateOfTransaction] == DBNull.Value ?
                            //                default(string) : ((DateTime?)reader[StoreProcedureTitle.DateOfTransaction]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string)

                        };
                        claimsInProcess.Add(charges);
                    }
                    reader.Close();
                }
                return claimsInProcess;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region
        public async Task<List<ClaimSummary>> GetClientAvgAllowedAmtTotalsAsync(GetClientAvgAllowedAmtTotalsQuery query)
        {
            List<ClaimSummary> response = new List<ClaimSummary>();

            var decryptedTenantClient = _tenantCryptographyService.Decrypt(query.TenantClientString);
            var clientId = decryptedTenantClient.Item2;
            string connStr = _tenantInfo.ConnectionString;
            SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
            try
            {
                await using (conn)
                {
                    await conn.OpenAsync();

                    //create the sp 
                    SqlCommand cmd = ExportCommonMethod.CreateDynamicSpCommand(StoreProcedureTitle.spGetAvgAllowedAmtDateWise, conn, clientId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, flattenedLineItemStatus: null);

                    //execute the sp
                    var claimsInProcessTask = ExecuteAvgAllowedAmtSp(cmd);

                    //wait for the task to complete and then map the result into response
                    await Task.WhenAll(claimsInProcessTask);
                    response = claimsInProcessTask.Result;
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<List<ClaimSummary>> ExecuteAvgAllowedAmtSp(SqlCommand cmd) //AA-137
        {
            try
            {
                List<ClaimSummary> avgAllowedAmt = new List<ClaimSummary>();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        var charges = new ClaimSummary()
                        {
                            ClientInsuranceName = GetStringValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceName),
                            ClientInsuranceId = GetIntValue(reader, rows, StoredProcedureColumnsHelper.ClientInsuranceId),
                            AllowedAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.AllowedAmount),
                            PaidAmountSum = GetDecimalValue(reader, rows, StoredProcedureColumnsHelper.PaidAmount),
                            Quantity = GetIntValue(reader, rows, StoredProcedureColumnsHelper.Quantity)
                        };
                        avgAllowedAmt.Add(charges);
                    }
                    reader.Close();
                }
                return avgAllowedAmt;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
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

        public async Task<IEnumerable<int>> GetAssginedClientsAsync()
        {
            IUserClientRepository userClientRepository = _tenantRepositoryFactory.GetUserClientRepository(_tenantInfo.Identifier);

            var allowedClients = (await userClientRepository.GetClientsForUser(_currentUserService.UserId))
                .Select(x => x.Id).ToList();

            return allowedClients;
        }
        #endregion
    }
}
