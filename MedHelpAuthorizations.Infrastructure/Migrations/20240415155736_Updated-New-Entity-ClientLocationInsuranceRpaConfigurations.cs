using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNewEntityClientLocationInsuranceRpaConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurations_ClienLocationId",
                table: "ClientLocationInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClienLocationId",
                table: "ClientLocationInsuranceRpaConfigurations");

            migrationBuilder.RenameColumn(
                name: "ClienLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                newName: "ClientLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLocationInsuranceRpaConfigurations_ClienLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                newName: "IX_ClientLocationInsuranceRpaConfigurations_ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClientInsuranceRpaConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurations_ClientInsuranceRpaConfigurationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClientInsuranceRpaConfigurationId",
                principalSchema: "IntegratedServices",
                principalTable: "ClientInsuranceRpaConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurations_ClientInsuranceRpaConfigurationId",
                table: "ClientLocationInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurationId",
                table: "ClientLocationInsuranceRpaConfigurations");

            migrationBuilder.RenameColumn(
                name: "ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                newName: "ClienLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLocationInsuranceRpaConfigurations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                newName: "IX_ClientLocationInsuranceRpaConfigurations_ClienLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurations_ClienLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClienLocationId",
                principalSchema: "IntegratedServices",
                principalTable: "ClientInsuranceRpaConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClienLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClienLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
