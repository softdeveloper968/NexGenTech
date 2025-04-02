using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedtheClientLocationInsuranceRpaConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations");

            migrationBuilder.AlterColumn<int>(
                name: "ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations");

            migrationBuilder.AlterColumn<int>(
                name: "ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
