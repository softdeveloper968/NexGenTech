using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedLocationToClienEndOfMonthTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientLocationId",
                table: "ClientEndOfMonthTotals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientEndOfMonthTotals_ClientLocationId",
                table: "ClientEndOfMonthTotals",
                column: "ClientLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientEndOfMonthTotals_ClientLocations_ClientLocationId",
                table: "ClientEndOfMonthTotals",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientEndOfMonthTotals_ClientLocations_ClientLocationId",
                table: "ClientEndOfMonthTotals");

            migrationBuilder.DropIndex(
                name: "IX_ClientEndOfMonthTotals_ClientLocationId",
                table: "ClientEndOfMonthTotals");

            migrationBuilder.DropColumn(
                name: "ClientLocationId",
                table: "ClientEndOfMonthTotals");
        }
    }
}
