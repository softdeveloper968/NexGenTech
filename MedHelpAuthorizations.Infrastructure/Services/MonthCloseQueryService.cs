using AutoMapper;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Features.MonthClose.Queries;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class MonthCloseQueryService : IMonthCloseQueryService
    {
        private readonly ApplicationContext _context;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;
        private IUnitOfWork<int> _unitOfWork;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IStringLocalizer<ClaimStatusQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITenantInfo _tenantInfo;

        // Constructor injection for dependencies
        public MonthCloseQueryService(
            ApplicationContext context,
            IDbContextFactory<ApplicationContext> contextFactory,
            IUnitOfWork<int> unitOfWork,
            ITenantRepositoryFactory tenantRepositoryFactory,
            IStringLocalizer<ClaimStatusQueryService> localizer,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IConfiguration configuration,
            ITenantInfo tenantInfo)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _tenantRepositoryFactory = tenantRepositoryFactory ?? throw new ArgumentNullException(nameof(tenantRepositoryFactory));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tenantInfo = tenantInfo ?? throw new ArgumentNullException(nameof(tenantInfo));
        }

        // Generic method for creating common claims query (stored procedure) for GetMonthCashCollectionData
        //private SqlCommand CreateMonthCloseQuery(string spName, SqlConnection conn, int clientId, IMonthCloseDashboardQuery filters)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(spName, conn)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        // Add parameters for the GetMonthCashCollectionData stored procedure
        //        cmd.Parameters.AddWithValue("@ClientId", filters?.ClientId ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@ClientLocationId", string.IsNullOrEmpty(filters?.ClientLocationId) ? (object)DBNull.Value : filters.ClientLocationId);
        //        cmd.Parameters.AddWithValue("@ClientProviderId", string.IsNullOrEmpty(filters?.ClientProviderId) ? (object)DBNull.Value : filters.ClientProviderId);
        //        cmd.Parameters.AddWithValue("@ClientInsuranceId", string.IsNullOrEmpty(filters?.ClientInsuranceId) ? (object)DBNull.Value : filters.ClientInsuranceId);
        //        cmd.Parameters.AddWithValue("@CptCodeId", string.IsNullOrEmpty(filters?.CptCodeId) ? (object)DBNull.Value : filters.CptCodeId);

        //        return cmd;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException("Error while creating MonthCashCollection query", ex);
        //    }
        //}

        // Generic method to execute the stored procedure and map results to the desired type

        private SqlCommand CreateMonthCloseQuery(string spName, SqlConnection conn, int clientId, IMonthCloseDashboardQuery filters)
        {
            // Create the SqlCommand for the stored procedure
            SqlCommand cmd = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add parameters based on the provided filters and clientId
            cmd.Parameters.AddWithValue("@ClientId", clientId);

            // Check each filter property and add it to the SqlCommand if it's not null
            cmd.Parameters.AddWithValue("@ClientLocationId", (object)filters.ClientLocationId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientProviderId", (object)filters.ClientProviderId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientInsuranceId", (object)filters.ClientInsuranceId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CptCodeId", (object)filters.CptCodeId ?? DBNull.Value);

            return cmd;
        }

        private async Task<List<T>> ExecuteStoredProcAsync<T>(SqlCommand cmd, Func<SqlDataReader, T> mapRow)
        {
            List<T> result = new List<T>();

            try
            {
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        await cmd.Connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    using (reader)
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(mapRow(reader));
                        }

                    }
                    reader.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while executing stored procedure", ex);
            }
        }
        // Refactored function to get month cash collection data
        //        public async Task<IEnumerable<MonthCashCollection>> GetMonthCashCollectionDataAsync(IMonthCloseDashboardQuery query)
        //        {
        //            try
        //            {
        //                var clientId = _currentUserService.ClientId;
        //                var connStr = _tenantInfo.ConnectionString;
        //                using var conn = new SqlConnection(connStr);
        //                await conn.OpenAsync();

        //                // Create the SqlCommand for stored procedure
        //                //SqlCommand cmd = CreateMonthCloseQuery("GetMonthCashCollectionData", conn, clientId, query);
        //                SqlCommand cmd = CreateMonthCloseQuery(
        //    "GetMonthCashCollectionData",
        //    conn,
        //    clientId,           // Required
        //    null,               // ClientLocationId (optional)
        //    null,               // ClientProviderId (optional)
        //    null,               // ClientInsuranceId (optional)
        //    null                // CptCodeId (optional)
        //);
        //                // Execute stored procedure and map result to MonthCashCollection objects
        //                List<MonthCashCollection> monthCashCollection = await ExecuteStoredProcAsync(cmd, reader =>
        //                {
        //                    return new MonthCashCollection(
        //                        reader.GetDateTime(reader.GetOrdinal("Date")).ToString(),
        //                        reader.GetDecimal(reader.GetOrdinal("Payment")),
        //                        reader.GetDecimal(reader.GetOrdinal("CollectionGoal")),
        //                        reader.GetDecimal(reader.GetOrdinal("Change")),
        //                        reader.GetString(reader.GetOrdinal("Status")))
        //                    {
        //                        Month = reader.GetInt32(reader.GetOrdinal("Month")),
        //                        Year = reader.GetInt32(reader.GetOrdinal("Year")),
        //                        ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
        //                        ClientLocationId = reader.GetInt32(reader.GetOrdinal("ClientLocationId")),
        //                        ClientProviderId = reader.GetInt32(reader.GetOrdinal("ClientProviderId")),
        //                        ClientInsuranceId = reader.GetInt32(reader.GetOrdinal("ClientInsuranceId")),
        //                        CptCodeId = reader.GetInt32(reader.GetOrdinal("CptCodeId"))
        //                    };
        //                });

        //                return monthCashCollection;
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new InvalidOperationException("Error while retrieving month cash collection data", ex);
        //            }
        //        }

        public async Task<IEnumerable<MonthlyCashCollectionData>> GetMonthCashCollectionDataAsync(IMonthCloseDashboardQuery query)
        {
            try
            {
                var clientId = _currentUserService.ClientId;
                var connStr = _tenantInfo.ConnectionString;
                using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();

                // Create the SqlCommand for the stored procedure
                SqlCommand cmd = CreateMonthCloseQuery(
                    "GetMonthlyCashCollectionData",    // Stored procedure name
                    conn,                            // Connection object
                    clientId,                        // ClientId (required)
                    query                            // IMonthCloseDashboardQuery filters (optional parameters)
                );

                // Execute stored procedure and map result to MonthCashCollection objects
                List<MonthlyCashCollectionData> monthCashCollection = await ExecuteMonthCashCollectionProcAsync(cmd);

                return monthCashCollection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while retrieving month cash collection data", ex);
            }
        }

        private async Task<List<MonthlyCashCollectionData>> ExecuteMonthCashCollectionProcAsync(SqlCommand cmd)
        {
            List<MonthlyCashCollectionData> result = new List<MonthlyCashCollectionData>();

            try
            {
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        await cmd.Connection.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    using (reader)
                    {
                        while (await reader.ReadAsync())
                        {
                            // Get the Date as a string
                            var dateStr = GetValueOrDefault<string>(reader, "Date");

                            // Parse the date string into a DateTime (use 'MMM-yy' format)
                            //DateTime parsedDate = DateTime.TryParseExact(dateStr, "MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempDate)
                            //    ? tempDate
                            //    : default;

                            //// If parsedDate is still default (i.e., parsing failed), assign it an empty string
                            //string formattedDate = parsedDate == default ? string.Empty : parsedDate.ToString("yyyy-MM-dd");

                            // Retrieve values for Payment, CollectionGoal, and Change, ensuring nulls are handled
                            decimal payment = GetValueOrDefault<decimal>(reader, "Payment");
                            decimal collectionGoal = GetValueOrDefault<decimal>(reader, "CollectionGoal");
                            decimal change = GetValueOrDefault<decimal>(reader, "ChangePercentage");

                            // Create the MonthCashCollection object
                            var monthCashCollection = new MonthlyCashCollectionData(
                                dateStr,  // Now properly formatted date string
                                payment,        // Ensures payment is not null (defaults to 0 if null)
                                collectionGoal, // Ensures collectionGoal is not null (defaults to 0 if null)
                                change,         // Ensures change is not null (defaults to 0 if null)
                                string.Empty    // Empty value for the last field
                            );

                            result.Add(monthCashCollection);
                        }
                    }

                    reader.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while executing stored procedure", ex);
            }
        }

        private T GetValueOrDefault<T>(SqlDataReader reader, string columnName, T defaultValue = default)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? defaultValue : (T)reader.GetValue(ordinal);
        }

        private static T GetValueOrDefault<T>(DbDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
                return default;

            var value = reader[columnName];

            if (typeof(T) == typeof(DateTime) && value is string stringValue)
            {
                return DateTime.TryParse(stringValue, out var dateValue)
                    ? (T)(object)dateValue
                    : default;
            }

            return (T)value;
        }

        public async Task<IEnumerable<MonthlyARData>> GetMonthlyARDataAsync(IMonthCloseDashboardQuery query)
        {
            try
            {
                var clientId = _currentUserService.ClientId;
                var connStr = _tenantInfo.ConnectionString;
                using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();

                // Create the SqlCommand for stored procedure
                SqlCommand cmd = CreateMonthCloseQuery("GetMonthlyARData", conn, clientId, query);

                List<MonthlyARData> monthlyARs = await ExecuteMonthlyARProcAsync(cmd);
                return monthlyARs;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while retrieving monthly AR data", ex);
            }
        }

        private async Task<List<MonthlyARData>> ExecuteMonthlyARProcAsync(SqlCommand cmd)
        {
            List<MonthlyARData> result = new List<MonthlyARData>();

            try
            {
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        await cmd.Connection.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    using (reader)
                    {
                        while (await reader.ReadAsync())
                        {
                            // Get the Date as a string
                            var dateStr = GetValueOrDefault<string>(reader, "Date");

                            // Parse the date string into a DateTime (use 'MMM-yy' format)
                            //DateTime parsedDate = DateTime.TryParseExact(dateStr, "MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempDate)
                            //    ? tempDate
                            //    : default;

                            //// If parsedDate is still default (i.e., parsing failed), assign it an empty string
                            //string formattedDate = parsedDate == default ? string.Empty : parsedDate.ToString("yyyy-MM-dd");

                            // Retrieve values for Charges, Denials, PercentageOfCharges, DenialPercentageGoal and ChangePercentage, ensuring nulls are handled
                            decimal receivables = GetValueOrDefault<decimal>(reader, "Receivables");
                            decimal denialGoal = GetValueOrDefault<decimal>(reader, "DenialGoal");
                            decimal percentageOfAR = GetValueOrDefault<decimal>(reader, "PercentageOfAR");
                            decimal denialPercentageGoal = GetValueOrDefault<decimal>(reader, "DenialPercentageGoal");
                            decimal change = GetValueOrDefault<decimal>(reader, "ChangePercentage");

                            // Create the MonthCashCollection object
                            var monthARData = new MonthlyARData(
                                dateStr,  // Now properly formatted date string
                                receivables,        // Ensures receivables is not null (defaults to 0 if null)
                                percentageOfAR, // Ensures percentageOfAR is not null (defaults to 0 if null)
                                denialGoal,// Ensures denialGoal is not null (defaults to 0 if null)
                                denialPercentageGoal, // Ensures denialPercentageGoal is not null (defaults to 0 if null)
                                change,         // Ensures change is not null (defaults to 0 if null)
                                string.Empty    // Empty value for the last field
                            );

                            result.Add(monthARData);
                        }
                    }
                    reader.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while executing stored procedure", ex);
            }
        }

        public async Task<IEnumerable<MonthlyDenialData>> GetMonthlyDenialDataAsync(IMonthCloseDashboardQuery query)
        {
            try
            {
                var clientId = _currentUserService.ClientId;
                var connStr = _tenantInfo.ConnectionString;
                using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();

                // Create the SqlCommand for stored procedure
                SqlCommand cmd = CreateMonthCloseQuery("GetMonthlyDenialData", conn, clientId, query);

                List<MonthlyDenialData> monthlyDenials = await ExecuteMonthlyDenialProcAsync(cmd);
                return monthlyDenials;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while retrieving");
            }
        }

        private async Task<List<MonthlyDenialData>> ExecuteMonthlyDenialProcAsync(SqlCommand cmd)
        {
            List<MonthlyDenialData> result = new List<MonthlyDenialData>();

            try
            {
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        await cmd.Connection.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    using (reader)
                    {
                        while (await reader.ReadAsync())
                        {
                            // Get the Date as a string
                            var dateStr = GetValueOrDefault<string>(reader, "Date");

                            // Parse the date string into a DateTime (use 'MMM-yy' format)
                            //DateTime parsedDate = DateTime.TryParseExact(dateStr, "MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempDate)
                            //    ? tempDate
                            //    : default;

                            //// If parsedDate is still default (i.e., parsing failed), assign it an empty string
                            //string formattedDate = parsedDate == default ? string.Empty : parsedDate.ToString("yyyy-MM-dd");

                            // Retrieve values for Charges, Denials, PercentageOfCharges, DenialPercentageGoal and ChangePercentage, ensuring nulls are handled
                            decimal charges = GetValueOrDefault<decimal>(reader, "Charges");
                            decimal denials = GetValueOrDefault<decimal>(reader, "Denials");
                            decimal percentageOfCharges = GetValueOrDefault<decimal>(reader, "PercentageOfCharges");
                            decimal denialPercentageGoal = GetValueOrDefault<decimal>(reader, "DenialPercentageGoal");
                            decimal change = GetValueOrDefault<decimal>(reader, "ChangePercentage");

                            // Create the MonthCashCollection object
                            var monthCashCollection = new MonthlyDenialData(
                                dateStr,  // Now properly formatted date string
                                charges,        // Ensures charges is not null (defaults to 0 if null)
                                denials, // Ensures denials is not null (defaults to 0 if null)
                                percentageOfCharges, // Ensures denials is not null (defaults to 0 if null)
                                denialPercentageGoal, // Ensures denials is not null (defaults to 0 if null)
                                change,         // Ensures change is not null (defaults to 0 if null)
                                string.Empty    // Empty value for the last field
                            );

                            result.Add(monthCashCollection);
                        }
                    }
                    reader.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while executing stored procedure", ex);
            }
        }

        public async Task<IEnumerable<MonthlyReceivablesData>> GetMonthlyReceivablesDataAsync(IMonthCloseDashboardQuery query)
        {
            try
            {
                var clientId = _currentUserService.ClientId;
                var connStr = _tenantInfo.ConnectionString;
                using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();

                // Create the SqlCommand for stored procedure
                SqlCommand cmd = CreateMonthCloseQuery("GetMonthlyReceivablesData", conn, clientId, query);

                List<MonthlyReceivablesData> monthlyReceivables = await ExecuteMonthlyReceivablesProcAsync(cmd);
                return monthlyReceivables;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while retrieving monthly receivables data", ex);
            }
        }

        private async Task<List<MonthlyReceivablesData>> ExecuteMonthlyReceivablesProcAsync(SqlCommand cmd)
        {
            List<MonthlyReceivablesData> result = new List<MonthlyReceivablesData>();

            try
            {
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        await cmd.Connection.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    using (reader)
                    {
                        while (await reader.ReadAsync())
                        {
                            // Get the Date as a string
                            var dateStr = GetValueOrDefault<string>(reader, "Date");

                            // Parse the date string into a DateTime (use 'MMM-yy' format)
                            //DateTime parsedDate = DateTime.TryParseExact(dateStr, "MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempDate)
                            //    ? tempDate
                            //    : default;

                            //// If parsedDate is still default (i.e., parsing failed), assign it an empty string
                            //string formattedDate = parsedDate == default ? string.Empty : parsedDate.ToString("yyyy-MM-dd");

                            decimal receivables = GetValueOrDefault<decimal>(reader, "Receivables");
                            int daysInAR = GetValueOrDefault<int>(reader, "DaysInAR");
                            decimal percentageOfCharges = GetValueOrDefault<decimal>(reader, "PercentageOfCharges");
                            decimal denialPercentageGoal = GetValueOrDefault<decimal>(reader, "DenialPercentageGoal");
                            decimal change = GetValueOrDefault<decimal>(reader, "ChangePercentage");

                            var monthlyReceivablesData = new MonthlyReceivablesData(
                                dateStr,  // Now properly formatted date string
                                receivables,        // Ensures receivables is not null (defaults to 0 if null)
                                daysInAR, // Ensures daysInAR is not null (defaults to 0 if null)
                                percentageOfCharges,// Ensures percentageOfCharges is not null (defaults to 0 if null)
                                denialPercentageGoal, // Ensures denialPercentageGoal is not null (defaults to 0 if null)
                                change,         // Ensures change is not null (defaults to 0 if null)
                                string.Empty    // Empty value for the last field
                            );

                            result.Add(monthlyReceivablesData);
                        }
                    }
                    reader.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while executing stored procedure", ex);
            }
        }

    }

    public class SafeSqlDataReader
    {
        private readonly SqlDataReader _reader;

        public SafeSqlDataReader(SqlDataReader reader)
        {
            _reader = reader;
        }

        public int GetInt32(string columnName)
        {
            int ordinal = _reader.GetOrdinal(columnName);
            return _reader.IsDBNull(ordinal) ? 0 : _reader.GetInt32(ordinal);
        }

        public decimal GetDecimal(string columnName)
        {
            int ordinal = _reader.GetOrdinal(columnName);
            return _reader.IsDBNull(ordinal) ? 0m : _reader.GetDecimal(ordinal);
        }

        public string GetString(string columnName)
        {
            int ordinal = _reader.GetOrdinal(columnName);
            return _reader.IsDBNull(ordinal) ? string.Empty : _reader.GetString(ordinal);
        }

        public DateTime GetDateTime(string columnName)
        {
            int ordinal = _reader.GetOrdinal(columnName);
            return _reader.IsDBNull(ordinal) ? DateTime.MinValue : _reader.GetDateTime(ordinal);
        }

        // Add more methods as needed for other types
    }

}

