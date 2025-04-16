using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedChartAndProviderKpiColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChartCompletionTiming",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CodingAccuracy",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ComplianceAccuracy",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyCompletedVisits",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DocumentationAccuracy",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NoShow",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OpenCharts",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OrganizationalPassRate",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ScheduledAppointments",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChartCompletionTiming",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "CodingAccuracy",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "ComplianceAccuracy",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "DailyCompletedVisits",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "DocumentationAccuracy",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "NoShow",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "OpenCharts",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "OrganizationalPassRate",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "ScheduledAppointments",
                schema: "dbo",
                table: "ClientKpi");
        }
    }
}
