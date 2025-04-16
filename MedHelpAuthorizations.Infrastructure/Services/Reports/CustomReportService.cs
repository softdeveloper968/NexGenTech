using AutoMapper;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.CustomAttributes.CustomReport;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services.Reports
{
    public class CustomReportService : ICustomReportService
    {
        private readonly ApplicationContext _context;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IStringLocalizer<ClaimStatusQueryService> _localizer;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITenantInfo _tenantInfo;

        public CustomReportService(ApplicationContext context, IDbContextFactory<ApplicationContext> contextFactory, IUnitOfWork<int> unitOfWork, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IMapper mapper, IStringLocalizer<ClaimStatusQueryService> localizer, IConfiguration configuration, ITenantInfo tenantInfo)
        {
            _context = context;
            _contextFactory = contextFactory;
            _unitOfWork = unitOfWork;
            _tenantInfo = tenantInfo;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _mapper = mapper;
            _configuration = configuration;
            _localizer = localizer;
            _tenantInfo = tenantInfo;
        }
        /// <summary>
        /// GetContextDetailForClaimReportType
        /// </summary>
        /// <returns></returns>
        public CustomReportTypeEntity GetContextDetailForClaimReportType()
        {
            // Get all entity types in the DbContext
            Type entityType = _context.Model.GetEntityTypes().Select(e => e.ClrType).FirstOrDefault(z => z.Name == CustomReportHelper._ClaimStatusBatchClaim);
            CustomReportTypeEntity result = ReadPropertiesWithAttributes(entityType);
            return result;
        }
        /// <summary>
        /// Recursive method to ReadPropertiesWithAttributes
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static CustomReportTypeEntity ReadPropertiesWithAttributes(Type entityType)
        {
            List<CustomReportTypeNestedAttributeColumns> customReportTypeNestedAttributeColumnsDetails = new();

            #region Get Main Entity Details
            PropertyInfo[] properties = entityType.GetProperties();
            var entityPropertiesDetail = properties.Where(propertyInfo => propertyInfo.IsDefined(typeof(CustomReportTypeColumnsHeaderForMainEntityAttribute), inherit: false)).Select(propertyInfo => propertyInfo.GetCustomAttribute<CustomReportTypeColumnsHeaderForMainEntityAttribute>()).ToList();

            #endregion

            #region Get All the Sub/Foreignkey dependent Entities
            var inheritedSubEntities = properties
                      .Where(propertyInfo => propertyInfo.IsDefined(typeof(CustomTypeSubEntityAttribute), inherit: false))
                      .ToList();

            GetNestedEntityAndPropertyDetails(inheritedSubEntities, ref customReportTypeNestedAttributeColumnsDetails, false);
            ;

            #endregion

            return new CustomReportTypeEntity()
            {
                MainEntityColumns = entityPropertiesDetail,
                MainEntityName = entityType.Name,
                SubEntityDetails = customReportTypeNestedAttributeColumnsDetails
            };
        }
        /// <summary>
        /// GetNestedEntityAndPropertyDetails
        /// </summary>
        /// <param name="inheritedSubEntities"></param>
        /// <param name="customReportTypeNestedAttributeColumnsDetails"></param>
        /// <param name="innerNestedEntity"></param>
        private static void GetNestedEntityAndPropertyDetails(List<PropertyInfo> inheritedSubEntities, ref List<CustomReportTypeNestedAttributeColumns> customReportTypeNestedAttributeColumnsDetails, bool innerNestedEntity = false)
        {
            foreach (PropertyInfo property in inheritedSubEntities)
            {
                // Check if the property has any custom attributes
                CustomTypeSubEntityAttribute customAttributes = property.GetCustomAttributes<CustomTypeSubEntityAttribute>().FirstOrDefault();

                PropertyInfo[] nestedEntityProperties = property.PropertyType.GetProperties();
                List<CustomReportTypeColumnsHeaderForMainEntityAttribute> nestedEntityColumns = nestedEntityProperties
                                                .Where(propertyInfo => propertyInfo.IsDefined(typeof(CustomReportTypeColumnsHeaderForMainEntityAttribute), inherit: false))
                                                .Select(propertyInfo => propertyInfo.GetCustomAttribute<CustomReportTypeColumnsHeaderForMainEntityAttribute>())
                                                .ToList();

                CustomReportTypeColumnsHeaderForMainEntityAttribute<ClaimStatusExceptionReasonCategoryEnum> enumTypeProperties = nestedEntityProperties
                                                .Where(pr => pr.IsDefined(typeof(CustomReportTypeColumnsHeaderForMainEntityAttribute<ClaimStatusExceptionReasonCategoryEnum>), inherit: false))
                                                .Select(propertyInfo => propertyInfo.GetCustomAttribute<CustomReportTypeColumnsHeaderForMainEntityAttribute<ClaimStatusExceptionReasonCategoryEnum>>())
                                                .FirstOrDefault();

                List<PropertyInfo> nestedEntityDetail = nestedEntityProperties
                                                .Where(propertyInfo => propertyInfo.IsDefined(typeof(CustomTypeSubEntityAttribute), inherit: false))
                                                .ToList();
                ///Recursion
                if (nestedEntityDetail.Any())
                {
                    GetNestedEntityAndPropertyDetails(nestedEntityDetail, ref customReportTypeNestedAttributeColumnsDetails, innerNestedEntity: true);
                }

                if (innerNestedEntity)
                {
                    ///Update Main Entity name.
                    string mainEntityName = customReportTypeNestedAttributeColumnsDetails.Select(z => z.NestedPropertyAttribute.EntityName).Distinct().FirstOrDefault();
                    if (customAttributes.EntityName != mainEntityName)
                    {
                        customAttributes.EntityName = mainEntityName;
                    }

                }
                if (enumTypeProperties is not null)
                {
                    ///Handle Enum Properties.
                    nestedEntityColumns.Add(new CustomReportTypeColumnsHeaderForMainEntityAttribute(enumTypeProperties.EntityName, enumTypeProperties.TypeCode, enumTypeProperties.PropertyName)
                    {
                        EnumTypeProperties = enumTypeProperties
                    });
                }
                if (customAttributes is not null)
                {
                    customReportTypeNestedAttributeColumnsDetails.Add(new CustomReportTypeNestedAttributeColumns()
                    {
                        NestedPropertyAttribute = customAttributes,
                        NestedColumns = nestedEntityColumns,
                        HasColumnsOnly = (nestedEntityColumns.Any() && !(enumTypeProperties is not null)),
                        HasEnumTypeColumns = nestedEntityColumns.Any(),
                        NestedEnumTypeColumns = !(enumTypeProperties is not null),
                    });

                }
            }

        }

        #region Preview Custom Report Query Generator. 
        /// <summary>
        /// GenerateDynamicSQLQuery
        /// </summary>
        /// <param name="customPreviewsReport"></param>
        /// <param name="columnsForSQLQuery"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="allowLimitPagination"></param>
        /// <returns></returns>
        public string GenerateDynamicSQLQuery(CustomPreviewsReportQuery customPreviewsReport, out string columnsForSQLQuery, int pageNumber = 1, int pageSize = 30, bool allowLimitPagination = true)
        {
            if (pageNumber == 0) pageNumber = 1;
            columnsForSQLQuery = string.Empty;
            switch (customPreviewsReport.CustomReportType)
            {
                case CustomReportTypeEnum.Appointment:
                    break;
                case CustomReportTypeEnum.Claim:
                    {
                        return ClaimReportTypeCustomReportSQLQuery(customPreviewsReport, out columnsForSQLQuery, pageNumber, pageSize, allowLimitPagination);
                    }
            }
            return string.Empty;
        }
        /// <summary>
        /// Get ClaimReportTypeCustomReportSQLQuery
        /// </summary>
        /// <param name="customPreviewsReport"></param>
        /// <param name="columnsForSQLQuery"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="allowLimitPagination"></param>
        /// <returns></returns>
        private string ClaimReportTypeCustomReportSQLQuery(CustomPreviewsReportQuery customPreviewsReport, out string columnsForSQLQuery, int pageNumber = 1, int pageSize = 30, bool allowLimitPagination = true)
        {
            string paginationQueryHeader = $"Declare @PageNumber int = {pageNumber};Declare @PageSize int = {pageSize};";
            string originalTableNameFor_ClaimStatusBatchClaim = string.Empty; string originalTableNameFor_ClaimStatusTransaction = string.Empty; string originalTableNameFor_ClaimStatusBatch = string.Empty; string originalTableNameFor_ClientInsurance = string.Empty; string originalTableNameFor_ClientProvider = string.Empty; string originalTableNameFOr_ClientLocation = string.Empty; string originalTableNameFOr_Patient = string.Empty;

            var columns = customPreviewsReport.ChoosedColumns.Select(z => new ChooseColumnsForCustomReport { EntityName = z.MainEntityName, Name = z.Name }).ToList();


            originalTableNameFor_ClaimStatusBatchClaim = $"IntegratedServices.ClaimStatusBatchClaims as {CustomReportHelper.mainEntityAlias} \r\n";

            ///Select * from IntegratedServices.ClaimStatusBatchClaims as _claimStatusBatchClaim
            ///Join IntegratedServices.ClaimStatusBatches as _claimStatusBatches on _claimStatusBatchClaim.ClaimStatusBatchId = _claimStatusBatches.Id
            ///Join IntegratedServices.ClaimStatusTransactions as _claimStatusTransaction on _claimStatusBatchClaim.ClaimStatusTransactionId = _claimStatusTransaction.Id
            ///Join[dbo].[ClientInsurances] as _clientInsurances on _claimStatusBatchClaim.ClientInsuranceId = _clientInsurances.Id
            ///Join[dbo].[Providers] as _clientProviders on _claimStatusBatchClaim.ClientProviderId = _clientProviders.Id
            ///Join[dbo].[ClientLocations] as _clientLocations on _claimStatusBatchClaim.ClientLocationId = _clientLocations.Id
            ///Join [dbo].[Patients] as _patients on _claimStatusBatchClaim.PatientId = _patients.Id

            string sqlQuery = $"{paginationQueryHeader} \r\n \r\n Select ";
            columnsForSQLQuery = string.Empty;
            string groupByStr = $"Group By \r\n";
            string orderByStr = $"Order By \r\n";

            #region Bind Selected columns instead of *.

            string bindClaimStatusBatchClaimColumns = string.Empty;
            string bindClaimStatusTransactionColumns = string.Empty;
            string bindClaimStatusBatchColumns = string.Empty;
            string bindClientInsuranceColumns = string.Empty;
            string bindClientProviderColumns = string.Empty;
            string bindClientLocationColumns = string.Empty;
            string bindPatientColumns = string.Empty;
            string bindPersonColumns = string.Empty;
            string originalTableNameFOr_Person = string.Empty;

            GetClaimReportTypeSQLQueryColumnNames(columns.GroupBy(c => c.EntityName).ToList(), bindClaimStatusBatchClaimColumns, bindClaimStatusTransactionColumns, bindClaimStatusBatchColumns, bindClientInsuranceColumns, bindClientProviderColumns, bindClientLocationColumns, bindPatientColumns, bindPersonColumns, ref columnsForSQLQuery, out string columnsForGroupByAndOrderBySQLQuery);

            if (!string.IsNullOrEmpty(columnsForSQLQuery))
            {
                sqlQuery += $"{columnsForSQLQuery} \r\n \r\n";
                ///commented for now to reduce loading time, will include if need to include aggregate functions.
                //groupByStr += $"{columnsForGroupByAndOrderBySQLQuery} \r\n";
                orderByStr += $"{columnsForGroupByAndOrderBySQLQuery} \r\n";
            }
            #endregion

            #region Join Tables and bind in Query.
            List<ChooseColumnsForCustomReport> tableNames = columns.DistinctBy(z => z.EntityName).ToList();
            if (customPreviewsReport.SetFilterColumnsWithValues.Any())
            {
                //customPreviewsReport.SetFilterColumnsWithValues.Where(z=>)
            }

            //foreach (var joinTables in tableNames)
            //{
            //    switch (joinTables.EntityName)

            foreach (var joinTablesEntityName in customPreviewsReport.TableNames)
            {
                switch (joinTablesEntityName)
                {
                    case CustomReportHelper._ClaimStatusTransaction:
                        {
                            originalTableNameFor_ClaimStatusTransaction = $"Join IntegratedServices.ClaimStatusTransactions as {CustomReportHelper.tableAliasForClaimStatusTransaction} on {CustomReportHelper.mainEntityAlias}.ClaimStatusTransactionId = {CustomReportHelper.tableAliasForClaimStatusTransaction}.Id  \r\n";
                            break;
                        }
                    case CustomReportHelper._ClaimStatusBatch:
                        {
                            originalTableNameFor_ClaimStatusBatch = $"Join IntegratedServices.ClaimStatusBatches as {CustomReportHelper.tableAliasForClaimStatusBatch} on {CustomReportHelper.mainEntityAlias}.ClaimStatusBatchId = {CustomReportHelper.tableAliasForClaimStatusBatch}.Id   \r\n";
                            break;
                        }
                    case CustomReportHelper._ClientInsurance:
                        {
                            originalTableNameFor_ClientInsurance = $"Join [dbo].[ClientInsurances] as {CustomReportHelper.tableAliasForClientInsurance} on {CustomReportHelper.mainEntityAlias}.ClientInsuranceId = {CustomReportHelper.tableAliasForClientInsurance}.Id  \r\n";
                            break;
                        }
                    case CustomReportHelper._ClientProvider:
                        {
                            originalTableNameFor_ClientProvider = $"Join [dbo].[Providers] as {CustomReportHelper.tableAliasForClientProvider} on {CustomReportHelper.mainEntityAlias}.ClientProviderId = {CustomReportHelper.tableAliasForClientProvider}.Id  \r\n";
                            break;
                        }
                    case CustomReportHelper._ClientLocation:
                        {
                            originalTableNameFOr_ClientLocation = $"Join [dbo].[ClientLocations] as {CustomReportHelper.tableAliasForClientLocation} on {CustomReportHelper.mainEntityAlias}.ClientLocationId = {CustomReportHelper.tableAliasForClientLocation}.Id  \r\n";
                            break;
                        }
                    case CustomReportHelper._Patient:
                        {
                            originalTableNameFOr_Patient = $"Join [dbo].[Patients] as {CustomReportHelper.tableAliasForPatient}  on {CustomReportHelper.mainEntityAlias}.PatientId = {CustomReportHelper.tableAliasForPatient}.Id \r\n";
                            break;
                        }
                    case CustomReportHelper._Person:
                        {
                            ///Join [dbo].[Persons] as _person on _person.Id=_provider.PersonId
                            if (string.IsNullOrEmpty(originalTableNameFor_ClientProvider))
                            {
                                originalTableNameFor_ClientProvider = $"Join [dbo].[Providers] as {CustomReportHelper.tableAliasForClientProvider} on {CustomReportHelper.mainEntityAlias}.ClientProviderId = {CustomReportHelper.tableAliasForClientProvider}.Id  \r\n";
                            }

                            originalTableNameFOr_Person = $"Join [dbo].[Persons] as {CustomReportHelper.tableAliasForPerson}  on {CustomReportHelper.tableAliasForClientProvider}.PersonId = {CustomReportHelper.tableAliasForPerson}.Id  \r\n";
                            break;
                        }
                }
            }

            sqlQuery += $"From {originalTableNameFor_ClaimStatusBatchClaim} ";
            if (!string.IsNullOrEmpty(originalTableNameFor_ClaimStatusTransaction)) { sqlQuery += originalTableNameFor_ClaimStatusTransaction; }
            if (!string.IsNullOrEmpty(originalTableNameFor_ClaimStatusBatch)) { sqlQuery += originalTableNameFor_ClaimStatusBatch; }
            if (!string.IsNullOrEmpty(originalTableNameFor_ClientInsurance)) { sqlQuery += originalTableNameFor_ClientInsurance; }
            if (!string.IsNullOrEmpty(originalTableNameFor_ClientProvider)) { sqlQuery += originalTableNameFor_ClientProvider; }
            if (!string.IsNullOrEmpty(originalTableNameFOr_ClientLocation)) { sqlQuery += originalTableNameFOr_ClientLocation; }
            if (!string.IsNullOrEmpty(originalTableNameFOr_Patient)) { sqlQuery += originalTableNameFOr_Patient; }
            if (!string.IsNullOrEmpty(originalTableNameFOr_Person)) { sqlQuery += originalTableNameFOr_Person; }

            #endregion

            #region Check CustomReport Model has Selected Filters || Bind Where clause in SQL Query if selected filters found.
            string whereClauseForSQLQuery = "\r\n \r\n Where \r\n";
            whereClauseForSQLQuery += $" {CustomReportHelper.mainEntityAlias}.ClientId = {customPreviewsReport.ClientId} \r\n";

            if (!string.IsNullOrEmpty(customPreviewsReport.CustomDateQueryForWhereClause))
            {
                whereClauseForSQLQuery += $"{customPreviewsReport.CustomDateQueryForWhereClause} \r\n";
            }
            string clauseData = CustomReportHelper.ConvertToConditionString(customPreviewsReport.SetFilterColumnsWithValues);
            if (!string.IsNullOrEmpty(clauseData))
            {
                sqlQuery += string.Concat(whereClauseForSQLQuery, " AND ", clauseData);
            }
            else
            {
                sqlQuery += whereClauseForSQLQuery;
            }

            #endregion

            #region GroupBy and OrderBy Clause

            ///sqlQuery += groupByStr;
            sqlQuery += "\r\n \r\n";
            sqlQuery += orderByStr;
            sqlQuery += "\r\n \r\n";

            #endregion

            #region Pagination
            if (allowLimitPagination)
            {
                string limitPagination = $"OFFSET (@PageNumber-1)*@PageSize ROWS  FETCH NEXT @PageSize ROWS ONLY \r\n";
                sqlQuery += limitPagination;
            }
            #endregion

            return sqlQuery;
        }
        /// <summary>
        /// GetClaimReportTypeSQLQueryColumnNames
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="bindClaimStatusBatchClaimColumns"></param>
        /// <param name="bindClaimStatusTransactionColumns"></param>
        /// <param name="bindClaimStatusBatchColumns"></param>
        /// <param name="bindClientInsuranceColumns"></param>
        /// <param name="bindClientProviderColumns"></param>
        /// <param name="bindClientLocationColumns"></param>
        /// <param name="bindPatientColumns"></param>
        /// <param name="bindPersonColumns"></param>
        /// <param name="columnsForSQLQuery"></param>
        /// <param name="columnsForGroupByAndOrderBySQLQuery"></param>
        private void GetClaimReportTypeSQLQueryColumnNames(IEnumerable<IGrouping<string, ChooseColumnsForCustomReport>> columns, string bindClaimStatusBatchClaimColumns, string bindClaimStatusTransactionColumns, string bindClaimStatusBatchColumns, string bindClientInsuranceColumns, string bindClientProviderColumns, string bindClientLocationColumns, string bindPatientColumns, string bindPersonColumns, ref string columnsForSQLQuery, out string columnsForGroupByAndOrderBySQLQuery)
        {
            columnsForGroupByAndOrderBySQLQuery = string.Empty;
            foreach (IGrouping<string, ChooseColumnsForCustomReport> bindColumnsByEntityName in columns)
            {
                switch (bindColumnsByEntityName.Key)
                {
                    case CustomReportHelper._ClaimStatusBatchClaim:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                bindClaimStatusBatchClaimColumns += $"{CustomReportHelper.mainEntityAlias}.[{columnDetails.Name}]  as '{columnDetails.Name}_ClaimStatusBatchClaim' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.mainEntityAlias}.[{columnDetails.Name}],\r\n";
                            }

                            break;
                        }
                    case CustomReportHelper._ClaimStatusTransaction:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                bindClaimStatusTransactionColumns += $"{CustomReportHelper.tableAliasForClaimStatusTransaction}.[{columnDetails.Name}]  as '{columnDetails.Name}_ClaimStatusTransaction' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.tableAliasForClaimStatusTransaction}.[{columnDetails.Name}],\r\n";
                            }
                            break;
                        }
                    case CustomReportHelper._ClaimStatusBatch:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                bindClaimStatusBatchColumns += $"{CustomReportHelper.tableAliasForClaimStatusBatch}.[{columnDetails.Name}]  as '{columnDetails.Name}_ClaimStatusBatch' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.tableAliasForClaimStatusBatch}.[{columnDetails.Name}],\r\n";
                            }
                            break;
                        }
                    case CustomReportHelper._ClientInsurance:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                bindClientInsuranceColumns += $"{CustomReportHelper.tableAliasForClientInsurance}.[{columnDetails.Name}]  as '{columnDetails.Name}_ClientInsurance' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.tableAliasForClientInsurance}.[{columnDetails.Name}],\r\n";
                            }
                            break;
                        }
                    case CustomReportHelper._ClientProvider:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                bindClientProviderColumns += $"{CustomReportHelper.tableAliasForClientProvider}.[{columnDetails.Name}]  as '{columnDetails.Name}_Provider' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.tableAliasForClientProvider}.[{columnDetails.Name}],\r\n";
                            }
                            break;
                        }
                    case CustomReportHelper._ClientLocation:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                bindClientLocationColumns += $"{CustomReportHelper.tableAliasForClientLocation}.[{columnDetails.Name}]  as '{columnDetails.Name}_ClientLocation' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.tableAliasForClientLocation}.[{columnDetails.Name}],\r\n";
                            }
                            break;
                        }
                    case CustomReportHelper._Patient:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                bindPatientColumns += $"{CustomReportHelper.tableAliasForPatient}.[{columnDetails.Name}]  as '{columnDetails.Name}_Patient' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.tableAliasForPatient}.[{columnDetails.Name}],\r\n";
                            }
                            break;
                        }
                    case CustomReportHelper._Person:
                        {
                            foreach (var columnDetails in bindColumnsByEntityName)
                            {
                                ///Join [dbo].[Persons] as _person on _person.Id=_provider.PersonId
                                bindPersonColumns += $"{CustomReportHelper.tableAliasForPerson}.[{columnDetails.Name}]  as '{columnDetails.Name}_Person' ,\r\n";
                                columnsForGroupByAndOrderBySQLQuery += $"{CustomReportHelper.tableAliasForPerson}.[{columnDetails.Name}],\r\n";
                            }
                            break;
                        }
                }

            }

            if (!string.IsNullOrEmpty(bindClaimStatusBatchClaimColumns)) { columnsForSQLQuery += bindClaimStatusBatchClaimColumns; }
            if (!string.IsNullOrEmpty(bindClaimStatusTransactionColumns)) { columnsForSQLQuery += bindClaimStatusTransactionColumns; }
            if (!string.IsNullOrEmpty(bindClaimStatusBatchColumns)) { columnsForSQLQuery += bindClaimStatusBatchColumns; }
            if (!string.IsNullOrEmpty(bindClientInsuranceColumns)) { columnsForSQLQuery += bindClientInsuranceColumns; }
            if (!string.IsNullOrEmpty(bindClientProviderColumns)) { columnsForSQLQuery += bindClientProviderColumns; }
            if (!string.IsNullOrEmpty(bindClientLocationColumns)) { columnsForSQLQuery += bindClientLocationColumns; }
            if (!string.IsNullOrEmpty(bindPatientColumns)) { columnsForSQLQuery += bindPatientColumns; }
            if (!string.IsNullOrEmpty(bindPersonColumns)) { columnsForSQLQuery += bindPersonColumns; }

            ///Check , in bindPatientColumns and remove from last index.
            int columnIndex = columnsForSQLQuery.LastIndexOf(',');
            int groupByColumnIndex = columnsForGroupByAndOrderBySQLQuery.LastIndexOf(',');

            if (!string.IsNullOrEmpty(columnsForSQLQuery) && columnIndex >= 0)
            {
                columnsForSQLQuery = columnsForSQLQuery.Remove(columnIndex);
            }
            if (!string.IsNullOrEmpty(columnsForGroupByAndOrderBySQLQuery) && groupByColumnIndex >= 0)
            {
                columnsForGroupByAndOrderBySQLQuery = columnsForGroupByAndOrderBySQLQuery.Remove(groupByColumnIndex);
            }

        }

        public async Task<UpdatedClaimReportTypePreviewModel> ExecutionClaimReportTypeSQLQuery(string claimSQLquery, string columnsForSQLQuery, bool allowLimitPagination = true)
        {
            try
            {
                //List<ClaimReportTypeColumns> claimReportTypeColumns = new();
                //SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);
                //var connectionString = _configuration.GetConnectionString(StoreProcedureTitle.DefaultConnection);
                var connectionString = _tenantInfo.ConnectionString;///EN-112
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("connection string not found!");
                }
                List<string> extractedColumns = GetExtractedColumns(columnsForSQLQuery);

                // Execute SQL Query
                DataTable resultTable = await ExecuteSQLQueryAsync(_tenantInfo.ConnectionString, claimSQLquery);
                return GetResponseModel(resultTable, extractedColumns, allowLimitPagination);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// ExecutionPreviewClaimReportTypeSQLQuery
        /// </summary>
        /// <param name="claimSQLquery"></param>
        /// <param name="includeColumns"></param>
        /// <returns></returns>
        public async Task<string> ExecutionPreviewClaimReportTypeSQLQuery(string claimSQLquery, bool includeColumns = false)
        {
            try
            {
                //List<ClaimReportTypeColumns> claimReportTypeColumns = new();
                //SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

                // Execute SQL Query
                DataTable resultTable = await ExecuteSQLQueryAsync(_tenantInfo.ConnectionString, claimSQLquery).ConfigureAwait(false);
                return GetCustomReportResponse(resultTable, includeColumns);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// GetExtractedColumns
        /// </summary>
        /// <param name="columnsForSQLQuery"></param>
        /// <returns></returns>
        private static List<string> GetExtractedColumns(string columnsForSQLQuery)
        {

            ///Extract Column Names
            // Remove \r\n characters from the input string
            columnsForSQLQuery = columnsForSQLQuery.Replace("\r\n", "");

            // Extract text inside single quotes ('') using regex
            List<string> extractedColumns = new();
            Regex regex = new(@"'([^']*)'");
            MatchCollection matches = regex.Matches(columnsForSQLQuery);

            foreach (Match match in matches)
            {
                string extractedText = match.Groups[1].Value;
                extractedColumns.Add(extractedText);
            }
            return extractedColumns;
        }
        /// <summary>
        /// ExecuteSQLQueryAsync commonly
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static async Task<DataTable> ExecuteSQLQueryAsync(string connectionString, string sqlQuery)
        {
            using (SqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    DataTable resultTable = new DataTable();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(resultTable));
                    }

                    return resultTable;
                }
            }
        }

        /// <summary>
        /// GetResponseModel for Report
        /// </summary>
        /// <param name="resultTable"></param>
        /// <param name="extractedColumns"></param>
        /// <param name="allowLimitPagination"></param>
        /// <returns></returns>
        public static UpdatedClaimReportTypePreviewModel GetResponseModel(DataTable resultTable, List<string> extractedColumns, bool allowLimitPagination)
        {
            List<ColumnsToolTip> ColumnWithToolTip = new();

            extractedColumns.ForEach(columnName =>
            {
                List<string> column = columnName.Split('_').ToList();
                ColumnWithToolTip.Add(item: new ColumnsToolTip()
                {
                    ColumnKey = column[0],
                    ColumnValue = column[1]
                });
                //ColumnsWithToolTip[column[0]] = column[1];
            });
            UpdatedClaimReportTypePreviewModel result = new()
            {
                ///Un-comment below line if A-z sorting needed.
                ///ColumnsWithToolTip = ColumnsWithToolTip.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value)
                ColumnWithToolTip = ColumnWithToolTip
            };
            // Convert DataTable rows to List of Dictionaries
            List<Dictionary<string, object>> resultData = new List<Dictionary<string, object>>();

            StringBuilder resultBuilder = new();
            // Add a header row with column names
            resultBuilder.AppendLine(string.Join(",", resultTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName)));

            foreach (DataRow row in resultTable.Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();

                // Convert the DataRow items to an array of strings
                string[] rowValues = Array.ConvertAll(row.ItemArray, item => item.ToString());

                // Concatenate the values with comma separation and append to the result
                string rowString = string.Join(",", rowValues);
                resultBuilder.AppendLine(rowString);


                foreach (DataColumn col in resultTable.Columns)
                {
                    string columnName = col.ColumnName;
                    object columnValue = row[col];

                    // Check if the column type is DateTime or DateTime?
                    if (col.DataType == typeof(DateTime) || col.DataType == typeof(DateTime?))
                    {
                        // Format the date as "MM/dd/yyyy" if it's not null
                        if (columnValue != DBNull.Value)
                        {
                            columnValue = Convert.ToDateTime(columnValue).ToString("MM/dd/yyyy");
                        }
                    }
                    // Check if the column type is decimal
                    else if (col.DataType == typeof(decimal) || col.DataType == typeof(decimal?))
                    {
                        // Format the decimal as needed (e.g., specify decimal places)
                        if (columnValue != DBNull.Value)
                        {
                            columnValue = Convert.ToDecimal(columnValue).ToString("0.00"); // Adjust the format as needed
                        }
                    }

                    if (allowLimitPagination && col.DataType == typeof(string))
                    {
                        //string truncatedVal = columnValue.ToString().Length <= 40 ? columnValue.ToString() : columnValue.ToString().Substring(0, 40);
                        if (!string.IsNullOrEmpty(columnValue.ToString()) && columnValue.ToString().Length > 50)
                        {
                            //rowData[columnName.Split('_')[0]] = columnValue.ToString()?.Substring(0, 50) ?? "";
                            rowData[columnName] = columnValue.ToString()?.Substring(0, 50) ?? "";
                        }
                        else
                        {
                            //rowData[columnName.Split('_')[0]] = columnValue;
                            rowData[columnName] = columnValue;
                        }
                    }
                    else
                    {
                        rowData[columnName] = columnValue;
                    }

                }

                resultData.Add(rowData);
            }

            if (resultData.Any())
            {
                result.PreviewReportDetails.AddRange(resultData);
            }
            return result;

        }

        public static string GetCustomReportResponse(DataTable resultTable, bool includeColumns = false)
        {
            StringBuilder resultBuilder = new();
            // Add a header row with column names
            if (includeColumns)
            {
                resultBuilder.AppendLine(string.Join(",", resultTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName.Split("_")[0])));
            }
            foreach (DataRow row in resultTable.Rows)
            {
                int indx = resultTable.Rows.IndexOf(row);
                if (indx >= 48)
                {

                }
                List<object> rowFilterValues = new();
                foreach (DataColumn col in resultTable.Columns)
                {
                    string columnName = col.ColumnName;
                    object columnValue = row[col];

                    // Check if the column type is DateTime or DateTime?
                    if (col.DataType == typeof(DateTime) || col.DataType == typeof(DateTime?))
                    {
                        // Format the date as "MM/dd/yyyy" if it's not null
                        if (columnValue != DBNull.Value)
                        {
                            columnValue = Convert.ToDateTime(columnValue).ToString("MM/dd/yyyy");
                        }
                    }
                    // Check if the column type is decimal
                    else if (col.DataType == typeof(decimal) || col.DataType == typeof(decimal?))
                    {
                        // Format the decimal as needed (e.g., specify decimal places)
                        if (columnValue != DBNull.Value)
                        {
                            columnValue = Convert.ToDecimal(columnValue).ToString("0.00"); // Adjust the format as needed
                        }
                    }
                    else if (col.DataType == typeof(string))
                    {
                        //string truncatedVal = columnValue.ToString().Length <= 40 ? columnValue.ToString() : columnValue.ToString().Substring(0, 40);
                        if (!string.IsNullOrEmpty(columnValue.ToString()))
                        {
                            //columnValue = (string)columnValue ?? string.Empty;
                            string colValue = (string)columnValue ?? string.Empty;
                            if (!string.IsNullOrEmpty(colValue)) { columnValue = CustomReportHelper.EscapeSheetData(colValue); }

                        }
                        else
                        {
                            columnValue = string.Empty;
                        }
                    }
                    rowFilterValues.Add(columnValue);
                }
                string rowString = string.Join(",", rowFilterValues);
                resultBuilder.AppendLine(rowString);
            }

            return resultBuilder.ToString();

        }

        #endregion


        /// <summary>
        /// GetChooseDisplayColumns for report
        /// </summary>
        /// <param name="headerEntityName"></param>
        /// <param name="customAttributeEntityDetails"></param>
        /// <param name="chooseColumnsDetails"></param>
        public void GetChooseDisplayColumns(string headerEntityName, CustomReportTypeEntity customAttributeEntityDetails, out List<CustomAttributeForEntitesDataItem> chooseColumnsDetails)
        {
            chooseColumnsDetails = new List<CustomAttributeForEntitesDataItem>();

            ////Add Main Entity columns.
            foreach (CustomReportTypeColumnsHeaderForMainEntityAttribute pr in customAttributeEntityDetails.MainEntityColumns)
            {
                var model = new CustomAttributeForEntitesDataItem()
                {
                    MainEntityName = headerEntityName,
                    IsLocked = false,
                    Name = pr.PropertyName,
                    MainEntityColumn = pr,
                    CustomPropertyType = pr.TypeCode
                };
                if (model is not null) { chooseColumnsDetails.Add(model); }
            }
            foreach (CustomReportTypeNestedAttributeColumns ns in customAttributeEntityDetails.SubEntityDetails)
            {
                var model = new CustomAttributeForEntitesDataItem()
                {
                    MainEntityName = ns.NestedPropertyAttribute.EntityName,
                    IsLocked = false,
                    Name = ns.NestedPropertyAttribute.SubEntityName,
                    SubEntityColumn = ns,
                    HasSubEntityColumn = ns.NestedColumns.Any(),
                };
                if (model is not null) { chooseColumnsDetails.Add(model); }
            }

        }
        /// <summary>
        /// GetSetFilterDisplayColumns
        /// </summary>
        /// <param name="headerEntityName"></param>
        /// <param name="customAttributeEntityDetails"></param>
        /// <param name="setFilterColumnsDetails"></param>
        public void GetSetFilterDisplayColumns(string headerEntityName, CustomReportTypeEntity customAttributeEntityDetails, out List<CustomReportSetFilterColumns> setFilterColumnsDetails)
        {

            setFilterColumnsDetails = new();
            ///SetFilter columns.
            var excludedDateOfServiceColumns = customAttributeEntityDetails.MainEntityColumns.Where(z => (z.PropertyName == CustomReportHelper.DateOfServiceFrom || z.PropertyName == CustomReportHelper.DateOfServiceTo)).ToList();

            ////Add Main Entity columns.
            foreach (CustomReportTypeColumnsHeaderForMainEntityAttribute pr in customAttributeEntityDetails.MainEntityColumns)
            {
                if (excludedDateOfServiceColumns.Any() && !excludedDateOfServiceColumns.Contains(pr))
                {
                    var model = new CustomReportSetFilterColumns()
                    {
                        MainEntityName = headerEntityName,
                        IsLocked = false,
                        Name = pr.PropertyName,
                        MainEntityColumn = pr,
                        CustomPropertyType = pr.TypeCode
                    };
                    if (model is not null) { setFilterColumnsDetails.Add(model); }
                }
            }
            ////CombinedColumn In one.
            if (excludedDateOfServiceColumns.Any())
            {
                setFilterColumnsDetails.Add(new CustomReportSetFilterColumns()
                {
                    MainEntityName = excludedDateOfServiceColumns[0].EntityName,
                    IsLocked = false,
                    Name = CustomReportHelper.DateOfService,
                    MainEntityColumn = excludedDateOfServiceColumns[0],
                });
            }
            foreach (CustomReportTypeNestedAttributeColumns ns in customAttributeEntityDetails.SubEntityDetails)
            {
                var model = new CustomReportSetFilterColumns()
                {
                    MainEntityName = ns.NestedPropertyAttribute.EntityName,
                    IsLocked = false,
                    Name = ns.NestedPropertyAttribute.SubEntityName,
                    SubEntityColumn = ns,
                    HasSubEntityColumn = ns.NestedColumns.Any(),
                };
                if (model is not null) { setFilterColumnsDetails.Add(model); }
            }
        }


    }
}
