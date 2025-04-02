using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedtheClientInsuranceRpaConfigurationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientLocationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientLocationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "ClientLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientLocationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "ClientLocationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");
        }
    }
}
