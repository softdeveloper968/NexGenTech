using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedClaimLineStatusToPatientLedgerCharges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                type: "int",
                nullable: true);            

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClaimLineItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLedgerCharges_ClaimLineItemStatuses_ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClaimLineItemStatusId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimLineItemStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLedgerCharges_ClaimLineItemStatuses_ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges");

            migrationBuilder.DropColumn(
                name: "ClientProviderId",
                table: "MonthlyARData");

            migrationBuilder.DropColumn(
                name: "CptCodeId",
                table: "MonthlyARData");
        }
    }
}
