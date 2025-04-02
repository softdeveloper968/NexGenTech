using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedKpiInClientProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysToBillKpi",
                schema: "dbo",
                table: "Providers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "NoShowRateKpi",
                schema: "dbo",
                table: "Providers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PatientsSeenPerDayKpi",
                schema: "dbo",
                table: "Providers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledVisitsPerDayKpi",
                schema: "dbo",
                table: "Providers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysToBillKpi",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "NoShowRateKpi",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "PatientsSeenPerDayKpi",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ScheduledVisitsPerDayKpi",
                schema: "dbo",
                table: "Providers");
        }
    }
}
