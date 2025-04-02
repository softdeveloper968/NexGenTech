using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetBillingKpi;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Infrastructure.Common;
using MedHelpAuthorizations.Shared.Models.Executive;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class KpiDashboardService : IKpiDashboardService
    {
        private readonly ITenantInfo _tenantInfo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public KpiDashboardService(ITenantInfo tenantInfo, ICurrentUserService currentUserService, IUserService userService)
        {
            _tenantInfo = tenantInfo;
            _currentUserService = currentUserService;
            _userService = userService;

        }

        #region get billing kpi data by clientId
        public async Task<GetBillingKpiByClientIdResponse> GetBillingKpiByClientIdAsync(GetBillingKpiByClientIdQuery query)
        {
            try
            {
                GetBillingKpiByClientIdResponse response = new GetBillingKpiByClientIdResponse();
                string connStr = _tenantInfo.ConnectionString;
                SqlConnection conn = new SqlConnection(_tenantInfo.ConnectionString);

                try
                {
                    await using (conn)
                    {
                        await conn.OpenAsync();

                        //create the sp
                        SqlCommand cmd = CreateBillingKpiSpCommand(StoreProcedureTitle.spGetBillingKpiQuery, conn, query);
                        cmd.Parameters.AddWithValue("@ClientId", _currentUserService.ClientId);
                        cmd.Parameters.AddWithValue("@ClaimBilledFrom", query.ClaimBilledFrom);
                        cmd.Parameters.AddWithValue("@ClaimBilledTo", query.ClaimBilledTo);

                        //execute the sp
                        var billingKpiTask = ExecuteBillingKpiSp(cmd);

                        //wait for the task to complete and then map the result into response
                        await Task.WhenAll(billingKpiTask);
                        response = billingKpiTask.Result;

                    }
                    return response;
                }
                catch (System.Exception ex)
                {

                    throw;
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public SqlCommand CreateBillingKpiSpCommand(string spName, SqlConnection conn, GetBillingKpiByClientIdQuery query)
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

        public async Task<GetBillingKpiByClientIdResponse> ExecuteBillingKpiSp(SqlCommand cmd)
        {
            try
            {
                GetBillingKpiByClientIdResponse billingKpiData = new GetBillingKpiByClientIdResponse();
                await using (cmd)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var rows = reader.GetSchemaTable().Rows;
                        billingKpiData = MapBillingKpiByClientIdResponse(reader, rows);
                    }
                    reader.Close();
                }
                return billingKpiData ?? new GetBillingKpiByClientIdResponse();
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public static GetBillingKpiByClientIdResponse MapBillingKpiByClientIdResponse(SqlDataReader reader, DataRowCollection rows)
        {
            return new GetBillingKpiByClientIdResponse
            {
                //Id = ExportCommonMethod.GetIntValue(reader, rows, KpiDashboardColumnsHelper.Id),
                //ClientId = ExportCommonMethod.GetIntValue(reader, rows, KpiDashboardColumnsHelper.ClientId),
                //DailyClaimCountGoal = ExportCommonMethod.GetIntValue(reader, rows, KpiDashboardColumnsHelper.DailyClaimCountGoal),
                CleanClaimRateGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.CleanClaimRateGoal),
                DenialRateGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.DenialRateGoal),
                ChargesGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.ChargesGoal),
                CollectionPercentageGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.CollectionPercentageGoal),
                CashCollectionsGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.CashCollectionsGoal),
                Over90DaysGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.Over90DaysGoal),
                //AverageDaysInReceivablesGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.AverageDaysInReceivablesGoal),
                BDRateGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.BDRateGoal),
                //DailyClaimCountValue = ExportCommonMethod.GetIntValue(reader, rows, KpiDashboardColumnsHelper.DailyClaimCountValue),
                CleanClaimRateValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.CleanClaimRateValue),
                DenialRateValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.DenialRateValue),
                ChargesValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.ChargesValue),
                CollectionPercentageValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.CollectionPercentageValue),
                CashCollectionsValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.CashCollectionsValue),
                Over90DaysValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.Over90DaysValue),
                //AverageDaysInReceivablesValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.AverageDaysInReceivablesValue),
                BDRateValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.BDRateValue),
                VisitsGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.VisitsGoal),
                VisitsValue = ExportCommonMethod.GetIntValue(reader, rows, KpiDashboardColumnsHelper.VisitsValue),
                DaysInARGoal = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.DaysInARGoal),
				DaysInARValue = ExportCommonMethod.GetDecimalValue(reader, rows, KpiDashboardColumnsHelper.DaysInARValue),
			};
        }
        #endregion
    }
}
