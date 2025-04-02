using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddToTqablesForFeeScheduleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ClientInsuranceFeeSchedules",
                newName: "ClientInsuranceFeeSchedules",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ClientFeeScheduleSpecialties",
                newName: "ClientFeeScheduleSpecialties",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ClientFeeSchedules",
                newName: "ClientFeeSchedules",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ClientFeeScheduleProviderLevels",
                newName: "ClientFeeScheduleProviderLevels",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ClientFeeScheduleEntries",
                newName: "ClientFeeScheduleEntries",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ClientInsuranceFeeSchedules",
                schema: "dbo",
                newName: "ClientInsuranceFeeSchedules");

            migrationBuilder.RenameTable(
                name: "ClientFeeScheduleSpecialties",
                schema: "dbo",
                newName: "ClientFeeScheduleSpecialties");

            migrationBuilder.RenameTable(
                name: "ClientFeeSchedules",
                schema: "dbo",
                newName: "ClientFeeSchedules");

            migrationBuilder.RenameTable(
                name: "ClientFeeScheduleProviderLevels",
                schema: "dbo",
                newName: "ClientFeeScheduleProviderLevels");

            migrationBuilder.RenameTable(
                name: "ClientFeeScheduleEntries",
                schema: "dbo",
                newName: "ClientFeeScheduleEntries");
        }
    }
}
