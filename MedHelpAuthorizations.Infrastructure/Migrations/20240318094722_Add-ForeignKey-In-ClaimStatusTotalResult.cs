using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyInClaimStatusTotalResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientInsuranceName",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "LocationNpi",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "ProcedureCode",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "ProviderName",
                table: "ClaimStatusTotalResults");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTotalResults_ClientCptCodeId",
                table: "ClaimStatusTotalResults",
                column: "ClientCptCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTotalResults_ClientId",
                table: "ClaimStatusTotalResults",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTotalResults_ClientInsuranceId",
                table: "ClaimStatusTotalResults",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTotalResults_ClientLocationId",
                table: "ClaimStatusTotalResults",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTotalResults_ClientProviderId",
                table: "ClaimStatusTotalResults",
                column: "ClientProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTotalResults_ClientCptCodes_ClientCptCodeId",
                table: "ClaimStatusTotalResults",
                column: "ClientCptCodeId",
                principalSchema: "dbo",
                principalTable: "ClientCptCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTotalResults_ClientInsurances_ClientInsuranceId",
                table: "ClaimStatusTotalResults",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTotalResults_ClientLocations_ClientLocationId",
                table: "ClaimStatusTotalResults",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTotalResults_Clients_ClientId",
                table: "ClaimStatusTotalResults",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTotalResults_Providers_ClientProviderId",
                table: "ClaimStatusTotalResults",
                column: "ClientProviderId",
                principalSchema: "dbo",
                principalTable: "Providers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTotalResults_ClientCptCodes_ClientCptCodeId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTotalResults_ClientInsurances_ClientInsuranceId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTotalResults_ClientLocations_ClientLocationId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTotalResults_Clients_ClientId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTotalResults_Providers_ClientProviderId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTotalResults_ClientCptCodeId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTotalResults_ClientId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTotalResults_ClientInsuranceId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTotalResults_ClientLocationId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTotalResults_ClientProviderId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.AddColumn<string>(
                name: "ClientInsuranceName",
                table: "ClaimStatusTotalResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "ClaimStatusTotalResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationNpi",
                table: "ClaimStatusTotalResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcedureCode",
                table: "ClaimStatusTotalResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderName",
                table: "ClaimStatusTotalResults",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
