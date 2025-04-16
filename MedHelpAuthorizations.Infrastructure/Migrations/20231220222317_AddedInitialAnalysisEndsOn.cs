using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedInitialAnalysisEndsOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InitialAnalysisEndOn",
                schema: "dbo",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlternateClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_AlternateClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "AlternateClientRpaCredentialConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientRpaCredentialConfigurations_AlternateClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "AlternateClientRpaCredentialConfigurationId",
                principalTable: "ClientRpaCredentialConfigurations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientRpaCredentialConfigurations_AlternateClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsuranceRpaConfigurations_AlternateClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "InitialAnalysisEndOn",
                schema: "dbo",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AlternateClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");
        }
    }
}
