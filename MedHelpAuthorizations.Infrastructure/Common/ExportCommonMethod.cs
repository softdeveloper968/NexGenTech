using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.ReadOnlyObjects;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Models.Exports;
using Microsoft.Data.SqlClient;
using MudBlazor.Charts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MedHelpAuthorizations.Infrastructure.Common
{
    public static class ExportCommonMethod
    {
        public static SqlCommand CreateDynamicSpCommand(string spName, SqlConnection conn, int clientId = 0, string delimitedLineItemStatusIds = "", string clientInsuranceIds = "", string clientAuthTypeIds = "", string clientProcedureCodes = "", string clientExceptionReasonCategoryIds = "", string clientProviderIds = "", string clientLocationIds = "", DateTime? dateOfServiceFrom = null, DateTime? dateOfServiceTo = null, DateTime? claimBilledFrom = null, DateTime? claimBilledTo = null, DateTime? receivedFrom = null, DateTime? receivedTo = null, DateTime? transactionDateFrom = null, DateTime? transactionDateTo = null, int? patientId = null, int? claimStatusBatchId = null, ProviderLevelEnum? providerLevelId = null, SpecialtyEnum? specialtyId = null, int filterForDays = 0, string filterBy = "", string flattenedLineItemStatus = "", string dashboardType = "", int? claimStatusType = null, string claimStatusTypeStatus = null, string denialStatusIds = "", bool hasIncludeClaimStatusTransactionLineItemStatusChange = false)
        {
            SqlCommand cmd = new(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (flattenedLineItemStatus != "In-Process") { flattenedLineItemStatus = null; }

            if (clientId > 0) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientId, clientId);
            if (!string.IsNullOrEmpty(delimitedLineItemStatusIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DelimitedLineItemStatusIds, delimitedLineItemStatusIds);
            if (!string.IsNullOrEmpty(clientInsuranceIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientInsuranceIds, clientInsuranceIds);
            if (!string.IsNullOrEmpty(clientAuthTypeIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientAuthTypeIds, clientAuthTypeIds);
            if (!string.IsNullOrEmpty(clientProcedureCodes)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProcedureCodes, clientProcedureCodes);
            if (!string.IsNullOrEmpty(clientExceptionReasonCategoryIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientExceptionReasonCategoryIds, clientExceptionReasonCategoryIds);
            if (!string.IsNullOrEmpty(clientProviderIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientProviderIds, clientProviderIds);
            if (!string.IsNullOrEmpty(clientLocationIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClientLocationIds, clientLocationIds);
            if (!string.IsNullOrEmpty(flattenedLineItemStatus)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.FlattenedLineItemStatus, flattenedLineItemStatus);
            if (!string.IsNullOrEmpty(dashboardType)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DashboardType, dashboardType);
            if (dateOfServiceFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceFrom, dateOfServiceFrom);
            if (dateOfServiceTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DateOfServiceTo, dateOfServiceTo);
            if (claimBilledFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledFrom, claimBilledFrom);
            if (claimBilledTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimBilledTo, claimBilledTo);
            if (receivedFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedFrom, receivedFrom);
            if (receivedTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ReceivedTo, receivedTo);
            if (transactionDateFrom.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateFrom, transactionDateFrom);
            if (transactionDateTo.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.TransactionDateTo, transactionDateTo);
            if (patientId > 0) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.PatientId, patientId);
            if (claimStatusBatchId > 0) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimStatusBatchId, claimStatusBatchId);
            if (filterForDays > 0) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.FilterForDays, filterForDays);
            if (!string.IsNullOrEmpty(filterBy)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.FilterBy, filterBy);
            if (providerLevelId.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ProviderLevelId, providerLevelId);
            if (specialtyId.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.SpecialtyId, specialtyId);
            if (claimStatusType.HasValue) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimStatusType, claimStatusType);
            if (!string.IsNullOrEmpty(claimStatusTypeStatus)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.ClaimStatusTypeStatus, claimStatusTypeStatus);
            if (!string.IsNullOrEmpty(denialStatusIds)) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.DenialStatusIds, denialStatusIds);
            if (hasIncludeClaimStatusTransactionLineItemStatusChange) cmd.Parameters.AddWithValue(StoreProcedureParamsHelper.HasIncludeClaimStatusTransactionLineItemStatusChange, hasIncludeClaimStatusTransactionLineItemStatusChange);
            return cmd;
        }
        public static object ConvertToPropertyType(object columnValue, Type propertyType)
        {
            if (propertyType == typeof(int))
            {
                return Convert.ToInt32(columnValue);
            }
            else if (propertyType == typeof(string))
            {
                return Convert.ToString(columnValue);
            }
            else if (propertyType == typeof(DateTime?))
            {
                if (columnValue == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return Convert.ToDateTime(columnValue);
                }
            }
            else if (propertyType == typeof(DateTime))
            {
                return Convert.ToDateTime(columnValue);
            }
            else if (propertyType == typeof(decimal))
            {
                return Convert.ToDecimal(columnValue);
            }
            else if (propertyType == typeof(double))
            {
                return Convert.ToDouble(columnValue);
            }
            else if (propertyType == typeof(float))
            {
                return Convert.ToSingle(columnValue);
            }
            // Add more conversions for other property types as needed

            // If the property type is not handled, return null
            return null;
        }

        /// <summary>
        ///  Generic method to select and filter items from a list
        ///  <para>
        ///     var response = SelectAndFilter(modelWhichContainFullList,
        ///     item => new ModelNameWhichYouWantToReturnAsResponse
        ///     {
        ///         PatientFirstName = item.PatientFirstName,
        ///         PatientLastName = item.PatientLastName,
        ///         PolicyNumber = item.PolicyNumber
        ///     },
        ///     item => item.PolicyNumber == "123");
        /// </para> 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectAndFilter<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector, Func<TSource, bool> predicate = null)
        {

            if (predicate is null)
                return source.Select(selector);
            else
                return source.Where(predicate).Select(selector);
        }

        public static DynamicExportQueryResponse GetDynamicExportQueryResponse(SqlDataReader reader)
        {
            var dynamicExportQueryResponse = new DynamicExportQueryResponse();
            try
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i);

                    // Use reflection to set the property value dynamically
                    var property = dynamicExportQueryResponse.GetType().GetProperty(columnName);
                    if (property != null && columnValue != null)
                    {
                        // Convert the column value to the property's data type
                        object convertedValue = ExportCommonMethod.ConvertToPropertyType(columnValue, property.PropertyType);
                        property.SetValue(dynamicExportQueryResponse, convertedValue);
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return dynamicExportQueryResponse ?? null;
        }

        public static object GetSQLCommandParametersWithValues(SqlCommand cmd)
        {
            return cmd.Parameters.Cast<SqlParameter>().Select(prm => new { prm.ParameterName, prm.SqlValue }).ToList();
        }

        #region helper methods
        public static bool HasColumn(DataRowCollection rows, string ColumnName)
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
        public static int GetIntValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) && reader[columnName] != DBNull.Value
                ? (int)reader[columnName]
                : 0;
        }

        public static decimal GetDecimalValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) && reader[columnName] != DBNull.Value
                ? (decimal)reader[columnName]
                : 0.00m;
        }

        public static string GetStringValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ? reader[columnName] as string
                     : default(string);
        }

        public static string GetDateStringValue(SqlDataReader reader, DataRowCollection rows, string columnName)
        {
            return HasColumn(rows, columnName) ?
                    (reader[columnName] == DBNull.Value ?
                    default(string) : ((DateTime?)reader[columnName]).Value.ToString(ClaimFiltersHelpers._dateFormat)) : default(string);
        }
        public static T GetEnumValue<T>(SqlDataReader reader, DataRowCollection rows, string columnName)
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
