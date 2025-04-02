using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedApiIntegratiuonToRpainsurances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApiIntegrationId",
                schema: "IntegratedServices",
                table: "RpaInsurances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RpaInsurances_ApiIntegrationId",
                schema: "IntegratedServices",
                table: "RpaInsurances",
                column: "ApiIntegrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RpaInsurances_ApiIntegrations_ApiIntegrationId",
                schema: "IntegratedServices",
                table: "RpaInsurances",
                column: "ApiIntegrationId",
                principalSchema: "dbo",
                principalTable: "ApiIntegrations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RpaInsurances_ApiIntegrations_ApiIntegrationId",
                schema: "IntegratedServices",
                table: "RpaInsurances");

            migrationBuilder.DropIndex(
                name: "IX_RpaInsurances_ApiIntegrationId",
                schema: "IntegratedServices",
                table: "RpaInsurances");

            migrationBuilder.DropColumn(
                name: "ApiIntegrationId",
                schema: "IntegratedServices",
                table: "RpaInsurances");
        }
    }
}
